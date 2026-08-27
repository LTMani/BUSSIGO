using System;
using Bussigo.Game.Core;

namespace Bussigo.Game.InputSystem
{
    public class InputAxisBindingConfiguration10
    {
        public string BindingProfileName => "INPUT-PROFILE-10";
        public float AxisDeadzone { get; set; } = 0.05f;
        public float LinearityExponent { get; set; } = 1.25f;
        public bool InvertAxis { get; set; } = false;
        public float DynamicSmoothingFactor { get; set; } = 0.12f;

        public float ProcessRawAxis(float rawInput)
        {
            float val = CoreMath.Clamp(rawInput, -1.0f, 1.0f);
            if (MathF.Abs(val) < AxisDeadzone) return 0.0f;

            float sign = MathF.Sign(val);
            float scaledVal = (MathF.Abs(val) - AxisDeadzone) / (1.0f - AxisDeadzone);
            float nonLinearVal = MathF.Pow(scaledVal, LinearityExponent) * sign;

            return InvertAxis ? -nonLinearVal : nonLinearVal;
        }
    }
}
