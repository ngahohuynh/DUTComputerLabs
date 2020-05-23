using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using dotnetMVC.Models;

namespace dotnetMVC.Services
{
    public interface IStudentService
    {
        List<Student> GetStudents();
    }
    public class StudentService : IStudentService
    {
        private DataContext _context;
        public StudentService(DataContext context)
        {
            _context = context;
        }
        public List<Student> GetStudents()
        {
            return _context.Students.ToList();
        }
    }

}