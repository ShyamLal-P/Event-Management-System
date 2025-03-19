﻿using EventManagementSystem.Models;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace EventManagementWithAuthentication.Models
{
    public class Ticket
    {
        public Guid Id { get; set; }
        public Guid EventId { get; set; }
        public virtual Event Event { get; set; }
        public string UserId { get; set; } // Foreign key to AspNetUsers
        [ForeignKey("UserId")]
        public virtual ApplicationUser User { get; set; }
        public DateOnly BookingDate { get; set; }
        public string Status { get; set; }
        public virtual ICollection<Notification>? Notifications { get; set; }
    }
}