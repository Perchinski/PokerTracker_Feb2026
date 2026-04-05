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
        public static class Tournament
        {
            public const int MaxNameLength = 50;
            public const int MinNameLength = 3;
            public const int MaxDescriptionLength = 500;
        }

        public static class TournamentFormat
        {
            public const int MaxNameLength = 50;
        }

        public static class Location
        {
            public const int MinNameLength = 2;
            public const int MaxNameLength = 100;
            public const int MinAddressLength = 2;
            public const int MaxAddressLength = 200;
            public const int MinCityLength = 2;
            public const int MaxCityLength = 100;
        }

        public static class Shared
        {
            public const int MaxImageUrlLength = 2048;
        }

        public static class ErrorMessages
        {
            public const string RequiredMessage = "{0} is required.";
            public const string StringLengthMessage = "{0} must be between {2} and {1} characters long.";
            
            public const string SelectFormatMessage = "Please select a format.";
            public const string ValidFormatMessage = "Please select a valid format.";
            public const string SelectLocationMessage = "Please select a location.";
            public const string ValidLocationMessage = "Please select a valid location.";
            public const string SelectWinnerMessage = "Please select a winner.";
            public const string DateInPastMessage = "The selected tournament date cannot be more than 1 day in the past.";
        }
    }
}
