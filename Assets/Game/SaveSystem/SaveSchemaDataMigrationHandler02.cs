using System;

namespace Bussigo.Game.SaveSystem
{
    public class SaveSchemaDataMigrationHandler02
    {
        public int FromSchemaVersion => 2;
        public int ToSchemaVersion => 3;

        public string MigratePayload(string oldPayloadJson)
        {
            if (string.IsNullOrEmpty(oldPayloadJson)) return "{}";
            string fromVer = "version":"" + 2 + ".0.0";
            string toVer = "version":"" + 3 + ".0.0";
            return oldPayloadJson.Replace(fromVer, toVer);
        }
    }
}
