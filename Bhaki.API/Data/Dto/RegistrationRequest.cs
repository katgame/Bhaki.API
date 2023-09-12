using Bhaki.API.Data.Models;
using System;

namespace Bhaki.API.Data.Dto
{
    public class RegistrationRequest
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string IdNumber { get; set; }
        public byte[]? IdDocument { get; set; }
        public string EmailAddress { get; set; }
        public string Cellphone { get; set; }
        public string CourseName { get; set; }
        public string RecieptReference { get; set; }
        public double AmountPaid { get; set; }
        public double Balance { get; set; }
        public Guid BranchId { get; set; }
        public Guid CourseId { get; set; }
        public Guid CreatedBy { get; set; }
        public Address Address { get; set; }
    }
}
