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
    }
}
