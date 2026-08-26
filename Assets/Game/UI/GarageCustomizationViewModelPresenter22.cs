using System;
using System.Collections.Generic;
using Bussigo.Game.Core;
using Bussigo.Game.Customization;

namespace Bussigo.Game.UI
{
    public class GarageCustomizationViewModelPresenter22
    {
        public string PresenterId => "GARAGE-PRESENTER-022";
        public float OrbitCameraYawDegrees { get; set; } = 45.0f;
        public float OrbitCameraPitchDegrees { get; set; } = 15.0f;
        public float OrbitCameraDistanceMeters { get; set; } = 14.5f;
        public bool IsHydraulicLiftRaised { get; set; } = false;

        public void RotateCamera(float deltaYaw, float deltaPitch)
        {
            OrbitCameraYawDegrees = CoreMath.NormalizeAngleDegrees(OrbitCameraYawDegrees + deltaYaw);
            OrbitCameraPitchDegrees = CoreMath.Clamp(OrbitCameraPitchDegrees + deltaPitch, -5.0f, 60.0f);
        }

        public void ToggleHydraulicLift()
        {
            IsHydraulicLiftRaised = !IsHydraulicLiftRaised;
        }
    }
}
