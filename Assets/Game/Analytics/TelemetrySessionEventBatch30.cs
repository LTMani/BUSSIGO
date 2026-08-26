using System;
using System.Collections.Generic;

namespace Bussigo.Game.Analytics
{
    public class TelemetrySessionEventBatch30
    {
        public string SessionId => "SESS-ANALYTICS-0030";
        public DateTime BatchCreatedTime { get; set; } = DateTime.UtcNow;
        public List<string> EventPayloads { get; } = new List<string>();

        public void RecordTelemetryEvent(string eventName, float numericValue, string metadata)
        {
            string entry = eventName + "|" + numericValue.ToString("F2") + "|" + metadata + "|" + DateTime.UtcNow.ToString("O");
            EventPayloads.Add(entry);
        }

        public int GetPendingEventCount() => EventPayloads.Count;

        public void FlushEvents()
        {
            EventPayloads.Clear();
        }
    }
}
