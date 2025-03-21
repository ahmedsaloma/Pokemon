﻿using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Net;

namespace Pokemon.Models
{
    public class AppUser : IdentityUser
    {

        

        public int? Pace { get; set; }
        public int? Mileage { get; set; }

        public string? ProfileImageUrl { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }

        [ForeignKey("Address")]

        public int? AdressId {  get; set; }

        public Address? Address { get; set; }

        public ICollection<Club> clubs { get; set; }
        public ICollection<Race>Races { get; set; }

    }
}
