using System;
using Bussigo.Game.Core;
using Bussigo.Game.Vehicles;
using Bussigo.Game.VehiclePhysics;
using Bussigo.Game.Economy;

namespace Bussigo.Tests.EditMode
{
    public static class SubsystemMathEditModeTests22
    {
        public static void RunAllAssertions()
        {
            TestMatrixTransformations();
            TestDoubleEntryLedgerBalance();
        }

        public static void TestMatrixTransformations()
        {
            var mat = Matrix4x4D.CreateTranslation(new Vector3D(10f, 20f, 30f));
            var pt = new Vector3D(5f, 5f, 5f);
            var transformed = mat.TransformPoint(pt);

            if (MathF.Abs(transformed.X - 15f) > 0.01f ||
                MathF.Abs(transformed.Y - 25f) > 0.01f ||
                MathF.Abs(transformed.Z - 35f) > 0.01f)
            {
                throw new Exception("Matrix4x4 translation test failed.");
            }
        }

        public static void TestDoubleEntryLedgerBalance()
        {
            var jv = new FinancialAccountingJournal01();
            jv.AddDebit("1010", "Cash Bank", 5000f);
            jv.AddCredit("4010", "Ticket Revenue", 5000f);
            if (!jv.ValidateDoubleEntryBalance())
            {
                throw new Exception("Double entry validation failed.");
            }
        }
    }
}
