using System;
using System.IO;
using System.Text;
using System.Security.Cryptography;
using Bussigo.Game.Company;

namespace Bussigo.Game.SaveSystem
{
    public class GameSaveData
    {
        public string SaveVersion { get; set; } = "1.0.0";
        public DateTime SaveTimestamp { get; set; } = DateTime.UtcNow;
        public float PlayerCurrencyCoins { get; set; } = 500000.0f;
        public int DriverLevel { get; set; } = 1;
        public long TotalXp { get; set; } = 0;
        public string SelectedBusId { get; set; } = "BUS-PAL-01";
        public string CompanyName { get; set; } = "Deccan Royal Express";
    }

    public static class SaveGameManager
    {
        public static string ComputeSha256Checksum(string content)
        {
            using var sha = SHA256.Create();
            byte[] bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(content));
            return Convert.ToHexString(bytes);
        }

        public static bool ValidateSaveData(string jsonContent, string expectedChecksum)
        {
            string hash = ComputeSha256Checksum(jsonContent);
            return string.Equals(hash, expectedChecksum, StringComparison.OrdinalIgnoreCase);
        }
    }
}
