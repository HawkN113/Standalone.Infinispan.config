using System.Net;
using Infinispan.v14.Shared.Models;

namespace Infinispan.v14.Shared.Clients.Abstraction;

internal interface ICacheMonitorClient
{
    Task<StatsModel?> GetStatisticsAsync(NetworkCredential credentials);
}