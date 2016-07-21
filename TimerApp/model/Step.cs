using System;

namespace TimerApp.model
{
    enum ContinuationMode
    {
        Automatic,
        Manual
    }

    class Step
    {
        public string Instruction { get; set; }
        public TimeSpan Time { get; set; }
        public ContinuationMode ContinuationMode { get; set; }
    }
}