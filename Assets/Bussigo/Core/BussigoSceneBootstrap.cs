using System;
using UnityEngine;
using Bussigo.Vehicle;
using Bussigo.Physics;
using Bussigo.Route;
using Bussigo.Traffic;
using Bussigo.Weather;
using Bussigo.Passengers;
using Bussigo.Audio;
using Bussigo.Economy;
using Bussigo.Company;
using Bussigo.Save;
using Bussigo.UI;
using Bussigo.World;
using Bussigo.Game.Runtime3D.Vehicle;
using Bussigo.Game.Runtime3D.Environment;

namespace Bussigo.Core
{
    /// <summary>
    /// Master Scene Bootstrapper for BUSSIGO V2.
    /// Orchestrates service initialization, pure C# domain services, Hero Bus instantiation, highway network streaming,
    /// traffic AI, dynamic weather, 44-seat passenger queues, audio mixer, and simulator HUD on scene launch.
    /// </summary>
    public class BussigoSceneBootstrap : MonoBehaviour
    {
        [Header("Root Systems & Services (MonoBehaviours)")]
        public EconomyManager economyManager;
        public CompanyManager companyManager;
        public SaveSystem saveSystem;

        [Header("World & Environment (MonoBehaviours)")]
        public TimeOfDayService timeOfDayService;
        public DynamicWeatherManager weatherManager;
        public RoadsideInfrastructureManager roadsideManager;

        [Header("Route & Highway Network")]
        public RoadSegmentStreamer roadStreamer;

        [Header("Traffic AI & Simulation (MonoBehaviours)")]
        public TrafficManager trafficManager;
        public TrafficSpawner trafficSpawner;

        [Header("Passenger Logistics (MonoBehaviours)")]
        public PassengerManager passengerManager;

        [Header("Audio System (MonoBehaviours)")]
        public BusAudioMixerController audioMixer;
        public MultiLayerEngineAudio engineAudio;
        public PneumaticAirSoundController airSounds;
        public TireRoadNoiseController tireSounds;

        [Header("Player Hero Bus (MonoBehaviours)")]
        public GameObject heroBusInstance;
        public BusChassisController chassisController;
        public BusModelRigHierarchy rigHierarchy;
        public BusWheelVisualSync wheelSync;
        public BusCockpitController cockpitController;
        public BusDoorActuator doorActuator;
        public BusCameraRig cameraRig;

        [Header("User Interface (MonoBehaviours)")]
        public TripHUDController tripHUD;

        // Pure C# Domain Services (Runtime non-serialized)
        [NonSerialized] public RouteGraph routeGraph;
        [NonSerialized] public RouteDistanceService distanceService;

        private void Awake()
        {
            Debug.Log("================================================================================");
            Debug.Log("          BUSSIGO V2 -- MASTER PLAYABLE SCENE BOOTSTRAP INITIALIZING            ");
            Debug.Log("================================================================================");

            // 1. Core Services
            EnsureCoreServices();

            // 2. Pure C# Route Graph & Distance Service
            EnsureRouteNetwork();

            // 3. World & Environment
            EnsureWorldEnvironment();

            // 4. Passenger Logistics & Traffic AI
            EnsureLogisticsAndTraffic();

            // 5. Player Hero Bus Rig & Physical Components
            EnsureHeroBus();

            // 6. Acoustic Audio & Simulator HUD
            EnsureAudioAndHUD();

            Debug.Log("[BUSSIGO Bootstrap] All V2 Subsystems Successfully Initialized & Connected!");
        }

        private void EnsureCoreServices()
        {
            if (economyManager == null) economyManager = FindOrCreate<EconomyManager>("[CORE_SERVICES]");
            if (companyManager == null) companyManager = FindOrCreate<CompanyManager>("[CORE_SERVICES]");
            if (saveSystem == null) saveSystem = FindOrCreate<SaveSystem>("[CORE_SERVICES]");

            economyManager.Initialize();
            companyManager.Initialize();
            saveSystem.Initialize();

            ServiceLocator.Register<EconomyManager>(economyManager);
            ServiceLocator.Register<CompanyManager>(companyManager);
            ServiceLocator.Register<SaveSystem>(saveSystem);
        }

        private void EnsureRouteNetwork()
        {
            // Pure C# data-driven route topology
            routeGraph = NH65HighwayNetworkBuilder.BuildCorridorGraph();

            // Pure C# distance computation service
            distanceService = new RouteDistanceService();
            distanceService.Initialize();
            distanceService.SetActiveGraph(routeGraph);

            // Unity Component road streamer
            if (roadStreamer == null) roadStreamer = FindOrCreate<RoadSegmentStreamer>("[ROAD_NETWORK]");
        }

        private void EnsureWorldEnvironment()
        {
            if (timeOfDayService == null) timeOfDayService = FindOrCreate<TimeOfDayService>("[WORLD_ENVIRONMENT]");
            if (weatherManager == null) weatherManager = FindOrCreate<DynamicWeatherManager>("[WORLD_ENVIRONMENT]");
            if (roadsideManager == null) roadsideManager = FindOrCreate<RoadsideInfrastructureManager>("[WORLD_ENVIRONMENT]");

            timeOfDayService.Initialize();
            weatherManager.Initialize();
            roadsideManager.InitializeCorridorInfrastructure();

            // Ensure 3D Highway Corridor, Terminals, and Roadside Scenery exist
            var worldGo = GameObject.Find("[WORLD_ENVIRONMENT]");
            if (worldGo != null && GameObject.Find("HighwayCorridor_NH65") == null)
            {
                var worldBuilder = worldGo.GetComponent<ProceduralSouthIndiaWorldBuilder>();
                if (worldBuilder == null) worldBuilder = worldGo.AddComponent<ProceduralSouthIndiaWorldBuilder>();
                worldBuilder.GenerateWorld(worldGo.transform);
            }
        }

        private void EnsureLogisticsAndTraffic()
        {
            if (passengerManager == null) passengerManager = FindOrCreate<PassengerManager>("[PASSENGER_SYSTEM]");
            passengerManager.Initialize();

            if (trafficManager == null) trafficManager = FindOrCreate<TrafficManager>("[TRAFFIC_SIMULATION]");
            if (trafficSpawner == null) trafficSpawner = FindOrCreate<TrafficSpawner>("[TRAFFIC_SIMULATION]");
            trafficManager.Initialize();
        }

        private void EnsureHeroBus()
        {
            if (heroBusInstance == null)
            {
                var existing = GameObject.Find("IndianIntercityCoach_12M_Hero_LOD0");
                if (existing != null)
                {
                    heroBusInstance = existing;
                }
                else
                {
                    // Create Hero Bus Root at Vijayawada PNBS Platform 4 (Lane 1)
                    heroBusInstance = new GameObject("IndianIntercityCoach_12M_Hero_LOD0");
                    heroBusInstance.transform.position = new Vector3(3.5f, 0.52f, 0f);
                    heroBusInstance.transform.rotation = Quaternion.identity;
                }
            }

            chassisController = GetOrAdd<BusChassisController>(heroBusInstance);
            rigHierarchy = GetOrAdd<BusModelRigHierarchy>(heroBusInstance);
            wheelSync = GetOrAdd<BusWheelVisualSync>(heroBusInstance);
            cockpitController = GetOrAdd<BusCockpitController>(heroBusInstance);
            doorActuator = GetOrAdd<BusDoorActuator>(heroBusInstance);
            cameraRig = GetOrAdd<BusCameraRig>(heroBusInstance);
            GetOrAdd<Bussigo.InputSystem.UnifiedInputManager>(heroBusInstance).targetBus = chassisController;

            // Ensure rig hierarchy transforms (camera mounts, wheels, steering wheel, glider door)
            EnsureRigHierarchyTransforms(heroBusInstance, rigHierarchy);

            // Ensure 3D Visual Mesh is attached if not already present
            if (heroBusInstance.GetComponentInChildren<MeshRenderer>() == null)
            {
                var meshBuilder = heroBusInstance.AddComponent<ProceduralBusMeshBuilder>();
                var visualModel = meshBuilder.BuildProceduralBus(heroBusInstance.transform.position, heroBusInstance.transform.rotation, BusCategoryType.SuperLuxuryRecliner);
                visualModel.transform.SetParent(heroBusInstance.transform, true);
                visualModel.name = "HeroBusVisualModel";
                var childRb = visualModel.GetComponent<Rigidbody>();
                if (childRb != null) Destroy(childRb);
                var childCtrl = visualModel.GetComponent<UnityBusController3D>();
                if (childCtrl != null) Destroy(childCtrl);
            }

            // Connect vehicle component references
            wheelSync.chassisController = chassisController;
            wheelSync.rigHierarchy = rigHierarchy;

            cockpitController.chassisController = chassisController;
            cockpitController.rigHierarchy = rigHierarchy;

            doorActuator.chassisController = chassisController;
            doorActuator.rigHierarchy = rigHierarchy;

            var mainCam = Camera.main ?? UnityEngine.Object.FindAnyObjectByType<Camera>();
            if (mainCam != null)
            {
                cameraRig.targetCamera = mainCam;
            }
            cameraRig.rigHierarchy = rigHierarchy;

            if (passengerManager != null)
            {
                passengerManager.playerBus = chassisController;
            }
        }

        private void EnsureRigHierarchyTransforms(GameObject busRoot, BusModelRigHierarchy rig)
        {
            Transform chassisGo = busRoot.transform.Find("Chassis");
            if (chassisGo == null)
            {
                chassisGo = new GameObject("Chassis").transform;
                chassisGo.SetParent(busRoot.transform, false);
            }
            rig.chassisRoot = chassisGo;

            Transform exteriorGo = busRoot.transform.Find("Exterior");
            if (exteriorGo == null)
            {
                exteriorGo = new GameObject("Exterior").transform;
                exteriorGo.SetParent(busRoot.transform, false);
            }
            rig.exteriorRoot = exteriorGo;

            Transform interiorGo = busRoot.transform.Find("Interior");
            if (interiorGo == null)
            {
                interiorGo = new GameObject("Interior").transform;
                interiorGo.SetParent(busRoot.transform, false);
            }
            rig.interiorRoot = interiorGo;

            Transform cockpitGo = interiorGo.Find("Cockpit");
            if (cockpitGo == null)
            {
                cockpitGo = new GameObject("Cockpit").transform;
                cockpitGo.SetParent(interiorGo, false);
            }
            rig.cockpitRoot = cockpitGo;

            Transform steerGo = cockpitGo.Find("SteeringWheel");
            if (steerGo == null)
            {
                steerGo = new GameObject("SteeringWheel").transform;
                steerGo.SetParent(cockpitGo, false);
                steerGo.localPosition = new Vector3(-0.60f, 1.65f, 5.15f);
            }
            rig.steeringWheelTransform = steerGo;

            Transform doorGo = exteriorGo.Find("FrontGliderDoor");
            if (doorGo == null)
            {
                doorGo = new GameObject("FrontGliderDoor").transform;
                doorGo.SetParent(exteriorGo, false);
                doorGo.localPosition = new Vector3(1.29f, 0.55f, 4.40f);
            }
            rig.frontGliderDoorTransform = doorGo;

            Transform wheelsRoot = chassisGo.Find("Wheels");
            if (wheelsRoot == null)
            {
                wheelsRoot = new GameObject("Wheels").transform;
                wheelsRoot.SetParent(chassisGo, false);
            }

            Transform FindOrCreateChild(Transform parent, string name, Vector3 localPos)
            {
                Transform child = parent.Find(name);
                if (child == null)
                {
                    child = new GameObject(name).transform;
                    child.SetParent(parent, false);
                    child.localPosition = localPos;
                }
                return child;
            }

            rig.wheelFrontLeft = FindOrCreateChild(wheelsRoot, "FrontLeft", new Vector3(-1.15f, 0.52f, 3.60f));
            rig.wheelFrontRight = FindOrCreateChild(wheelsRoot, "FrontRight", new Vector3(1.15f, 0.52f, 3.60f));
            rig.wheelRearLeftOuter = FindOrCreateChild(wheelsRoot, "RearLeftOuter", new Vector3(-1.22f, 0.52f, -3.20f));
            rig.wheelRearLeftInner = FindOrCreateChild(wheelsRoot, "RearLeftInner", new Vector3(-0.90f, 0.52f, -3.20f));
            rig.wheelRearRightInner = FindOrCreateChild(wheelsRoot, "RearRightInner", new Vector3(0.90f, 0.52f, -3.20f));
            rig.wheelRearRightOuter = FindOrCreateChild(wheelsRoot, "RearRightOuter", new Vector3(1.22f, 0.52f, -3.20f));

            Transform cameraMounts = busRoot.transform.Find("CameraMounts");
            if (cameraMounts == null)
            {
                cameraMounts = new GameObject("CameraMounts").transform;
                cameraMounts.SetParent(busRoot.transform, false);
            }

            Transform FindOrCreateMount(Transform parent, string name, Vector3 localPos, Quaternion localRot)
            {
                Transform mount = parent.Find(name);
                if (mount == null)
                {
                    mount = new GameObject(name).transform;
                    mount.SetParent(parent, false);
                    mount.localPosition = localPos;
                    mount.localRotation = localRot;
                }
                return mount;
            }

            rig.cameraMountChase = FindOrCreateMount(cameraMounts, "Mount_ExteriorChase", new Vector3(0f, 4.2f, -12.5f), Quaternion.Euler(14f, 0f, 0f));
            rig.cameraMountBumper = FindOrCreateMount(cameraMounts, "Mount_FrontBumper", new Vector3(0f, 0.85f, 6.45f), Quaternion.identity);
            rig.cameraMountCockpitDriverEye = FindOrCreateMount(cameraMounts, "Mount_DriverEye", new Vector3(-0.60f, 2.15f, 4.75f), Quaternion.identity);
            rig.cameraMountPassengerCabin = FindOrCreateMount(cameraMounts, "Mount_PassengerCabin", new Vector3(0f, 2.35f, 1.20f), Quaternion.identity);
        }

        private void EnsureAudioAndHUD()
        {
            if (audioMixer == null) audioMixer = FindOrCreate<BusAudioMixerController>("[AUDIO_SYSTEM]");
            if (engineAudio == null) engineAudio = FindOrCreate<MultiLayerEngineAudio>("[AUDIO_SYSTEM]");
            if (airSounds == null) airSounds = FindOrCreate<PneumaticAirSoundController>("[AUDIO_SYSTEM]");
            if (tireSounds == null) tireSounds = FindOrCreate<TireRoadNoiseController>("[AUDIO_SYSTEM]");

            if (tripHUD == null) tripHUD = FindOrCreate<TripHUDController>("[SIMULATOR_HUD]");

            // Connect HUD Telemetry Sources
            tripHUD.chassis = chassisController;
            tripHUD.timeService = timeOfDayService;
            tripHUD.weatherManager = weatherManager;
            tripHUD.passengerManager = passengerManager;
            tripHUD.economyManager = economyManager;
        }

        private T FindOrCreate<T>(string goName) where T : Component
        {
            T found = UnityEngine.Object.FindAnyObjectByType<T>();
            if (found != null) return found;

            GameObject go = GameObject.Find(goName);
            if (go == null) go = new GameObject(goName);
            return go.AddComponent<T>();
        }

        private T GetOrAdd<T>(GameObject go) where T : Component
        {
            T comp = go.GetComponent<T>();
            if (comp == null) comp = go.AddComponent<T>();
            return comp;
        }
    }
}
