using System;
using System.Collections.Generic;

namespace Bussigo.Game.Fleet
{

    public class FleetServiceMaintenanceSchedule08
    {
        public string ScheduleId => "MAINT-SCHED-BUS-008";
        public float NextServiceDueKm { get; set; } = 15000.0f;
        public ServiceMaintenanceTier NextServiceGrade { get; set; } = (ServiceMaintenanceTier)(0);
        public float EstimatedServiceCostRupees { get; set; } = 8500.00f;
        public float EstimatedDowntimeHours { get; set; } = 4.0f;

        public bool IsServiceOverdue(float currentOdometerKm)
        {
            return currentOdometerKm >= NextServiceDueKm;
        }
    }
}
