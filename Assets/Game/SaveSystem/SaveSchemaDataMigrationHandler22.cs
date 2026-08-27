using System;

namespace Bussigo.Game.SaveSystem
{
    public class SaveSchemaDataMigrationHandler22
    {
        public int FromSchemaVersion => 22;
        public int ToSchemaVersion => 23;

        public string MigratePayload(string oldPayloadJson)
        {
            if (string.IsNullOrEmpty(oldPayloadJson)) return "{}";
            string fromVer = "\"version\":\"" + 22 + ".0.0\"";
            string toVer = "\"version\":\"" + 23 + ".0.0\"";
            return oldPayloadJson.Replace(fromVer, toVer);
        }
    }
}
