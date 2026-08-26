using System;
using Bussigo.Game.Core;

namespace Bussigo.Game.Customization
{
    public class BilingualLedDestinationScrollMatrix08
    {
        public string DisplayId => "LED-MATRIX-PANEL-008";
        public int MatrixWidthPixels { get; set; } = 128;
        public int MatrixHeightPixels { get; set; } = 16;
        public string PrimaryMessageEnglish { get; set; } = "VIJAYAWADA - SURYAPET - HYDERABAD EXPRESS";
        public string PrimaryMessageTelugu { get; set; } = "విజయవాడ - సూర్యాపేట - హైదరాబాద్ ఎక్స్‌ప్రెస్";
        public float ScrollSpeedPixelsPerSec { get; set; } = 40.0f;
        public float CurrentScrollOffsetPixels { get; private set; } = 0.0f;

        public void UpdateScroll(float deltaTime)
        {
            CurrentScrollOffsetPixels += ScrollSpeedPixelsPerSec * deltaTime;
            float totalEstimatedTextLength = PrimaryMessageEnglish.Length * 8.0f;
            if (CurrentScrollOffsetPixels > totalEstimatedTextLength + MatrixWidthPixels)
            {
                CurrentScrollOffsetPixels = 0.0f;
            }
        }
    }
}
