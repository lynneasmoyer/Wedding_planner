using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.RegularExpressions;

namespace WeddingPlanner.Models
{
    public class User
    {
        [Key]
        public int UserId {get; set;}

        [Required(ErrorMessage="Please provide your first name.")]
        [MinLength(2, ErrorMessage="Your first name must be at least two characters.")]
        [Display(Name="First Name: ")]
        public string FirstName {get; set;}


        [Required(ErrorMessage="Please provide a last name.")]
        [MinLength(2, ErrorMessage="Your last name must be at least two characters.")]
        [Display(Name="Last Name: ")]
        public string LastName {get; set;}


        [Required(ErrorMessage="Email is required.")]
        [EmailAddress]
        [Display(Name="Email: ")]
        public string Email {get; set;}

        [StrongPassword]
        [Required(ErrorMessage="You must create a password.")]
        [DataType(DataType.Password)]
        [MinLength(8, ErrorMessage="Your password must be at least 8 characters.")]
        [Display(Name="Password: ")]
        public string Password {get; set;}


        [Required(ErrorMessage="Please confirm your password.")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage="Those don't match.")]
        [Display(Name="Confirm Password: ")]
        [NotMapped]
        public string ConfirmPassword {get; set;}

        public DateTime CreatedAt {get; set;} = DateTime.Now;

        public DateTime UpdatedAt {get; set;} = DateTime.Now;



        //////Navigational Properties

        public List<Wedding> PlannedWeddings {get; set;}
        public List<Rsvp> GoingTo {get; set;}
    }

    public class StrongPasswordAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            string check;
            if(value is string)
            {
                check = (string)value;
                Regex VARIABLE = new Regex ("^(?=.*[0-9])(?=.*[a-z])(?=.*[A-Z])(?=.*[#?!@$%^&*-]).{8,20}$");
                Match password = VARIABLE.Match(check);
                if(password.Success)
                {
                    return ValidationResult.Success;
                }
                else
                {
                    return new ValidationResult("Your password must be at least 8 characters and contain at least 1 number, 1 uppercase letter, and 1 special character.");
                }
            }
            else
            {
                return new ValidationResult("Your password must be at least 8 characters and contain at least 1 number, 1 uppercase letter, and 1 special character.");
            }
        }
    }
}