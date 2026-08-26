using System;
using Bussigo.Game.Core;
using Bussigo.Game.Vehicles;
using Bussigo.Game.VehiclePhysics;
using Bussigo.Game.Traffic;

namespace Bussigo.Tests.PlayMode
{
    public static class PlayModeSimulationTest02
    {
        public static void ExecutePlaySimulation()
        {
            var spec = new VehicleChassisSpec();
            var rigidBody = new ChassisRigidBody(spec);

            for (int f = 0; f < 50; f++)
            {
                rigidBody.IntegratePhysics(12000f, 0f, 0f, 0f, 0f, 0f, 0.02f);
            }

            if (rigidBody.SpeedKmh <= 0.0f)
            {
                throw new Exception("Chassis should have gained forward speed during physics stepping.");
            }
        }
    }
}
