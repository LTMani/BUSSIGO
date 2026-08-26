using System;
using Bussigo.Game.Core;
using Bussigo.Game.Vehicles;
using Bussigo.Game.VehiclePhysics;

namespace Bussigo.Tests.EditMode
{
    public static class VehiclePhysicsTests
    {
        public static void RunAllTests()
        {
            TestPacejkaTyreModel();
            TestAirBrakePneumatics();
            TestDieselTorqueCurve();
            TestChassisRigidBodyIntegration();
        }

        public static void TestPacejkaTyreModel()
        {
            var tyre = new PacejkaTyreModel();
            float force = tyre.EvaluateMagicFormula(0.12f, 25000f, 1.0f);
            if (force <= 0.0f) throw new Exception("Pacejka tyre force should be positive for positive slip.");
            if (force > 25000f * 1.5f) throw new Exception("Pacejka tyre force exceeded realistic friction limit.");
        }

        public static void TestAirBrakePneumatics()
        {
            var airBrakes = new PneumaticAirBrakeSystem();
            airBrakes.SetTreadleFootValve(1.0f);
            airBrakes.Update(0.1f, 1200f, true);

            float brakeTorque = airBrakes.CalculateBrakeTorqueNm(8000f, true);
            if (brakeTorque <= 0.0f) throw new Exception("Air brake torque should be non-zero when pedal applied.");
        }

        public static void TestDieselTorqueCurve()
        {
            var spec = new VehicleChassisSpec();
            var engine = new DieselPowertrain(spec);
            engine.StartEngine();

            float torque = engine.EvaluateTorqueCurve(1400f, 1.0f);
            if (torque < spec.MaxTorqueNm * 0.8f) throw new Exception("Torque at peak plateau should be near maximum.");
        }

        public static void TestChassisRigidBodyIntegration()
        {
            var spec = new VehicleChassisSpec();
            var body = new ChassisRigidBody(spec);
            body.IntegratePhysics(15000f, 0f, 0f, 0f, 0f, 0f, 0.02f);

            if (body.ForwardSpeedMps <= 0.0f) throw new Exception("Chassis should accelerate forward under positive drive force.");
        }
    }
}
