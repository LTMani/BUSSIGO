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

            // 4. Hero Bus Root (IndianIntercityCoach_12M_Hero_LOD0)
            GameObject heroBus = new GameObject("IndianIntercityCoach_12M_Hero_LOD0");
            heroBus.transform.position = new Vector3(3.5f, 0.52f, 0f);

            var chassis = heroBus.AddComponent<BusChassisController>();
            var physics = heroBus.AddComponent<HeavyVehiclePhysicsModel>();
            var wheelSync = heroBus.AddComponent<BusWheelVisualSync>();
            var cockpit = heroBus.AddComponent<BusCockpitController>();
            var door = heroBus.AddComponent<BusDoorActuator>();
            var cameraRig = heroBus.AddComponent<BusCameraRig>();

            chassis.physicsModel = physics;
            chassis.doorActuator = door;
            wheelSync.chassis = chassis;
            cockpit.chassis = chassis;
            cameraRig.targetBus = heroBus.transform;
            cameraRig.chassis = chassis;

            bootstrap.heroBusInstance = heroBus;
            bootstrap.chassisController = chassis;
            bootstrap.physicsModel = physics;
            bootstrap.wheelSync = wheelSync;
            bootstrap.cockpitController = cockpit;
            bootstrap.doorActuator = door;
            bootstrap.cameraRig = cameraRig;

            // 5. Save Scene
            EditorSceneManager.SaveScene(newScene, SCENE_PATH);
            Debug.Log("[BUSSIGO Scene Builder] SUCCESS! Saved playable scene to: " + SCENE_PATH);

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
