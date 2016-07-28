using System.Collections.Generic;
using System.Linq;

namespace TimerApp.Model
{
    public class Recipes
    {
        public static List<Recipe> RecipeList { get; set; } = new List<Recipe>();

        public static List<Session> SessionList { get; set; } = new List<Session>();

        public static List<string> Categories { get { return RecipeList.SelectMany(x => x.Categories).ToList(); } }

        public static void AddOrUpdate(Recipe recipe)
        {
            if (recipe._id == -1)
            {
                recipe._id = RecipeList.Count;
                RecipeList.Add(recipe);
            }
            else
            {
                var index = RecipeList.FindIndex(x => x._id == recipe._id);
                if (index > -1)
                {
                    RecipeList[index] = recipe;
                }
                else
                {
                    RecipeList.Add(recipe);
                }
            }
        }

        public static void AddOrUpdate(IList<Recipe> recipes)
        {
            foreach (var recipe in recipes)
            {
                AddOrUpdate(recipe);
            }
        }

        public static bool NameExists(string title, int? _id)
        {
            return RecipeList.Any(x => x._id != _id && x.Title == title);
        }
    }
}