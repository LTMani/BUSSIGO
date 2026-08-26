using System;
using System.Collections.Generic;

namespace Bussigo.Game.Fleet
{
    public enum ServiceMaintenanceTier
    {
        GradeA_5000Km_Inspection,
        GradeB_15000Km_EngineOilFilterOverhaul,
        GradeC_45000Km_BrakeLiningAndAirDryer,
        GradeD_100000Km_MajorTransmissionAndDifferential
    }

    public class FleetServiceMaintenanceSchedule21
    {
        public string ScheduleId => "MAINT-SCHED-BUS-021";
        public float NextServiceDueKm { get; set; } = 30000.0f;
        public ServiceMaintenanceTier NextServiceGrade { get; set; } = (ServiceMaintenanceTier)(1);
        public float EstimatedServiceCostRupees { get; set; } = 21000.00f;
        public float EstimatedDowntimeHours { get; set; } = 10.0f;

        public bool IsServiceOverdue(float currentOdometerKm)
        {
            return currentOdometerKm >= NextServiceDueKm;
        }
    }
}
