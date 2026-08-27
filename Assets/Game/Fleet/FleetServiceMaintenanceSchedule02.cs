using System;
using System.Collections.Generic;

namespace Bussigo.Game.Fleet
{

    public class FleetServiceMaintenanceSchedule02
    {
        public string ScheduleId => "MAINT-SCHED-BUS-002";
        public float NextServiceDueKm { get; set; } = 45000.0f;
        public ServiceMaintenanceTier NextServiceGrade { get; set; } = (ServiceMaintenanceTier)(2);
        public float EstimatedServiceCostRupees { get; set; } = 33500.00f;
        public float EstimatedDowntimeHours { get; set; } = 16.0f;

        public bool IsServiceOverdue(float currentOdometerKm)
        {
            return currentOdometerKm >= NextServiceDueKm;
        }
    }
}
