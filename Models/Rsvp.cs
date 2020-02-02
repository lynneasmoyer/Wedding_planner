using System;
using System.ComponentModel.DataAnnotations;

namespace WeddingPlanner.Models
{
    public class Rsvp
    {
        [Key]
        public int RsvpId {get; set;}
        public int UserId {get; set;}
        public int WeddingId {get; set;}

        ////Navigational Properties
        public User Guest {get; set;}
        public Wedding Attending {get; set;}
    
    }
}