namespace PokerTracker.GCommon
{
    public static class ApplicationConstants
    {
        public const int DefaultPageSize = 12;

        public static class Roles
        {
            public const string Administrator = "Administrator";
        }

        public static class SortOrders
        {
            public const string DateAscending = "date_asc";
            public const string DateDescending = "date_desc";
            public const string NameAscending = "name_asc";
            public const string StatusDefault = "status";
        }

        public static class Formatting
        {
            public const string DetailsDateFormat = "dddd, MMM dd, yyyy";
            public const string DetailsTimeFormat = "HH:mm";
            public const string IndexDateFormat = "MMM dd, HH:mm";
            public const string AddFormDateFormat = "yyyy-MM-ddTHH:mm";
        }
    }
}