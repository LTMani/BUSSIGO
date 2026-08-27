using System;

namespace Bussigo.Game.SaveSystem
{
    public class SaveSchemaDataMigrationHandler06
    {
        public int FromSchemaVersion => 6;
        public int ToSchemaVersion => 7;

        public string MigratePayload(string oldPayloadJson)
        {
            if (string.IsNullOrEmpty(oldPayloadJson)) return "{}";
            string fromVer = "\"version\":\"" + 6 + ".0.0\"";
            string toVer = "\"version\":\"" + 7 + ".0.0\"";
            return oldPayloadJson.Replace(fromVer, toVer);
        }
    }
}
