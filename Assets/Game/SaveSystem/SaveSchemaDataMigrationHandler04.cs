using System;

namespace Bussigo.Game.SaveSystem
{
    public class SaveSchemaDataMigrationHandler04
    {
        public int FromSchemaVersion => 4;
        public int ToSchemaVersion => 5;

        public string MigratePayload(string oldPayloadJson)
        {
            if (string.IsNullOrEmpty(oldPayloadJson)) return "{}";
            string fromVer = "\"version\":\"" + 4 + ".0.0\"";
            string toVer = "\"version\":\"" + 5 + ".0.0\"";
            return oldPayloadJson.Replace(fromVer, toVer);
        }
    }
}
