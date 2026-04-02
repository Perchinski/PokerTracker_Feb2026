using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokerTracker.GCommon
{
    /// <summary>
    /// Centralized validation constants shared between entity models and view models
    /// to ensure consistent constraints across the data and presentation layers.
    /// </summary>
    public static class EntityValidation
    {
        public const int MaxTournamentNameLength = 50;
        public const int MinTournamentNameLength = 3;
        public const int MaxTournamentDescriptionLength = 500;
        public const int MaxFormatNameLength = 50;
        public const int MaxImageUrlLength = 2048;

        public const int MinLocationNameLength = 2;
        public const int MaxLocationNameLength = 100;
        public const int MinLocationAddressLength = 2;
        public const int MaxLocationAddressLength = 200;
        public const int MinLocationCityLength = 2;
        public const int MaxLocationCityLength = 100;

        public static class ErrorMessages
        {
            public const string RequiredMessage = "{0} is required.";
            public const string StringLengthMessage = "{0} must be between {2} and {1} characters long.";
        }
    }
}
