using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace EventManagementWithAuthentication.Models
{
    public class ApplicationUser : IdentityUser
    {
        public virtual ICollection<Ticket> Tickets { get; set; }
        public virtual ICollection<Feedback> Feedbacks { get; set; }
        public virtual ICollection<Notification> Notifications { get; set; }
    }
}