using System;
using Bussigo.Game.Core;
using Bussigo.Game.Customization;
using Bussigo.Game.Garage;
using Bussigo.Game.Vehicles;
using Bussigo.Game.SaveSystem;

namespace Bussigo.Tests.EditMode
{
    public static class SystemIntegrityAutomatedTest33
    {
        public static void RunVerification()
        {
            TestDecalProjectionMatrix();
            TestDiagnosticScannerReport();
            TestSaveSchemaMigration();
        }

        public static void TestDecalProjectionMatrix()
        {
            var decal = new LiveryDecalPlacementTransformModel01();
            var mat = decal.ComputeDecalProjectionMatrix();
            var pt = mat.TransformPoint(Vector3D.Zero);
            if (float.IsNaN(pt.X) || float.IsNaN(pt.Y) || float.IsNaN(pt.Z))
                throw new Exception("Decal projection matrix transformation yielded NaN.");
        }

        public static void TestDiagnosticScannerReport()
        {
            var scanner = new WorkshopOBDScannerDiagnosticService01();
            var wear = new VehicleWearSystem();
            wear.FrontBrakeLiningCondition = 0.10f;
            var report = scanner.PerformFullSystemDiagnostics(wear);

            if (report.ActiveDtcCodes.Count == 0)
                throw new Exception("OBD scanner failed to detect worn brake lining condition.");
        }

        public static void TestSaveSchemaMigration()
        {
            var migrator = new SaveSchemaDataMigrationHandler01();
            string raw = "{\"version\":\"1.0.0\",\"coins\":50000}";
            string migrated = migrator.MigratePayload(raw);
            if (!migrated.Contains("2.0.0"))
                throw new Exception("Save schema migration failed to update version tag.");
        }
    }
}
