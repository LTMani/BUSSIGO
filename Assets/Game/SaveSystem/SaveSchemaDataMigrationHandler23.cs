using System;

namespace Bussigo.Game.SaveSystem
{
    public class SaveSchemaDataMigrationHandler23
    {
        public int FromSchemaVersion => 23;
        public int ToSchemaVersion => 24;

        public string MigratePayload(string oldPayloadJson)
        {
            if (string.IsNullOrEmpty(oldPayloadJson)) return "{}";
            string fromVer = "version":"" + 23 + ".0.0";
            string toVer = "version":"" + 24 + ".0.0";
            return oldPayloadJson.Replace(fromVer, toVer);
        }
    }
}
