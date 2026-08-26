using System;
using System.Collections.Generic;
using Bussigo.Game.Core;

namespace Bussigo.Game.World
{
    public struct SplineVertexData
    {
        public Vector3D Position;
        public Vector3D Normal;
        public Vector2D UV;
    }

    public class HighwaySplineMeshBuilder30
    {
        public string SplineSegmentCode => "SPLINE-CORRIDOR-SEG-030";
        public float RoadWidthMeters { get; set; } = 21.0f;
        public float ShoulderWidthMeters { get; set; } = 2.5f;
        public float MedianBarrierWidthMeters { get; set; } = 1.8f;
        public int TessellationSubdivisions { get; set; } = 32;

        public List<SplineVertexData> GenerateSplineRibbon(Vector3D p0, Vector3D p1, Vector3D p2, Vector3D p3)
        {
            var vertices = new List<SplineVertexData>();
            float step = 1.0f / TessellationSubdivisions;

            for (int i = 0; i <= TessellationSubdivisions; i++)
            {
                float t = i * step;
                Vector3D centerPoint = SplineMath.EvaluateCatmullRom(p0, p1, p2, p3, t);
                Vector3D tangent = SplineMath.EvaluateCatmullRomTangent(p0, p1, p2, p3, t);
                Vector3D normal = Vector3D.Up;
                Vector3D binormal = Vector3D.Cross(tangent, normal).Normalized;

                // Left Edge, Center Left, Center Right, Right Edge
                Vector3D leftPt = centerPoint - (binormal * (RoadWidthMeters * 0.5f + ShoulderWidthMeters));
                Vector3D rightPt = centerPoint + (binormal * (RoadWidthMeters * 0.5f + ShoulderWidthMeters));

                vertices.Add(new SplineVertexData { Position = leftPt, Normal = normal, UV = new Vector2D(0.0f, t * 10.0f) });
                vertices.Add(new SplineVertexData { Position = rightPt, Normal = normal, UV = new Vector2D(1.0f, t * 10.0f) });
            }

            return vertices;
        }
    }
}
