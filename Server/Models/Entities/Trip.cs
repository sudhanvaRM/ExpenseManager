using System;
using System.ComponentModel.DataAnnotations;

namespace Server.Models.Entities
{
    public class Trip
    {
        public Guid TripId { get; set; }
        public string TripName { get; set; }
        public DateTime TripDate { get; set; }
    }
}