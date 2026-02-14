using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokerTracker.ViewModels.Tournaments
{
    public class TournamentIndexViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Format { get; set; } = null!; 
        public string Creator { get; set; } = null!; 
        public DateTime Date { get; set; }
        public string Status { get; set; } = "Open";

        public string? ImageUrl { get; set; }
    }
}
