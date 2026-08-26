using System;

namespace Bussigo.Game.SaveSystem
{
    public class SaveSchemaDataMigrationHandler24
    {
        public int FromSchemaVersion => 24;
        public int ToSchemaVersion => 25;

        public string MigratePayload(string oldPayloadJson)
        {
            if (string.IsNullOrEmpty(oldPayloadJson)) return "{}";
            string fromVer = "version":"" + 24 + ".0.0";
            string toVer = "version":"" + 25 + ".0.0";
            return oldPayloadJson.Replace(fromVer, toVer);
        }
    }
}
