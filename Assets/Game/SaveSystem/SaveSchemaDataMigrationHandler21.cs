using System;

namespace Bussigo.Game.SaveSystem
{
    public class SaveSchemaDataMigrationHandler21
    {
        public int FromSchemaVersion => 21;
        public int ToSchemaVersion => 22;

        public string MigratePayload(string oldPayloadJson)
        {
            if (string.IsNullOrEmpty(oldPayloadJson)) return "{}";
            string fromVer = "\"version\":\"" + 21 + ".0.0\"";
            string toVer = "\"version\":\"" + 22 + ".0.0\"";
            return oldPayloadJson.Replace(fromVer, toVer);
        }
    }
}
