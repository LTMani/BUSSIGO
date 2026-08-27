using System;
using System.Collections.Generic;

namespace Bussigo.Game.Fleet
{

    public class FleetServiceMaintenanceSchedule27
    {
        public string ScheduleId => "MAINT-SCHED-BUS-027";
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
