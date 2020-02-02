using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WeddingPlanner.Models
{
    public class Wedding
    {
        [Key]
        public int WeddingId {get; set;}

        [Required(ErrorMessage="A Wedder One must be provided.")]
        public string WedderOne {get; set;}

        [Required(ErrorMessage="A Wedder Two must be provided.")]
        public string WedderTwo {get; set;}

        [Required(ErrorMessage="A wedding date must be provided.")]
        [FutureDate]
        public DateTime Date {get; set;}

        [Required(ErrorMessage="A wedding must have an end time.")]
        [FutureDate]
        public DateTime EndTime {get; set;}
        
        [Required(ErrorMessage="An address must be provided.")]
        public string Address {get; set;}
        public DateTime CreatedAt {get; set;} = DateTime.Now;
        public DateTime UpdatedAt {get; set;} = DateTime.Now;

        ////Navigational Properties

        public int UserId {get; set;}
        public User Planner {get; set;}

        public List<Rsvp> GuestList {get; set;}

    }


    public class FutureDateAttribute : ValidationAttribute
        {

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {

            DateTime check;
            if(value is DateTime)
            {
                check = (DateTime)value;
            }
            else
            {
                return new ValidationResult("Invalid date");
            }
            if(check < DateTime.Now)
            {
                return new ValidationResult("Your wedding date must be in the future!");
            }
            return ValidationResult.Success;
        }
    }


    
}