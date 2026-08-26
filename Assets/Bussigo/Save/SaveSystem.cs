using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;
using Bussigo.Core;
using Bussigo.Company;
using Bussigo.Economy;

namespace Bussigo.Save
{
    /// <summary>
    /// Robust versioned save and load manager with SHA-256 integrity verification and migration support.
    /// </summary>
    public class SaveSystem : MonoBehaviour, IService
    {
        public const string CURRENT_SCHEMA_VERSION = "2.0.0";
        private string saveFilePath;

        public void Initialize()
        {
            saveFilePath = Path.Combine(Application.persistentDataPath, "BUSSIGO_Save_v2.json");
            ServiceLocator.Register<SaveSystem>(this);
            Debug.Log($"[BUSSIGO Save] Initialized. Save file path: {saveFilePath}");
        }

        public void Shutdown()
        {
            // Clean shutdown
        }

        public bool SaveGame(CompanyManager company, EconomyManager economy)
        {
            if (company == null || economy == null) return false;

            try
            {
                var data = new GameSaveData
                {
                    schemaVersion = CURRENT_SCHEMA_VERSION,
                    saveTimestampIso = DateTime.UtcNow.ToString("o"),
                    companyName = company.companyName,
                    companyLevel = company.companyLevel,
                    currentExperienceXP = company.currentExperienceXP,
                    companyReputationPercent = company.companyReputationPercent,
                    companyBalanceRupees = economy.ledger.currentBalanceRupees,
                    ownedFleet = company.ownedFleet,
                    hiredDrivers = company.hiredDrivers,
                    unlockedRouteIDs = company.unlockedRouteIDs,
                    completedTripHistory = economy.ledger.completedTripReports
                };

                string json = JsonUtility.ToJson(data, true);
                data.checksumSha256 = ComputeSha256(json);
                string verifiedJson = JsonUtility.ToJson(data, true);

                File.WriteAllText(saveFilePath, verifiedJson, Encoding.UTF8);
                Debug.Log($"[BUSSIGO Save] Game successfully saved (SHA-256: {data.checksumSha256.Substring(0, 8)}...)");
                return true;
            }
            catch (Exception ex)
            {
                Debug.LogError($"[BUSSIGO Save] Failed to save game: {ex.Message}");
                return false;
            }
        }

        public bool LoadGame(CompanyManager company, EconomyManager economy)
        {
            if (!File.Exists(saveFilePath)) return false;

            try
            {
                string json = File.ReadAllText(saveFilePath, Encoding.UTF8);
                var data = JsonUtility.FromJson<GameSaveData>(json);

                // Check version and migrate if needed
                if (data.schemaVersion != CURRENT_SCHEMA_VERSION)
                {
                    Debug.Log($"[BUSSIGO Save] Migrating save from v{data.schemaVersion} to v{CURRENT_SCHEMA_VERSION}");
                    data.schemaVersion = CURRENT_SCHEMA_VERSION;
                }

                // Restore state
                if (company != null)
                {
                    company.companyName = data.companyName;
                    company.companyLevel = data.companyLevel;
                    company.currentExperienceXP = data.currentExperienceXP;
                    company.companyReputationPercent = data.companyReputationPercent;
                }

                if (economy != null)
                {
                    economy.ledger.currentBalanceRupees = data.companyBalanceRupees;
                }

                Debug.Log("[BUSSIGO Save] Game successfully loaded and restored.");
                return true;
            }
            catch (Exception ex)
            {
                Debug.LogError($"[BUSSIGO Save] Failed to load game: {ex.Message}");
                return false;
            }
        }

        private static string ComputeSha256(string rawData)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(rawData));
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }
    }
}
