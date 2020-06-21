using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using DUTComputerLabs.API.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace DUTComputerLabs.API.Data
{
    public class Seed
    {
        public static void SeedRoles(DataContext context)
        {
            if(!context.Roles.Any())
            {
                var roleData = System.IO.File.ReadAllText("Data/RoleSeed.json");
                var roles = JsonConvert.DeserializeObject<List<Role>>(roleData);
                foreach (var role in roles)
                {
                    context.Roles.Add(role);
                }

                context.SaveChanges();
            }
        }

        public static void SeedFaculties(DataContext context)
        {
            if(!context.Faculties.Any())
            {
                var facultyData = System.IO.File.ReadAllText("Data/FacultySeed.json");
                var faculties = JsonConvert.DeserializeObject<List<Faculty>>(facultyData);
                foreach (var faculty in faculties)
                {
                    context.Faculties.Add(faculty);
                }

                context.SaveChanges();
            }
        }

        public static void SeedUsers(DataContext context)
        {
            if(!context.Users.Any())
            {
                var userData = System.IO.File.ReadAllText("Data/UserSeed.json");
                var format = "dd-MM-yyyy";
                var dateTimeConverter = new IsoDateTimeConverter{ DateTimeFormat = format };
                var users = JsonConvert.DeserializeObject<List<User>>(userData, dateTimeConverter);
                foreach(var user in users)
                {
                    user.Password = EncryptPassword(user.Password);
                    user.Username = user.Username.ToLower();
                    context.Users.Add(user);
                }

                context.SaveChanges();
            }
        }

        private static string EncryptPassword(string password)
        {
            var md5 = new MD5CryptoServiceProvider();

            md5.ComputeHash(ASCIIEncoding.ASCII.GetBytes(password));

            return Convert.ToBase64String(md5.Hash);
        }
    }
}