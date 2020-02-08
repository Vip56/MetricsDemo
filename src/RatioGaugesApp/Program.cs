using App.Metrics;
using App.Metrics.Gauge;
using App.Metrics.Meter;
using App.Metrics.Timer;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace RatioGaugesApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var metrics = new MetricsBuilder()
                .Report.ToConsole()
                .Build();

            var cacheHitRatioGauge = new GaugeOptions
            {
                Name = "Cache Gauge",
                MeasurementUnit = Unit.Calls
            };

            var cacheHitsMeter = new MeterOptions
            {
                Name = "Cache Hits Meter",
                MeasurementUnit = Unit.Calls
            };

            var databaseQueryTimer = new TimerOptions
            {
                Name = "Database Query Timer",
                MeasurementUnit = Unit.Calls,
                DurationUnit = TimeUnit.Milliseconds,
                RateUnit = TimeUnit.Milliseconds
            };

            var cacheHits = metrics.Provider.Meter.Instance(cacheHitsMeter);
            var calls = metrics.Provider.Timer.Instance(databaseQueryTimer);

            var cacheHit = new Random().Next(0, 2) == 0;

            using(calls.NewContext())
            {
                if (cacheHit)
                {
                    cacheHits.Mark(5);
                }

                Thread.Sleep(cacheHit ? 10 : 100);
            }

            var val = cacheHits.GetValueOrDefault();

            metrics.Measure.Gauge.SetValue(cacheHitRatioGauge, () => new HitRatioGauge(cacheHits, calls, m => m.OneMinuteRate));

            Task.WhenAll(metrics.ReportRunner.RunAllAsync());

            Console.ReadKey();
        }
    }
}
