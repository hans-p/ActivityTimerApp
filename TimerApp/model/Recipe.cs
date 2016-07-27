using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace TimerApp.Model
{
    public class Recipe
    {
        public long _id { get; set; } = -1L;
        public string Title { get; set; }
        public string Description { get; set; }
        public TimeSpan Time { get; set; }
        public List<string> Categories { get; set; } = new List<string>();
        public List<Step> Steps { get; set; } = new List<Step>();

        public static string IntentKey { get; } = "Recipe";

        public Step GetStepOrNull(int at)
        {
            if (at >= 0 && at < Steps.Count)
            {
                return Steps[at];
            }
            return null;
        }

        public void CopyFrom(Recipe recipe)
        {
            _id = recipe._id;
            Title = recipe.Title;
            Description = recipe.Description;
            Time = recipe.Time;
            Categories = recipe.Categories;
            Steps = recipe.Steps;
        }

        public string Serialize()
        {
            return JsonConvert.SerializeObject(this);
        }

        public static Recipe DeSerialize(string json)
        {
            return JsonConvert.DeserializeObject<Recipe>(json);
        }
    }
}