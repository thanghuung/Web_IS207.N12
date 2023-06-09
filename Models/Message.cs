﻿using System;

namespace WEB2.Models
{
    public class Message
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Text { get; set; }
        public DateTime Timestamp { get; set; }
        public string ChatId { get; internal set; }
    }
}