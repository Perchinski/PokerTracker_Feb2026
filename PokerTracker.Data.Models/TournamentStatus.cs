using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokerTracker.Data.Models
{
    /// <summary>
    /// Lifecycle: Open → Running → Finished.
    /// Transitions are enforced in TournamentService (StartAsync / FinishAsync).
    /// </summary>
    public enum TournamentStatus
    {
        Open = 0,
        Running = 1,
        Finished = 2
    }
}
