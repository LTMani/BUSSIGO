using System;
using System.Collections.Generic;
using UnityEngine;

namespace Bussigo.Game.Runtime3D.Environment
{
    public class ProceduralSouthIndiaWorldBuilder : MonoBehaviour
    {
        public ProceduralHighwayRoadBuilder roadBuilder;
        
        [Header("Spawned Route Assets")]
        public GameObject spawnedOriginStation;
        public GameObject spawnedTollPlaza;
        public GameObject spawnedDestinationStation;
        public List<Vector3> forwardLaneWaypoints;
        public List<Vector3> returnLaneWaypoints;

        public void GenerateWorld(Transform worldRoot)
        {
            if (roadBuilder == null)
            {
                GameObject rbObj = new GameObject("HighwayRoadBuilder");
                rbObj.transform.SetParent(worldRoot, false);
                roadBuilder = rbObj.AddComponent<ProceduralHighwayRoadBuilder>();
            }

            // 1. Build Highway Corridor
            roadBuilder.BuildHighwayCorridor(worldRoot, out forwardLaneWaypoints, out returnLaneWaypoints);

            // 2. Build Origin Terminal (Vijayawada PNBS)
            Vector3 originPos = new Vector3(-3.8f, 0f, 15f);
            spawnedOriginStation = BusTerminalStation3D.CreateTerminalStation(originPos, Quaternion.identity, "Vijayawada Pandit Nehru Bus Station (PNBS)", true);
            spawnedOriginStation.transform.SetParent(worldRoot, true);

            // 3. Build FASTag Toll Plaza (Kanchikacherla NH65)
            Vector3 tollPos = new Vector3(0f, 0f, 1200f);
            spawnedTollPlaza = TollPlazaTrigger3D.CreateTollPlaza(tollPos, Quaternion.identity);
            spawnedTollPlaza.transform.SetParent(worldRoot, true);

            // 4. Build Roadside 7-Hotel Food Court Highway Hub
            CreateRoadsideDhabaFoodCourt(worldRoot, new Vector3(-25f, 0f, 1800f));

            // 5. Build Destination Terminal (Hyderabad MGBS)
            Vector3 destPos = new Vector3(-3.8f, 0f, 2850f);
            spawnedDestinationStation = BusTerminalStation3D.CreateTerminalStation(destPos, Quaternion.identity, "Hyderabad Mahatma Gandhi Bus Station (MGBS)", false);
            spawnedDestinationStation.transform.SetParent(worldRoot, true);

            // 6. Build Roadside Scenery (Trees, Overhead Signs, Streetlights)
            BuildRoadsideDecorations(worldRoot);
        }

        private void CreateRoadsideDhabaFoodCourt(Transform parent, Vector3 pos)
        {
            GameObject dhabaObj = GameObject.CreatePrimitive(PrimitiveType.Cube);
            dhabaObj.name = "Suryapet_FoodCourt_Hub";
            dhabaObj.transform.SetParent(parent, false);
            dhabaObj.transform.position = pos + new Vector3(0f, 3.5f, 0f);
            dhabaObj.transform.localScale = new Vector3(25f, 7f, 40f);

            Material dhabaMat = new Material(Shader.Find("Standard"));
            dhabaMat.color = new Color(0.85f, 0.65f, 0.45f);
            dhabaObj.GetComponent<Renderer>().material = dhabaMat;
        }

        private void BuildRoadsideDecorations(Transform parent)
        {
            Material foliageMat = new Material(Shader.Find("Standard"));
            foliageMat.color = new Color(0.12f, 0.48f, 0.18f); // Tropical Palm Green

            Material trunkMat = new Material(Shader.Find("Standard"));
            trunkMat.color = new Color(0.35f, 0.25f, 0.15f);

            Material signMat = new Material(Shader.Find("Standard"));
            signMat.color = new Color(0.1f, 0.55f, 0.25f); // Indian Highway Green Board

            // Plant roadside trees along the corridor
            for (int z = 50; z < 2800; z += 65)
            {
                // Left Side Palm Tree
                CreatePalmTree(parent, new Vector3(-14f, 0f, z), trunkMat, foliageMat);
                // Right Side Palm Tree
                CreatePalmTree(parent, new Vector3(14f, 0f, z + 30), trunkMat, foliageMat);

                // Overhead Green Highway Direction Boards at key intervals
                if (z == 400 || z == 1600 || z == 2400)
                {
                    CreateOverheadHighwaySign(parent, new Vector3(0f, 0f, z), signMat);
                }
            }
        }

        private void CreatePalmTree(Transform parent, Vector3 pos, Material trunkMat, Material foliageMat)
        {
            GameObject treeObj = new GameObject("PalmTree");
            treeObj.transform.SetParent(parent, false);
            treeObj.transform.position = pos;

            // Trunk
            GameObject trunk = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            trunk.transform.SetParent(treeObj.transform, false);
            trunk.transform.localScale = new Vector3(0.5f, 4.5f, 0.5f);
            trunk.transform.localPosition = new Vector3(0f, 4.5f, 0f);
            trunk.GetComponent<Renderer>().material = trunkMat;

            // Foliage Canopy
            GameObject canopy = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            canopy.transform.SetParent(treeObj.transform, false);
            canopy.transform.localScale = new Vector3(4.5f, 2.5f, 4.5f);
            canopy.transform.localPosition = new Vector3(0f, 9.0f, 0f);
            canopy.GetComponent<Renderer>().material = foliageMat;
        }

        private void CreateOverheadHighwaySign(Transform parent, Vector3 pos, Material signMat)
        {
            GameObject signRoot = new GameObject("OverheadHighwaySign_NH65");
            signRoot.transform.SetParent(parent, false);
            signRoot.transform.position = pos;

            // Gantry Beam
            GameObject beam = GameObject.CreatePrimitive(PrimitiveType.Cube);
            beam.transform.SetParent(signRoot.transform, false);
            beam.transform.localScale = new Vector3(22f, 0.6f, 0.6f);
            beam.transform.localPosition = new Vector3(0f, 7.5f, 0f);

            // Sign Board
            GameObject board = GameObject.CreatePrimitive(PrimitiveType.Cube);
            board.transform.SetParent(signRoot.transform, false);
            board.transform.localScale = new Vector3(16f, 2.8f, 0.2f);
            board.transform.localPosition = new Vector3(0f, 7.5f, 0f);
            board.GetComponent<Renderer>().material = signMat;
        }
    }
}
