namespace FamilyTree.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Person")]
    public partial class Person
    {
        public Person()
        {
            FatherChilds = new List<Person>();
            MotherChilds = new List<Person>();
        }

        public int Id { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        public bool Gender { get; set; }

        public DateTime? Birthdate { get; set; }

        [ForeignKey("Mother")]
        public int? MotherId { get; set; }

        [ForeignKey("Father")]
        public int? FatherID { get; set; }

        [ForeignKey("Partner")]
        public int? MarriageFrom { get; set; }

        public string ImagePath { get; set; }

        public virtual Person Mother { get; set; }

        public virtual Person Father { get; set; }

        public virtual Person Partner { get; set; }

        public virtual ICollection<Person> FatherChilds { get; set; }

        public virtual ICollection<Person> MotherChilds { get; set; }
    }
}