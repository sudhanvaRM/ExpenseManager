using System.ComponentModel.DataAnnotations;

namespace Server.Models.Data
{
    public class AddTrip
    {
        public Guid TripId { get; set; }
        public Guid UserId { get; set; }
    }
}