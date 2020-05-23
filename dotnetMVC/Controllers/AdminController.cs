using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using dotnetMVC.Models;
using dotnetMVC.Services;
using Microsoft.AspNetCore.Mvc;

namespace dotnetMVC.Controllers
{
    public class AdminController : Controller
    {
        // private IStudentService _studentService;
        // public StudentController(IStudentService studentService)
        // {
        //     _studentService = studentService;
        // }
        public IActionResult ChangPass()
        {
            return View();
        }

    }
}