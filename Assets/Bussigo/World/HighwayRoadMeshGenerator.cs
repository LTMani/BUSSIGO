using System;
using System.Collections.Generic;
using UnityEngine;
using Bussigo.Route;

namespace Bussigo.World
{
    /// <summary>
    /// Generates physical 3D highway corridor segments, asphalt pavement, lane markings, median barriers, and terminal bays for NH65.
    /// Uses dedicated procedural Mesh generation without Unity primitive shortcuts.
    /// </summary>
    public static class HighwayRoadMeshGenerator
    {
        public static void GenerateCorridorGeometry(Transform parent, RouteGraph graph, RoadSegmentStreamer streamer)
        {
            if (parent == null || graph == null) return;

            // Materials
            Material asphaltMat = new Material(Shader.Find("Standard") ?? Shader.Find("Diffuse"));
            asphaltMat.name = "NH65_Asphalt_PBR";
            asphaltMat.color = new Color(0.18f, 0.18f, 0.20f);
            asphaltMat.SetFloat("_Glossiness", 0.35f);

            Material laneMarkingMat = new Material(Shader.Find("Standard") ?? Shader.Find("Diffuse"));
            laneMarkingMat.name = "NH65_LaneMarking_Mat";
            laneMarkingMat.color = new Color(0.95f, 0.95f, 0.92f);

            Material medianMat = new Material(Shader.Find("Standard") ?? Shader.Find("Diffuse"));
            medianMat.name = "NH65_MedianConcrete_Mat";
            medianMat.color = new Color(0.40f, 0.40f, 0.42f);

            Material platformMat = new Material(Shader.Find("Standard") ?? Shader.Find("Diffuse"));
            platformMat.name = "TerminalPlatform_Mat";
            platformMat.color = new Color(0.60f, 0.58f, 0.55f);

            // 1. Build Origin Terminal Platform (Vijayawada PNBS Platform 4)
            Mesh platformMesh = BuildExtrudedBoxMesh(4.0f, 0.30f, 60.0f);
            GameObject terminalPlatform = CreateMeshGameObject("PNBS_TerminalPlatform_Bay4", platformMesh, platformMat, parent, new Vector3(9.5f, 0.15f, 0f), Quaternion.identity, true);

            // Terminal Platform Canopy Roof
            Mesh canopyMesh = BuildExtrudedBoxMesh(5.6f, 0.25f, 60.0f);
            CreateMeshGameObject("PNBS_PlatformCanopy_Roof", canopyMesh, medianMat, terminalPlatform.transform, new Vector3(0f, 4.8f, 0f), Quaternion.identity, false);

            // 2. Build Continuous Highway Segments (Streamable 100m chunks up to initial 3,000 meters)
            float totalCorridorDistance = 3000f;
            float chunkLength = 100f;
            float roadWidth = 16.0f; // 4 lanes (each 3.75m) + 0.5m shoulders

            Mesh roadSurfaceMesh = BuildExtrudedBoxMesh(roadWidth, 0.2f, chunkLength);
            Mesh medianBarrierMesh = BuildExtrudedBoxMesh(0.8f, 0.7f, chunkLength);
            Mesh guardRailMesh = BuildExtrudedBoxMesh(0.25f, 0.65f, chunkLength);
            Mesh laneDashMesh = BuildExtrudedBoxMesh(0.22f, 0.02f, 5.0f);

            for (float z = -60f; z < totalCorridorDistance; z += chunkLength)
            {
                string chunkId = $"ROAD_CHUNK_{(int)z:D4}";
                GameObject chunkGo = new GameObject(chunkId);
                chunkGo.transform.SetParent(parent, false);
                chunkGo.transform.position = new Vector3(0f, 0f, z + chunkLength * 0.5f);

                // Asphalt Road Surface
                CreateMeshGameObject("AsphaltSurface", roadSurfaceMesh, asphaltMat, chunkGo.transform, new Vector3(0f, -0.1f, 0f), Quaternion.identity, true);

                // Central Concrete Median Divider Barrier
                CreateMeshGameObject("ConcreteMedianBarrier", medianBarrierMesh, medianMat, chunkGo.transform, new Vector3(0f, 0.35f, 0f), Quaternion.identity, true);

                // Left & Right Metal Crash Barriers
                CreateMeshGameObject("GuardRail_L", guardRailMesh, medianMat, chunkGo.transform, new Vector3(-roadWidth * 0.5f - 0.2f, 0.4f, 0f), Quaternion.identity, true);
                CreateMeshGameObject("GuardRail_R", guardRailMesh, medianMat, chunkGo.transform, new Vector3(roadWidth * 0.5f + 0.2f, 0.4f, 0f), Quaternion.identity, true);

                // Dashed Lane Markings along Chunk
                for (float lz = -chunkLength * 0.5f + 5f; lz < chunkLength * 0.5f; lz += 12f)
                {
                    CreateMeshGameObject("LaneDash_L", laneDashMesh, laneMarkingMat, chunkGo.transform, new Vector3(-3.75f, 0.02f, lz), Quaternion.identity, false);
                    CreateMeshGameObject("LaneDash_R", laneDashMesh, laneMarkingMat, chunkGo.transform, new Vector3(3.75f, 0.02f, lz), Quaternion.identity, false);
                }

                if (streamer != null)
                {
                    streamer.RegisterSegmentObject(chunkId, chunkGo);
                }
            }

            // 3. Overhead NH65 Highway Gantry at 400m and 1200m
            CreateHighwayGantry(parent, new Vector3(0f, 0f, 400f), "WELCOME TO NH65 - HYDERABAD 271 KM");
            CreateHighwayGantry(parent, new Vector3(0f, 0f, 1200f), "KANCHIKACHERLA FASTAG TOLL PLAZA 800M");
        }

        private static GameObject CreateMeshGameObject(string name, Mesh mesh, Material mat, Transform parent, Vector3 localPos, Quaternion localRot, bool addCollider)
        {
            GameObject go = new GameObject(name);
            go.transform.SetParent(parent, false);
            go.transform.localPosition = localPos;
            go.transform.localRotation = localRot;

            MeshFilter mf = go.AddComponent<MeshFilter>();
            mf.sharedMesh = mesh;

            MeshRenderer mr = go.AddComponent<MeshRenderer>();
            mr.sharedMaterial = mat;

            if (addCollider)
            {
                BoxCollider bc = go.AddComponent<BoxCollider>();
                bc.size = mesh.bounds.size;
                bc.center = mesh.bounds.center;
            }

            return go;
        }

        private static void CreateHighwayGantry(Transform parent, Vector3 worldPos, string label)
        {
            GameObject gantry = new GameObject($"Gantry_{worldPos.z:F0}m");
            gantry.transform.SetParent(parent, false);
            gantry.transform.position = worldPos;

            Material steelMat = new Material(Shader.Find("Standard") ?? Shader.Find("Diffuse"));
            steelMat.color = new Color(0.35f, 0.38f, 0.42f);

            Material greenBoardMat = new Material(Shader.Find("Standard") ?? Shader.Find("Diffuse"));
            greenBoardMat.color = new Color(0.08f, 0.45f, 0.20f);

            Mesh columnMesh = BuildExtrudedBoxMesh(0.6f, 8.4f, 0.6f);
            Mesh beamMesh = BuildExtrudedBoxMesh(18.5f, 0.7f, 0.7f);
            Mesh signMesh = BuildExtrudedBoxMesh(14.0f, 2.4f, 0.15f);

            // Left & Right Columns
            CreateMeshGameObject("Column_L", columnMesh, steelMat, gantry.transform, new Vector3(-8.8f, 4.2f, 0f), Quaternion.identity, true);
            CreateMeshGameObject("Column_R", columnMesh, steelMat, gantry.transform, new Vector3(8.8f, 4.2f, 0f), Quaternion.identity, true);

            // Overhead Beam & Sign
            CreateMeshGameObject("OverheadBeam", beamMesh, steelMat, gantry.transform, new Vector3(0f, 8.2f, 0f), Quaternion.identity, false);
            CreateMeshGameObject("SignBoard", signMesh, greenBoardMat, gantry.transform, new Vector3(0f, 7.8f, 0f), Quaternion.identity, false);
        }

        private static Mesh BuildExtrudedBoxMesh(float width, float height, float length)
        {
            Mesh mesh = new Mesh();
            mesh.name = $"Box_{width}x{height}x{length}";

            float hw = width * 0.5f;
            float hh = height * 0.5f;
            float hl = length * 0.5f;

            Vector3[] p = new Vector3[]
            {
                new Vector3(-hw, -hh, -hl),
                new Vector3( hw, -hh, -hl),
                new Vector3( hw, -hh,  hl),
                new Vector3(-hw, -hh,  hl),
                new Vector3(-hw,  hh, -hl),
                new Vector3( hw,  hh, -hl),
                new Vector3( hw,  hh,  hl),
                new Vector3(-hw,  hh,  hl)
            };

            Vector3[] vertices = new Vector3[]
            {
                // Bottom
                p[0], p[1], p[2], p[3],
                // Top
                p[7], p[6], p[5], p[4],
                // Front (+Z)
                p[3], p[2], p[6], p[7],
                // Back (-Z)
                p[1], p[0], p[4], p[5],
                // Left (-X)
                p[0], p[3], p[7], p[4],
                // Right (+X)
                p[2], p[1], p[5], p[6]
            };

            Vector3[] normals = new Vector3[]
            {
                Vector3.down, Vector3.down, Vector3.down, Vector3.down,
                Vector3.up, Vector3.up, Vector3.up, Vector3.up,
                Vector3.forward, Vector3.forward, Vector3.forward, Vector3.forward,
                Vector3.back, Vector3.back, Vector3.back, Vector3.back,
                Vector3.left, Vector3.left, Vector3.left, Vector3.left,
                Vector3.right, Vector3.right, Vector3.right, Vector3.right
            };

            Vector2[] uvs = new Vector2[24];
            for (int i = 0; i < 6; i++)
            {
                uvs[i * 4 + 0] = new Vector2(0f, 0f);
                uvs[i * 4 + 1] = new Vector2(1f, 0f);
                uvs[i * 4 + 2] = new Vector2(1f, 1f);
                uvs[i * 4 + 3] = new Vector2(0f, 1f);
            }

            int[] triangles = new int[36];
            for (int i = 0; i < 6; i++)
            {
                int vi = i * 4;
                int ti = i * 6;
                triangles[ti + 0] = vi + 0;
                triangles[ti + 1] = vi + 2;
                triangles[ti + 2] = vi + 1;
                triangles[ti + 3] = vi + 0;
                triangles[ti + 4] = vi + 3;
                triangles[ti + 5] = vi + 2;
            }

            mesh.vertices = vertices;
            mesh.normals = normals;
            mesh.uv = uvs;
            mesh.triangles = triangles;
            mesh.RecalculateBounds();
            return mesh;
        }
    }
}
