using EventManagementSystem.Models;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace EventManagementWithAuthentication.Models
{
    public class Event
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }
        public string Location { get; set; }
        public DateOnly Date { get; set; }
        public TimeOnly Time { get; set; }
        public int NoOfTickets { get; set; }
        public double EventPrice { get; set; }
        public string OrganizerId { get; set; } // Foreign key to AspNetUsers

        [ForeignKey("OrganizerId")]
        public virtual ApplicationUser? Organizer { get; set; }
        public virtual ICollection<Ticket>? Tickets { get; set; }
        public virtual ICollection<Notification>? Notifications { get; set; }
        public virtual ICollection<Feedback>? Feedbacks { get; set; }
    }
}