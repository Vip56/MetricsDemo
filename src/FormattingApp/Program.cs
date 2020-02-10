using App.Metrics;
using App.Metrics.Gauge;
using System;
using System.IO;
using System.Text;

namespace FormattingApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var metrics = new MetricsBuilder()
                .OutputMetrics.AsPlainText()
                .OutputMetrics.AsJson()
                .OutputMetrics.Using<CustomOutputFormatter>()
                .Build();

            var gaugeOptions = new GaugeOptions()
            {
                Name = "Test"
            };

            metrics.Measure.Gauge.SetValue(gaugeOptions, 10);

            var snapshot = metrics.Snapshot.Get();

            foreach(var formatter in metrics.OutputMetricsFormatters)
            {
                using(var stream = new MemoryStream())
                {
                    formatter.WriteAsync(stream, snapshot).Wait();
                    var result = Encoding.UTF8.GetString(stream.ToArray());

                    Console.WriteLine(result);
                }
            }



            Console.ReadKey();
        }
    }
}
