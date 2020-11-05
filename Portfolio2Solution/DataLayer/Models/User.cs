﻿using System;
using System.ComponentModel.DataAnnotations;

namespace DataLayer.Models
{
    public class User
    {

        public int UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime? Birthday { get; set; }
        public bool IsStaff { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string UserName { get; set; }
        public Address Address { get; set; }

        public override string ToString()
        {
            return $"Id = {UserId}, first name: {FirstName}, birthday: {Birthday.Value.Year}-{Birthday.Value.Month}-{Birthday.Value.Day},"+
                 $" Address: {Address.City}";
        }
    }
}
