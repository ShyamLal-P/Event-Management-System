using EventManagementSystem.Models;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace EventManagementWithAuthentication.Models
{
    public class Feedback
    {
        public Guid Id { get; set; }
        public string UserId { get; set; } // Foreign key to AspNetUsers
        [ForeignKey("UserId")]
        public virtual ApplicationUser User { get; set; }
        public Guid EventId { get; set; }
        public virtual Event Event { get; set; }
        public int Rating { get; set; }
        public string Comments { get; set; }
        public TimeOnly SubmittedTimestamp { get; set; }
    }
}