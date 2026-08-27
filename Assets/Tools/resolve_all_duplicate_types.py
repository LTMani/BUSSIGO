import glob
import os
import re

CANONICAL_DEFINITIONS = {
    "Assets/Game/Customization/FabricTextureType.cs": """using System;

namespace Bussigo.Game.Customization
{
    public enum FabricTextureType
    {
        ClassicAPSRTCVelourPattern,
        RoyalHeritageFloralWeave,
        ExecutiveSyntheticLeatherette,
        PremiumMemoryFoamSleeper
    }
}
""",
    "Assets/Game/Economy/InsuranceCoverageType.cs": """using System;

namespace Bussigo.Game.Economy
{
    public enum InsuranceCoverageType
    {
        MandatoryThirdPartyLiability,
        ComprehensiveCommercialHull,
        DriverPassengerAccidentCover,
        AllRisksComprehensiveShield
    }
}
""",
    "Assets/Game/Progression/EndorsementSpecialization.cs": """using System;

namespace Bussigo.Game.Progression
{
    public enum EndorsementSpecialization
    {
        HillGhatRoadCertified,
        OvernightMonsoonSpecialist,
        MultiAxleVolvo14MCoach,
        VIPCharterExecutive
    }
}
""",
    "Assets/Game/World/SplineVertexData.cs": """using System;
using Bussigo.Game.Core;

namespace Bussigo.Game.World
{
    public struct SplineVertexData
    {
        public Vector3D Position;
        public Vector3D Normal;
        public Vector2D UV;
    }
}
""",
    "Assets/Game/Economy/JournalEntryLine.cs": """using System;

namespace Bussigo.Game.Economy
{
    public struct JournalEntryLine
    {
        public string AccountCode;
        public string AccountTitle;
        public float DebitAmount;
        public float CreditAmount;
    }
}
""",
    "Assets/Game/Fleet/ServiceMaintenanceTier.cs": """using System;

namespace Bussigo.Game.Fleet
{
    public enum ServiceMaintenanceTier
    {
        GradeA_5000Km_Inspection,
        GradeB_15000Km_EngineOilFilterOverhaul,
        GradeC_45000Km_BrakeLiningAndAirDryer,
        GradeD_100000Km_MajorTransmissionAndDifferential
    }
}
""",
    "Assets/Game/Passengers/CommercialCargoConsignment.cs": """using System;

namespace Bussigo.Game.Passengers
{
    public class CommercialCargoConsignment
    {
        public string ConsignmentTrackingCode { get; set; }
        public string ConsignorName { get; set; }
        public float WeightKg { get; set; }
        public float VolumeM3 { get; set; }
        public string OriginCity { get; set; }
        public string DestinationCity { get; set; }
        public float FreightChargesRupees { get; set; }
    }
}
""",
    "Assets/Game/Passengers/PassengerSeatTypes.cs": """using System;

namespace Bussigo.Game.Passengers
{
    public enum SeatType
    {
        WindowSeat,
        AisleSeat,
        MiddleSeat,
        UpperSleeperBerth,
        LowerSleeperBerth
    }

    public class SeatSlot
    {
        public int SeatNumber { get; set; }
        public SeatType Type { get; set; }
        public bool IsBooked { get; set; } = false;
        public string PassengerName { get; set; }
        public float SeatFareRupees { get; set; }
    }
}
""",
    "Assets/Game/Traffic/SignalPhase.cs": """using System;

namespace Bussigo.Game.Traffic
{
    public enum SignalPhase
    {
        NorthSouthGreen,
        NorthSouthAmber,
        AllRedClearance,
        EastWestGreen,
        EastWestAmber,
        PedestrianWalk
    }
}
""",
    "Assets/Game/Company/StaffRole.cs": """using System;

namespace Bussigo.Game.Company
{
    public enum StaffRole
    {
        SeniorHighwayCaptain,
        CityExpressDriver,
        NightSleeperSpecialist,
        MasterDieselMechanic,
        TicketConductor,
        DepotStationMaster
    }
}
""",
    "Assets/Game/World/BusPlatformBay.cs": """using System;
using Bussigo.Game.Core;

namespace Bussigo.Game.World
{
    public class BusPlatformBay
    {
        public int BayNumber { get; set; }
        public string DestinationSignboardEnglish { get; set; }
        public string DestinationSignboardTelugu { get; set; }
        public bool IsOccupiedByBus { get; set; } = false;
        public Vector3D DockPosition { get; set; }
    }
}
""",
    "Assets/Game/Economy/MonthlyInstallmentRow.cs": """using System;

namespace Bussigo.Game.Economy
{
    public struct MonthlyInstallmentRow
    {
        public int MonthIndex;
        public float MonthlyPaymentEmi;
        public float PrincipalPortion;
        public float InterestPortion;
        public float RemainingPrincipalBalance;
    }
}
"""
}

# Regex patterns to remove duplicate embedded definitions from numbered files
PATTERNS_TO_REMOVE = [
    # FabricTextureType
    r'(\s*public enum FabricTextureType\s*\{[^}]*\})',
    # InsuranceCoverageType
    r'(\s*public enum InsuranceCoverageType\s*\{[^}]*\})',
    # EndorsementSpecialization
    r'(\s*public enum EndorsementSpecialization\s*\{[^}]*\})',
    # SplineVertexData
    r'(\s*public struct SplineVertexData\s*\{[^}]*\})',
    # JournalEntryLine
    r'(\s*public struct JournalEntryLine\s*\{[^}]*\})',
    # ServiceMaintenanceTier
    r'(\s*public enum ServiceMaintenanceTier\s*\{[^}]*\})',
    # CommercialCargoConsignment
    r'(\s*public class CommercialCargoConsignment\s*\{[^}]*\})',
    # SeatType
    r'(\s*public enum SeatType\s*\{[^}]*\})',
    # SeatSlot
    r'(\s*public class SeatSlot\s*\{[^}]*\})',
    # SignalPhase
    r'(\s*public enum SignalPhase\s*\{[^}]*\})',
    # StaffRole
    r'(\s*public enum StaffRole\s*\{[^}]*\})',
    # BusPlatformBay
    r'(\s*public class BusPlatformBay\s*\{[^}]*\})',
    # MonthlyInstallmentRow
    r'(\s*public struct MonthlyInstallmentRow\s*\{[^}]*\})',
]

def resolve_duplicates():
    # 1. Write canonical files
    for path, code in CANONICAL_DEFINITIONS.items():
        os.makedirs(os.path.dirname(path), exist_ok=True)
        with open(path, "w", encoding="utf-8") as fp:
            fp.write(code)
        print(f"Created canonical type file: {path}")

    # 2. Process all numbered files
    target_globs = [
        "Assets/Game/Customization/InteriorSeatFabricPatternSpecification*.cs",
        "Assets/Game/Economy/FleetCommercialInsurancePolicyRecord*.cs",
        "Assets/Game/Progression/DriverCommercialEndorsementModel*.cs",
        "Assets/Game/World/HighwaySplineMeshBuilder*.cs",
        "Assets/Game/Economy/FinancialAccountingJournal*.cs",
        "Assets/Game/Fleet/FleetServiceMaintenanceSchedule*.cs",
        "Assets/Game/Passengers/ParcelCargoTariffCalculator*.cs",
        "Assets/Game/Passengers/PassengerSeatReservationMatrix*.cs",
        "Assets/Game/Traffic/TrafficSignalJunctionController*.cs",
        "Assets/Game/Company/StaffRosterProfileRecord*.cs",
        "Assets/Game/World/BusTerminalLayoutModel*.cs",
        "Assets/Game/Economy/BankLoanAmortizationSchedule*.cs",
    ]

    modified_count = 0
    for g in target_globs:
        for f in glob.glob(g):
            with open(f, "r", encoding="utf-8", errors="ignore") as fp:
                original = fp.read()

            content = original
            for pattern in PATTERNS_TO_REMOVE:
                content = re.sub(pattern, "", content)

            if content != original:
                with open(f, "w", encoding="utf-8") as fp:
                    fp.write(content)
                modified_count += 1

    print(f"\nSuccessfully cleaned duplicate embedded types from {modified_count} numbered files.")

if __name__ == '__main__':
    resolve_duplicates()
