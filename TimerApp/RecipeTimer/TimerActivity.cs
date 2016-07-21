using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using TimerApp.Model;

namespace TimerApp.RecipeTimer
{
    [Activity(Label = "TimerActivity")]
    public class TimerActivity : Activity
    {
        Recipe recipe;
        int currentStep;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            recipe = Recipes.RecipeList[Intent.GetIntExtra("RecipeId", 0)];
            SetContentView(Resource.Layout.Timer);
        }

        protected override void OnResume()
        {
            base.OnResume();

            loadStep(currentStep);

            FindViewById<Button>(Resource.Id.previousStepButton).Click += PreviousStepButton_Click;
            FindViewById<Button>(Resource.Id.pauseStepTimerButton).Click += PauseStepTimerButton_Click;
            FindViewById<Button>(Resource.Id.nextStepButton).Click += NextStepButton_Click;
            FindViewById<Button>(Resource.Id.stepContinueButton).Click += StepContinueButton_Click;
        }

        protected override void OnPause()
        {
            FindViewById<Button>(Resource.Id.previousStepButton).Click -= PreviousStepButton_Click;
            FindViewById<Button>(Resource.Id.pauseStepTimerButton).Click -= PauseStepTimerButton_Click;
            FindViewById<Button>(Resource.Id.nextStepButton).Click -= NextStepButton_Click;
            FindViewById<Button>(Resource.Id.stepContinueButton).Click -= StepContinueButton_Click;

            base.OnPause();
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