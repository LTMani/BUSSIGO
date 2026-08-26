using System;
using UnityEngine;

namespace Bussigo.Game.Runtime3D.Vehicle
{
    public class ProceduralBusMeshBuilder : MonoBehaviour
    {
        [Header("Bus Dimensions")]
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

        public GameObject BuildProceduralBus(Vector3 spawnPosition, Quaternion spawnRotation)
        {
            GameObject busObj = new GameObject("PlayerBus_SuperLuxury");
            busObj.transform.position = spawnPosition;
            busObj.transform.rotation = spawnRotation;
            busObj.tag = "BusPlayer";

            Rigidbody rb = busObj.AddComponent<Rigidbody>();
            rb.mass = 12500f;
            rb.linearDamping = 0.05f;
            rb.angularDamping = 1.2f;

            BoxCollider mainCollider = busObj.AddComponent<BoxCollider>();
            mainCollider.size = new Vector3(busWidth, busHeight * 0.85f, busLength);
            mainCollider.center = new Vector3(0f, busHeight * 0.55f, 0f);

            // 1. Bus Main Body Shell
            GameObject bodyObj = GameObject.CreatePrimitive(PrimitiveType.Cube);
            bodyObj.name = "ChassisBody";
            bodyObj.transform.SetParent(busObj.transform, false);
            bodyObj.transform.localScale = new Vector3(busWidth, busHeight * 0.75f, busLength);
            bodyObj.transform.localPosition = new Vector3(0f, busHeight * 0.60f, 0f);
            DestroyImmediate(bodyObj.GetComponent<BoxCollider>());

            Renderer bodyRen = bodyObj.GetComponent<Renderer>();
            Material bodyMat = new Material(Shader.Find("Standard"));
            bodyMat.color = primaryLiveryColor;
            bodyMat.SetFloat("_Glossiness", 0.85f);
            bodyMat.SetFloat("_Metallic", 0.35f);
            bodyRen.material = bodyMat;

            // 2. Windshield & Windows Strip
            GameObject glassStripObj = GameObject.CreatePrimitive(PrimitiveType.Cube);
            glassStripObj.name = "WindowGlassStrip";
            glassStripObj.transform.SetParent(busObj.transform, false);
            glassStripObj.transform.localScale = new Vector3(busWidth * 1.02f, busHeight * 0.32f, busLength * 0.92f);
            glassStripObj.transform.localPosition = new Vector3(0f, busHeight * 0.72f, 0.2f);
            DestroyImmediate(glassStripObj.GetComponent<BoxCollider>());

            Renderer glassRen = glassStripObj.GetComponent<Renderer>();
            Material glassMat = new Material(Shader.Find("Standard"));
            glassMat.color = windowGlassColor;
            glassMat.SetFloat("_Glossiness", 0.95f);
            glassRen.material = glassMat;

            // 3. LED Destination Board (Bilingual Telugu / English)
            GameObject ledBoardObj = GameObject.CreatePrimitive(PrimitiveType.Cube);
            ledBoardObj.name = "LED_DestinationBoard";
            ledBoardObj.transform.SetParent(busObj.transform, false);
            ledBoardObj.transform.localScale = new Vector3(busWidth * 0.8f, 0.45f, 0.15f);
            ledBoardObj.transform.localPosition = new Vector3(0f, busHeight * 0.95f, (busLength * 0.5f) + 0.05f);
            DestroyImmediate(ledBoardObj.GetComponent<BoxCollider>());

            Renderer ledRen = ledBoardObj.GetComponent<Renderer>();
            Material ledMat = new Material(Shader.Find("Standard"));
            ledMat.color = Color.black;
            ledMat.EnableKeyword("_EMISSION");
            ledMat.SetColor("_EmissionColor", new Color(1.0f, 0.7f, 0.1f) * 2.0f); // Amber LED Glow
            ledRen.material = ledMat;

            // 4. Glider Passenger Door
            GameObject doorObj = GameObject.CreatePrimitive(PrimitiveType.Cube);
            doorObj.name = "PassengerGliderDoor";
            doorObj.transform.SetParent(busObj.transform, false);
            doorObj.transform.localScale = new Vector3(0.12f, 2.2f, 1.1f);
            doorObj.transform.localPosition = new Vector3((busWidth * 0.5f) + 0.02f, 1.4f, (busLength * 0.38f));
            DestroyImmediate(doorObj.GetComponent<BoxCollider>());

            Renderer doorRen = doorObj.GetComponent<Renderer>();
            doorRen.material = bodyMat;

            // 5. Build 3D Wheels
            Material wheelRubberMat = new Material(Shader.Find("Standard"));
            wheelRubberMat.color = new Color(0.12f, 0.12f, 0.12f);
            wheelRubberMat.SetFloat("_Glossiness", 0.2f);

            Transform[] flWheels = new Transform[1];
            Transform[] frWheels = new Transform[1];
            Transform[] rlWheels = new Transform[2];
            Transform[] rrWheels = new Transform[2];

            flWheels[0] = CreateWheel(busObj.transform, "Wheel_FL", new Vector3(-trackWidth * 0.5f, wheelRadius, wheelbase * 0.5f), wheelRubberMat);
            frWheels[0] = CreateWheel(busObj.transform, "Wheel_FR", new Vector3(trackWidth * 0.5f, wheelRadius, wheelbase * 0.5f), wheelRubberMat);

            rlWheels[0] = CreateWheel(busObj.transform, "Wheel_RL1", new Vector3(-trackWidth * 0.5f, wheelRadius, -wheelbase * 0.5f), wheelRubberMat);
            rlWheels[1] = CreateWheel(busObj.transform, "Wheel_RL2", new Vector3(-trackWidth * 0.5f - 0.25f, wheelRadius, -wheelbase * 0.5f), wheelRubberMat);

            rrWheels[0] = CreateWheel(busObj.transform, "Wheel_RR1", new Vector3(trackWidth * 0.5f, wheelRadius, -wheelbase * 0.5f), wheelRubberMat);
            rrWheels[1] = CreateWheel(busObj.transform, "Wheel_RR2", new Vector3(trackWidth * 0.5f + 0.25f, wheelRadius, -wheelbase * 0.5f), wheelRubberMat);

            // 6. Lights
            GameObject hlLeft = CreateHeadlight(busObj.transform, "Headlight_L", new Vector3(-busWidth * 0.38f, 0.85f, (busLength * 0.5f) + 0.05f));
            GameObject hlRight = CreateHeadlight(busObj.transform, "Headlight_R", new Vector3(busWidth * 0.38f, 0.85f, (busLength * 0.5f) + 0.05f));

            GameObject tlLeft = CreateTailLight(busObj.transform, "TailLight_L", new Vector3(-busWidth * 0.40f, 1.1f, -(busLength * 0.5f) - 0.05f));
            GameObject tlRight = CreateTailLight(busObj.transform, "TailLight_R", new Vector3(busWidth * 0.40f, 1.1f, -(busLength * 0.5f) - 0.05f));

            // 7. Attach Bus Controller & Configure References
            UnityBusController3D controller = busObj.AddComponent<UnityBusController3D>();
            controller.frontLeftWheelTransforms = flWheels;
            controller.frontRightWheelTransforms = frWheels;
            controller.rearLeftWheelTransforms = rlWheels;
            controller.rearRightWheelTransforms = rrWheels;
            controller.headlightLowBeams = new Light[] { hlLeft.GetComponent<Light>(), hlRight.GetComponent<Light>() };
            controller.headlightHighBeams = new Light[] { hlLeft.GetComponent<Light>(), hlRight.GetComponent<Light>() };
            controller.brakeTailLights = new Light[] { tlLeft.GetComponent<Light>(), tlRight.GetComponent<Light>() };
            controller.passengerGliderDoorTransform = doorObj.transform;

            // 8. Attach Audio Controller
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

        private Transform CreateWheel(Transform parent, string name, Vector3 localPos, Material mat)
        {
            GameObject wheel = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            wheel.name = name;
            wheel.transform.SetParent(parent, false);
            wheel.transform.localPosition = localPos;
            wheel.transform.localRotation = Quaternion.Euler(0f, 0f, 90f);
            wheel.transform.localScale = new Vector3(wheelRadius * 2f, wheelWidth * 0.5f, wheelRadius * 2f);
            DestroyImmediate(wheel.GetComponent<CapsuleCollider>());

            Renderer ren = wheel.GetComponent<Renderer>();
            ren.material = mat;
            return wheel.transform;
        }

        private GameObject CreateHeadlight(Transform parent, string name, Vector3 localPos)
        {
            GameObject lightObj = new GameObject(name);
            lightObj.transform.SetParent(parent, false);
            lightObj.transform.localPosition = localPos;

            Light light = lightObj.AddComponent<Light>();
            light.type = LightType.Spot;
            light.range = 65f;
            light.spotAngle = 48f;
            light.intensity = 2.4f;
            light.color = new Color(1.0f, 0.95f, 0.85f);
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
            light.range = 8f;
            light.intensity = 1.8f;
            light.color = Color.red;
            light.enabled = false;
            return lightObj;
        }
    }
}
