using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Bussigo.Game.Runtime3D.UI
{
    public class MainMenuUIController : MonoBehaviour
    {
        public string companyName = "Deccan Royal Express Travels";
        public float companyCoins = 500000f;
        public int driverLevel = 1;

        private void OnGUI()
        {
            float menuWidth = 460f;
            float menuHeight = 440f;
            float menuX = (Screen.width - menuWidth) * 0.5f;
            float menuY = (Screen.height - menuHeight) * 0.5f;

            GUIStyle titleStyle = new GUIStyle(GUI.skin.label);
            titleStyle.fontSize = 28;
            titleStyle.fontStyle = FontStyle.Bold;
            titleStyle.alignment = TextAnchor.MiddleCenter;
            titleStyle.normal.textColor = new Color(0.95f, 0.75f, 0.15f); // Crimson & Gold theme

            GUIStyle subStyle = new GUIStyle(GUI.skin.label);
            subStyle.fontSize = 14;
            subStyle.alignment = TextAnchor.MiddleCenter;
            subStyle.normal.textColor = Color.white;

            GUIStyle buttonStyle = new GUIStyle(GUI.skin.button);
            buttonStyle.fontSize = 16;
            buttonStyle.fontStyle = FontStyle.Bold;

            GUI.Box(new Rect(menuX, menuY, menuWidth, menuHeight), "");

            GUI.Label(new Rect(menuX + 10, menuY + 15, menuWidth - 20, 35), "BUSSIGO", titleStyle);
            GUI.Label(new Rect(menuX + 10, menuY + 50, menuWidth - 20, 25), "SOUTH INDIA BUS & TRAVEL EMPIRE SIMULATOR", subStyle);
            GUI.Label(new Rect(menuX + 10, menuY + 75, menuWidth - 20, 25), $"Company: {companyName} | Balance: ₹{companyCoins:N0} | Level: {driverLevel}", subStyle);

            float btnY = menuY + 115f;
            float btnHeight = 48f;
            float btnGap = 12f;

            if (GUI.Button(new Rect(menuX + 40, btnY, menuWidth - 80, btnHeight), "1. DRIVE: VIJAYAWADA ➔ HYDERABAD (NH65)", buttonStyle))
            {
                SceneManager.LoadScene("VijayawadaHyderabadPlayableRoute");
            }

            btnY += btnHeight + btnGap;
            if (GUI.Button(new Rect(menuX + 40, btnY, menuWidth - 80, btnHeight), "2. FREE ROAM HIGHWAY DRIVE", buttonStyle))
            {
                SceneManager.LoadScene("VijayawadaHyderabadPlayableRoute");
            }

            btnY += btnHeight + btnGap;
            if (GUI.Button(new Rect(menuX + 40, btnY, menuWidth - 80, btnHeight), "3. GARAGE & FLEET CUSTOMIZATION", buttonStyle))
            {
                Debug.Log("[Garage] Fleet garage opened.");
            }

            btnY += btnHeight + btnGap;
            if (GUI.Button(new Rect(menuX + 40, btnY, menuWidth - 80, btnHeight), "4. REGIONAL DEPOTS & TYCOON HQ", buttonStyle))
            {
                Debug.Log("[Depots] Regional depots view opened.");
            }

            btnY += btnHeight + btnGap;
            if (GUI.Button(new Rect(menuX + 40, btnY, menuWidth - 80, btnHeight), "5. EXIT GAME", buttonStyle))
            {
                Application.Quit();
            }
        }
    }
}
