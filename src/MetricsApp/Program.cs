using App.Metrics;
using App.Metrics.Counter;
using App.Metrics.Filtering;
using App.Metrics.Scheduling;
using System;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MetricsApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var app = new MetricsBuilder()
                .Configuration.Configure(
                    options =>
                    {
                        options.DefaultContextLabel = "MetricsApp";
                        options.GlobalTags.Add("version", "1.2");
                    }
                )
                .Report.ToConsole()
                .Build();

            //var counter = new CounterOptions()
            //{
            //    Name = "my_counter"
            //};
            //var env = app.EnvironmentInfo;
            //var tags = new MetricTags("server", env.MachineName);
            //app.Measure.Counter.Increment(counter, tags);

            //var filter = new MetricsFilter().WhereType(MetricType.Counter);
            //var snapshot = app.Snapshot.Get(filter);
            //var snapshot = app.Snapshot.Get();

            //using(var stream = new MemoryStream())
            //{
            //    app.DefaultOutputMetricsFormatter.WriteAsync(stream, snapshot).Wait();
            //    var result = Encoding.UTF8.GetString(stream.ToArray());
            //    Console.WriteLine(result);
            //}

            //using(var stream = new MemoryStream())
            //{
            //    app.DefaultOutputEnvFormatter.WriteAsync(stream, app.EnvironmentInfo).Wait();
            //    var result = Encoding.UTF8.GetString(stream.ToArray());
            //    Console.WriteLine(result);
            //}

            //var scheduler = new AppMetricsTaskScheduler(
            //    TimeSpan.FromSeconds(3),
            //    async () =>
            //    {
            //        await Task.WhenAll(app.ReportRunner.RunAllAsync());
            //    }
            //    );

            //scheduler.Start();

            app.Measure.Counter.Increment(MyMetricsRegistry.SampleCounter);

            app.Measure.Gauge.SetValue(MyMetricsRegistry.Errors, 1);

            app.Measure.Histogram.Update(MyMetricsRegistry.SampleHistogram, 1);

            app.Measure.Meter.Mark(MyMetricsRegistry.SampleMeter, 1);

            using(app.Measure.Timer.Time(MyMetricsRegistry.SampleTimer))
            {
                Thread.Sleep(100);
            }

            using(app.Measure.Apdex.Track(MyMetricsRegistry.SampleApdex))
            {
                Thread.Sleep(100);
            }

            Task.WhenAll(app.ReportRunner.RunAllAsync()).Wait();

            Console.ReadKey();
        }
    }
}
