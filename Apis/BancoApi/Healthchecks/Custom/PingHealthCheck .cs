using Microsoft.Extensions.Diagnostics.HealthChecks;
using System;
using System.Net.NetworkInformation;
using System.Threading;
using System.Threading.Tasks;

namespace BancoApi.Healthchecks.Custom
{
    public class PingHealthCheck : IHealthCheck
    {
        private string _host;
        private int _timeout;

        public PingHealthCheck(string host, int timeout)
        {
            _host = host;
            _timeout = timeout;
        }

        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            try
            {
                using (var ping = new Ping())
                {
                    var reply = await ping.SendPingAsync(_host, _timeout);
                    if (reply.Status != IPStatus.Success)
                    {
                        return new HealthCheckResult(
                                    HealthStatus.Unhealthy,
                                    description: "Servirdor Down!");
                    }

                    if (reply.RoundtripTime >= _timeout)
                    {
                        return new HealthCheckResult(
                                HealthStatus.Degraded,
                                description: "Time out!");
                    }

                    return new HealthCheckResult(
                                HealthStatus.Healthy,
                                description: "Servirdor UP!");
                }
            }
            catch (Exception ex)
            {
                return new HealthCheckResult(
                                    HealthStatus.Unhealthy,
                                    description: $"Servirdor Down!{ex.Message}");
            }
        }
    }
}
