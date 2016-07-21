using System;
using System.Collections.Generic;

namespace TimerApp.Model
{
    class Recipes
    {
        public static List<Recipe> RecipeList { get; set; } = new List<Recipe> {
            new Recipe()
            {
                Title = "Test recipe",
                Categories = new List<string> { "test", "test1" },
                Description = "description of the recipe",
                Time = new TimeSpan(1, 2, 3),
                Steps = new List<Step> {
                    new Step() { Title = "1", Instruction = "initial step", Time = new TimeSpan(0, 0, 30), ContinuationMode = ContinuationMode.Automatic  },
                    new Step() { Title = "2", Instruction = "second step", Time = new TimeSpan(0, 0, 15), ContinuationMode = ContinuationMode.Automatic  },
                    new Step() { Title = "3", Instruction = "final step", Time = new TimeSpan(0, 0, 0), ContinuationMode = ContinuationMode.Manual }
                }
            },
            new Recipe()
            {
                Title = "Test abc def",
                Categories = new List<string> { "abc", "def" },
                Description = "description of the abc def",
                Time = new TimeSpan(1, 2, 3),
                Steps = new List<Step> {
                    new Step() { Title = "111", Instruction = "aaaaaaaaaaaaaaa", Time = new TimeSpan(0, 1, 30), ContinuationMode = ContinuationMode.Automatic  },
                    new Step() { Title = "222", Instruction = "bbbbbbbbbbbbbbb", Time = new TimeSpan(0, 1, 15), ContinuationMode = ContinuationMode.Automatic  },
                    new Step() { Title = "333", Instruction = "ccccccccccccccc", Time = new TimeSpan(0, 0, 0), ContinuationMode = ContinuationMode.Manual }
                }
            },
        };
    }
}