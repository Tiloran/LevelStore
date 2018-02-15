using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LevelStore.Models
{
    public interface IShareRepository
    {
        IEnumerable<Share> Shares { get; }
        int? SaveShare(Share share);
        void Delete(int shareId);

    }
}

