using System;
using System.Collections.Generic;

namespace Bussigo.Game.Fleet
{

    public class FleetServiceMaintenanceSchedule09
    {
        public string ScheduleId => "MAINT-SCHED-BUS-009";
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
