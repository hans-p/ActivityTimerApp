using Newtonsoft.Json;
using SQLite;
using System;

namespace TimerApp.Model
{
    public enum ContinuationMode
    {
        Automatic,
        Manual
    }

    public class Step
    {
        [PrimaryKey, AutoIncrement]
        public int? _id { get; set; } = null;
        public int? recipeId { get; set; } = null;

        public string Title { get; set; }
        public string Instruction { get; set; }
        public TimeSpan Time { get; set; }
        public ContinuationMode ContinuationMode { get; set; }
        [Ignore]
        public bool IsTimed { get { return Time.TotalMilliseconds > 0; } }
        [Ignore]
        public bool IsTitleOnly { get { return string.IsNullOrWhiteSpace(Instruction); } }
        [Ignore]
        public static string IntentKey { get; } = "Step";

        public string Serialize()
        {
            return JsonConvert.SerializeObject(this);
        }

        public static Step DeSerialize(string json)
        {
            return JsonConvert.DeserializeObject<Step>(json);
        }
    }
}