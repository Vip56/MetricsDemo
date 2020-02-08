using App.Metrics;
using App.Metrics.Gauge;
using App.Metrics.Timer;
using System;
using System.Collections.Generic;
using System.Text;

namespace OrganizingApp
{
    public static class MyMetrics
    {
        public static class ProcessMetrics
        {
            private static readonly string ContextName = "Process";

            public static GaugeOptions SystemNonPagedMemoryGauge = new GaugeOptions
            {
                Context = ContextName,
                Name = "System Non-Paged Memory",
                MeasurementUnit = Unit.Bytes
            };

            public static GaugeOptions ProcessVirtualMemorySizeGauge = new GaugeOptions
            {
                Context = ContextName,
                Name = "Process Virtual Memory Size",
                MeasurementUnit = Unit.Bytes
            };
        }

        public static class DatabaseMetrics
        {
            private static readonly string ContextName = "Database";

            public static TimerOptions SearchUsersSqlTimer = new TimerOptions
            {
                Context = ContextName,
                Name = "Search Users",
                MeasurementUnit = Unit.Calls
            };
        }
    }
}
