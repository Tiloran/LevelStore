using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using LevelStore.Models;
using Microsoft.Extensions.Logging;
using RecurrentTasks;

namespace LevelStore.Infrastructure.Tasks
{
    public class WorkWihShares : IRunnable
    {
        private ILogger _logger;
        private readonly IShareRepository _repository;

        public WorkWihShares(ILogger<WorkWihShares> logger, IShareRepository repo)
        {
            _logger = logger;
            _repository = repo;
        }

        public void Run(ITask currentTask, CancellationToken cancellationToken)
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
            
        }
    }
}
