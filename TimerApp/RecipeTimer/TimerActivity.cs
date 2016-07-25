using Android.App;
using Android.Content;
using Android.OS;
using Android.Support.V7.App;
using Android.Widget;
using System;
using System.Timers;
using TimerApp.Model;
using Android.Views;

namespace TimerApp.RecipeTimer
{
    [Activity(Label = "TimerActivity")]
    public class TimerActivity : AppCompatActivity
    {
        Session session;
        Timer timer;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            session = Recipes.SessionList[Intent.GetIntExtra("SessionId", 0)];
            SetContentView(Resource.Layout.Timer);

            SupportActionBar.SetDisplayHomeAsUpEnabled(true);
            SupportActionBar.SetDisplayShowHomeEnabled(true);
        }

        public override bool OnSupportNavigateUp()
        {
            if (!IsFinishing)
            {
                Finish();
                return true;
            }
            return false;
        }

        public override void Finish()
        {
            if (Recipes.SessionList.Contains(session))
            {
                Recipes.SessionList.Remove(session);
            }

            base.Finish();
        }

        protected override void OnResume()
        {
            base.OnResume();

            if (session.CurrentStepStart == default(DateTime))
            {
                updateViewWithStep(session.StartCurrentStep());
            }
            else
            {
                updateViewWithStep(session.CurrentStep);
            }

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

            stopTimer();

            base.OnPause();
        }

        private void PreviousStepButton_Click(object sender, EventArgs e)
        {
            if (session.CanLoadPreviousStep)
            {
                updateViewWithStep(session.StartPrevStep());
            }
        }

        private void PauseStepTimerButton_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        void LoadNextStep()
        {
            if (session.CanLoadNextStep)
            {
                updateViewWithStep(session.StartNextStep());
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

        void updateViewWithStep(Step step)
        {
            if (session.CurrentStep.IsTimed)
            {
                startTimer();
            }
            else
            {
                stopTimer();
            }

            RunOnUiThread(() =>
            {
                FindViewById<TextView>(Resource.Id.stepInstructionTextView).Text = step.IsTitleOnly ? step.Title : step.Instruction;
                FindViewById<TextView>(Resource.Id.stepTimeTextView).Text = step.IsTimed ? session.RemainingStepTime.ToString("hh\\:mm\\:ss") : "";
                FindViewById<Button>(Resource.Id.stepContinueButton).Enabled = step.ContinuationMode == ContinuationMode.Manual;
                FindViewById<Button>(Resource.Id.previousStepButton).Enabled = session.CanLoadPreviousStep;
                FindViewById<Button>(Resource.Id.pauseStepTimerButton).Enabled = step.IsTimed;
                FindViewById<Button>(Resource.Id.nextStepButton).Enabled = session.CanLoadNextStep;
            });
        }

        void stopTimer()
        {
            if (timer != null)
            {
                timer.Stop();
                timer.Dispose();
            }
        }

        void startTimer()
        {
            stopTimer();
            timer = new Timer();
            timer.Interval = 500;
            timer.Elapsed += Timer_Elapsed;
            timer.Start();
        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (session.CanUpdateTimer)
            {
                RunOnUiThread(() =>
                {
                    FindViewById<TextView>(Resource.Id.stepTimeTextView).Text = session.RemainingStepTime.ToString("hh\\:mm\\:ss");
                });
            }
            else
            {
                if (session.CurrentStep.ContinuationMode == ContinuationMode.Automatic)
                {
                    LoadNextStep();
                }
                else
                {
                    stopTimer();
                    //todo notify user timer finished
                }
            }
        }
    }
}