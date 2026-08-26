using System;
using UnityEngine;

namespace Bussigo.Game.Runtime3D.Environment
{
    public class BusTerminalStation3D : MonoBehaviour
    {
        [Header("Terminal Identity")]
        public string terminalStationName = "Vijayawada Pandit Nehru Bus Station (PNBS)";
        public bool isOriginStation = true;
        public int platformBayNumber = 4;
        public int scheduledPassengerCount = 45;

        [Header("Docking State")]
        public bool isBusDockedInPlatform = false;
        public bool arePassengersBoarded = false;
        public bool arePassengersDroppedOff = false;

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("BusPlayer"))
            {
                isBusDockedInPlatform = true;
                Debug.Log($"[Station] Bus docked into Platform {platformBayNumber} at {terminalStationName}. Open doors (Key E) to board/drop-off passengers.");
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("BusPlayer"))
            {
                isBusDockedInPlatform = false;
                Debug.Log($"[Station] Bus departed Platform {platformBayNumber} at {terminalStationName}.");
            }
        }

        public static GameObject CreateTerminalStation(Vector3 position, Quaternion rotation, string stationName, bool isOrigin)
        {
            GameObject stationRoot = new GameObject(isOrigin ? "Station_Origin_PNBS" : "Station_Dest_MGBS");
            stationRoot.transform.position = position;
            stationRoot.transform.rotation = rotation;
            stationRoot.tag = isOrigin ? "BusStation" : "DestinationTrigger";

            // Platform Base
            GameObject platformObj = GameObject.CreatePrimitive(PrimitiveType.Cube);
            platformObj.name = "PlatformSlab";
            platformObj.transform.SetParent(stationRoot.transform, false);
            platformObj.transform.localScale = new Vector3(14f, 0.6f, 30f);
            platformObj.transform.localPosition = new Vector3(8.5f, 0.3f, 0f);

            Material platMat = new Material(Shader.Find("Standard"));
            platMat.color = new Color(0.45f, 0.45f, 0.45f);
            platformObj.GetComponent<Renderer>().material = platMat;

            // Canopy Roof
            GameObject roofObj = GameObject.CreatePrimitive(PrimitiveType.Cube);
            roofObj.name = "StationCanopy";
            roofObj.transform.SetParent(stationRoot.transform, false);
            roofObj.transform.localScale = new Vector3(18f, 0.4f, 32f);
            roofObj.transform.localPosition = new Vector3(8.5f, 5.8f, 0f);

            Material roofMat = new Material(Shader.Find("Standard"));
            roofMat.color = new Color(0.15f, 0.35f, 0.65f); // RTC Terminal Blue
            roofObj.GetComponent<Renderer>().material = roofMat;

            // Terminal Station Name Billboard Sign
            GameObject signObj = GameObject.CreatePrimitive(PrimitiveType.Cube);
            signObj.name = "StationNameBoard";
            signObj.transform.SetParent(stationRoot.transform, false);
            signObj.transform.localScale = new Vector3(12f, 1.8f, 0.3f);
            signObj.transform.localPosition = new Vector3(8.5f, 6.9f, 0f);

            Material signMat = new Material(Shader.Find("Standard"));
            signMat.color = new Color(0.85f, 0.15f, 0.15f);
            signObj.GetComponent<Renderer>().material = signMat;

            // Docking Platform Bay Trigger Box
            BoxCollider bayTrigger = stationRoot.AddComponent<BoxCollider>();
            bayTrigger.isTrigger = true;
            bayTrigger.size = new Vector3(6.5f, 4.5f, 18f);
            bayTrigger.center = new Vector3(0f, 2.2f, 0f);

            BusTerminalStation3D termCtrl = stationRoot.AddComponent<BusTerminalStation3D>();
            termCtrl.terminalStationName = stationName;
            termCtrl.isOriginStation = isOrigin;

            return stationRoot;
        }
    }
}
