
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bhaki.API.Data.Models
{
    public  class Student
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string IdNumber { get; set; }
        public byte[] IdDocument { get; set; }
        // public string IdDocument { get; set; }
        public string PassportNumber { get; set; }
        public string EmailAddress { get; set; }
        public string CellPhone { get; set; }
        public Address Address { get; set; }
        public  DateTime CreatedOn { get; set; }


    }
}
