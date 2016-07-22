using System;
using System.Collections.Generic;

namespace TimerApp.Model
{
    class Recipes
    {
        public static List<Recipe> RecipeList { get; set; } = new List<Recipe>();

        public static List<Session> SessionList { get; set; } = new List<Session>();
    }
}