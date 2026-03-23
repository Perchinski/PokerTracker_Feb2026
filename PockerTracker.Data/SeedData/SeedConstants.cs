using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokerTracker.Data.SeedData
{
    // Fixed GUIDs so seed data is stable across migrations
    public static class SeedConstants
    {
        public const string PlayerOneId = "a1b2c3d4-e5f6-4a7b-8c9d-0e1f2a3b4c5d";
        public const string PlayerTwoId = "f6e5d4c3-b2a1-4f7e-9d8c-1a2b3c4d5e6f";

        public const string AdminOneId = "B22698B8-42A2-4115-9631-1C2D1E2AC5F7";

        public const string UserRoleId = "7D9B7113-A8F8-4035-99A7-A20DD400F6A3";
        public const string AdminRoleId = "2301D884-221A-4E7D-B509-0113DCC043E1";
    }
}
