using System;
using Bussigo.Game.Core;

namespace Bussigo.Game.UI
{
    public class CockpitInteractiveInstrumentGaugeView07
    {
        public string GaugeTag => "COCKPIT-GAUGE-VDO-007";
        public float DialAngleMinDegrees { get; set; } = -135.0f;
        public float DialAngleMaxDegrees { get; set; } = 135.0f;
        public float DisplayValueMin { get; set; } = 0.0f;
        public float DisplayValueMax { get; set; } = 160.0f;
        public float CurrentSmoothedNeedleAngleDegrees { get; private set; } = -135.0f;

        public void UpdateNeedlePosition(float targetValue, float deltaTime)
        {
            float normValue = CoreMath.InverseLerp(DisplayValueMin, DisplayValueMax, targetValue);
            float targetAngle = CoreMath.Lerp(DialAngleMinDegrees, DialAngleMaxDegrees, normValue);
            CurrentSmoothedNeedleAngleDegrees = CoreMath.MoveTowards(CurrentSmoothedNeedleAngleDegrees, targetAngle, deltaTime * 450.0f);
        }
    }
}
