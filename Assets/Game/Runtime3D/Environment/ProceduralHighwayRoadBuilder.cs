using System;
using System.Collections.Generic;
using UnityEngine;

namespace Bussigo.Game.Runtime3D.Environment
{
    public class ProceduralHighwayRoadBuilder : MonoBehaviour
    {
        public float roadWidthMeters = 16f; // 4 lanes (each 3.75m + shoulders)
        public float segmentLengthMeters = 20f;
        public int totalSegments = 150; // ~3,000 meters playable road corridor

        public GameObject BuildHighwayCorridor(Transform parent, out List<Vector3> laneWaypointsForward, out List<Vector3> laneWaypointsReturn)
        {
            GameObject roadRoot = new GameObject("HighwayCorridor_NH65");
            roadRoot.transform.SetParent(parent, false);
            roadRoot.tag = "RoadWay";

            laneWaypointsForward = new List<Vector3>();
            laneWaypointsReturn = new List<Vector3>();

            Material asphaltMat = new Material(Shader.Find("Standard"));
            asphaltMat.color = new Color(0.18f, 0.18f, 0.20f);
            asphaltMat.SetFloat("_Glossiness", 0.35f);

            Material laneMarkingMat = new Material(Shader.Find("Standard"));
            laneMarkingMat.color = new Color(0.95f, 0.95f, 0.95f);

            Material medianMat = new Material(Shader.Find("Standard"));
            medianMat.color = new Color(0.35f, 0.35f, 0.38f);

            Vector3 currentPos = Vector3.zero;
            float currentHeadingDegrees = 0f;

            for (int s = 0; s < totalSegments; s++)
            {
                // Gentle realistic highway curvature
                float curveDelta = Mathf.Sin(s * 0.08f) * 1.2f;
                currentHeadingDegrees += curveDelta;
                Quaternion segmentRot = Quaternion.Euler(0f, currentHeadingDegrees, 0f);

                // 1. Road Surface Segment Mesh
                GameObject segmentObj = GameObject.CreatePrimitive(PrimitiveType.Cube);
                segmentObj.name = $"RoadSegment_{s:D3}";
                segmentObj.transform.SetParent(roadRoot.transform, false);
                segmentObj.transform.position = currentPos + segmentRot * new Vector3(0f, -0.1f, segmentLengthMeters * 0.5f);
                segmentObj.transform.rotation = segmentRot;
                segmentObj.transform.localScale = new Vector3(roadWidthMeters, 0.2f, segmentLengthMeters);
                segmentObj.GetComponent<Renderer>().material = asphaltMat;

                // 2. Central Concrete Median Divider
                GameObject medianObj = GameObject.CreatePrimitive(PrimitiveType.Cube);
                medianObj.name = $"Median_{s:D3}";
                medianObj.transform.SetParent(roadRoot.transform, false);
                medianObj.transform.position = currentPos + segmentRot * new Vector3(0f, 0.35f, segmentLengthMeters * 0.5f);
                medianObj.transform.rotation = segmentRot;
                medianObj.transform.localScale = new Vector3(0.8f, 0.7f, segmentLengthMeters);
                medianObj.GetComponent<Renderer>().material = medianMat;

                // 3. Lane Divider Dashed Lines
                if (s % 2 == 0)
                {
                    CreateDashedLine(roadRoot.transform, currentPos + segmentRot * new Vector3(-3.8f, 0.02f, segmentLengthMeters * 0.5f), segmentRot, laneMarkingMat);
                    CreateDashedLine(roadRoot.transform, currentPos + segmentRot * new Vector3(3.8f, 0.02f, segmentLengthMeters * 0.5f), segmentRot, laneMarkingMat);
                }

                // Record waypoints for traffic AI and GPS
                Vector3 forwardLanePoint = currentPos + segmentRot * new Vector3(-3.8f, 0.5f, segmentLengthMeters * 0.5f);
                Vector3 returnLanePoint = currentPos + segmentRot * new Vector3(3.8f, 0.5f, segmentLengthMeters * 0.5f);
                laneWaypointsForward.Add(forwardLanePoint);
                laneWaypointsReturn.Add(returnLanePoint);

                // Step forward
                currentPos += segmentRot * new Vector3(0f, 0f, segmentLengthMeters);
            }

            return roadRoot;
        }

        private void CreateDashedLine(Transform parent, Vector3 pos, Quaternion rot, Material mat)
        {
            GameObject lineObj = GameObject.CreatePrimitive(PrimitiveType.Cube);
            lineObj.transform.SetParent(parent, false);
            lineObj.transform.position = pos;
            lineObj.transform.rotation = rot;
            lineObj.transform.localScale = new Vector3(0.2f, 0.02f, segmentLengthMeters * 0.45f);
            DestroyImmediate(lineObj.GetComponent<BoxCollider>());
            lineObj.GetComponent<Renderer>().material = mat;
        }
    }
}
