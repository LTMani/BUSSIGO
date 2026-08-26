using System;

namespace Bussigo.Game.SaveSystem
{
    public class SaveSchemaDataMigrationHandler17
    {
        public int FromSchemaVersion => 17;
        public int ToSchemaVersion => 18;

        public string MigratePayload(string oldPayloadJson)
        {
            if (string.IsNullOrEmpty(oldPayloadJson)) return "{}";
            string fromVer = "version":"" + 17 + ".0.0";
            string toVer = "version":"" + 18 + ".0.0";
            return oldPayloadJson.Replace(fromVer, toVer);
        }
    }
}
