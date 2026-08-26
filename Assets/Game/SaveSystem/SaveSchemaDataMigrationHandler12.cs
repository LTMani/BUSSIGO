using System;

namespace Bussigo.Game.SaveSystem
{
    public class SaveSchemaDataMigrationHandler12
    {
        public int FromSchemaVersion => 12;
        public int ToSchemaVersion => 13;

        public string MigratePayload(string oldPayloadJson)
        {
            if (string.IsNullOrEmpty(oldPayloadJson)) return "{}";
            string fromVer = "version":"" + 12 + ".0.0";
            string toVer = "version":"" + 13 + ".0.0";
            return oldPayloadJson.Replace(fromVer, toVer);
        }
    }
}
