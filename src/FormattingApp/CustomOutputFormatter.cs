using App.Metrics;
using App.Metrics.Formatters;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FormattingApp
{
    public class CustomOutputFormatter : IMetricsOutputFormatter
    {
        public MetricsMediaTypeValue MediaType => new MetricsMediaTypeValue("text", "vnd.custom.metrics", "v1", "plain");

        public MetricFields MetricFields { get; set; }

        public Task WriteAsync(Stream output, MetricsDataValueSource metricsData, CancellationToken cancellationToken = default)
        {
            return Task.CompletedTask;
        }
    }
}
