using System;

namespace Bussigo.Game.SaveSystem
{
    public class SaveSchemaDataMigrationHandler25
    {
        public int FromSchemaVersion => 25;
        public int ToSchemaVersion => 26;

        public string MigratePayload(string oldPayloadJson)
        {
            if (string.IsNullOrEmpty(oldPayloadJson)) return "{}";
            string fromVer = "version":"" + 25 + ".0.0";
            string toVer = "version":"" + 26 + ".0.0";
            return oldPayloadJson.Replace(fromVer, toVer);
        }
    }
}
