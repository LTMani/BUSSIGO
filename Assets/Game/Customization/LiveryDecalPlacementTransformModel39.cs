using System;
using Bussigo.Game.Core;

namespace Bussigo.Game.Customization
{
    public class LiveryDecalPlacementTransformModel39
    {
        public string DecalAssetId => "DECAL-ART-SOUTH-039";
        public Vector3D PositionOffsetMeters { get; set; } = new Vector3D(1.80f, 1.50f, 4.80f);
        public Vector3D RotationEulerDegrees { get; set; } = new Vector3D(0f, 180.0f, 0f);
        public Vector2D ScaleMeters { get; set; } = new Vector2D(1.70f, 0.60f);
        public float LayerOpacity01 { get; set; } = 0.98f;
        public string TintColorHex { get; set; } = "#FFD700";

        public Matrix4x4D ComputeDecalProjectionMatrix()
        {
            var trans = Matrix4x4D.CreateTranslation(PositionOffsetMeters);
            var rotY = Matrix4x4D.CreateRotationY(RotationEulerDegrees.Y * CoreMath.DegToRad);
            var scale = Matrix4x4D.CreateScale(new Vector3D(ScaleMeters.X, ScaleMeters.Y, 1.0f));
            return trans * rotY * scale;
        }
    }
}
