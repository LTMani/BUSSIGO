using System;

namespace Bussigo.Game.SaveSystem
{
    public class SaveSchemaDataMigrationHandler18
    {
        public int FromSchemaVersion => 18;
        public int ToSchemaVersion => 19;

        public string MigratePayload(string oldPayloadJson)
        {
            if (string.IsNullOrEmpty(oldPayloadJson)) return "{}";
            string fromVer = "version":"" + 18 + ".0.0";
            string toVer = "version":"" + 19 + ".0.0";
            return oldPayloadJson.Replace(fromVer, toVer);
        }
    }
}
