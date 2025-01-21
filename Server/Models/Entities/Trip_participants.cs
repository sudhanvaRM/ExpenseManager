using System;

namespace Server.Models.Entities
{
    public class Trip_Participants
    {
        public Guid TripId { get; set; }
        public Guid UserId { get; set; }

        public Trip Trip { get; set; }
        public Users User { get; set; }
    }
}