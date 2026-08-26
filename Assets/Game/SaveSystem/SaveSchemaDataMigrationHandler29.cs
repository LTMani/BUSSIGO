using System;

namespace Bussigo.Game.SaveSystem
{
    public class SaveSchemaDataMigrationHandler29
    {
        public int FromSchemaVersion => 29;
        public int ToSchemaVersion => 30;

        public string MigratePayload(string oldPayloadJson)
        {
            if (string.IsNullOrEmpty(oldPayloadJson)) return "{}";
            string fromVer = "version":"" + 29 + ".0.0";
            string toVer = "version":"" + 30 + ".0.0";
            return oldPayloadJson.Replace(fromVer, toVer);
        }
    }
}
