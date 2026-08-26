using System;

namespace Bussigo.Game.SaveSystem
{
    public class SaveSchemaDataMigrationHandler09
    {
        public int FromSchemaVersion => 9;
        public int ToSchemaVersion => 10;

        public string MigratePayload(string oldPayloadJson)
        {
            if (string.IsNullOrEmpty(oldPayloadJson)) return "{}";
            string fromVer = "version":"" + 9 + ".0.0";
            string toVer = "version":"" + 10 + ".0.0";
            return oldPayloadJson.Replace(fromVer, toVer);
        }
    }
}
