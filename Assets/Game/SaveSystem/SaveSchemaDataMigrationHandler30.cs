using System;

namespace Bussigo.Game.SaveSystem
{
    public class SaveSchemaDataMigrationHandler30
    {
        public int FromSchemaVersion => 30;
        public int ToSchemaVersion => 31;

        public string MigratePayload(string oldPayloadJson)
        {
            if (string.IsNullOrEmpty(oldPayloadJson)) return "{}";
            string fromVer = "version":"" + 30 + ".0.0";
            string toVer = "version":"" + 31 + ".0.0";
            return oldPayloadJson.Replace(fromVer, toVer);
        }
    }
}
