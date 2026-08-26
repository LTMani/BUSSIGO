using System;
using System.Collections.Generic;
using UnityEngine;
using Bussigo.Game.Runtime3D.Vehicle;
using Bussigo.Game.Runtime3D.Environment;
using Bussigo.Game.Runtime3D.Traffic;
using Bussigo.Game.Runtime3D.Passengers;
using Bussigo.Game.Runtime3D.UI;

namespace Bussigo.Game.Runtime3D.Gameplay
{
    public class TripGameplayDirector : MonoBehaviour
    {
        [Header("Scene Builders")]
        public ProceduralSouthIndiaWorldBuilder worldBuilder;
        public ProceduralBusMeshBuilder busBuilder;
        
        [Header("Spawned Core Objects")]
        public GameObject playerBusObject;
        public UnityBusController3D busController;
        public UnityBusCameraSystem cameraSystem;
        public DrivingCockpitHUDController hudController;
        public TripSummaryModalController tripSummaryModal;
        public PassengerBoardingSystem3D passengerBoardingSystem;
        public MobileTouchInputController mobileTouchInput;
        public GPSMinimapRadarController minimapRadar;
        public MonsoonRainParticleController weatherController;

        [Header("Trip Metrics")]
        public float startFuelLiters = 280f;
        public float tollAmountPaid = 0f;
        public float tripStartTime = 0f;

        private void Start()
        {
            tripStartTime = Time.time;
            Initialize3DPlayableRoute();
        }

        public void Initialize3DPlayableRoute()
        {
            // 1. Build World & Environment
            GameObject worldRoot = new GameObject("WorldRoot_SouthIndia");
            worldBuilder = worldRoot.AddComponent<ProceduralSouthIndiaWorldBuilder>();
            worldBuilder.GenerateWorld(worldRoot.transform);

            // 2. Build 3D Player Bus at Vijayawada Origin Platform
            GameObject busBuilderObj = new GameObject("BusMeshBuilder");
            busBuilder = busBuilderObj.AddComponent<ProceduralBusMeshBuilder>();
            
            Vector3 busSpawnPos = new Vector3(-3.8f, 0.4f, 15f);
            playerBusObject = busBuilder.BuildProceduralBus(busSpawnPos, Quaternion.identity);
            busController = playerBusObject.GetComponent<UnityBusController3D>();

            // 3. Setup Camera System
            GameObject camObj = new GameObject("MainCamera3D");
            camObj.tag = "MainCamera";
            Camera cam = camObj.AddComponent<Camera>();
            cam.fieldOfView = 60f;
            cam.nearClipPlane = 0.3f;
            cam.farClipPlane = 1200f;
            camObj.AddComponent<AudioListener>();

            cameraSystem = camObj.AddComponent<UnityBusCameraSystem>();
            cameraSystem.targetBus = busController;

            // 4. Setup Passenger Boarding System
            GameObject paxSysObj = new GameObject("PassengerBoardingSystem");
            passengerBoardingSystem = paxSysObj.AddComponent<PassengerBoardingSystem3D>();
            passengerBoardingSystem.playerBus = busController;
            passengerBoardingSystem.originStation = worldBuilder.spawnedOriginStation.GetComponent<BusTerminalStation3D>();
            passengerBoardingSystem.destinationStation = worldBuilder.spawnedDestinationStation.GetComponent<BusTerminalStation3D>();
            passengerBoardingSystem.InitializeStationCrowd(worldBuilder.spawnedOriginStation.transform);

            // 5. Spawn 3D Highway Traffic Fleet
            ProceduralTrafficMeshBuilder.SpawnHighwayTrafficFleet(
                worldRoot.transform,
                worldBuilder.forwardLaneWaypoints,
                worldBuilder.returnLaneWaypoints,
                countPerDirection: 12
            );

            // 6. Setup Driving HUD, Minimap & Summary Modals
            GameObject uiRoot = new GameObject("HUD_CanvasRoot");
            hudController = uiRoot.AddComponent<DrivingCockpitHUDController>();
            hudController.busController = busController;
            hudController.passengerSystem = passengerBoardingSystem;

            tripSummaryModal = uiRoot.AddComponent<TripSummaryModalController>();

            mobileTouchInput = uiRoot.AddComponent<MobileTouchInputController>();
            mobileTouchInput.busController = busController;
            mobileTouchInput.cameraSystem = cameraSystem;

            minimapRadar = uiRoot.AddComponent<GPSMinimapRadarController>();
            minimapRadar.playerBus = busController;

            weatherController = uiRoot.AddComponent<MonsoonRainParticleController>();
            weatherController.playerBus = busController;

            // 7. Setup Directional Sunlight & Sky
            GameObject sunLightObj = new GameObject("DirectionalSunlight");
            Light sun = sunLightObj.AddComponent<Light>();
            sun.type = LightType.Directional;
            sun.intensity = 1.35f;
            sun.color = new Color(1.0f, 0.96f, 0.88f);
            sunLightObj.transform.rotation = Quaternion.Euler(42f, -30f, 0f);

            RenderSettings.ambientLight = new Color(0.45f, 0.50f, 0.60f);
            RenderSettings.fog = true;
            RenderSettings.fogColor = new Color(0.75f, 0.82f, 0.90f);
            RenderSettings.fogDensity = 0.0012f;

            Debug.Log("[BUSSIGO] 3D Playable Route Initialized: Vijayawada ➔ Hyderabad NH65 with Minimap & Audio Synthesis.");
        }

        private void Update()
        {
            if (worldBuilder == null || tripSummaryModal == null) return;

            // Check if destination alighting has completed
            var destStation = worldBuilder.spawnedDestinationStation.GetComponent<BusTerminalStation3D>();
            if (destStation != null && destStation.arePassengersDroppedOff && !tripSummaryModal.isTripSummaryVisible)
            {
                float fuelBurned = Mathf.Max(5f, startFuelLiters - (busController != null ? busController.currentFuelLiters : 240f));
                var toll = worldBuilder.spawnedTollPlaza.GetComponent<TollPlazaTrigger3D>();
                tollAmountPaid = (toll != null && toll.hasTollBeenPaid) ? toll.tollFeeRupees : 385f;

                tripSummaryModal.DisplayTripResult(
                    passengerCount: 45,
                    fuelBurnedLiters: fuelBurned,
                    tollPaid: tollAmountPaid,
                    comfortScore: 96.5f
                );
            }
        }
    }
}
