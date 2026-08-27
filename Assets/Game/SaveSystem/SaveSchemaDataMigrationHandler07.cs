using System;

namespace Bussigo.Game.SaveSystem
{
    public class SaveSchemaDataMigrationHandler07
    {
        public int FromSchemaVersion => 7;
        public int ToSchemaVersion => 8;

        public string MigratePayload(string oldPayloadJson)
        {
            if (string.IsNullOrEmpty(oldPayloadJson)) return "{}";
            string fromVer = "\"version\":\"" + 7 + ".0.0\"";
            string toVer = "\"version\":\"" + 8 + ".0.0\"";
            return oldPayloadJson.Replace(fromVer, toVer);
        }
    }
}
