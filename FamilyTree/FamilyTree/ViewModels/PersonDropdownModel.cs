using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FamilyTree.ViewModels
{
    public class PersonDropdownModel
    {
        public int Id { set; get; }
        public string FullName { set; get; }
        public bool IsMarriage { get; set; }
    }
}