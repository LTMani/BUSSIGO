using System;

namespace Bussigo.Game.SaveSystem
{
    public class SaveSchemaDataMigrationHandler08
    {
        public int FromSchemaVersion => 8;
        public int ToSchemaVersion => 9;

        public string MigratePayload(string oldPayloadJson)
        {
            if (string.IsNullOrEmpty(oldPayloadJson)) return "{}";
            string fromVer = "\"version\":\"" + 8 + ".0.0\"";
            string toVer = "\"version\":\"" + 9 + ".0.0\"";
            return oldPayloadJson.Replace(fromVer, toVer);
        }
    }
}
