﻿using System.ComponentModel.DataAnnotations;

namespace Cw3.Animal
{
    public class PutAdnimal
    {
        [Required]
        [MaxLength(200)]
        public string Name { get; set; }
        [Required]
        [MaxLength(200)]
        public string Description { get; set; }
        [Required]
        [MaxLength(200)]
        public string Category { get; set; }
        [Required]
        [MaxLength(200)]
        public string Area { get; set; }
    }
}