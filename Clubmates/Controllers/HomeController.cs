using Clubmates.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using static System.Net.Mime.MediaTypeNames;


namespace Clubmates.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            Student student = new Student()
            {
                StudentID = 1,
                StudentName = "John",
                DateOfBirth = new DateTime(1990, 1, 1),
                Height = 5.5M,
                Weight = 150
            };

            List<Student> students = new List<Student>()
            {
                new Student() { StudentID = 1, StudentName = "John", DateOfBirth = new DateTime(1990, 1, 1), Height = 5.5M, Weight = 150 },
                new Student() { StudentID = 2, StudentName = "Steve", DateOfBirth = new DateTime(1992, 1, 1), Height = 6.0M, Weight = 160 },
                new Student() { StudentID = 3, StudentName = "Bill", DateOfBirth = new DateTime(1993, 1, 1), Height = 5.8M, Weight = 155 }
            };

            return View(students);
        }
        public ActionResult AddStudent(string test)
        {
            return View();
        }
    }
}