using System;

namespace Bussigo.Game.SaveSystem
{
    public class SaveSchemaDataMigrationHandler01
    {
        public int FromSchemaVersion => 1;
        public int ToSchemaVersion => 2;

        public string MigratePayload(string oldPayloadJson)
        {
            if (string.IsNullOrEmpty(oldPayloadJson)) return "{}";
            string fromVer = "\"version\":\"" + 1 + ".0.0\"";
            string toVer = "\"version\":\"" + 2 + ".0.0\"";
            return oldPayloadJson.Replace(fromVer, toVer);
        }
    }
}
