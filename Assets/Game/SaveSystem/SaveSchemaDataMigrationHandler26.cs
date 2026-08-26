using System;

namespace Bussigo.Game.SaveSystem
{
    public class SaveSchemaDataMigrationHandler26
    {
        public int FromSchemaVersion => 26;
        public int ToSchemaVersion => 27;

        public string MigratePayload(string oldPayloadJson)
        {
            if (string.IsNullOrEmpty(oldPayloadJson)) return "{}";
            string fromVer = "version":"" + 26 + ".0.0";
            string toVer = "version":"" + 27 + ".0.0";
            return oldPayloadJson.Replace(fromVer, toVer);
        }
    }
}
