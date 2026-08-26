using System;
using System.Collections.Generic;
using UnityEngine;

namespace Bussigo.Game.Runtime3D.Traffic
{
    public class ProceduralTrafficMeshBuilder : MonoBehaviour
    {
        public static GameObject CreateTataTruck(Vector3 position, Quaternion rotation, List<Vector3> waypoints)
        {
            GameObject truck = new GameObject("Traffic_TataLorry");
            truck.transform.position = position;
            truck.transform.rotation = rotation;
            truck.tag = "TrafficAI";

            // Cabin
            GameObject cabin = GameObject.CreatePrimitive(PrimitiveType.Cube);
            cabin.transform.SetParent(truck.transform, false);
            cabin.transform.localScale = new Vector3(2.4f, 2.6f, 2.5f);
            cabin.transform.localPosition = new Vector3(0f, 1.6f, 2.2f);
            
            Material cabMat = new Material(Shader.Find("Standard"));
            cabMat.color = new Color(0.95f, 0.45f, 0.1f); // Indian Truck Orange
            cabin.GetComponent<Renderer>().material = cabMat;

            // Cargo Bed
            GameObject cargo = GameObject.CreatePrimitive(PrimitiveType.Cube);
            cargo.transform.SetParent(truck.transform, false);
            cargo.transform.localScale = new Vector3(2.5f, 2.8f, 6.5f);
            cargo.transform.localPosition = new Vector3(0f, 1.8f, -2.0f);

            Material cargoMat = new Material(Shader.Find("Standard"));
            cargoMat.color = new Color(0.2f, 0.55f, 0.75f); // Indian Truck Blue
            cargo.GetComponent<Renderer>().material = cargoMat;

            // AI Controller
            UnityTrafficVehicleAI ai = truck.AddComponent<UnityTrafficVehicleAI>();
            ai.vehicleType = UnityTrafficVehicleAI.VehicleType.TataHeavyTruck;
            ai.desiredSpeedKmh = UnityEngine.Random.Range(55f, 72f);
            ai.pathWaypoints = waypoints;

            return truck;
        }

        public static GameObject CreateAutoRickshaw(Vector3 position, Quaternion rotation, List<Vector3> waypoints)
        {
            GameObject rickshaw = new GameObject("Traffic_AutoRickshaw");
            rickshaw.transform.position = position;
            rickshaw.transform.rotation = rotation;
            rickshaw.tag = "TrafficAI";

            // Lower Body (Yellow)
            GameObject body = GameObject.CreatePrimitive(PrimitiveType.Cube);
            body.transform.SetParent(rickshaw.transform, false);
            body.transform.localScale = new Vector3(1.4f, 0.9f, 2.4f);
            body.transform.localPosition = new Vector3(0f, 0.6f, 0f);

            Material yelMat = new Material(Shader.Find("Standard"));
            yelMat.color = new Color(0.95f, 0.85f, 0.15f); // Auto Yellow
            body.GetComponent<Renderer>().material = yelMat;

            // Canvas Canopy (Black/Green)
            GameObject canopy = GameObject.CreatePrimitive(PrimitiveType.Cube);
            canopy.transform.SetParent(rickshaw.transform, false);
            canopy.transform.localScale = new Vector3(1.35f, 1.1f, 2.2f);
            canopy.transform.localPosition = new Vector3(0f, 1.5f, -0.1f);

            Material canMat = new Material(Shader.Find("Standard"));
            canMat.color = new Color(0.12f, 0.35f, 0.18f); // Auto Green
            canopy.GetComponent<Renderer>().material = canMat;

            // AI Controller
            UnityTrafficVehicleAI ai = rickshaw.AddComponent<UnityTrafficVehicleAI>();
            ai.vehicleType = UnityTrafficVehicleAI.VehicleType.AutoRickshaw;
            ai.desiredSpeedKmh = UnityEngine.Random.Range(45f, 58f);
            ai.pathWaypoints = waypoints;

            return rickshaw;
        }

        public static void SpawnHighwayTrafficFleet(Transform parent, List<Vector3> forwardWaypoints, List<Vector3> returnWaypoints, int countPerDirection = 10)
        {
            if (forwardWaypoints == null || forwardWaypoints.Count < 10) return;

            int step = forwardWaypoints.Count / countPerDirection;

            for (int i = 0; i < countPerDirection; i++)
            {
                int wpIdx = Mathf.Clamp(i * step + 5, 0, forwardWaypoints.Count - 1);
                Vector3 spawnPos = forwardWaypoints[wpIdx];
                Quaternion spawnRot = Quaternion.identity;

                GameObject vehicle;
                if (i % 3 == 0)
                {
                    vehicle = CreateTataTruck(spawnPos, spawnRot, forwardWaypoints);
                }
                else if (i % 3 == 1)
                {
                    vehicle = CreateAutoRickshaw(spawnPos, spawnRot, forwardWaypoints);
                }
                else
                {
                    vehicle = CreateTataTruck(spawnPos, spawnRot, forwardWaypoints);
                }

                vehicle.transform.SetParent(parent, true);
                var ai = vehicle.GetComponent<UnityTrafficVehicleAI>();
                ai.currentWaypointIndex = wpIdx;
            }
        }
    }
}
