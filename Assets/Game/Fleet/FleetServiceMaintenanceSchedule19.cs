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

    public class FleetServiceMaintenanceSchedule19
    {
        public string ScheduleId => "MAINT-SCHED-BUS-019";
        public float NextServiceDueKm { get; set; } = 60000.0f;
        public ServiceMaintenanceTier NextServiceGrade { get; set; } = (ServiceMaintenanceTier)(3);
        public float EstimatedServiceCostRupees { get; set; } = 46000.00f;
        public float EstimatedDowntimeHours { get; set; } = 22.0f;

        public bool IsServiceOverdue(float currentOdometerKm)
        {
            return currentOdometerKm >= NextServiceDueKm;
        }
    }
}
