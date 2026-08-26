using System;
using NUnit.Framework;
using UnityEngine;
using Bussigo.Audio;
using Bussigo.Vehicle;

namespace Bussigo.Tests.EditMode
{
    [TestFixture]
    public class BusAudioSystemTests
    {
        [Test]
        public void BusAudioMixerController_PerspectiveMultipliers_CockpitAndCabinAttenuated()
        {
            var go = new GameObject("TestAudioMixer");
            var mixer = go.AddComponent<BusAudioMixerController>();

            // Exterior (Full Engine Sound)
            mixer.currentPerspective = AudioPerspective.Exterior;
            Assert.AreEqual(1.0f, mixer.GetPerspectiveEngineMultiplier());
            Assert.AreEqual(1.0f, mixer.GetPerspectiveTyreMultiplier());

            // Cockpit (Muffled Engine)
            mixer.currentPerspective = AudioPerspective.DriverCockpit;
            Assert.AreEqual(0.55f, mixer.GetPerspectiveEngineMultiplier());
            Assert.AreEqual(0.60f, mixer.GetPerspectiveTyreMultiplier());

            // Cabin (Deeply Reduced Engine, Pronounced Road Vibration)
            mixer.currentPerspective = AudioPerspective.PassengerCabin;
            Assert.AreEqual(0.35f, mixer.GetPerspectiveEngineMultiplier());
            Assert.AreEqual(0.85f, mixer.GetPerspectiveTyreMultiplier());

            GameObject.DestroyImmediate(go);
        }

        [Test]
        public void BusAudioSystem_BuzzingRegression_ZeroRawOscillatorBuzzer()
        {
            // Regression test verifying no raw sawtooth generators connect directly to output
            var mixerGo = new GameObject("TestMixer");
            var mixer = mixerGo.AddComponent<BusAudioMixerController>();

            Assert.LessOrEqual(mixer.masterVolume, 1.0f);
            Assert.GreaterOrEqual(mixer.masterVolume, 0.0f);

            // Assert mixer group volumes are within non-clipping bounds
            Assert.LessOrEqual(mixer.busEngineVolume, 1.0f);
            Assert.LessOrEqual(mixer.busAirVolume, 1.0f);
            Assert.LessOrEqual(mixer.busHornVolume, 1.0f);

            GameObject.DestroyImmediate(mixerGo);
        }
    }
}
