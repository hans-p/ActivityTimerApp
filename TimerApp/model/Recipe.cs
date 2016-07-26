using System;
using System.Collections.Generic;

namespace TimerApp.Model
{
    public class Recipe
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public TimeSpan Time { get; set; }
        public List<string> Categories { get; set; } = new List<string>();
        public List<Step> Steps { get; set; } = new List<Step>();

        public Step GetStepOrNull(int at)
        {
            if (at >= 0 && at < Steps.Count)
            {
                return Steps[at];
            }
            return null;
        }
    }
}