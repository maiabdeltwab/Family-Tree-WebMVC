using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace FamilyTree.Models
{
    public partial class FamilyTreeContext : DbContext
    {
        public FamilyTreeContext()
            : base("name=FamilyTreeDataBase")
        {
        }

        public virtual DbSet<Person> People { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Person>()
                .HasMany(e => e.FatherChilds)
                .WithOptional(e => e.Father)
                .HasForeignKey(e => e.FatherID);

            modelBuilder.Entity<Person>()
                .HasMany(e => e.MotherChilds)
                .WithOptional(e => e.Mother)
                .HasForeignKey(e => e.MotherId);
        }
    }
}