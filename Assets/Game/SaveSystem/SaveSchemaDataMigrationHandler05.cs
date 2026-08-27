using System;

namespace Bussigo.Game.SaveSystem
{
    public class SaveSchemaDataMigrationHandler05
    {
        public int FromSchemaVersion => 5;
        public int ToSchemaVersion => 6;

        public string MigratePayload(string oldPayloadJson)
        {
            if (string.IsNullOrEmpty(oldPayloadJson)) return "{}";
            string fromVer = "\"version\":\"" + 5 + ".0.0\"";
            string toVer = "\"version\":\"" + 6 + ".0.0\"";
            return oldPayloadJson.Replace(fromVer, toVer);
        }
    }
}
