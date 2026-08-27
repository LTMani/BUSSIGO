using System;
using System.Collections.Generic;
using System.IO;
using System.Globalization;
using UnityEngine;

namespace Bussigo.Vehicle
{
    /// <summary>
    /// Lightweight runtime OBJ file parser to load genuine 3D coach models directly into UnityEngine.Mesh.
    /// </summary>
    public static class ObjMeshLoader
    {
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
