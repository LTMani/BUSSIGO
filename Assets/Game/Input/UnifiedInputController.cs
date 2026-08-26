using System;
using Bussigo.Game.Core;

namespace Bussigo.Game.Input
{
    public class UnifiedInputState
    {
        public float SteeringAxis { get; set; } = 0.0f; // -1.0 (Left) to +1.0 (Right)
        public float ThrottleAxis { get; set; } = 0.0f; // 0.0 to 1.0
        public float BrakeAxis { get; set; } = 0.0f;    // 0.0 to 1.0
        public float ClutchAxis { get; set; } = 0.0f;   // 0.0 to 1.0
        public bool HandbrakeEngaged { get; set; } = false;

        public bool HornTriggered { get; set; } = false;
        public bool ToggleDoorTriggered { get; set; } = false;
        public bool ToggleHeadlightsTriggered { get; set; } = false;
        public bool ToggleWipersTriggered { get; set; } = false;
        public bool ShiftUpTriggered { get; set; } = false;
        public bool ShiftDownTriggered { get; set; } = false;
        public int RetarderStageDelta { get; set; } = 0;
    }

    public class UnifiedInputController
    {
        public UnifiedInputState CurrentState { get; } = new UnifiedInputState();
        public GamePlatformMode ActivePlatform { get; set; } = GamePlatformMode.PC;

        public void ProcessVirtualSteeringTouch(float normalizedTouchX)
        {
            CurrentState.SteeringAxis = CoreMath.Clamp(normalizedTouchX, -1.0f, 1.0f);
        }

        public void ProcessVirtualThrottleTouch(float throttle01)
        {
            CurrentState.ThrottleAxis = CoreMath.Clamp01(throttle01);
        }

        public void ProcessVirtualBrakeTouch(float brake01)
        {
            CurrentState.BrakeAxis = CoreMath.Clamp01(brake01);
        }
    }
}
