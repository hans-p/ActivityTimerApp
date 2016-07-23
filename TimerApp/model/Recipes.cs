using System;
using System.Collections.Generic;
using System.Linq;

namespace TimerApp.Model
{
    public class Recipes
    {
        public static List<Recipe> RecipeList { get; set; } = new List<Recipe>();

        public static List<Session> SessionList { get; set; } = new List<Session>();

        public static List<string> Categories { get { return RecipeList.SelectMany(x => x.Categories).ToList(); } }
    }
}