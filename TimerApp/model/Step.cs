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
        public long _id { get; set; } = -1L;
        public string Title { get; set; }
        public string Instruction { get; set; }
        public TimeSpan Time { get; set; }
        public ContinuationMode ContinuationMode { get; set; }

        public bool IsTimed { get { return Time.TotalMilliseconds > 0; } }
        public bool IsTitleOnly { get { return string.IsNullOrWhiteSpace(Instruction); } }

        public static string IntentKey { get; } = "Step";

        public void CopyFrom(Step step)
        {
            _id = step._id;
            Title = step.Title;
            Instruction = step.Instruction;
            Time = step.Time;
            ContinuationMode = step.ContinuationMode;
        }
    }
}