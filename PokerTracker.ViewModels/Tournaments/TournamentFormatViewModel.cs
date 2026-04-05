using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokerTracker.ViewModels.Tournaments
{
    /// <summary>
    /// A simple representation of a Tournament Format for display in dropdowns or lists.
    /// </summary>
    public class TournamentFormatViewModel
    {

        public int Id { get; set; }
        public string Name { get; set; } = null!;


    }
}
