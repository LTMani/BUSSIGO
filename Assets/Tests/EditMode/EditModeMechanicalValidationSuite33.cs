using System;
using Bussigo.Game.Core;
using Bussigo.Game.Vehicles;
using Bussigo.Game.VehiclePhysics;
using Bussigo.Game.Passengers;

namespace Bussigo.Tests.EditMode
{
    public static class EditModeMechanicalValidationSuite33
    {
        public static void RunAllTests()
        {
            TestHVACCoolingCapacity();
            TestGearboxSynchronizerTiming();
            TestParcelCargoPricing();
        }

        public static void TestHVACCoolingCapacity()
        {
            var hvac = new CabinAirConditioningThermalModel01();
            hvac.UpdateThermalCycle(40.0f, 45, 800f, 60.0f);
            if (hvac.CurrentCabinTemperatureCelsius > 42.0f)
                throw new Exception("Cabin temperature exceeded thermodynamic safety ceiling.");
        }

        public static void TestGearboxSynchronizerTiming()
        {
            var synchro = new GearboxSynchronizerMeshSolver01();
            float syncTime = synchro.CalculateSynchronizationTimeSec(0.85f, 120.0f);
            if (syncTime <= 0.01f || syncTime > 2.0f)
                throw new Exception("Synchronizer mesh time outside realistic shifting envelope.");
        }

        public static void TestParcelCargoPricing()
        {
            var calc = new ParcelCargoTariffCalculator01();
            float fare = calc.CalculateFreightFare(50.0f, 275.0f, true);
            if (fare < 150.0f)
                throw new Exception("Freight fare below minimum commercial docket tariff.");
        }
    }
}
