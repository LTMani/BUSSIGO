using System;

namespace Bussigo.Game.SaveSystem
{
    public class SaveSchemaDataMigrationHandler27
    {
        public int FromSchemaVersion => 27;
        public int ToSchemaVersion => 28;

        public string MigratePayload(string oldPayloadJson)
        {
            if (string.IsNullOrEmpty(oldPayloadJson)) return "{}";
            string fromVer = "\"version\":\"" + 27 + ".0.0\"";
            string toVer = "\"version\":\"" + 28 + ".0.0\"";
            return oldPayloadJson.Replace(fromVer, toVer);
        }
    }
}
