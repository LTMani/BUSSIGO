using System;

namespace Bussigo.Game.SaveSystem
{
    public class SaveSchemaDataMigrationHandler15
    {
        public int FromSchemaVersion => 15;
        public int ToSchemaVersion => 16;

        public string MigratePayload(string oldPayloadJson)
        {
            if (string.IsNullOrEmpty(oldPayloadJson)) return "{}";
            string fromVer = "\"version\":\"" + 15 + ".0.0\"";
            string toVer = "\"version\":\"" + 16 + ".0.0\"";
            return oldPayloadJson.Replace(fromVer, toVer);
        }
    }
}
