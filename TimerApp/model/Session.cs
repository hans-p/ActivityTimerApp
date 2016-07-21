using System;

namespace TimerApp.Model
{
    class Session
    {
        public Recipe Recipe { get; set; }
        public DateTime SessionStart { get; set; }
        public int CurrentStepIndex { get; set; }

        public bool CanLoadPreviousStep { get { return (CurrentStepIndex - 1) >= 0; } }
        public bool CanLoadNextStep { get { return (CurrentStepIndex + 1) < Recipe.Steps.Count; } }

        public Step CurrentStep { get { return Recipe.Steps[CurrentStepIndex]; } }

        public Session(Recipe recipe)
        {
            Recipe = recipe;
            SessionStart = DateTime.UtcNow;
            CurrentStepIndex = 0;
        }

        public Step GetNextStep()
        {
            CurrentStepIndex += 1;
            return Recipe.Steps[CurrentStepIndex];
        }

        public Step GetPrevStep()
        {
            CurrentStepIndex -= 1;
            return Recipe.Steps[CurrentStepIndex];
        }
    }
}