using System;

namespace Bussigo.Game.SaveSystem
{
    public class SaveSchemaDataMigrationHandler14
    {
        public int FromSchemaVersion => 14;
        public int ToSchemaVersion => 15;

        public string MigratePayload(string oldPayloadJson)
        {
            if (string.IsNullOrEmpty(oldPayloadJson)) return "{}";
            string fromVer = "\"version\":\"" + 14 + ".0.0\"";
            string toVer = "\"version\":\"" + 15 + ".0.0\"";
            return oldPayloadJson.Replace(fromVer, toVer);
        }
    }
}
