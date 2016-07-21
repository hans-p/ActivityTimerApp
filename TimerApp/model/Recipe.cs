using System;
using System.Collections.Generic;

namespace TimerApp.Model
{
    class Recipe
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public TimeSpan Time { get; set; }
        public List<string> Categories { get; set; }
        public List<Step> Steps { get; set; }
    }
}