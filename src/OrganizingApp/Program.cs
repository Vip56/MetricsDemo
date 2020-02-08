using App.Metrics;
using App.Metrics.Gauge;
using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace OrganizingApp
{
    class Program
    {
        static void Main(string[] args)
        {
            // 将会自动将 app（程序集名）、server（机器名）和env（运行环境）作为标签
            var metrics = AppMetrics.CreateDefaultBuilder()
                .Report.ToConsole()
                .Build();

            //metrics.Measure.Gauge.SetValue(MyMetrics.ProcessMetrics.SystemNonPagedMemoryGauge, 2);

            //metrics.Measure.Gauge.SetValue(MyMetrics.ProcessMetrics.ProcessVirtualMemorySizeGauge, 20);

            //using (metrics.Measure.Timer.Time(MyMetrics.DatabaseMetrics.SearchUsersSqlTimer))
            //{
            //    Thread.Sleep(100);
            //}

            var process = Process.GetCurrentProcess();

            var derivedGauge = new GaugeOptions
            {
                Name = "Derived Gauge",
                MeasurementUnit = Unit.MegaBytes
            };

            var processPhysicalMemoryGauge = new GaugeOptions
            {
                Name = "Process Physical Memory",
                MeasurementUnit = Unit.Bytes
            };

            var physicalMemoryGauge = new FunctionGauge(() => process.WorkingSet64);

            metrics.Measure.Gauge.SetValue(derivedGauge, () => new DerivedGauge(physicalMemoryGauge, g => g / 1024 / 1024));

            metrics.Measure.Gauge.SetValue(processPhysicalMemoryGauge, () => physicalMemoryGauge);

            Task.WhenAll(metrics.ReportRunner.RunAllAsync()).Wait();

            Console.ReadKey();
        }
    }
}
