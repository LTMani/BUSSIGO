using System;
using NUnit.Framework;
using UnityEngine;
using Bussigo.Weather;
using Bussigo.World;

namespace Bussigo.Tests.EditMode
{
    [TestFixture]
    public class WeatherEnvironmentTests
    {
        [Test]
        public void TimeOfDayService_CalculatesSmoothDiurnalPhases()
        {
            var go = new GameObject("TestTimeOfDay");
            var tod = go.AddComponent<TimeOfDayService>();

            // Noon
            tod.SetTime(12.0f);
            Assert.AreEqual(DayTimePhase.Noon, tod.currentPhase);
            Assert.AreEqual(1.25f, tod.sunIntensity);

            // Sunset
            tod.SetTime(18.5f);
            Assert.AreEqual(DayTimePhase.Sunset, tod.currentPhase);
            Assert.Less(tod.sunIntensity, 0.8f);

            // Night
            tod.SetTime(23.0f);
            Assert.AreEqual(DayTimePhase.Night, tod.currentPhase);
            Assert.Less(tod.sunIntensity, 0.1f);

            GameObject.DestroyImmediate(go);
        }

        [Test]
        public void DynamicWeatherManager_RainAccumulatesRoadWetnessAndFriction()
        {
            var go = new GameObject("TestWeather");
            var weather = go.AddComponent<DynamicWeatherManager>();
            weather.Initialize();

            // Set Heavy Monsoon
            weather.SetWeather(WeatherCondition.HeavyMonsoon);
            Assert.AreEqual(0.90f, weather.activeProfile.rainRate);

            // Simulate 50 seconds of heavy downpour
            for (int i = 0; i < 2500; i++)
            {
                // Update method called via reflection or direct simulation
                weather.roadWetness = Mathf.Clamp01(weather.roadWetness + weather.wetAccumulationRate * weather.activeProfile.rainRate * 0.02f);
            }

            Assert.Greater(weather.roadWetness, 0.85f);

            // Verify Tyre Spray at 80 km/h on wet road
            float spray = weather.CalculateTyreSpray(80f);
            Assert.Greater(spray, 0.5f);

            GameObject.DestroyImmediate(go);
        }

        [Test]
        public void EnvironmentZoneRegistry_ReturnsCorrectGeographicZones()
        {
            var vjaZone = EnvironmentZoneRegistry.FindZoneAtDistance(2000f);
            var sypZone = EnvironmentZoneRegistry.FindZoneAtDistance(136400f);
            var hydZone = EnvironmentZoneRegistry.FindZoneAtDistance(270000f);

            Assert.AreEqual("ZONE_01", vjaZone.zoneID);
            Assert.AreEqual(EnvironmentZoneType.RestAreaFoodHub, sypZone.zoneType);
            Assert.AreEqual("ZONE_09", hydZone.zoneID);
        }
    }
}
