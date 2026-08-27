using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using Bussigo.Game.Economy;
using Bussigo.Game.SaveSystem;

namespace Bussigo.Game.Runtime3D.Gameplay
{
    public class GameBootstrap : MonoBehaviour
    {
        public static GameBootstrap Instance { get; private set; }

        [Header("Global Persistent State")]
        public string playerCompanyName = "Deccan Royal Express Travels";
        public double companyCashRupees = 500000.0;
        public int driverLevel = 1;
        public long driverXp = 0;
        [NonSerialized] public FinancialLedger companyFinancialLedger;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
                InitializeGameServices();
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void InitializeGameServices()
        {
            companyFinancialLedger = new FinancialLedger();
            companyFinancialLedger.RecordTransaction(TransactionType.TicketRevenue, 500000f, "Initial Company Founding Capital");
            
            Application.targetFrameRate = 60;
            QualitySettings.vSyncCount = 1;
            
            Debug.Log($"[BUSSIGO Bootstrap] Engine initialized. Company: {playerCompanyName} | Balance: ₹{companyCashRupees:N2}");
        }

        public void LoadRouteScene(string routeSceneName)
        {
            SceneManager.LoadScene(routeSceneName);
        }

        public void LoadMainMenuScene()
        {
            SceneManager.LoadScene("MainMenu");
        }
    }
}
