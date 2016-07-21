using Android.App;
using Android.OS;
using Android.Widget;
using System;
using System.Collections.Generic;
using TimerApp.Model;

namespace TimerApp
{
    [Activity(Label = "TimerApp", /*MainLauncher = true,*/ Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        Recipe recipe;
        int currentStep;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            SetContentView(Resource.Layout.Timer);
        }

        protected override void OnResume()
        {
            base.OnResume();

            recipe = new Recipe()
            {
                Title = "Test recipe",
                Categories = new List<string> { "test", "test1" },
                Description = "description of the recipe",
                Time = new TimeSpan(1, 2, 3),
                Steps = new List<Step> {
                    new Step() { Instruction = "initial step", Time = new TimeSpan(0, 0, 30), ContinuationMode = ContinuationMode.Automatic  },
                    new Step() { Instruction = "second step", Time = new TimeSpan(0, 0, 15), ContinuationMode = ContinuationMode.Automatic  },
                    new Step() { Instruction = "final step", Time = new TimeSpan(0, 0, 0), ContinuationMode = ContinuationMode.Manual }
                }
            };

            loadStep(0);

            FindViewById<Button>(Resource.Id.previousStepButton).Click += PreviousStepButton_Click;
            FindViewById<Button>(Resource.Id.pauseStepTimerButton).Click += PauseStepTimerButton_Click;
            FindViewById<Button>(Resource.Id.nextStepButton).Click += NextStepButton_Click;
            FindViewById<Button>(Resource.Id.stepContinueButton).Click += StepContinueButton_Click;
        }

        protected override void OnPause()
        {
            base.OnPause();

            FindViewById<Button>(Resource.Id.previousStepButton).Click -= PreviousStepButton_Click;
            FindViewById<Button>(Resource.Id.pauseStepTimerButton).Click -= PauseStepTimerButton_Click;
            FindViewById<Button>(Resource.Id.nextStepButton).Click -= NextStepButton_Click;
            FindViewById<Button>(Resource.Id.stepContinueButton).Click -= StepContinueButton_Click;
        }

        private void PreviousStepButton_Click(object sender, EventArgs e)
        {
            if (currentStep > 0)
            {
                loadStep(currentStep - 1);
            }
        }

        private void PauseStepTimerButton_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        void LoadNextStep()
        {
            if (currentStep < (recipe.Steps.Count - 1))
            {
                loadStep(currentStep + 1);
            }
        }

        private void NextStepButton_Click(object sender, EventArgs e)
        {
            LoadNextStep();
        }

        private void StepContinueButton_Click(object sender, EventArgs e)
        {
            LoadNextStep();
        }

        void loadStep(int atIndex)
        {
            currentStep = atIndex;
            var step = recipe.Steps[atIndex];
            FindViewById<TextView>(Resource.Id.stepInstructionTextView).Text = step.Instruction;
            FindViewById<TextView>(Resource.Id.stepTimeTextView).Text = step.Time.ToString("hh\\:mm\\:ss");
            FindViewById<Button>(Resource.Id.stepContinueButton).Enabled = step.ContinuationMode == ContinuationMode.Manual;
            FindViewById<Button>(Resource.Id.previousStepButton).Enabled = currentStep > 0;
            FindViewById<Button>(Resource.Id.pauseStepTimerButton).Enabled = step.Time.TotalSeconds > 0;
            FindViewById<Button>(Resource.Id.nextStepButton).Enabled = currentStep < (recipe.Steps.Count - 1);
        }
    }
}

