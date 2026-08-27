using System;
using System.Collections.Generic;
using UnityEngine;
using Bussigo.Route;

namespace Bussigo.World
{
    /// <summary>
    /// Generates physical 3D highway corridor segments, asphalt pavement, lane markings, median barriers,
    /// streetlights, highway gantries, and terminal infrastructure for NH65 without primitive shortcuts.
    /// </summary>
    public static class HighwayRoadMeshGenerator
    {
        public static void GenerateCorridorGeometry(Transform parent, RouteGraph graph, RoadSegmentStreamer streamer)
        {
            if (parent == null || graph == null) return;

            // 1. PBR Road Materials
            Material asphaltMat = new Material(Shader.Find("Standard") ?? Shader.Find("Diffuse"));
            asphaltMat.name = "NH65_Asphalt_PBR";
            asphaltMat.color = new Color(0.20f, 0.20f, 0.22f);
            asphaltMat.SetFloat("_Glossiness", 0.30f);

            Material laneWhiteMat = new Material(Shader.Find("Standard") ?? Shader.Find("Diffuse"));
            laneWhiteMat.name = "NH65_LaneWhite_Mat";
            laneWhiteMat.color = new Color(0.96f, 0.96f, 0.94f);
            laneWhiteMat.SetFloat("_Glossiness", 0.40f);

            Material shoulderYellowMat = new Material(Shader.Find("Standard") ?? Shader.Find("Diffuse"));
            shoulderYellowMat.name = "NH65_ShoulderYellow_Mat";
            shoulderYellowMat.color = new Color(0.95f, 0.82f, 0.12f);

            Material medianMat = new Material(Shader.Find("Standard") ?? Shader.Find("Diffuse"));
            medianMat.name = "NH65_ConcreteMedian_Mat";
            medianMat.color = new Color(0.45f, 0.45f, 0.48f);
            medianMat.SetFloat("_Glossiness", 0.20f);

            Material platformMat = new Material(Shader.Find("Standard") ?? Shader.Find("Diffuse"));
            platformMat.name = "TerminalPlatform_Concrete_Mat";
            platformMat.color = new Color(0.62f, 0.60f, 0.58f);

            Material steelMat = new Material(Shader.Find("Standard") ?? Shader.Find("Diffuse"));
            steelMat.name = "GalvanizedSteel_Mat";
            steelMat.color = new Color(0.65f, 0.68f, 0.72f);
            steelMat.SetFloat("_Metallic", 0.85f);
            steelMat.SetFloat("_Glossiness", 0.60f);

            Material signBoardMat = new Material(Shader.Find("Standard") ?? Shader.Find("Diffuse"));
            signBoardMat.name = "NH65_SignBoard_Green";
            signBoardMat.color = new Color(0.06f, 0.42f, 0.18f); // Indian Highway Signboard Green

            // 2. Build Continuous Highway Segments (Streamable 100m chunks up to initial 3,000 meters)
            float totalCorridorDistance = 3000f;
            float chunkLength = 100f;
            float roadWidth = 16.0f; // 4 lanes (each 3.75m) + 0.5m shoulders

            Mesh roadSurfaceMesh = BuildExtrudedBoxMesh(roadWidth, 0.2f, chunkLength);
            Mesh medianBarrierMesh = BuildExtrudedBoxMesh(0.6f, 0.65f, chunkLength);
            Mesh guardRailMesh = BuildExtrudedBoxMesh(0.2f, 0.55f, chunkLength);
            Mesh dashedLineMesh = BuildExtrudedBoxMesh(0.20f, 0.02f, 4.5f);
            Mesh solidLineMesh = BuildExtrudedBoxMesh(0.20f, 0.02f, chunkLength);

            for (float z = -60f; z < totalCorridorDistance; z += chunkLength)
            {
                string chunkId = $"ROAD_CHUNK_{(int)z:D4}";
                GameObject chunkGo = new GameObject(chunkId);
                chunkGo.transform.SetParent(parent, false);
                chunkGo.transform.position = new Vector3(0f, 0f, z + chunkLength * 0.5f);

                // Asphalt Road Surface
                CreateMeshGameObject("AsphaltSurface", roadSurfaceMesh, asphaltMat, chunkGo.transform, new Vector3(0f, -0.1f, 0f), Quaternion.identity, true);

                // Concrete Median Divider Barrier (Center X = 0)
                CreateMeshGameObject("ConcreteMedianBarrier", medianBarrierMesh, medianMat, chunkGo.transform, new Vector3(0f, 0.32f, 0f), Quaternion.identity, true);

                // Left & Right Metal W-Beam Guardrails
                CreateMeshGameObject("GuardRail_L", guardRailMesh, steelMat, chunkGo.transform, new Vector3(-roadWidth * 0.5f - 0.15f, 0.35f, 0f), Quaternion.identity, true);
                CreateMeshGameObject("GuardRail_R", guardRailMesh, steelMat, chunkGo.transform, new Vector3(roadWidth * 0.5f + 0.15f, 0.35f, 0f), Quaternion.identity, true);

                // Solid Yellow Shoulder Lines (Left & Right)
                CreateMeshGameObject("ShoulderLine_L", solidLineMesh, shoulderYellowMat, chunkGo.transform, new Vector3(-7.5f, 0.015f, 0f), Quaternion.identity, false);
                CreateMeshGameObject("ShoulderLine_R", solidLineMesh, shoulderYellowMat, chunkGo.transform, new Vector3(7.5f, 0.015f, 0f), Quaternion.identity, false);

                // Dashed White Lane Lines (Lane 0/1 dividing lines at X = -3.75m and X = +3.75m)
                for (float lz = -chunkLength * 0.5f + 4f; lz < chunkLength * 0.5f; lz += 10f)
                {
                    CreateMeshGameObject("LaneDash_L", dashedLineMesh, laneWhiteMat, chunkGo.transform, new Vector3(-3.75f, 0.015f, lz), Quaternion.identity, false);
                    CreateMeshGameObject("LaneDash_R", dashedLineMesh, laneWhiteMat, chunkGo.transform, new Vector3(3.75f, 0.015f, lz), Quaternion.identity, false);
                }

                if (streamer != null)
                {
                    streamer.RegisterSegmentObject(chunkId, chunkGo);
                }
            }

            // 3. Grounded Vijayawada PNBS Platform Curb (Right side beside Lane 1, starts forward of bus door)
            Mesh curbPlatformMesh = BuildExtrudedBoxMesh(3.5f, 0.25f, 36.0f);
            GameObject platformGo = CreateMeshGameObject("PNBS_TerminalPlatform_Bay4", curbPlatformMesh, platformMat, parent, new Vector3(9.8f, 0.125f, 16.0f), Quaternion.identity, true);

            // Platform Shelter with proper vertical support columns (grounded, starts forward of bus door)
            BuildPlatformShelter(platformGo.transform, steelMat, medianMat);

            // 4. Highway Streetlights along right shoulder every 60m
            for (float sz = -30f; sz < totalCorridorDistance; sz += 60f)
            {
                CreateStreetLight(parent, new Vector3(9.2f, 0f, sz), steelMat);
                CreateStreetLight(parent, new Vector3(-9.2f, 0f, sz + 30f), steelMat);
            }

            // 5. Roadside Palm Trees & Greenery along embankment
            for (float tz = 25f; tz < totalCorridorDistance; tz += 50f)
            {
                CreatePalmTree(parent, new Vector3(-12.5f, 0f, tz));
                CreatePalmTree(parent, new Vector3(12.5f, 0f, tz + 25f));
            }

            // 6. Overhead NH65 Highway Gantries with Green Signboards (Placed safely forward at 160m and 800m)
            CreateOverheadGantry(parent, 160f, "NH 65: HYDERABAD 271 KM | SURYAPET 136 KM | KODAD 89 KM", steelMat, signBoardMat);
            CreateOverheadGantry(parent, 800f, "KANCHIKACHERLA FASTAG TOLL PLAZA 32 KM -- ALL LANES ELECTRONIC", steelMat, signBoardMat);
        }

        private static void BuildPlatformShelter(Transform platformParent, Material steelMat, Material roofMat)
        {
            GameObject shelter = new GameObject("PassengerWaitingShelter");
            shelter.transform.SetParent(platformParent, false);
            shelter.transform.localPosition = Vector3.zero;

            Mesh colMesh = BuildExtrudedBoxMesh(0.15f, 3.2f, 0.15f);
            Mesh roofMesh = BuildExtrudedBoxMesh(3.2f, 0.12f, 24.0f);

            // 4 Support Pillars
            CreateMeshGameObject("Pillar_1", colMesh, steelMat, shelter.transform, new Vector3(0f, 1.6f, -9f), Quaternion.identity, true);
            CreateMeshGameObject("Pillar_2", colMesh, steelMat, shelter.transform, new Vector3(0f, 1.6f, -3f), Quaternion.identity, true);
            CreateMeshGameObject("Pillar_3", colMesh, steelMat, shelter.transform, new Vector3(0f, 1.6f, 3f), Quaternion.identity, true);
            CreateMeshGameObject("Pillar_4", colMesh, steelMat, shelter.transform, new Vector3(0f, 1.6f, 9f), Quaternion.identity, true);

            // Grounded Curved Canopy Roof
            CreateMeshGameObject("ShelterRoof", roofMesh, roofMat, shelter.transform, new Vector3(0f, 3.25f, 0f), Quaternion.identity, false);
        }

        private static void CreateStreetLight(Transform parent, Vector3 basePos, Material steelMat)
        {
            GameObject lightPost = new GameObject("HighwayStreetLight");
            lightPost.transform.SetParent(parent, false);
            lightPost.transform.position = basePos;

            Mesh poleMesh = BuildExtrudedBoxMesh(0.18f, 7.2f, 0.18f);
            Mesh armMesh = BuildExtrudedBoxMesh(2.4f, 0.12f, 0.12f);
            Mesh luminaireMesh = BuildExtrudedBoxMesh(0.5f, 0.12f, 0.25f);

            // Vertical Pole
            CreateMeshGameObject("Pole", poleMesh, steelMat, lightPost.transform, new Vector3(0f, 3.6f, 0f), Quaternion.identity, true);

            // Overhanging Arm
            float armSign = basePos.x > 0 ? -1f : 1f;
            CreateMeshGameObject("Arm", armMesh, steelMat, lightPost.transform, new Vector3(armSign * 1.2f, 7.15f, 0f), Quaternion.identity, false);

            // Luminaire Lamp
            CreateMeshGameObject("Luminaire", luminaireMesh, steelMat, lightPost.transform, new Vector3(armSign * 2.2f, 7.05f, 0f), Quaternion.identity, false);
        }

        private static void CreateOverheadGantry(Transform parent, float zPos, string label, Material steelMat, Material signMat)
        {
            GameObject gantry = new GameObject($"Gantry_NH65_{zPos:F0}m");
            gantry.transform.SetParent(parent, false);
            gantry.transform.position = new Vector3(0f, 0f, zPos);

            Mesh colMesh = BuildExtrudedBoxMesh(0.5f, 7.5f, 0.5f);
            Mesh beamMesh = BuildExtrudedBoxMesh(18.5f, 0.6f, 0.6f);
            Mesh signMesh = BuildExtrudedBoxMesh(15.0f, 2.2f, 0.12f);

            // Left & Right Support Columns
            CreateMeshGameObject("Col_L", colMesh, steelMat, gantry.transform, new Vector3(-8.8f, 3.75f, 0f), Quaternion.identity, true);
            CreateMeshGameObject("Col_R", colMesh, steelMat, gantry.transform, new Vector3(8.8f, 3.75f, 0f), Quaternion.identity, true);

            // Cross Beam
            CreateMeshGameObject("CrossBeam", beamMesh, steelMat, gantry.transform, new Vector3(0f, 7.2f, 0f), Quaternion.identity, false);

            // Green Signboard
            CreateMeshGameObject("SignBoard", signMesh, signMat, gantry.transform, new Vector3(0f, 6.8f, 0f), Quaternion.identity, false);
        }

        private static void CreatePalmTree(Transform parent, Vector3 worldPos)
        {
            GameObject tree = new GameObject("RoadsidePalmTree");
            tree.transform.SetParent(parent, false);
            tree.transform.position = worldPos;

            Material trunkMat = new Material(Shader.Find("Standard") ?? Shader.Find("Diffuse"));
            trunkMat.color = new Color(0.38f, 0.28f, 0.18f);

            Material palmFrondMat = new Material(Shader.Find("Standard") ?? Shader.Find("Diffuse"));
            palmFrondMat.color = new Color(0.15f, 0.48f, 0.20f);
            palmFrondMat.SetFloat("_Glossiness", 0.40f);

            Mesh trunkMesh = BuildExtrudedBoxMesh(0.35f, 6.5f, 0.35f);
            Mesh frondMesh = BuildExtrudedBoxMesh(3.5f, 0.4f, 3.5f);

            CreateMeshGameObject("Trunk", trunkMesh, trunkMat, tree.transform, new Vector3(0f, 3.25f, 0f), Quaternion.identity, true);
            CreateMeshGameObject("Fronds", frondMesh, palmFrondMat, tree.transform, new Vector3(0f, 6.4f, 0f), Quaternion.identity, false);
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
