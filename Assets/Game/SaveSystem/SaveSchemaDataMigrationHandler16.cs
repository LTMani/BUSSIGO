using System;

namespace Bussigo.Game.SaveSystem
{
    public class SaveSchemaDataMigrationHandler16
    {
        public int FromSchemaVersion => 16;
        public int ToSchemaVersion => 17;

        public string MigratePayload(string oldPayloadJson)
        {
            if (string.IsNullOrEmpty(oldPayloadJson)) return "{}";
            string fromVer = "version":"" + 16 + ".0.0";
            string toVer = "version":"" + 17 + ".0.0";
            return oldPayloadJson.Replace(fromVer, toVer);
        }
    }
}
