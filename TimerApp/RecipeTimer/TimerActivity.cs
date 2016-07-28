using Android.App;
using Android.Content;
using Android.OS;
using Android.Support.V7.App;
using Android.Widget;
using System;
using System.Timers;
using TimerApp.Model;
using Android.Views;
using Android.Graphics;

namespace TimerApp.RecipeTimer
{
    [Activity(Label = "TimerActivity")]
    public class TimerActivity : AppCompatActivity
    {
        Session session;
        Timer timer;
        PorterDuffColorFilter buttonProceedFilter;
        ImageButton previousStepButton, nextStepButton;
        TextView stepTimeTextView, stepTitleTextView, stepInstructionTextView, next0StepLabelTextView, next0StepTextView, next1StepLabelTextView, next1StepTextView;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            session = Session.DeSerialize(Intent.GetStringExtra(Session.IntentKey));
            SetContentView(Resource.Layout.Timer);

            SupportActionBar.SetDisplayHomeAsUpEnabled(true);
            SupportActionBar.SetDisplayShowHomeEnabled(true);

            var color = new Color((int)Build.VERSION.SdkInt < 23 ? Resources.GetColor(Resource.Color.colorAccent) : GetColor(Resource.Color.colorAccent));
            buttonProceedFilter = new PorterDuffColorFilter(color, PorterDuff.Mode.SrcAtop);

            previousStepButton = FindViewById<ImageButton>(Resource.Id.previousStepButton);
            nextStepButton = FindViewById<ImageButton>(Resource.Id.nextStepButton);
            stepTimeTextView = FindViewById<TextView>(Resource.Id.stepTimeTextView);
            stepTitleTextView = FindViewById<TextView>(Resource.Id.stepTitleTextView);
            stepInstructionTextView = FindViewById<TextView>(Resource.Id.stepInstructionTextView);
            next0StepLabelTextView = FindViewById<TextView>(Resource.Id.next0StepLabelTextView);
            next0StepTextView = FindViewById<TextView>(Resource.Id.next0StepTextView);
            next1StepLabelTextView = FindViewById<TextView>(Resource.Id.next1StepLabelTextView);
            next1StepTextView = FindViewById<TextView>(Resource.Id.next1StepTextView);
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

            previousStepButton.Click += PreviousStepButton_Click;
            nextStepButton.Click += NextStepButton_Click;
        }

        protected override void OnPause()
        {
            previousStepButton.Click -= PreviousStepButton_Click;
            nextStepButton.Click -= NextStepButton_Click;

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
                stepTitleTextView.Text = step.Title;
                stepInstructionTextView.Text = step.Instruction;
                stepTimeTextView.Text = step.IsTimed ? session.RemainingStepTime.ToString("hh\\:mm\\:ss") : "";
                previousStepButton.Enabled = session.CanLoadPreviousStep;
                nextStepButton.Enabled = session.CanLoadNextStep;
                if (step.IsTimed)
                {
                    nextStepButton.ClearColorFilter();
                }
                else
                {
                    nextStepButton.SetColorFilter(buttonProceedFilter);
                }
                var next0 = session.Recipe.GetStepOrNull(session.CurrentStepIndex + 1);
                var next1 = session.Recipe.GetStepOrNull(session.CurrentStepIndex + 2);
                next0StepLabelTextView.Visibility = (next0 == null) ? ViewStates.Invisible : ViewStates.Visible;
                next0StepTextView.Text = next0?.Title ?? "";
                next1StepLabelTextView.Visibility = (next1 == null) ? ViewStates.Invisible : ViewStates.Visible;
                next1StepTextView.Text = next1?.Title ?? "";
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
                    stepTimeTextView.Text = session.RemainingStepTime.ToString("hh\\:mm\\:ss");
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
                    RunOnUiThread(() =>
                    {
                        nextStepButton.SetColorFilter(buttonProceedFilter);
                    });
                    //todo notify user timer finished
                }
            }
        }
    }
}