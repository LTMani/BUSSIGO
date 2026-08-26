using System;
using UnityEngine;

namespace Bussigo.Game.Runtime3D.Vehicle
{
    public enum BusCategoryType
    {
        PalleveluguRural,
        ExpressIntercity,
        SuperLuxuryRecliner,
        VennelaMultiAxleSleeper,
        AmaravatiMultiAxlePremium
    }

    public class ProceduralBusMeshBuilder : MonoBehaviour
    {
        [Header("Active Category Configuration")]
        public BusCategoryType busCategory = BusCategoryType.SuperLuxuryRecliner;

        [Header("Dimensions")]
        public float busLength = 12.5f;
        public float busWidth = 2.6f;
        public float busHeight = 3.6f;
        public float wheelRadius = 0.52f;
        public float wheelWidth = 0.32f;
        public float wheelbase = 6.2f;
        public float trackWidth = 2.05f;

        [Header("Materials / Colors")]
        public Color primaryLiveryColor = new Color(0.78f, 0.12f, 0.16f); // Crimson AP Heritage Red
        public Color secondaryLiveryColor = new Color(0.95f, 0.78f, 0.15f); // Royal Gold Trim
        public Color windowGlassColor = new Color(0.1f, 0.15f, 0.2f, 0.65f); // Tinted Glass
        public Color interiorUpholsteryColor = new Color(0.18f, 0.22f, 0.35f); // Deep Navy Velour
        public Color chromeTrimColor = new Color(0.9f, 0.9f, 0.92f);

        public GameObject BuildProceduralBus(Vector3 spawnPosition, Quaternion spawnRotation, BusCategoryType category = BusCategoryType.SuperLuxuryRecliner)
        {
            busCategory = category;
            ApplyCategorySpecs();

            string busName = $"PlayerBus_{busCategory}";
            GameObject busObj = new GameObject(busName);
            busObj.transform.position = spawnPosition;
            busObj.transform.rotation = spawnRotation;
            busObj.tag = "BusPlayer";

            Rigidbody rb = busObj.AddComponent<Rigidbody>();
            rb.mass = GetCategoryMass();
            rb.linearDamping = 0.05f;
            rb.angularDamping = 1.2f;

            BoxCollider mainCollider = busObj.AddComponent<BoxCollider>();
            mainCollider.size = new Vector3(busWidth, busHeight * 0.85f, busLength);
            mainCollider.center = new Vector3(0f, busHeight * 0.55f, 0f);

            // Create Master Materials
            Material bodyMat = new Material(Shader.Find("Standard"));
            bodyMat.color = primaryLiveryColor;
            bodyMat.SetFloat("_Glossiness", 0.88f);
            bodyMat.SetFloat("_Metallic", 0.35f);

            Material goldMat = new Material(Shader.Find("Standard"));
            goldMat.color = secondaryLiveryColor;
            goldMat.SetFloat("_Glossiness", 0.90f);
            goldMat.SetFloat("_Metallic", 0.60f);

            Material glassMat = new Material(Shader.Find("Standard"));
            glassMat.color = windowGlassColor;
            glassMat.SetFloat("_Glossiness", 0.95f);

            Material chromeMat = new Material(Shader.Find("Standard"));
            chromeMat.color = chromeTrimColor;
            chromeMat.SetFloat("_Glossiness", 0.98f);
            chromeMat.SetFloat("_Metallic", 0.95f);

            Material interiorMat = new Material(Shader.Find("Standard"));
            interiorMat.color = interiorUpholsteryColor;
            interiorMat.SetFloat("_Glossiness", 0.20f);

            // 1. Lower Chassis Skirt & Luggage Bay Section
            GameObject lowerSkirt = GameObject.CreatePrimitive(PrimitiveType.Cube);
            lowerSkirt.name = "LowerChassisSkirt";
            lowerSkirt.transform.SetParent(busObj.transform, false);
            lowerSkirt.transform.localScale = new Vector3(busWidth, 1.1f, busLength);
            lowerSkirt.transform.localPosition = new Vector3(0f, 0.95f, 0f);
            DestroyImmediate(lowerSkirt.GetComponent<BoxCollider>());
            lowerSkirt.GetComponent<Renderer>().material = bodyMat;

            // 2. Upper Passenger Cabin Structure
            GameObject cabinObj = GameObject.CreatePrimitive(PrimitiveType.Cube);
            cabinObj.name = "PassengerCabinUpper";
            cabinObj.transform.SetParent(busObj.transform, false);
            cabinObj.transform.localScale = new Vector3(busWidth * 0.98f, busHeight * 0.55f, busLength * 0.98f);
            cabinObj.transform.localPosition = new Vector3(0f, 2.35f, -0.05f);
            DestroyImmediate(cabinObj.GetComponent<BoxCollider>());
            cabinObj.GetComponent<Renderer>().material = bodyMat;

            // 3. Aerodynamic Front Fascia & Chrome Radiator Grille
            GameObject frontFascia = GameObject.CreatePrimitive(PrimitiveType.Cube);
            frontFascia.name = "AerodynamicFrontFascia";
            frontFascia.transform.SetParent(busObj.transform, false);
            frontFascia.transform.localScale = new Vector3(busWidth * 0.96f, 1.4f, 0.45f);
            frontFascia.transform.localPosition = new Vector3(0f, 1.2f, (busLength * 0.5f) + 0.15f);
            DestroyImmediate(frontFascia.GetComponent<BoxCollider>());
            frontFascia.GetComponent<Renderer>().material = bodyMat;

            GameObject chromeGrille = GameObject.CreatePrimitive(PrimitiveType.Cube);
            chromeGrille.name = "ChromeRadiatorGrille";
            chromeGrille.transform.SetParent(busObj.transform, false);
            chromeGrille.transform.localScale = new Vector3(busWidth * 0.65f, 0.55f, 0.12f);
            chromeGrille.transform.localPosition = new Vector3(0f, 0.85f, (busLength * 0.5f) + 0.38f);
            DestroyImmediate(chromeGrille.GetComponent<BoxCollider>());
            chromeGrille.GetComponent<Renderer>().material = chromeMat;

            // 4. Panoramic Dual-Pane Windshield & Wipers
            GameObject windshieldObj = GameObject.CreatePrimitive(PrimitiveType.Cube);
            windshieldObj.name = "PanoramicWindshield";
            windshieldObj.transform.SetParent(busObj.transform, false);
            windshieldObj.transform.localScale = new Vector3(busWidth * 0.92f, 1.35f, 0.18f);
            windshieldObj.transform.localPosition = new Vector3(0f, 2.45f, (busLength * 0.5f) - 0.05f);
            windshieldObj.transform.localRotation = Quaternion.Euler(14f, 0f, 0f);
            DestroyImmediate(windshieldObj.GetComponent<BoxCollider>());
            windshieldObj.GetComponent<Renderer>().material = glassMat;

            // Windshield Wipers
            GameObject wiperL = GameObject.CreatePrimitive(PrimitiveType.Cube);
            wiperL.name = "WindshieldWiper_L";
            wiperL.transform.SetParent(busObj.transform, false);
            wiperL.transform.localScale = new Vector3(0.04f, 0.65f, 0.04f);
            wiperL.transform.localPosition = new Vector3(-0.55f, 2.05f, (busLength * 0.5f) + 0.08f);
            wiperL.transform.localRotation = Quaternion.Euler(14f, 0f, -25f);
            DestroyImmediate(wiperL.GetComponent<BoxCollider>());
            wiperL.GetComponent<Renderer>().material = chromeMat;

            GameObject wiperR = GameObject.CreatePrimitive(PrimitiveType.Cube);
            wiperR.name = "WindshieldWiper_R";
            wiperR.transform.SetParent(busObj.transform, false);
            wiperR.transform.localScale = new Vector3(0.04f, 0.65f, 0.04f);
            wiperR.transform.localPosition = new Vector3(0.55f, 2.05f, (busLength * 0.5f) + 0.08f);
            wiperR.transform.localRotation = Quaternion.Euler(14f, 0f, -25f);
            DestroyImmediate(wiperR.GetComponent<BoxCollider>());
            wiperR.GetComponent<Renderer>().material = chromeMat;

            // 5. Side Tinted Windows Bay
            GameObject windowStripL = GameObject.CreatePrimitive(PrimitiveType.Cube);
            windowStripL.name = "SideWindows_Left";
            windowStripL.transform.SetParent(busObj.transform, false);
            windowStripL.transform.localScale = new Vector3(0.10f, 1.15f, busLength * 0.88f);
            windowStripL.transform.localPosition = new Vector3((-busWidth * 0.5f) - 0.01f, 2.35f, -0.2f);
            DestroyImmediate(windowStripL.GetComponent<BoxCollider>());
            windowStripL.GetComponent<Renderer>().material = glassMat;

            GameObject windowStripR = GameObject.CreatePrimitive(PrimitiveType.Cube);
            windowStripR.name = "SideWindows_Right";
            windowStripR.transform.SetParent(busObj.transform, false);
            windowStripR.transform.localScale = new Vector3(0.10f, 1.15f, busLength * 0.88f);
            windowStripR.transform.localPosition = new Vector3((busWidth * 0.5f) + 0.01f, 2.35f, -0.2f);
            DestroyImmediate(windowStripR.GetComponent<BoxCollider>());
            windowStripR.GetComponent<Renderer>().material = glassMat;

            // 6. LED Destination Marquee Board
            GameObject ledBoardObj = GameObject.CreatePrimitive(PrimitiveType.Cube);
            ledBoardObj.name = "LED_DestinationBoard";
            ledBoardObj.transform.SetParent(busObj.transform, false);
            ledBoardObj.transform.localScale = new Vector3(busWidth * 0.78f, 0.42f, 0.15f);
            ledBoardObj.transform.localPosition = new Vector3(0f, busHeight * 0.94f, (busLength * 0.5f) - 0.15f);
            DestroyImmediate(ledBoardObj.GetComponent<BoxCollider>());

            Renderer ledRen = ledBoardObj.GetComponent<Renderer>();
            Material ledMat = new Material(Shader.Find("Standard"));
            ledMat.color = Color.black;
            ledMat.EnableKeyword("_EMISSION");
            ledMat.SetColor("_EmissionColor", new Color(1.0f, 0.75f, 0.1f) * 2.2f);
            ledRen.material = ledMat;

            // 7. Glider Passenger Door
            GameObject doorObj = GameObject.CreatePrimitive(PrimitiveType.Cube);
            doorObj.name = "PassengerGliderDoor";
            doorObj.transform.SetParent(busObj.transform, false);
            doorObj.transform.localScale = new Vector3(0.12f, 2.15f, 1.1f);
            doorObj.transform.localPosition = new Vector3((busWidth * 0.5f) + 0.02f, 1.55f, (busLength * 0.38f));
            DestroyImmediate(doorObj.GetComponent<BoxCollider>());
            doorObj.GetComponent<Renderer>().material = bodyMat;

            // 8. Exterior Rear-view Wing Mirrors
            GameObject mirrorL = CreateWingMirror(busObj.transform, "WingMirror_Left", new Vector3((-busWidth * 0.5f) - 0.32f, 2.25f, (busLength * 0.5f) - 0.4f), chromeMat, glassMat);
            GameObject mirrorR = CreateWingMirror(busObj.transform, "WingMirror_Right", new Vector3((busWidth * 0.5f) + 0.32f, 2.25f, (busLength * 0.5f) - 0.4f), chromeMat, glassMat);

            // 9. 3D Driver Cockpit & Steering Column
            GameObject cockpitDashboard = GameObject.CreatePrimitive(PrimitiveType.Cube);
            cockpitDashboard.name = "CockpitDashboardBinnacle";
            cockpitDashboard.transform.SetParent(busObj.transform, false);
            cockpitDashboard.transform.localScale = new Vector3(busWidth * 0.85f, 0.75f, 1.1f);
            cockpitDashboard.transform.localPosition = new Vector3(0f, 1.65f, (busLength * 0.5f) - 1.2f);
            DestroyImmediate(cockpitDashboard.GetComponent<BoxCollider>());
            cockpitDashboard.GetComponent<Renderer>().material = interiorMat;

            // Contoured Steering Column & Wheel
            GameObject steeringColumn = new GameObject("SteeringColumn");
            steeringColumn.transform.SetParent(busObj.transform, false);
            steeringColumn.transform.localPosition = new Vector3(-0.6f, 1.75f, (busLength * 0.5f) - 1.55f);
            steeringColumn.transform.localRotation = Quaternion.Euler(28f, 0f, 0f);

            GameObject steeringWheelObj = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            steeringWheelObj.name = "SteeringWheel_Mesh";
            steeringWheelObj.transform.SetParent(steeringColumn.transform, false);
            steeringWheelObj.transform.localScale = new Vector3(0.52f, 0.04f, 0.52f);
            DestroyImmediate(steeringWheelObj.GetComponent<CapsuleCollider>());
            Material wheelInteriorMat = new Material(Shader.Find("Standard"));
            wheelInteriorMat.color = new Color(0.12f, 0.12f, 0.14f);
            steeringWheelObj.GetComponent<Renderer>().material = wheelInteriorMat;

            // 10. 3D Passenger Seats & Aisle Layout
            BuildPassengerCabinSeats(busObj.transform, interiorMat);

            // 11. Build 10-Lug Heavy Wheel Assemblies
            Material wheelRubberMat = new Material(Shader.Find("Standard"));
            wheelRubberMat.color = new Color(0.12f, 0.12f, 0.12f);
            wheelRubberMat.SetFloat("_Glossiness", 0.25f);

            bool isMultiAxle = (busCategory == BusCategoryType.VennelaMultiAxleSleeper || busCategory == BusCategoryType.AmaravatiMultiAxlePremium);
            int rearAxleCount = isMultiAxle ? 2 : 1;

            Transform[] flWheels = new Transform[1];
            Transform[] frWheels = new Transform[1];
            Transform[] rlWheels = new Transform[rearAxleCount * 2];
            Transform[] rrWheels = new Transform[rearAxleCount * 2];

            flWheels[0] = CreateDetailedWheel(busObj.transform, "Wheel_FL", new Vector3(-trackWidth * 0.5f, wheelRadius, wheelbase * 0.5f), wheelRubberMat, chromeMat);
            frWheels[0] = CreateDetailedWheel(busObj.transform, "Wheel_FR", new Vector3(trackWidth * 0.5f, wheelRadius, wheelbase * 0.5f), wheelRubberMat, chromeMat);

            rlWheels[0] = CreateDetailedWheel(busObj.transform, "Wheel_RL1", new Vector3(-trackWidth * 0.5f, wheelRadius, -wheelbase * 0.5f), wheelRubberMat, chromeMat);
            rlWheels[1] = CreateDetailedWheel(busObj.transform, "Wheel_RL1_Dual", new Vector3(-trackWidth * 0.5f - 0.28f, wheelRadius, -wheelbase * 0.5f), wheelRubberMat, chromeMat);

            rrWheels[0] = CreateDetailedWheel(busObj.transform, "Wheel_RR1", new Vector3(trackWidth * 0.5f, wheelRadius, -wheelbase * 0.5f), wheelRubberMat, chromeMat);
            rrWheels[1] = CreateDetailedWheel(busObj.transform, "Wheel_RR1_Dual", new Vector3(trackWidth * 0.5f + 0.28f, wheelRadius, -wheelbase * 0.5f), wheelRubberMat, chromeMat);

            if (isMultiAxle)
            {
                rlWheels[2] = CreateDetailedWheel(busObj.transform, "Wheel_RL2_Tag", new Vector3(-trackWidth * 0.5f, wheelRadius, (-wheelbase * 0.5f) - 1.35f), wheelRubberMat, chromeMat);
                rlWheels[3] = CreateDetailedWheel(busObj.transform, "Wheel_RL2_TagDual", new Vector3(-trackWidth * 0.5f - 0.28f, wheelRadius, (-wheelbase * 0.5f) - 1.35f), wheelRubberMat, chromeMat);

                rrWheels[2] = CreateDetailedWheel(busObj.transform, "Wheel_RR2_Tag", new Vector3(trackWidth * 0.5f, wheelRadius, (-wheelbase * 0.5f) - 1.35f), wheelRubberMat, chromeMat);
                rrWheels[3] = CreateDetailedWheel(busObj.transform, "Wheel_RR2_TagDual", new Vector3(trackWidth * 0.5f + 0.28f, wheelRadius, (-wheelbase * 0.5f) - 1.35f), wheelRubberMat, chromeMat);
            }

            // 12. Headlights & Taillights
            GameObject hlLeft = CreateHeadlight(busObj.transform, "Headlight_L", new Vector3(-busWidth * 0.38f, 0.85f, (busLength * 0.5f) + 0.25f));
            GameObject hlRight = CreateHeadlight(busObj.transform, "Headlight_R", new Vector3(busWidth * 0.38f, 0.85f, (busLength * 0.5f) + 0.25f));

            GameObject tlLeft = CreateTailLight(busObj.transform, "TailLight_L", new Vector3(-busWidth * 0.40f, 1.1f, -(busLength * 0.5f) - 0.05f));
            GameObject tlRight = CreateTailLight(busObj.transform, "TailLight_R", new Vector3(busWidth * 0.40f, 1.1f, -(busLength * 0.5f) - 0.05f));

            // 13. Attach Bus Controller & Configure References
            UnityBusController3D controller = busObj.AddComponent<UnityBusController3D>();
            controller.frontLeftWheelTransforms = flWheels;
            controller.frontRightWheelTransforms = frWheels;
            controller.rearLeftWheelTransforms = rlWheels;
            controller.rearRightWheelTransforms = rrWheels;
            controller.headlightLowBeams = new Light[] { hlLeft.GetComponent<Light>(), hlRight.GetComponent<Light>() };
            controller.headlightHighBeams = new Light[] { hlLeft.GetComponent<Light>(), hlRight.GetComponent<Light>() };
            controller.brakeTailLights = new Light[] { tlLeft.GetComponent<Light>(), tlRight.GetComponent<Light>() };
            controller.passengerGliderDoorTransform = doorObj.transform;

            // 14. Attach Cockpit Instrument Animator
            Cockpit3DInstrumentAnimator animator = busObj.AddComponent<Cockpit3DInstrumentAnimator>();
            animator.busController = controller;
            animator.steeringWheelTransform = steeringColumn.transform;

            // 15. Attach Audio Controller
            UnityBusAudioController audioCtrl = busObj.AddComponent<UnityBusAudioController>();
            audioCtrl.busController = controller;
            audioCtrl.engineAudioSource = busObj.AddComponent<AudioSource>();
            audioCtrl.engineAudioSource.spatialBlend = 0.8f;
            audioCtrl.engineAudioSource.loop = true;

            audioCtrl.airBrakePurgeAudioSource = busObj.AddComponent<AudioSource>();
            audioCtrl.airBrakePurgeAudioSource.spatialBlend = 0.9f;

            audioCtrl.airHornAudioSource = busObj.AddComponent<AudioSource>();
            audioCtrl.airHornAudioSource.spatialBlend = 0.9f;

            return busObj;
        }

        private void ApplyCategorySpecs()
        {
            switch (busCategory)
            {
                case BusCategoryType.PalleveluguRural:
                    busLength = 10.8f;
                    busWidth = 2.5f;
                    busHeight = 3.3f;
                    wheelbase = 5.4f;
                    primaryLiveryColor = new Color(0.18f, 0.52f, 0.22f); // Rural Green & Yellow
                    secondaryLiveryColor = new Color(0.95f, 0.82f, 0.15f);
                    break;
                case BusCategoryType.ExpressIntercity:
                    busLength = 11.5f;
                    busWidth = 2.55f;
                    busHeight = 3.4f;
                    wheelbase = 5.8f;
                    primaryLiveryColor = new Color(0.12f, 0.35f, 0.72f); // Deep Royal Blue
                    secondaryLiveryColor = Color.white;
                    break;
                case BusCategoryType.SuperLuxuryRecliner:
                    busLength = 12.5f;
                    busWidth = 2.60f;
                    busHeight = 3.6f;
                    wheelbase = 6.2f;
                    primaryLiveryColor = new Color(0.78f, 0.12f, 0.16f); // Crimson Red & Gold
                    secondaryLiveryColor = new Color(0.95f, 0.78f, 0.15f);
                    break;
                case BusCategoryType.VennelaMultiAxleSleeper:
                    busLength = 14.5f;
                    busWidth = 2.60f;
                    busHeight = 3.85f;
                    wheelbase = 7.1f;
                    primaryLiveryColor = new Color(0.32f, 0.12f, 0.48f); // Midnight Royal Violet
                    secondaryLiveryColor = new Color(0.92f, 0.85f, 0.65f);
                    break;
                case BusCategoryType.AmaravatiMultiAxlePremium:
                    busLength = 13.8f;
                    busWidth = 2.60f;
                    busHeight = 3.75f;
                    wheelbase = 6.8f;
                    primaryLiveryColor = new Color(0.08f, 0.62f, 0.65f); // Amaravati Emerald Teal
                    secondaryLiveryColor = Color.white;
                    break;
            }
        }

        private float GetCategoryMass()
        {
            switch (busCategory)
            {
                case BusCategoryType.PalleveluguRural: return 9800f;
                case BusCategoryType.ExpressIntercity: return 11200f;
                case BusCategoryType.SuperLuxuryRecliner: return 12800f;
                case BusCategoryType.VennelaMultiAxleSleeper: return 16500f;
                case BusCategoryType.AmaravatiMultiAxlePremium: return 15200f;
                default: return 12500f;
            }
        }

        private void BuildPassengerCabinSeats(Transform busParent, Material seatMat)
        {
            GameObject seatsGroup = new GameObject("PassengerCabinSeats");
            seatsGroup.transform.SetParent(busParent, false);

            float startZ = (busLength * 0.30f);
            float endZ = (-busLength * 0.40f);
            float stepZ = 1.05f;

            for (float z = startZ; z >= endZ; z -= stepZ)
            {
                // Left Double Seat
                CreateSeatPair(seatsGroup.transform, new Vector3(-0.75f, 1.45f, z), seatMat);
                // Right Double Seat
                CreateSeatPair(seatsGroup.transform, new Vector3(0.75f, 1.45f, z), seatMat);
            }
        }

        private void CreateSeatPair(Transform parent, Vector3 localPos, Material mat)
        {
            GameObject seatObj = GameObject.CreatePrimitive(PrimitiveType.Cube);
            seatObj.name = "SeatPair_2x2";
            seatObj.transform.SetParent(parent, false);
            seatObj.transform.localScale = new Vector3(0.85f, 0.85f, 0.55f);
            seatObj.transform.localPosition = localPos;
            DestroyImmediate(seatObj.GetComponent<BoxCollider>());
            seatObj.GetComponent<Renderer>().material = mat;
        }

        private GameObject CreateWingMirror(Transform parent, string name, Vector3 localPos, Material frameMat, Material glassMat)
        {
            GameObject mirrorGroup = new GameObject(name);
            mirrorGroup.transform.SetParent(parent, false);
            mirrorGroup.transform.localPosition = localPos;

            GameObject frame = GameObject.CreatePrimitive(PrimitiveType.Cube);
            frame.transform.SetParent(mirrorGroup.transform, false);
            frame.transform.localScale = new Vector3(0.12f, 0.45f, 0.22f);
            DestroyImmediate(frame.GetComponent<BoxCollider>());
            frame.GetComponent<Renderer>().material = frameMat;

            GameObject mirrorGlass = GameObject.CreatePrimitive(PrimitiveType.Cube);
            mirrorGlass.transform.SetParent(mirrorGroup.transform, false);
            mirrorGlass.transform.localScale = new Vector3(0.02f, 0.40f, 0.18f);
            mirrorGlass.transform.localPosition = new Vector3(0.06f, 0f, -0.01f);
            DestroyImmediate(mirrorGlass.GetComponent<BoxCollider>());
            mirrorGlass.GetComponent<Renderer>().material = glassMat;

            return mirrorGroup;
        }

        private Transform CreateDetailedWheel(Transform parent, string name, Vector3 localPos, Material rubberMat, Material rimMat)
        {
            GameObject wheelGroup = new GameObject(name);
            wheelGroup.transform.SetParent(parent, false);
            wheelGroup.transform.localPosition = localPos;

            // Rubber Tyre
            GameObject tyre = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            tyre.name = "RubberTyre";
            tyre.transform.SetParent(wheelGroup.transform, false);
            tyre.transform.localRotation = Quaternion.Euler(0f, 0f, 90f);
            tyre.transform.localScale = new Vector3(wheelRadius * 2f, wheelWidth * 0.5f, wheelRadius * 2f);
            DestroyImmediate(tyre.GetComponent<CapsuleCollider>());
            tyre.GetComponent<Renderer>().material = rubberMat;

            // Steel Hubcap / Rim
            GameObject hub = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            hub.name = "SteelHubcap";
            hub.transform.SetParent(wheelGroup.transform, false);
            hub.transform.localRotation = Quaternion.Euler(0f, 0f, 90f);
            hub.transform.localScale = new Vector3(wheelRadius * 1.1f, wheelWidth * 0.52f, wheelRadius * 1.1f);
            DestroyImmediate(hub.GetComponent<CapsuleCollider>());
            hub.GetComponent<Renderer>().material = rimMat;

            return wheelGroup.transform;
        }

        private GameObject CreateHeadlight(Transform parent, string name, Vector3 localPos)
        {
            GameObject lightObj = new GameObject(name);
            lightObj.transform.SetParent(parent, false);
            lightObj.transform.localPosition = localPos;

            Light light = lightObj.AddComponent<Light>();
            light.type = LightType.Spot;
            light.range = 75f;
            light.spotAngle = 45f;
            light.intensity = 2.8f;
            light.color = new Color(1.0f, 0.96f, 0.88f);
            light.enabled = true;
            return lightObj;
        }

        private GameObject CreateTailLight(Transform parent, string name, Vector3 localPos)
        {
            GameObject lightObj = new GameObject(name);
            lightObj.transform.SetParent(parent, false);
            lightObj.transform.localPosition = localPos;
            lightObj.transform.localRotation = Quaternion.Euler(0f, 180f, 0f);

            Light light = lightObj.AddComponent<Light>();
            light.type = LightType.Point;
            light.range = 10f;
            light.intensity = 2.0f;
            light.color = Color.red;
            light.enabled = false;
            return lightObj;
        }
    }
}
