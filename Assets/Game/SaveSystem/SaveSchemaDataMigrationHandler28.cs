using System;

namespace Bussigo.Game.SaveSystem
{
    public class SaveSchemaDataMigrationHandler28
    {
        public int FromSchemaVersion => 28;
        public int ToSchemaVersion => 29;

        public string MigratePayload(string oldPayloadJson)
        {
            if (string.IsNullOrEmpty(oldPayloadJson)) return "{}";
            string fromVer = "\"version\":\"" + 28 + ".0.0\"";
            string toVer = "\"version\":\"" + 29 + ".0.0\"";
            return oldPayloadJson.Replace(fromVer, toVer);
        }
    }
}
