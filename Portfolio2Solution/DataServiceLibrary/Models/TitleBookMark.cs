﻿using System;
using System.Collections.Generic;
using System.Text;

namespace DataServiceLibrary.Models
{
    public class TitleBookMark
    {
        public string TitleConst { get; set; }
        public TitleBasics Title { get; set; }
        public int UserId { get; set; }
        public User User {get; set;}
        public string Notes { get; set; }
        public override string ToString()
        {
            return $"User Id: {UserId}, title bookmarked: {TitleConst}, Personal Notes: {Notes}";
        }
    }
}
