using System;

namespace Bussigo.Game.SaveSystem
{
    public class SaveSchemaDataMigrationHandler19
    {
        public int FromSchemaVersion => 19;
        public int ToSchemaVersion => 20;

        public string MigratePayload(string oldPayloadJson)
        {
            if (string.IsNullOrEmpty(oldPayloadJson)) return "{}";
            string fromVer = "version":"" + 19 + ".0.0";
            string toVer = "version":"" + 20 + ".0.0";
            return oldPayloadJson.Replace(fromVer, toVer);
        }
    }
}
