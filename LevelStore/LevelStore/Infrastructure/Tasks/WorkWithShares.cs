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
    public class WorkWihShares : IRunnable
    {
        private ILogger logger;
        private readonly IShareRepository repository;

        public WorkWihShares(ILogger<WorkWihShares> logger, IShareRepository repo)
        {
            this.logger = logger;
            repository = repo;
        }

        public void Run(ITask currentTask, CancellationToken cancellationToken)
        {
            List<Share> shares = repository.Shares.Where(e => e.Enabled).ToList();
            if (shares.Count > 0)
            {
                foreach (var share in shares)
                {
                    if (share.DateOfStart > DateTime.Now)
                    {
                        share.Enabled = false;
                        repository.SaveShare(share);
                    }
                }
            }
            
        }
    }
}
