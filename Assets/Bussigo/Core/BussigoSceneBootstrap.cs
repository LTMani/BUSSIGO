using System;
using System.IO;
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
using Bussigo.InputSystem;

namespace Bussigo.Core
{
    /// <summary>
    /// Master Scene Bootstrapper for BUSSIGO V2.
    /// Orchestrates service initialization, pure C# domain services, multi-material Hero Bus instantiation,
    /// highway network streaming, roadside infrastructure, atmospheric lighting, audio mixer, and simulator HUD on scene launch.
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

            // 3. World & Atmospheric Environment
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
            if (roadStreamer != null) roadStreamer.Initialize(routeGraph);
        }

        private void EnsureWorldEnvironment()
        {
            var worldGo = GameObject.Find("[WORLD_ENVIRONMENT]");
            if (worldGo == null) worldGo = new GameObject("[WORLD_ENVIRONMENT]");

            if (timeOfDayService == null) timeOfDayService = GetOrAdd<TimeOfDayService>(worldGo);
            if (weatherManager == null) weatherManager = GetOrAdd<DynamicWeatherManager>(worldGo);
            if (roadsideManager == null) roadsideManager = GetOrAdd<RoadsideInfrastructureManager>(worldGo);

            timeOfDayService.Initialize();
            weatherManager.Initialize();
            roadsideManager.InitializeCorridorInfrastructure();

            // Configure Lighting and Skybox
            ConfigureAtmosphericLighting();

            // Instantiate NH65 Highway Corridor, Lane Markings, Dividers, Guardrails, and PNBS Terminal Platform
            if (worldGo.transform.Find("PNBS_TerminalPlatform_Bay4") == null)
            {
                HighwayRoadMeshGenerator.GenerateCorridorGeometry(worldGo.transform, routeGraph, roadStreamer);
            }
        }

        private void ConfigureAtmosphericLighting()
        {
            // 1. Procedural Skybox
            Shader skyboxShader = Shader.Find("Skybox/Procedural");
            if (skyboxShader != null)
            {
                Material skyMat = new Material(skyboxShader);
                skyMat.SetFloat("_SunDisk", 2);
                skyMat.SetFloat("_SunSize", 0.04f);
                skyMat.SetFloat("_AtmosphereThickness", 1.05f);
                skyMat.SetColor("_SkyTint", new Color(0.48f, 0.65f, 0.85f));
                skyMat.SetColor("_GroundColor", new Color(0.35f, 0.32f, 0.28f));
                RenderSettings.skybox = skyMat;
            }

            // 2. Ambient Trilight
            RenderSettings.ambientMode = UnityEngine.Rendering.AmbientMode.Trilight;
            RenderSettings.ambientSkyColor = new Color(0.65f, 0.75f, 0.90f);
            RenderSettings.ambientEquatorColor = new Color(0.70f, 0.65f, 0.55f);
            RenderSettings.ambientGroundColor = new Color(0.25f, 0.22f, 0.18f);
            RenderSettings.ambientIntensity = 1.1f;

            // 3. Directional Sun
            Light sunLight = null;
            var sunGo = GameObject.Find("Directional Light (Sun)");
            if (sunGo != null) sunLight = sunGo.GetComponent<Light>();
            if (sunLight == null) sunLight = UnityEngine.Object.FindAnyObjectByType<Light>();

            if (sunLight != null)
            {
                sunLight.type = LightType.Directional;
                sunLight.transform.rotation = Quaternion.Euler(42f, -35f, 0f);
                sunLight.color = new Color(1.0f, 0.96f, 0.88f);
                sunLight.intensity = 1.25f;
                sunLight.shadows = LightShadows.Soft;
                sunLight.shadowStrength = 0.85f;
            }

            // 4. Fog
            RenderSettings.fog = true;
            RenderSettings.fogMode = FogMode.ExponentialSquared;
            RenderSettings.fogDensity = 0.0006f;
            RenderSettings.fogColor = new Color(0.68f, 0.76f, 0.86f);
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
                    // Create Hero Bus Root at Vijayawada PNBS Platform 4 (Lane 1, forward heading)
                    heroBusInstance = new GameObject("IndianIntercityCoach_12M_Hero_LOD0");
                    heroBusInstance.transform.position = new Vector3(3.75f, 0.52f, 0f);
                    heroBusInstance.transform.rotation = Quaternion.identity;
                }
            }

            chassisController = GetOrAdd<BusChassisController>(heroBusInstance);
            rigHierarchy = GetOrAdd<BusModelRigHierarchy>(heroBusInstance);
            wheelSync = GetOrAdd<BusWheelVisualSync>(heroBusInstance);
            cockpitController = GetOrAdd<BusCockpitController>(heroBusInstance);
            doorActuator = GetOrAdd<BusDoorActuator>(heroBusInstance);
            cameraRig = GetOrAdd<BusCameraRig>(heroBusInstance);
            GetOrAdd<UnifiedInputManager>(heroBusInstance).targetBus = chassisController;

            // Ensure rig hierarchy transforms (camera mounts, wheels, steering wheel, glider door)
            EnsureRigHierarchyTransforms(heroBusInstance, rigHierarchy);

            // Ensure 3D Visual Mesh Hierarchy is loaded and rendered on Hero Bus
            EnsureHeroBusVisualMesh(heroBusInstance, rigHierarchy);

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
                mainCam.cullingMask = ~0; // Render all layers
                mainCam.clearFlags = CameraClearFlags.Skybox;
                mainCam.nearClipPlane = 0.1f;
                mainCam.farClipPlane = 3500f;
                mainCam.fieldOfView = 54f;

                // Immediately snap camera to Mount_ExteriorChase
                if (rigHierarchy.cameraMountChase != null)
                {
                    mainCam.transform.position = rigHierarchy.cameraMountChase.position;
                    mainCam.transform.rotation = rigHierarchy.cameraMountChase.rotation;
                }
            }
            cameraRig.rigHierarchy = rigHierarchy;

            if (passengerManager != null)
            {
                passengerManager.playerBus = chassisController;
            }
        }

        private void EnsureHeroBusVisualMesh(GameObject busRoot, BusModelRigHierarchy rig)
        {
            Transform exterior = rig.exteriorRoot != null ? rig.exteriorRoot : busRoot.transform;
            if (exterior.Find("CoachModel_RiggedLOD0") == null && exterior.GetComponentInChildren<MeshRenderer>() == null)
            {
                // Create Dedicated PBR Materials for Coach Sub-assemblies
                Material bodyMat = new Material(Shader.Find("Standard") ?? Shader.Find("Diffuse"));
                bodyMat.name = "Coach_Livery_CrimsonRed_PBR";
                bodyMat.color = new Color(0.80f, 0.10f, 0.14f);
                bodyMat.SetFloat("_Glossiness", 0.88f);
                bodyMat.SetFloat("_Metallic", 0.35f);

                string texPath = Path.Combine(Application.dataPath, "Bussigo/Assets/Textures/Coach_Livery_Albedo_2K.png");
                if (File.Exists(texPath))
                {
                    byte[] texBytes = File.ReadAllBytes(texPath);
                    Texture2D albedoTex = new Texture2D(2048, 2048, TextureFormat.RGBA32, true);
                    if (albedoTex.LoadImage(texBytes))
                    {
                        bodyMat.mainTexture = albedoTex;
                        bodyMat.color = Color.white;
                    }
                }

                Material glassMat = new Material(Shader.Find("Standard") ?? Shader.Find("Diffuse"));
                glassMat.name = "Coach_TintedGlass_PBR";
                glassMat.color = new Color(0.08f, 0.12f, 0.18f, 0.85f);
                glassMat.SetFloat("_Glossiness", 0.96f);
                glassMat.SetFloat("_Metallic", 0.20f);

                Material skirtingMat = new Material(Shader.Find("Standard") ?? Shader.Find("Diffuse"));
                skirtingMat.name = "Coach_LowerSkirting_PBR";
                skirtingMat.color = new Color(0.15f, 0.15f, 0.16f);
                skirtingMat.SetFloat("_Glossiness", 0.40f);

                Material roofMat = new Material(Shader.Find("Standard") ?? Shader.Find("Diffuse"));
                roofMat.name = "Coach_RoofAC_PBR";
                roofMat.color = new Color(0.88f, 0.88f, 0.90f);
                roofMat.SetFloat("_Glossiness", 0.70f);

                Material wheelMat = new Material(Shader.Find("Standard") ?? Shader.Find("Diffuse"));
                wheelMat.name = "Coach_WheelRubberRim_PBR";
                wheelMat.color = new Color(0.12f, 0.12f, 0.12f);
                wheelMat.SetFloat("_Glossiness", 0.35f);

                // Load Complete Multi-Material OBJ Hierarchy
                string objPath = Path.Combine(Application.dataPath, "Bussigo/Assets/Models/Bus/IndianIntercityCoach_12M_Hero_LOD0.obj");
                if (!File.Exists(objPath))
                {
                    objPath = Path.Combine(Application.dataPath, "Bussigo/Assets/Models/Bus/IndianIntercityCoach_12M.obj");
                }

                ObjMeshLoader.LoadObjHierarchy(objPath, exterior, bodyMat, glassMat, skirtingMat, roofMat, wheelMat);

                // Front Projector Headlights
                CreateProjectorHeadlight(exterior, new Vector3(-0.95f, 0.85f, 6.25f));
                CreateProjectorHeadlight(exterior, new Vector3(0.95f, 0.85f, 6.25f));

                // LED Destination Display Board
                CreateDestinationBoard(exterior);

                // Wing Mirrors on A-Pillars
                CreateWingMirror(exterior, new Vector3(-1.42f, 2.2f, 5.85f), true);
                CreateWingMirror(exterior, new Vector3(1.42f, 2.2f, 5.85f), false);
            }
        }

        private void CreateProjectorHeadlight(Transform parent, Vector3 localPos)
        {
            GameObject lightGo = new GameObject("ProjectorHeadlight");
            lightGo.transform.SetParent(parent, false);
            lightGo.transform.localPosition = localPos;
            Light spotLight = lightGo.AddComponent<Light>();
            spotLight.type = LightType.Spot;
            spotLight.range = 90f;
            spotLight.spotAngle = 46f;
            spotLight.intensity = 2.8f;
            spotLight.color = new Color(1.0f, 0.96f, 0.88f);
        }

        private void CreateDestinationBoard(Transform parent)
        {
            GameObject boardGo = new GameObject("LED_DestinationBoard");
            boardGo.transform.SetParent(parent, false);
            boardGo.transform.localPosition = new Vector3(0f, 3.25f, 5.65f);
            boardGo.transform.localRotation = Quaternion.identity;

            MeshFilter mf = boardGo.AddComponent<MeshFilter>();
            MeshRenderer mr = boardGo.AddComponent<MeshRenderer>();

            Mesh boxMesh = new Mesh();
            boxMesh.name = "LED_Board_Mesh";
            float w = 1.6f, h = 0.26f;
            boxMesh.vertices = new Vector3[]
            {
                new Vector3(-w*0.5f, -h*0.5f, 0), new Vector3(w*0.5f, -h*0.5f, 0),
                new Vector3(w*0.5f, h*0.5f, 0), new Vector3(-w*0.5f, h*0.5f, 0)
            };
            boxMesh.triangles = new int[] { 0, 2, 1, 0, 3, 2 };
            boxMesh.normals = new Vector3[] { Vector3.forward, Vector3.forward, Vector3.forward, Vector3.forward };
            boxMesh.RecalculateBounds();
            mf.sharedMesh = boxMesh;

            Material ledMat = new Material(Shader.Find("Standard") ?? Shader.Find("Diffuse"));
            ledMat.name = "LED_Amber_Display_Mat";
            ledMat.color = new Color(1.0f, 0.65f, 0.05f);
            ledMat.EnableKeyword("_EMISSION");
            ledMat.SetColor("_EmissionColor", new Color(1.0f, 0.55f, 0.05f) * 1.5f);
            mr.sharedMaterial = ledMat;
        }

        private void CreateWingMirror(Transform parent, Vector3 localPos, bool isLeft)
        {
            GameObject mirrorGo = new GameObject(isLeft ? "WingMirror_Left" : "WingMirror_Right");
            mirrorGo.transform.SetParent(parent, false);
            mirrorGo.transform.localPosition = localPos;

            MeshFilter mf = mirrorGo.AddComponent<MeshFilter>();
            MeshRenderer mr = mirrorGo.AddComponent<MeshRenderer>();

            Mesh mirrorMesh = new Mesh();
            mirrorMesh.name = "MirrorMesh";
            float mw = 0.16f, mh = 0.38f, md = 0.08f;
            float hw = mw * 0.5f, hh = mh * 0.5f, hd = md * 0.5f;

            Vector3[] vertices = new Vector3[]
            {
                new Vector3(-hw, -hh, -hd), new Vector3(hw, -hh, -hd), new Vector3(hw, hh, -hd), new Vector3(-hw, hh, -hd),
                new Vector3(-hw, -hh, hd), new Vector3(hw, -hh, hd), new Vector3(hw, hh, hd), new Vector3(-hw, hh, hd)
            };
            mirrorMesh.vertices = vertices;
            mirrorMesh.triangles = new int[]
            {
                0, 2, 1, 0, 3, 2, // Front
                5, 6, 4, 6, 7, 4, // Back
                4, 7, 0, 7, 3, 0, // Left
                1, 2, 5, 2, 6, 5, // Right
                3, 7, 2, 7, 6, 2, // Top
                0, 1, 4, 1, 5, 4  // Bottom
            };
            mirrorMesh.RecalculateNormals();
            mirrorMesh.RecalculateBounds();
            mf.sharedMesh = mirrorMesh;

            Material mirrorMat = new Material(Shader.Find("Standard") ?? Shader.Find("Diffuse"));
            mirrorMat.color = new Color(0.18f, 0.18f, 0.18f);
            mirrorMat.SetFloat("_Glossiness", 0.90f);
            mirrorMat.SetFloat("_Metallic", 0.85f);
            mr.sharedMaterial = mirrorMat;
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

            rig.cameraMountChase = FindOrCreateMount(cameraMounts, "Mount_ExteriorChase", new Vector3(0f, 3.6f, -13.5f), Quaternion.Euler(10.5f, 0f, 0f));
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
