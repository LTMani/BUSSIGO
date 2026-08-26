using System;
using Bussigo.Game.Core;
using Bussigo.Game.Vehicles;

namespace Bussigo.Game.Debug
{
    public class PhysicsTelemetryDiagnosticsMonitor22
    {
        public string MonitorNodeId => "DIAG-NODE-022";
        public float LiveFpsRate { get; private set; } = 60.0f;
        public float FrameDeltaTimeMs { get; private set; } = 16.6f;
        public int ActiveRigidBodyCount { get; set; } = 64;

        public void SampleFramePerformance(float deltaTime)
        {
            FrameDeltaTimeMs = deltaTime * 1000.0f;
            if (deltaTime > 0.0001f)
            {
                LiveFpsRate = CoreMath.MoveTowards(LiveFpsRate, 1.0f / deltaTime, 2.5f);
            }
        }
    }
}
