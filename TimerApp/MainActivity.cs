using Android.App;
using Android.Widget;
using Android.OS;
using TimerApp.model;
using System;

namespace TimerApp
{
    [Activity(Label = "TimerApp", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            SetContentView(Resource.Layout.Timer);
        }

        protected override void OnResume()
        {
            base.OnResume();

            var step = new Step() { Instruction = "test instruction", Time = new TimeSpan(0, 15, 32) };

            FindViewById<TextView>(Resource.Id.stepInstructionTextView).Text = step.Instruction;
            FindViewById<TextView>(Resource.Id.stepTimeTextView).Text = step.Time.ToString("hh\\:mm\\:ss");
        }
    }
}

