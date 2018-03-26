using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using LevelStore.Models;
using Microsoft.Extensions.Logging;
using RecurrentTasks;

namespace LevelStore.Infrastructure.Tasks
{
    public class WorkWithShares : IRunnable
    {
        private ILogger _logger;
        private readonly IShareRepository _repository;

        public WorkWithShares(ILogger<WorkWithShares> logger, IShareRepository repo)
        {
            _logger = logger;
            _repository = repo;
        }

       
        public Task RunAsync(ITask currentTask, IServiceProvider scopeServiceProvider, CancellationToken cancellationToken)
        {
            List<Share> shares = _repository.Shares.Where(e => e.Enabled).ToList();
            if (shares.Count > 0)
            {
                foreach (var share in shares)
                {
                    if (share.DateOfStart > DateTime.Now)
                    {
                        share.Enabled = false;
                        _repository.SaveShare(share);
                    }
                }
            }
            return Task.CompletedTask;
        }
    }
}
