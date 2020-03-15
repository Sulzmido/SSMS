using InstitutionsAPI.Core.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SchoolManager.ViewModels
{
    public class SubjectViewModel
    {
        public int ID { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 1)]
        public string Name { get; set; }

        [Required]
        public string Category { get; set; }

        public List<SelectListItem> Categories { get; set; }

        public Level Level { get; set; }
    }
}
