using System;

namespace Bussigo.Game.SaveSystem
{
    public class SaveSchemaDataMigrationHandler13
    {
        public int FromSchemaVersion => 13;
        public int ToSchemaVersion => 14;

        public string MigratePayload(string oldPayloadJson)
        {
            if (string.IsNullOrEmpty(oldPayloadJson)) return "{}";
            string fromVer = "\"version\":\"" + 13 + ".0.0\"";
            string toVer = "\"version\":\"" + 14 + ".0.0\"";
            return oldPayloadJson.Replace(fromVer, toVer);
        }
    }
}
