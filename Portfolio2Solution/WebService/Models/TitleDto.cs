﻿using System;
namespace WebService.Models
{
    public class TitleDto
    {
        public string Type { get; set; }
        public string PrimaryTitle { get; set; }
        public string StartYear { get; set; }
        public int? Runtime { get; set; }
        public string Genre { get; set; }
        public string DetailsUrl { get; set; }
    }
}
