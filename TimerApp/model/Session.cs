using System;

namespace TimerApp.Model
{
    class Session
    {
        public Recipe Recipe { get; set; }
        public DateTime SessionStart { get; set; }
        public int CurrentStepIndex { get; set; }
        public DateTime CurrentStepStart { get; set; }

        public bool CanLoadPreviousStep { get { return (CurrentStepIndex - 1) >= 0; } }
        public bool CanLoadNextStep { get { return (CurrentStepIndex + 1) < Recipe.Steps.Count; } }

        public Step CurrentStep { get { return Recipe.Steps[CurrentStepIndex]; } }
        public TimeSpan RemainingStepTime { get { return CurrentStep.Time - (DateTime.UtcNow - CurrentStepStart); } }
        public bool CanUpdateTimer { get { return RemainingStepTime >= TimeSpan.Zero; } }

        public Session(Recipe recipe)
        {
            Recipe = recipe;
            SessionStart = DateTime.UtcNow;
            CurrentStepStart = DateTime.UtcNow;
            CurrentStepIndex = 0;
        }

        public Step StartCurrentStep()
        {
            CurrentStepStart = DateTime.UtcNow;
            return Recipe.Steps[CurrentStepIndex];
        }

        public Step StartNextStep()
        {
            CurrentStepIndex += 1;
            CurrentStepStart = DateTime.UtcNow;
            return Recipe.Steps[CurrentStepIndex];
        }

        public Step StartPrevStep()
        {
            CurrentStepIndex -= 1;
            CurrentStepStart = DateTime.UtcNow;
            return Recipe.Steps[CurrentStepIndex];
        }
    }
}