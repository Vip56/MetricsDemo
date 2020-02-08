using App.Metrics;
using App.Metrics.Counter;
using App.Metrics.Histogram;
using App.Metrics.Meter;
using System;
using System.Collections;
using System.Linq;
using System.Threading.Tasks;

namespace CountersApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var metrics = new MetricsBuilder()
                .Report.ToConsole()
                .Build();

            //var sentEmailsCounter = new CounterOptions()
            //{
            //    Name = "Sent Emails",
            //    MeasurementUnit = Unit.Calls
            //};

            //metrics.Measure.Counter.Increment(sentEmailsCounter, "email-a-friend");
            //metrics.Measure.Counter.Increment(sentEmailsCounter, "forget-password");
            //metrics.Measure.Counter.Increment(sentEmailsCounter, "account-verification");

            //var httpStatusMeter = new MeterOptions()
            //{
            //    Name = "Http Status",
            //    MeasurementUnit = Unit.Calls
            //};

            //metrics.Measure.Meter.Mark(httpStatusMeter, "200");
            //metrics.Measure.Meter.Mark(httpStatusMeter, "500");
            //metrics.Measure.Meter.Mark(httpStatusMeter, "401");

            //metrics.Provider.Meter.Instance(httpStatusMeter).Reset();

            var rnd = new Random();

            var postAndPutRequestSize = new HistogramOptions()
            {
                Name = "Web Request Post & Put Size",
                MeasurementUnit = Unit.Bytes
            };

            foreach(var i in Enumerable.Range(0, 50))
            {
                var t = rnd.Next(0, 10);

                metrics.Measure.Histogram.Update(postAndPutRequestSize, t);
            }

            Task.WhenAll(metrics.ReportRunner.RunAllAsync()).Wait();

            Console.ReadKey();
        }
    }
}
