#if UNITY_EDITOR
using System;
using System.IO;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using Bussigo.Core;
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

namespace Bussigo.Editor
{
    [InitializeOnLoad]
    public static class BUSSIGO_SceneBuilder
    {
        private const string SCENE_PATH = "Assets/Bussigo/Scenes/BUSSIGO_Main.unity";
        private const string HERO_MODEL_PATH = "Assets/Bussigo/Assets/Models/Bus/IndianIntercityCoach_12M_Hero_LOD0.obj";

        static BUSSIGO_SceneBuilder()
        {
            EditorApplication.delayCall += EnsureMainSceneRegistered;
        }

        [MenuItem("BUSSIGO/Scene/Generate and Open BUSSIGO_Main Scene")]
        public static void GenerateAndOpenMainScene()
        {
            Debug.Log("[BUSSIGO Scene Builder] Generating production playable scene: " + SCENE_PATH);

            string sceneDir = Path.GetDirectoryName(SCENE_PATH);
            if (!Directory.Exists(sceneDir))
            {
                Directory.CreateDirectory(sceneDir);
            }

            Scene newScene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);

            // 1. Sun Directional Light
            GameObject sunGo = new GameObject("Directional Light (Sun)");
            Light sunLight = sunGo.AddComponent<Light>();
            sunLight.type = LightType.Directional;
            sunLight.color = new Color(1.0f, 0.96f, 0.90f);
            sunLight.intensity = 1.35f;
            sunLight.shadows = LightShadows.Soft;
            sunGo.transform.rotation = Quaternion.Euler(50f, -30f, 0f);
            sunGo.transform.position = new Vector3(0f, 100f, 0f);

            // 2. Main Camera
            GameObject camGo = new GameObject("Main Camera");
            Camera cam = camGo.AddComponent<Camera>();
            cam.tag = "MainCamera";
            cam.fieldOfView = 52f;
            cam.nearClipPlane = 0.1f;
            cam.farClipPlane = 2000f;
            camGo.AddComponent<AudioListener>();
            camGo.transform.position = new Vector3(3.5f, 4.5f, -15.5f);
            camGo.transform.rotation = Quaternion.Euler(10f, 0f, 0f);

            // 3. Master Bootstrap
            GameObject bootstrapGo = new GameObject("[BUSSIGO_MASTER_BOOTSTRAP]");
            BussigoSceneBootstrap bootstrap = bootstrapGo.AddComponent<BussigoSceneBootstrap>();

            // 4. Core Services
            GameObject coreGo = new GameObject("[CORE_SERVICES]");
            var econMgr = coreGo.AddComponent<EconomyManager>();
            var compMgr = coreGo.AddComponent<CompanyManager>();
            var saveSys = coreGo.AddComponent<SaveSystem>();

            // 5. World Environment
            GameObject worldGo = new GameObject("[WORLD_ENVIRONMENT]");
            var timeSvc = worldGo.AddComponent<TimeOfDayService>();
            var weatherMgr = worldGo.AddComponent<DynamicWeatherManager>();
            var roadsideMgr = worldGo.AddComponent<RoadsideInfrastructureManager>();

            // 6. Road Network
            GameObject roadGo = new GameObject("[ROAD_NETWORK]");
            var routeGraph = roadGo.AddComponent<RouteGraph>();
            var distSvc = roadGo.AddComponent<RouteDistanceService>();
            var roadStreamer = roadGo.AddComponent<RoadSegmentStreamer>();

            // 7. Traffic Simulation
            GameObject trafficGo = new GameObject("[TRAFFIC_SIMULATION]");
            var trafficMgr = trafficGo.AddComponent<TrafficManager>();
            var trafficSpawn = trafficGo.AddComponent<TrafficSpawner>();

            // 8. Passenger System
            GameObject paxGo = new GameObject("[PASSENGER_SYSTEM]");
            var paxMgr = paxGo.AddComponent<PassengerManager>();
            var boardMgr = paxGo.AddComponent<BoardingManager>();

            // 9. Audio System
            GameObject audioGo = new GameObject("[AUDIO_SYSTEM]");
            var audioMixer = audioGo.AddComponent<BusAudioMixerController>();
            var engineAudio = audioGo.AddComponent<MultiLayerEngineAudio>();
            var airSounds = audioGo.AddComponent<PneumaticAirSoundController>();
            var tireSounds = audioGo.AddComponent<TireRoadNoiseController>();

            // 10. Hero Bus Root (IndianIntercityCoach_12M_Hero_LOD0)
            GameObject heroModelPrefab = AssetDatabase.LoadAssetAtPath<GameObject>(HERO_MODEL_PATH);
            GameObject heroBus = IndianCoachAssetFactory.CreateRiggedCoach(
                new Vector3(3.5f, 0.52f, 0f),
                Quaternion.identity,
                heroModelPrefab
            );
            heroBus.name = "IndianIntercityCoach_12M_Hero_LOD0";

            var chassis = heroBus.GetComponent<BusChassisController>();
            var physics = heroBus.GetComponent<HeavyVehiclePhysicsModel>();
            var wheelSync = heroBus.GetComponent<BusWheelVisualSync>();
            var cockpit = heroBus.GetComponent<BusCockpitController>();
            var door = heroBus.GetComponent<BusDoorActuator>();
            var cameraRig = heroBus.GetComponent<BusCameraRig>();

            // 11. Simulator HUD
            GameObject hudGo = new GameObject("[SIMULATOR_HUD]");
            var tripHUD = hudGo.AddComponent<TripHUDController>();
            tripHUD.chassis = chassis;
            tripHUD.timeService = timeSvc;
            tripHUD.weatherManager = weatherMgr;
            tripHUD.passengerManager = paxMgr;
            tripHUD.economyManager = econMgr;

            // Connect Bootstrap references
            bootstrap.economyManager = econMgr;
            bootstrap.companyManager = compMgr;
            bootstrap.saveSystem = saveSys;
            bootstrap.timeOfDayService = timeSvc;
            bootstrap.weatherManager = weatherMgr;
            bootstrap.roadsideManager = roadsideMgr;
            bootstrap.routeGraph = routeGraph;
            bootstrap.distanceService = distSvc;
            bootstrap.roadStreamer = roadStreamer;
            bootstrap.trafficManager = trafficMgr;
            bootstrap.trafficSpawner = trafficSpawn;
            bootstrap.passengerManager = paxMgr;
            bootstrap.boardingManager = boardMgr;
            bootstrap.audioMixer = audioMixer;
            bootstrap.engineAudio = engineAudio;
            bootstrap.airSounds = airSounds;
            bootstrap.tireSounds = tireSounds;
            bootstrap.heroBusInstance = heroBus;
            bootstrap.chassisController = chassis;
            bootstrap.physicsModel = physics;
            bootstrap.wheelSync = wheelSync;
            bootstrap.cockpitController = cockpit;
            bootstrap.doorActuator = door;
            bootstrap.cameraRig = cameraRig;
            bootstrap.tripHUD = tripHUD;

            // 12. Save Scene
            EditorSceneManager.SaveScene(newScene, SCENE_PATH);
            Debug.Log("[BUSSIGO Scene Builder] SUCCESS! Saved production playable scene to: " + SCENE_PATH);

            EnsureMainSceneRegistered();
        }

        public static void EnsureMainSceneRegistered()
        {
            var scenes = EditorBuildSettings.scenes;
            bool alreadyPresent = false;
            for (int i = 0; i < scenes.Length; i++)
            {
                if (scenes[i].path == SCENE_PATH)
                {
                    alreadyPresent = true;
                    break;
                }
            }

            if (!alreadyPresent)
            {
                var newScenes = new EditorBuildSettingsScene[scenes.Length + 1];
                newScenes[0] = new EditorBuildSettingsScene(SCENE_PATH, true);
                for (int i = 0; i < scenes.Length; i++)
                {
                    newScenes[i + 1] = scenes[i];
                }
                EditorBuildSettings.scenes = newScenes;
                Debug.Log("[BUSSIGO Scene Builder] Registered " + SCENE_PATH + " as Primary Build Scene (index 0).");
            }
        }
    }
}
#endif
