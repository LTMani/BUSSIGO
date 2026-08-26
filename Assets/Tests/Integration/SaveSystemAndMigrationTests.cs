using System;
using Bussigo.Game.SaveSystem;

namespace Bussigo.Tests.Integration
{
    public static class SaveSystemAndMigrationTests
    {
        public static void RunAllTests()
        {
            TestChecksumGenerationAndValidation();
        }

        public static void TestChecksumGenerationAndValidation()
        {
            string sampleJson = "{\"version\":\"1.0.0\",\"coins\":500000,\"driverLevel\":5}";
            string checksum = SaveGameManager.ComputeSha256Checksum(sampleJson);

            if (!SaveGameManager.ValidateSaveData(sampleJson, checksum))
                throw new Exception("Checksum validation failed on identical JSON payload.");

            if (SaveGameManager.ValidateSaveData(sampleJson + " ", checksum))
                throw new Exception("Checksum validation should fail on tampered content.");
        }
    }
}
