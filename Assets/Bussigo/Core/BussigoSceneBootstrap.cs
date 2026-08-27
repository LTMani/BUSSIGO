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

            // Connect vehicle component references
            wheelSync.chassisController = chassisController;
            wheelSync.rigHierarchy = rigHierarchy;

            cockpitController.chassisController = chassisController;
            cockpitController.rigHierarchy = rigHierarchy;

            doorActuator.chassisController = chassisController;
            doorActuator.rigHierarchy = rigHierarchy;

            cameraRig.targetCamera = Camera.main ?? UnityEngine.Object.FindAnyObjectByType<Camera>();
            cameraRig.rigHierarchy = rigHierarchy;

            if (passengerManager != null)
            {
                passengerManager.playerBus = chassisController;
            }
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
