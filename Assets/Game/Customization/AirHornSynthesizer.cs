using System;
using System.Collections.Generic;
using Bussigo.Game.Core;

namespace Bussigo.Game.Customization
{
    public class AirHornMelodyChord
    {
        public float[] FrequenciesHz { get; set; }
        public float DurationSeconds { get; set; }

        public AirHornMelodyChord(float[] freqs, float duration)
        {
            FrequenciesHz = freqs;
            DurationSeconds = duration;
        }
    }

    public class AirHornMelodicPattern
    {
        public string MelodyName { get; set; }
        public List<AirHornMelodyChord> Chords { get; } = new List<AirHornMelodyChord>();

        public AirHornMelodicPattern(string name)
        {
            MelodyName = name;
        }

        public void AddChord(float[] freqs, float duration)
        {
            Chords.Add(new AirHornMelodyChord(freqs, duration));
        }
    }

    public static class AirHornCatalog
    {
        public static List<AirHornMelodicPattern> Melodies { get; } = new List<AirHornMelodicPattern>();

        static AirHornCatalog()
        {
            // 1. Classic Telugu Dual Tone Pressure Horn
            var dualTone = new AirHornMelodicPattern("Classic Deccan Dual Tone");
            dualTone.AddChord(new float[] { 349.23f, 440.0f }, 0.4f); // F4 + A4
            dualTone.AddChord(new float[] { 392.00f, 523.25f }, 0.6f); // G4 + C5
            Melodies.Add(dualTone);

            // 2. High-Deck Triple Trombone Highway Horn
            var tripleHorn = new AirHornMelodicPattern("Highway King Triple Trombone");
            tripleHorn.AddChord(new float[] { 311.13f, 370.0f, 466.16f }, 0.35f);
            tripleHorn.AddChord(new float[] { 349.23f, 415.3f, 523.25f }, 0.35f);
            tripleHorn.AddChord(new float[] { 392.00f, 466.16f, 587.33f }, 0.8f);
            Melodies.Add(tripleHorn);

            // 3. Iconic South Indian 5-Tone Musical Chime
            var chime = new AirHornMelodicPattern("South Indian Highway Symphony");
            chime.AddChord(new float[] { 261.63f }, 0.18f); // C4
            chime.AddChord(new float[] { 293.66f }, 0.18f); // D4
            chime.AddChord(new float[] { 329.63f }, 0.18f); // E4
            chime.AddChord(new float[] { 392.00f }, 0.18f); // G4
            chime.AddChord(new float[] { 523.25f }, 0.60f); // C5
            Melodies.Add(chime);
        }
    }
}
