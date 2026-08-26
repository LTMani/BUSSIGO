using System;
using Bussigo.Game.Core;
using Bussigo.Game.Vehicles;
using Bussigo.Game.VehiclePhysics;

namespace Bussigo.Tests.EditMode
{
    public static class AutomatedSubsystemEditModeTest18
    {
        public static void RunVerification()
        {
            VerifyPacejkaFrictionCalculations();
            VerifyPneumaticReservoirFlow();
        }

        public static void VerifyPacejkaFrictionCalculations()
        {
            var tyre = new PacejkaTyreModel();
            float force = tyre.EvaluateMagicFormula(0.10f, 29000.0f, 1.0f);
            if (force <= 0.0f) throw new Exception("Tire friction force must be positive under positive slip.");
        }

        public static void VerifyPneumaticReservoirFlow()
        {
            var air = new PneumaticAirBrakeSystem();
            air.SetTreadleFootValve(0.85f);
            air.Update(0.05f, 1400f, true);
            float torque = air.CalculateBrakeTorqueNm(7500f, true);
            if (torque <= 0.0f) throw new Exception("Brake torque must be delivered upon treadle valve application.");
        }
    }
}
