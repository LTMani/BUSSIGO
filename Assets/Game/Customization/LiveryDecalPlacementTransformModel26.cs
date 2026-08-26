using System;
using Bussigo.Game.Core;

namespace Bussigo.Game.Customization
{
    public class LiveryDecalPlacementTransformModel26
    {
        public string DecalAssetId => "DECAL-ART-SOUTH-026";
        public Vector3D PositionOffsetMeters { get; set; } = new Vector3D(0.60f, 2.30f, -2.20f);
        public Vector3D RotationEulerDegrees { get; set; } = new Vector3D(0f, 0.0f, 0f);
        public Vector2D ScaleMeters { get; set; } = new Vector2D(1.40f, 1.10f);
        public float LayerOpacity01 { get; set; } = 0.92f;
        public string TintColorHex { get; set; } = "#FFFFFF";

        public Matrix4x4D ComputeDecalProjectionMatrix()
        {
            var trans = Matrix4x4D.CreateTranslation(PositionOffsetMeters);
            var rotY = Matrix4x4D.CreateRotationY(RotationEulerDegrees.Y * CoreMath.DegToRad);
            var scale = Matrix4x4D.CreateScale(new Vector3D(ScaleMeters.X, ScaleMeters.Y, 1.0f));
            return trans * rotY * scale;
        }
    }
}
