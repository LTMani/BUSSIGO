using System;
using System.Collections.Generic;
using System.IO;
using System.Globalization;
using UnityEngine;

namespace Bussigo.Vehicle
{
    /// <summary>
    /// Multi-material runtime OBJ parser that instantiates distinct visual sub-assemblies
    /// (Exterior Body, Windshield, Wheels, Dashboard, 44 Passenger Seats) with accurate PBR materials and normals.
    /// </summary>
    public static class ObjMeshLoader
    {
        public class SubMeshData
        {
            public string groupName;
            public List<Vector3> vertices = new List<Vector3>();
            public List<Vector3> normals = new List<Vector3>();
            public List<Vector2> uvs = new List<Vector2>();
            public List<int> triangles = new List<int>();
        }

        public static GameObject LoadObjHierarchy(string filePath, Transform parent, Material bodyMat, Material glassMat, Material wheelMat, Material dashMat, Material seatMat)
        {
            if (!File.Exists(filePath))
            {
                Debug.LogWarning($"[ObjMeshLoader] File not found: {filePath}");
                return null;
            }

            var rawVertices = new List<Vector3>();
            var rawNormals = new List<Vector3>();
            var rawUvs = new List<Vector2>();

            var subMeshes = new Dictionary<string, SubMeshData>();
            SubMeshData currentSubMesh = null;

            void EnsureSubMesh(string name)
            {
                if (!subMeshes.TryGetValue(name, out currentSubMesh))
                {
                    currentSubMesh = new SubMeshData { groupName = name };
                    subMeshes[name] = currentSubMesh;
                }
            }

            EnsureSubMesh("Exterior_Body");

            string[] lines = File.ReadAllLines(filePath);
            foreach (string line in lines)
            {
                string l = line.Trim();
                if (l.Length == 0 || l.StartsWith("#")) continue;

                string[] tokens = l.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                if (tokens.Length < 2) continue;

                if (tokens[0] == "o" || tokens[0] == "g")
                {
                    string groupName = tokens[1];
                    EnsureSubMesh(groupName);
                }
                else if (tokens[0] == "v" && tokens.Length >= 4)
                {
                    float x = float.Parse(tokens[1], CultureInfo.InvariantCulture);
                    float y = float.Parse(tokens[2], CultureInfo.InvariantCulture);
                    float z = float.Parse(tokens[3], CultureInfo.InvariantCulture);
                    rawVertices.Add(new Vector3(x, y, z));
                }
                else if (tokens[0] == "vn" && tokens.Length >= 4)
                {
                    float x = float.Parse(tokens[1], CultureInfo.InvariantCulture);
                    float y = float.Parse(tokens[2], CultureInfo.InvariantCulture);
                    float z = float.Parse(tokens[3], CultureInfo.InvariantCulture);
                    rawNormals.Add(new Vector3(x, y, z));
                }
                else if (tokens[0] == "vt" && tokens.Length >= 3)
                {
                    float u = float.Parse(tokens[1], CultureInfo.InvariantCulture);
                    float v = float.Parse(tokens[2], CultureInfo.InvariantCulture);
                    rawUvs.Add(new Vector2(u, v));
                }
                else if (tokens[0] == "f" && tokens.Length >= 4)
                {
                    int firstIdx = AddFaceVertex(tokens[1], rawVertices, rawNormals, rawUvs, currentSubMesh);
                    int prevIdx = AddFaceVertex(tokens[2], rawVertices, rawNormals, rawUvs, currentSubMesh);

                    for (int i = 3; i < tokens.Length; i++)
                    {
                        int nextIdx = AddFaceVertex(tokens[i], rawVertices, rawNormals, rawUvs, currentSubMesh);
                        currentSubMesh.triangles.Add(firstIdx);
                        currentSubMesh.triangles.Add(prevIdx);
                        currentSubMesh.triangles.Add(nextIdx);
                        prevIdx = nextIdx;
                    }
                }
            }

            GameObject coachRoot = new GameObject("CoachModel_RiggedLOD0");
            coachRoot.transform.SetParent(parent, false);
            coachRoot.transform.localPosition = Vector3.zero;
            coachRoot.transform.localRotation = Quaternion.identity;

            foreach (var kvp in subMeshes)
            {
                SubMeshData sm = kvp.Value;
                if (sm.vertices.Count == 0 || sm.triangles.Count == 0) continue;

                Mesh mesh = new Mesh();
                mesh.name = sm.groupName;
                if (sm.vertices.Count > 65000)
                {
                    mesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;
                }
                mesh.SetVertices(sm.vertices);
                if (sm.normals.Count == sm.vertices.Count && sm.normals.Count > 0)
                {
                    mesh.SetNormals(sm.normals);
                }
                else
                {
                    mesh.RecalculateNormals();
                }

                if (sm.uvs.Count == sm.vertices.Count)
                {
                    mesh.SetUVs(0, sm.uvs);
                }
                mesh.SetTriangles(sm.triangles, 0);
                mesh.RecalculateBounds();

                GameObject subGo = new GameObject(sm.groupName);
                subGo.transform.SetParent(coachRoot.transform, false);
                subGo.transform.localPosition = Vector3.zero;
                subGo.transform.localRotation = Quaternion.identity;

                MeshFilter mf = subGo.AddComponent<MeshFilter>();
                mf.sharedMesh = mesh;

                MeshRenderer mr = subGo.AddComponent<MeshRenderer>();
                Material chosenMat = bodyMat;

                string gn = sm.groupName.ToLowerInvariant();
                if (gn.Contains("windshield") || gn.Contains("glass") || gn.Contains("window"))
                {
                    chosenMat = glassMat;
                }
                else if (gn.Contains("wheel") || gn.Contains("tire"))
                {
                    chosenMat = wheelMat;
                }
                else if (gn.Contains("dashboard") || gn.Contains("steering"))
                {
                    chosenMat = dashMat;
                }
                else if (gn.Contains("seat"))
                {
                    chosenMat = seatMat;
                }

                mr.sharedMaterial = chosenMat;
            }

            return coachRoot;
        }

        public static Mesh LoadObjFile(string filePath)
        {
            if (!File.Exists(filePath))
            {
                Debug.LogWarning($"[ObjMeshLoader] File not found: {filePath}");
                return null;
            }

            var vertices = new List<Vector3>();
            var normals = new List<Vector3>();
            var uvs = new List<Vector2>();

            var meshVertices = new List<Vector3>();
            var meshNormals = new List<Vector3>();
            var meshUvs = new List<Vector2>();
            var meshTriangles = new List<int>();

            string[] lines = File.ReadAllLines(filePath);
            foreach (string line in lines)
            {
                string l = line.Trim();
                if (l.Length == 0 || l.StartsWith("#")) continue;

                string[] tokens = l.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                if (tokens.Length < 2) continue;

                if (tokens[0] == "v" && tokens.Length >= 4)
                {
                    float x = float.Parse(tokens[1], CultureInfo.InvariantCulture);
                    float y = float.Parse(tokens[2], CultureInfo.InvariantCulture);
                    float z = float.Parse(tokens[3], CultureInfo.InvariantCulture);
                    vertices.Add(new Vector3(x, y, z));
                }
                else if (tokens[0] == "vn" && tokens.Length >= 4)
                {
                    float x = float.Parse(tokens[1], CultureInfo.InvariantCulture);
                    float y = float.Parse(tokens[2], CultureInfo.InvariantCulture);
                    float z = float.Parse(tokens[3], CultureInfo.InvariantCulture);
                    normals.Add(new Vector3(x, y, z));
                }
                else if (tokens[0] == "vt" && tokens.Length >= 3)
                {
                    float u = float.Parse(tokens[1], CultureInfo.InvariantCulture);
                    float v = float.Parse(tokens[2], CultureInfo.InvariantCulture);
                    uvs.Add(new Vector2(u, v));
                }
                else if (tokens[0] == "f" && tokens.Length >= 4)
                {
                    int firstIdx = AddFaceVertex(tokens[1], vertices, normals, uvs, meshVertices, meshNormals, meshUvs);
                    int prevIdx = AddFaceVertex(tokens[2], vertices, normals, uvs, meshVertices, meshNormals, meshUvs);

                    for (int i = 3; i < tokens.Length; i++)
                    {
                        int nextIdx = AddFaceVertex(tokens[i], vertices, normals, uvs, meshVertices, meshNormals, meshUvs);
                        meshTriangles.Add(firstIdx);
                        meshTriangles.Add(prevIdx);
                        meshTriangles.Add(nextIdx);
                        prevIdx = nextIdx;
                    }
                }
            }

            Mesh mesh = new Mesh();
            mesh.name = Path.GetFileNameWithoutExtension(filePath);
            if (meshVertices.Count > 65000)
            {
                mesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;
            }
            mesh.SetVertices(meshVertices);
            if (meshNormals.Count == meshVertices.Count && meshNormals.Count > 0)
            {
                mesh.SetNormals(meshNormals);
            }
            else
            {
                mesh.RecalculateNormals();
            }
            if (meshUvs.Count == meshVertices.Count)
            {
                mesh.SetUVs(0, meshUvs);
            }
            mesh.SetTriangles(meshTriangles, 0);
            mesh.RecalculateBounds();
            return mesh;
        }

        private static int AddFaceVertex(string token, List<Vector3> rawV, List<Vector3> rawN, List<Vector2> rawUv, SubMeshData sm)
        {
            string[] parts = token.Split('/');
            int vIdx = int.Parse(parts[0], CultureInfo.InvariantCulture) - 1;
            int vtIdx = (parts.Length > 1 && parts[1].Length > 0) ? int.Parse(parts[1], CultureInfo.InvariantCulture) - 1 : -1;
            int vnIdx = (parts.Length > 2 && parts[2].Length > 0) ? int.Parse(parts[2], CultureInfo.InvariantCulture) - 1 : -1;

            sm.vertices.Add(vIdx >= 0 && vIdx < rawV.Count ? rawV[vIdx] : Vector3.zero);
            if (vtIdx >= 0 && vtIdx < rawUv.Count) sm.uvs.Add(rawUv[vtIdx]);
            else sm.uvs.Add(Vector2.zero);

            if (vnIdx >= 0 && vnIdx < rawN.Count) sm.normals.Add(rawN[vnIdx]);
            else sm.normals.Add(Vector3.up);

            return sm.vertices.Count - 1;
        }

        private static int AddFaceVertex(string token, List<Vector3> vList, List<Vector3> vnList, List<Vector2> vtList,
            List<Vector3> outV, List<Vector3> outN, List<Vector2> outUv)
        {
            string[] parts = token.Split('/');
            int vIdx = int.Parse(parts[0], CultureInfo.InvariantCulture) - 1;
            int vtIdx = (parts.Length > 1 && parts[1].Length > 0) ? int.Parse(parts[1], CultureInfo.InvariantCulture) - 1 : -1;
            int vnIdx = (parts.Length > 2 && parts[2].Length > 0) ? int.Parse(parts[2], CultureInfo.InvariantCulture) - 1 : -1;

            outV.Add(vIdx >= 0 && vIdx < vList.Count ? vList[vIdx] : Vector3.zero);
            if (vtIdx >= 0 && vtIdx < vtList.Count) outUv.Add(vtList[vtIdx]);
            else outUv.Add(Vector2.zero);

            if (vnIdx >= 0 && vnIdx < vnList.Count) outN.Add(vnList[vnIdx]);
            else outN.Add(Vector3.up);

            return outV.Count - 1;
        }
    }
}
