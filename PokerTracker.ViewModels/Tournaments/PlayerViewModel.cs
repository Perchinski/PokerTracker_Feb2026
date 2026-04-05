using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokerTracker.ViewModels.Tournaments
{
    /// <summary>
    /// Represents a user enrolled as a player in a tournament.
    /// </summary>
    public class PlayerViewModel
    {
        public string Id { get; set; } = null!;
        public string Name { get; set; } = null!;
    }
}
