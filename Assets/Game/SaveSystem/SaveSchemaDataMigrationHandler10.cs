using System;

namespace Bussigo.Game.SaveSystem
{
    public class SaveSchemaDataMigrationHandler10
    {
        public int FromSchemaVersion => 10;
        public int ToSchemaVersion => 11;

        public string MigratePayload(string oldPayloadJson)
        {
            if (string.IsNullOrEmpty(oldPayloadJson)) return "{}";
            string fromVer = "version":"" + 10 + ".0.0";
            string toVer = "version":"" + 11 + ".0.0";
            return oldPayloadJson.Replace(fromVer, toVer);
        }
    }
}
