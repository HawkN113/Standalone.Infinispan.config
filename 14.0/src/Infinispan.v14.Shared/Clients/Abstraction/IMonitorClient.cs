﻿using Infinispan.v14.Shared.Models;

namespace Infinispan.v14.Shared.Clients.Abstraction;

public interface IMonitorClient
{
    Task<StatsModel?> GetStatisticsAsync();
}