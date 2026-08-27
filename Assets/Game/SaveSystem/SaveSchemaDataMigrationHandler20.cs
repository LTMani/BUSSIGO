using System;

namespace Bussigo.Game.SaveSystem
{
    public class SaveSchemaDataMigrationHandler20
    {
        public int FromSchemaVersion => 20;
        public int ToSchemaVersion => 21;

        public string MigratePayload(string oldPayloadJson)
        {
            if (string.IsNullOrEmpty(oldPayloadJson)) return "{}";
            string fromVer = "\"version\":\"" + 20 + ".0.0\"";
            string toVer = "\"version\":\"" + 21 + ".0.0\"";
            return oldPayloadJson.Replace(fromVer, toVer);
        }
    }
}
