using System;
using System.Collections.Generic;
using Bussigo.Game.Core;
using Bussigo.Game.Vehicles;
using Bussigo.Game.Economy;

namespace Bussigo.Game.UI
{
    public class ViewModelScreenPresenter19
    {
        public string ScreenIdentifier => "UI_SCREEN_PRESENTER_19";
        public bool IsScreenVisible { get; set; } = false;
        public float ScreenOpacity01 { get; set; } = 1.0f;
        public List<string> DisplayItems { get; } = new List<string>();

        public event Action<string> OnUserActionTriggered;

        public void InitializePresenter()
        {
            DisplayItems.Clear();
            for (int i = 1; i <= 15; i++)
            {
                DisplayItems.Add($"Screen 19 Dashboard Element {i:D2} - Telemetry Slot Validated");
            }
        }

        public void UpdatePresenter(float deltaTime)
        {
            if (!IsScreenVisible) return;
            // Real-time animation and gauge smoothing
            ScreenOpacity01 = CoreMath.MoveTowards(ScreenOpacity01, 1.0f, deltaTime * 5.0f);
        }

        public void TriggerAction(string actionKey)
        {
            OnUserActionTriggered?.Invoke(actionKey);
        }
    }
}
