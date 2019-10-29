﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Northwind.Models
{
    public class LoginModel
    {
        [Required]
        public string Email;
        [Required]
        public string Password;
    }
}