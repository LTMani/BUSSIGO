using System;
using System.Collections.Generic;

namespace Bussigo.Game.Customization
{
    public class LiveryConfiguration
    {
        public string LiveryName { get; set; } = "APSRTC Classic Heritage";
        public string PrimaryColorHex { get; set; } = "#C8232C"; // Deep Crimson Red
        public string SecondaryColorHex { get; set; } = "#FFFFFF"; // Pure White
        public string AccentStripeColorHex { get; set; } = "#F9A825"; // Andhra Gold
        public bool HasMetallicFinish { get; set; } = false;
        public float PaintGlossiness { get; set; } = 0.85f;

        // Bilingual Destination Board LED
        public string DestinationTextEnglish { get; set; } = "VIJAYAWADA -> HYDERABAD";
        public string DestinationTextTelugu { get; set; } = "విజయవాడ -> హైదరాబాద్";
        public string LedBoardColorHex { get; set; } = "#FFB300"; // Amber LED

        // Horn sound selection
        public int SelectedHornIndex { get; set; } = 1; // 0: Standard Electric, 1: Double Tone Air Horn, 2: Triple Tone Musical

        // Cosmetic accessories
        public bool FrontBullBarInstalled { get; set; } = true;
        public bool RoofLuggageCarrierInstalled { get; set; } = true;
        public bool ChromeWheelCapsInstalled { get; set; } = true;
        public bool WindshieldSunVisorInstalled { get; set; } = true;
        public bool DashboardIdolInstalled { get; set; } = true;
    }

    public class LiveryStudio
    {
        public LiveryConfiguration CurrentLivery { get; set; } = new LiveryConfiguration();

        public void ApplyPreset(string presetName)
        {
            if (presetName == "PalleveluguGreen")
            {
                CurrentLivery.PrimaryColorHex = "#2E7D32";
                CurrentLivery.SecondaryColorHex = "#FFFFFF";
                CurrentLivery.AccentStripeColorHex = "#FDD835";
            }
            else if (presetName == "GarudaSilver")
            {
                CurrentLivery.PrimaryColorHex = "#E0E0E0";
                CurrentLivery.SecondaryColorHex = "#1565C0";
                CurrentLivery.AccentStripeColorHex = "#D32F2F";
                CurrentLivery.HasMetallicFinish = true;
            }
            else if (presetName == "AmaravatiWhiteGold")
            {
                CurrentLivery.PrimaryColorHex = "#FAFAFA";
                CurrentLivery.SecondaryColorHex = "#FFD700";
                CurrentLivery.AccentStripeColorHex = "#0D47A1";
                CurrentLivery.HasMetallicFinish = true;
            }
        }
    }
}
