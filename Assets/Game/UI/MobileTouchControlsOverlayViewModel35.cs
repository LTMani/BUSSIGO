using System;
using Bussigo.Game.Core;

namespace Bussigo.Game.UI
{
    public class MobileTouchControlsOverlayViewModel35
    {
        public string ControlLayoutProfile => "TOUCH-PROFILE-035";
        public float VirtualWheelRadiusPixels { get; set; } = 140.0f;
        public float TouchDeadzonePixels { get; set; } = 12.0f;
        public float ReturnToCenterSpringStiffness { get; set; } = 8.5f;

        public float ComputeSteeringAngle(float touchDeltaX, float deltaTime)
        {
            if (MathF.Abs(touchDeltaX) < TouchDeadzonePixels) return 0.0f;
            float rawAngle = touchDeltaX / VirtualWheelRadiusPixels;
            return CoreMath.Clamp(rawAngle, -1.0f, 1.0f);
        }
    }
}
