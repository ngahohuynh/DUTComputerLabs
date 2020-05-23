using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace dotnetMVC.Models
{
    [Table("Student")]
    public class Student
    {
        [Key]
        public int StudentId { get; set; }
        public string StudentName { get; set; }
        public DateTime BirthDate { get; set; }
        
        [ForeignKey("Class")]
        public int ClassId { get; set; }
        public Class Class { get; set; }
    }
}