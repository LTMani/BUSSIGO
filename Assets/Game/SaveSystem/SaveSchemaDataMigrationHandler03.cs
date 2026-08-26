using System;

namespace Bussigo.Game.SaveSystem
{
    public class SaveSchemaDataMigrationHandler03
    {
        public int FromSchemaVersion => 3;
        public int ToSchemaVersion => 4;

        public string MigratePayload(string oldPayloadJson)
        {
            if (string.IsNullOrEmpty(oldPayloadJson)) return "{}";
            string fromVer = "version":"" + 3 + ".0.0";
            string toVer = "version":"" + 4 + ".0.0";
            return oldPayloadJson.Replace(fromVer, toVer);
        }
    }
}
