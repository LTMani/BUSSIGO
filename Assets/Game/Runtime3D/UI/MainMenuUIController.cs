using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using Bussigo.Core;
using Bussigo.Company;
using Bussigo.Economy;
using Bussigo.Save;

namespace Bussigo.Game.Runtime3D.UI
{
    public class MainMenuUIController : MonoBehaviour
    {
        // Cached references to persistent managers (do not own lifetime)
        private CompanyManager _companyManager;
        private EconomyManager _economyManager;
        private SaveSystem _saveSystem;

        [Header("UI State (fallback if managers not yet available)")]
        public string companyName = "Deccan Royal Express Travels";
        public float companyCoins = 500000f;
        public int driverLevel = 1;

        private void Awake()
        {
            // Try to get existing persisted managers via ServiceLocator
            // Managers are created in BussigoSceneBootstrap and marked DontDestroyOnLoad
            if (!ServiceLocator.TryGet<CompanyManager>(out _companyManager))
            {
                // Fallback: managers not yet initialized (e.g., before first scene load)
                _companyManager = null;
            }
            if (!ServiceLocator.TryGet<EconomyManager>(out _economyManager))
            {
                _economyManager = null;
            }
            if (!ServiceLocator.TryGet<SaveSystem>(out _saveSystem))
            {
                _saveSystem = null;
            }
        }

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

            // Use live manager data if available, else fallback to hardcoded fields
            string displayCompanyName = companyName;
            float displayCompanyCoins = companyCoins;
            int displayDriverLevel = driverLevel;

            if (_companyManager != null)
            {
                displayCompanyName = _companyManager.companyName;
                displayDriverLevel = _companyManager.companyLevel;
            }
            if (_economyManager != null)
            {
                displayCompanyCoins = (float)_economyManager.CurrentBalance;
            }

            GUI.Box(new Rect(menuX, menuY, menuWidth, menuHeight), "");

            GUI.Label(new Rect(menuX + 10, menuY + 15, menuWidth - 20, 35), "BUSSIGO", titleStyle);
            GUI.Label(new Rect(menuX + 10, menuY + 50, menuWidth - 20, 25), "SOUTH INDIA BUS & TRAVEL EMPIRE SIMULATOR", subStyle);
            GUI.Label(new Rect(menuX + 10, menuY + 75, menuWidth - 20, 25), $"Company: {displayCompanyName} | Balance: ₹{displayCompanyCoins:N0} | Level: {displayDriverLevel}", subStyle);

            float btnY = menuY + 115f;
            float btnHeight = 48f;
            float btnGap = 12f;

            if (GUI.Button(new Rect(menuX + 40, btnY, menuWidth - 80, btnHeight), "1. NEW GAME", buttonStyle))
            {
                // Reset existing manager instances to default state
                if (_companyManager != null) _companyManager.ResetToDefault();
                if (_economyManager != null) _economyManager.ResetToDefault();

                // Load the main gameplay scene
                SceneManager.LoadScene("BUSSIGO_Main");
            }

            btnY += btnHeight + btnGap;
            if (GUI.Button(new Rect(menuX + 40, btnY, menuWidth - 80, btnHeight), "2. CONTINUE GAME", buttonStyle))
            {
                // Attempt to load game into existing manager instances
                bool loadSuccess = false;
                if (_saveSystem != null && _companyManager != null && _economyManager != null)
                {
                    loadSuccess = _saveSystem.LoadGame(_companyManager, _economyManager);
                }

                if (loadSuccess)
                {
                    SceneManager.LoadScene("BUSSIGO_Main");
                }
                else
                {
                    // If no save exists or load failed, fall back to New Game behavior
                    if (_companyManager != null) _companyManager.ResetToDefault();
                    if (_economyManager != null) _economyManager.ResetToDefault();
                    SceneManager.LoadScene("BUSSIGO_Main");
                }
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