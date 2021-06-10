using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FamilyTree.ViewModels
{
    public class PersonVM
    {
        public int Id { get; set; }

        [Required]
        [MinLength(3, ErrorMessage = "name has minimum length of '3'")]
        [Display(Name = "First name")]
        public string FirstName { get; set; }

        [Required]
        [MinLength(3, ErrorMessage = "name has minimum length of '3'")]
        [Display(Name = "Last name")]
        public string LastName { get; set; }

        [Required]
        public bool Gender { get; set; } = false;

        [Required]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd", ApplyFormatInEditMode = true)]
        [DataType(DataType.Date)]
        public DateTime Birthdate { get; set; } = DateTime.Now;

        [Display(Name = "Mother")]
        public int? MotherId { get; set; }

        [Display(Name = "Father")]
        public int? FatherID { get; set; }

        [Display(Name = "Partner")]
        public int? MarriageFrom { get; set; }

        public HttpPostedFileBase Image { get; set; }

        public string ImagePath { get; set; }

        public bool RemoveImage { get; set; } = true;
    }
}