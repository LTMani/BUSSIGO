using System;
using Bussigo.Game.Core;
using Bussigo.Game.Vehicles;
using Bussigo.Game.VehiclePhysics;
using Bussigo.Game.Routes;
using Bussigo.Game.Navigation;

namespace Bussigo.Tests.Integration
{
    public static class EndToEndTripIntegrationSimulation09
    {
        public static void RunTripSimulation()
        {
            var spec = new VehicleChassisSpec();
            var body = new ChassisRigidBody(spec);
            var nav = new TurnByTurnNavigation();
            var corridor = CorridorRegistry.VijayawadaToHyderabad;

            if (corridor.Waypoints.Count == 0)
                throw new Exception("Corridor waypoints not loaded.");

            // Simulate driving 100 meters
            body.IntegratePhysics(15000f, 0f, 0f, 0f, 0f, 0f, 0.5f);
            nav.UpdateGPS(body.Position, body.SpeedKmh);

            if (body.SpeedKmh <= 0.0f)
                throw new Exception("Bus failed to build speed during integration simulation.");
        }
    }
}
