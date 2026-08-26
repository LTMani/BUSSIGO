using System;

namespace Bussigo.Game.SaveSystem
{
    public class SaveSchemaDataMigrationHandler11
    {
        public int FromSchemaVersion => 11;
        public int ToSchemaVersion => 12;

        public string MigratePayload(string oldPayloadJson)
        {
            if (string.IsNullOrEmpty(oldPayloadJson)) return "{}";
            string fromVer = "version":"" + 11 + ".0.0";
            string toVer = "version":"" + 12 + ".0.0";
            return oldPayloadJson.Replace(fromVer, toVer);
        }
    }
}
