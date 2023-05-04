using System;
using System.ComponentModel.DataAnnotations;

namespace WEB2.Models {

    public class Calendar {

        [Key]
        public int CarlendarId { get; set; }

        public string Title { get; set; }
        public string Classname { get; set; }
        public DateTime DayStart { get; set; }
        public DateTime DayEnd { get; set; }
    }
}