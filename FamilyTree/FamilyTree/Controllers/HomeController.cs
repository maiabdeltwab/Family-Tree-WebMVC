using FamilyTree.Models;
using FamilyTree.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FamilyTree.Controllers
{
    public class HomeController : Controller
    {
        private readonly FamilyTreeContext _context;

        public HomeController()
        {
            _context = new FamilyTreeContext();
        }

        public ActionResult Index()
        {
            var parents = GetParents();

            return View(parents);
        }

        [HttpGet]
        public ActionResult Create()
        {
            ViewBag.MotherList = GetFemales();
            ViewBag.FatherList = GetMales();

            return PartialView(new PersonVM());
        }

        [HttpPost]
        public ActionResult Create(PersonVM personVm)
        {
            if (ModelState.IsValid)
            {
                Person person = new Person
                {
                    FirstName = personVm.FirstName,
                    LastName = personVm.LastName,
                    Gender = personVm.Gender,
                    Birthdate = personVm.Birthdate,
                    FatherID = personVm.FatherID,
                    MotherId = personVm.MotherId
                };
                _context.People.Add(person);
                _context.SaveChanges();

                TempData["message"] = "person created successfully";
            }
            else
            {
                ViewBag.MotherList = GetFemales();

                ViewBag.FatherList = GetMales();

                return PartialView(personVm);
            }

            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }
            var person = _context.People.Find(id);

            if (person == null)
            {
                return HttpNotFound();
            }
            return PartialView(person);
        }

        [HttpGet]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }

            var person = _context.People.Find(id);

            if (person == null)
            {
                return HttpNotFound();
            }

            PersonVM personVM = new PersonVM
            {
                Id = person.Id,
                FirstName = person.FirstName,
                LastName = person.LastName,
                Gender = person.Gender,
                Birthdate = (DateTime)person.Birthdate,
                FatherID = person.FatherID,
                MotherId = person.MotherId,
                MarriageFrom = person.MarriageFrom,
                ImagePath = person.ImagePath
            };

            ViewBag.MotherList = GetFemales();
            ViewBag.FatherList = GetMales();

            if (person.Gender)
            {
                ViewBag.PartnerList = GetFemales().Where(p => !p.IsMarriage || p.Id == person.MarriageFrom).ToList();
            }
            else
            {
                ViewBag.PartnerList = GetMales().Where(p => !p.IsMarriage || p.Id == person.MarriageFrom).ToList();
            }

            return PartialView(personVM);
        }

        [HttpPost]
        public ActionResult Edit(PersonVM personVm)
        {
            if (ModelState.IsValid)
            {
                Person person = _context.People.Find(personVm.Id);

                person.FirstName = personVm.FirstName;
                person.LastName = personVm.LastName;
                person.Gender = personVm.Gender;
                person.Birthdate = personVm.Birthdate;
                person.FatherID = personVm.FatherID;
                person.MotherId = personVm.MotherId;

                if (person.MarriageFrom != personVm.MarriageFrom)
                {
                    if (personVm.MarriageFrom == null)
                    {
                        Person person2 = _context.People.Find(person.MarriageFrom);
                        person2.MarriageFrom = null;
                    }
                    else
                    {
                        if (person.MarriageFrom != null)
                        {
                            Person person2 = _context.People.Find(person.MarriageFrom);
                            person2.MarriageFrom = null;
                        }
                        Person person3 = _context.People.Find(personVm.MarriageFrom);
                        person3.MarriageFrom = person.Id;
                    }
                }

                person.MarriageFrom = personVm.MarriageFrom;

                if (personVm.Image != null)
                {
                    try
                    {
                        if (personVm.Image.ContentLength > 0)
                        {
                            string _FileName = Path.GetFileName(personVm.Image.FileName);
                            string _path = Path.Combine(Server.MapPath("~/Content/upload"), _FileName);
                            personVm.Image.SaveAs(_path);
                            person.ImagePath = _FileName;
                        }
                    }
                    catch
                    {
                        person.ImagePath = null;
                    }
                }
                else if (personVm.RemoveImage)
                {
                    person.ImagePath = null;
                }

                _context.SaveChanges();

                TempData["message"] = "person updated successfully";
            }
            else
            {
                ViewBag.MotherList = GetFemales();
                ViewBag.FatherList = GetMales();
                if (personVm.Gender)
                {
                    ViewBag.PartnerList = GetFemales().Where(p => !p.IsMarriage).ToList();
                }
                else
                {
                    ViewBag.PartnerList = GetMales().Where(p => !p.IsMarriage).ToList();
                }
                return PartialView(personVm);
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult Delete(int? personId)
        {
            if (personId == null)
            {
                return HttpNotFound();
            }

            var person = _context.People.Find(personId);

            if (person == null)
            {
                return HttpNotFound();
            }
            _context.People.Remove(person);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        [NonAction]
        public List<Person> GetParents()
        {
            var parents = _context.People
                .Where(p => (p.FatherID == null && p.MotherId == null)
                 && (p.Partner.MotherId == null && p.Partner.FatherID == null))
                .ToList();

            return parents;
        }

        [NonAction]
        public List<int> ChildsRowWidth(List<Person> Model, int max_col)
        {
            int col_count = 0;
            var row_width = 0;
            for (int j = 0; j < Model.Count; j++)
            {
                if (Model[j].MarriageFrom != null)
                {
                    if (col_count < max_col - 3)
                    {
                        row_width += 292;
                        col_count += 3;
                    }
                    else { break; }
                }
                else
                {
                    if (col_count < max_col - 2)
                    {
                        row_width += 160;
                        col_count += 2;
                    }
                    else { break; }
                }
            }
            return new List<int> { row_width, col_count };
        }

        [NonAction]
        public List<PersonDropdownModel> GetFemales()
        {
            return (from p in _context.People
                    where p.Gender == false
                    select new PersonDropdownModel
                    {
                        Id = p.Id,
                        FullName = p.FatherID != null ?
                        p.FirstName + " " + p.LastName
                        + " " + p.Father.LastName
                        : p.FirstName + " " + p.LastName,
                        IsMarriage = p.MarriageFrom != null
                    }).ToList();
        }

        [NonAction]
        public List<PersonDropdownModel> GetMales()
        {
            return (from p in _context.People
                    where p.Gender == true
                    select new PersonDropdownModel
                    {
                        Id = p.Id,
                        FullName = p.FatherID != null ?
                        p.FirstName + " " + p.LastName
                        + " " + p.Father.LastName
                        : p.FirstName + " " + p.LastName,
                        IsMarriage = p.MarriageFrom != null
                    }).ToList(); ;
        }
    }
}