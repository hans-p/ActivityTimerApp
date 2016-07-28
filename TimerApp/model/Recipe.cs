using Newtonsoft.Json;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;

namespace TimerApp.Model
{
    public class Recipe
    {
        [PrimaryKey, AutoIncrement]
        public int? _id { get; set; } = null;
        public string Title { get; set; }
        public string Description { get; set; }
        public TimeSpan Time { get; set; }
        [Ignore]
        public List<string> Categories { get; set; } = new List<string>();
        [Ignore]
        public List<Step> Steps { get; set; } = new List<Step>();
        [Ignore]
        public static string IntentKey { get; } = "Recipe";

        public string CategoriesSqlite
        {
            //hack property for sqlite
            get
            {
                return string.Join(",", Categories);
            }
            set
            {
                Categories.AddRange(value.Split(',').ToList());
            }
        }

        public Step GetStepOrNull(int at)
        {
            if (at >= 0 && at < Steps.Count)
            {
                return Steps[at];
            }
            return null;
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