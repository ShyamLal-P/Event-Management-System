using EventManagementSystem.Models;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace EventManagementWithAuthentication.Models
{
    public class Notification
    {
        public Guid Id { get; set; }
        public string UserId { get; set; } // Foreign key to AspNetUsers
        [ForeignKey("UserId")]
        public virtual ApplicationUser User { get; set; }
        public Guid EventId { get; set; }
        public virtual Event Event { get; set; }
        public Guid TicketId { get; set; }
        public virtual Ticket Ticket { get; set; }
        public string Message { get; set; }
        public TimeOnly SentTime { get; set; }
    }
}