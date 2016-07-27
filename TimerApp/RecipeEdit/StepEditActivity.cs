using Android.App;
using Android.Content;
using Android.Graphics.Drawables;
using Android.OS;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;
using Newtonsoft.Json;
using System;
using System.Linq;
using TimerApp.Model;

namespace TimerApp.RecipeEdit
{
    [Activity(Label = "@string/StepEdit")]
    public class StepEditActivity : AppCompatActivity
    {
        Step step;
        EditText titleEditText, timeEditText, instructionEditText;
        RadioButton automaticRadioButton, manualRadioButton;
        Drawable errorDrawable;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.StepEdit);

            errorDrawable = (int)Build.VERSION.SdkInt < 23 ? Resources.GetDrawable(Resource.Drawable.Report) : GetDrawable(Resource.Drawable.Report);

            titleEditText = FindViewById<EditText>(Resource.Id.titleEditText);
            timeEditText = FindViewById<EditText>(Resource.Id.timeEditText);
            instructionEditText = FindViewById<EditText>(Resource.Id.instructionEditText);
            automaticRadioButton = FindViewById<RadioButton>(Resource.Id.automaticRadioButton);
            manualRadioButton = FindViewById<RadioButton>(Resource.Id.manualRadioButton);

            step = Step.DeSerialize(Intent.GetStringExtra(Step.IntentKey));

            updateFields();
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.MenuStepEdit, menu);
            return base.OnCreateOptionsMenu(menu);
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            if (item.ItemId == Resource.Id.MenuCancelItem)
            {
                SetResult(Result.Canceled);
                Finish();
            }
            else if (item.ItemId == Resource.Id.MenuSaveItem)
            {
                if (saveFields())
                {
                    var intent = new Intent();
                    intent.PutExtra(Step.IntentKey, step.Serialize());
                    SetResult(Result.Ok, intent);
                    Finish();
                }
            }
            return base.OnOptionsItemSelected(item);
        }

        protected override void OnResume()
        {
            base.OnResume();

            titleEditText.TextChanged += TitleEditText_TextChanged;
            timeEditText.TextChanged += TimeEditText_TextChanged;
            instructionEditText.TextChanged += InstructionEditText_TextChanged;
            automaticRadioButton.CheckedChange += AutomaticRadioButton_CheckedChange;
            manualRadioButton.CheckedChange += ManualRadioButton_CheckedChange;
        }

        protected override void OnPause()
        {
            titleEditText.TextChanged -= TitleEditText_TextChanged;
            timeEditText.TextChanged -= TimeEditText_TextChanged;
            instructionEditText.TextChanged -= InstructionEditText_TextChanged;
            automaticRadioButton.CheckedChange -= AutomaticRadioButton_CheckedChange;
            manualRadioButton.CheckedChange -= ManualRadioButton_CheckedChange;

            base.OnPause();
        }

        private void TitleEditText_TextChanged(object sender, Android.Text.TextChangedEventArgs e)
        {
            updateLabel();
            if (checkTitle())
            {
                titleEditText.Error = null;
            }
        }

        private void TimeEditText_TextChanged(object sender, Android.Text.TextChangedEventArgs e)
        {
            updateLabel();
            if (checkTime())
            {
                timeEditText.Error = null;
            }
        }

        private void InstructionEditText_TextChanged(object sender, Android.Text.TextChangedEventArgs e)
        {
            updateLabel();
        }

        private void AutomaticRadioButton_CheckedChange(object sender, CompoundButton.CheckedChangeEventArgs e)
        {
            updateLabel();
        }

        private void ManualRadioButton_CheckedChange(object sender, CompoundButton.CheckedChangeEventArgs e)
        {
            updateLabel();
        }

        bool checkTitle()
        {
            var title = titleEditText.Text;
            if (string.IsNullOrWhiteSpace(title))
            {
                titleEditText.SetError("Title can't be empty", errorDrawable);
                titleEditText.RequestFocus();
                return false;
            }
            if (Recipes.RecipeList.Any(x => x.Title == title))
            {
                titleEditText.SetError("Title already in use", errorDrawable);
                titleEditText.RequestFocus();
                return false;
            }
            return true;
        }

        bool checkTime()
        {
            TimeSpan time;
            if (TimeSpan.TryParse(timeEditText.Text, out time))
            {
                return true;
            }
            timeEditText.SetError("Error parsing time, 00:00:00 for untimed", errorDrawable);
            timeEditText.RequestFocus();
            return false;
        }

        void updateFields()
        {
            titleEditText.Text = step.Title;
            timeEditText.Text = step.Time.ToString("hh\\:mm\\:ss");
            instructionEditText.Text = step.Instruction;
            automaticRadioButton.Checked = step.ContinuationMode == ContinuationMode.Automatic;
            manualRadioButton.Checked = step.ContinuationMode == ContinuationMode.Manual;
        }

        bool saveFields()
        {
            if (!checkTitle())
            {
                return false;
            }
            step.Title = titleEditText.Text.ToString();

            if (!checkTime())
            {
                return false;
            }
            TimeSpan time;
            if (TimeSpan.TryParse(timeEditText.Text.ToString(), out time))
            {
                step.Time = time;
            }

            step.ContinuationMode = automaticRadioButton.Checked ? ContinuationMode.Automatic : ContinuationMode.Manual;

            return true;
        }

        void updateLabel()
        {
            //Update the activity title to reflect the status of the step
            if (titleEditText.Text != step.Title ||
                timeEditText.Text != (step.Time.TotalMilliseconds > 0 ? step.Time.ToString("hh\\:mm\\:ss") : "") ||
                instructionEditText.Text != step.Instruction ||
                (manualRadioButton.Checked && step.ContinuationMode != ContinuationMode.Manual) ||
                (automaticRadioButton.Checked && step.ContinuationMode != ContinuationMode.Automatic))
            {
                ActionBar?.SetTitle(Resource.String.StepEditUnSaved);
                SupportActionBar?.SetTitle(Resource.String.StepEditUnSaved);
            }
            else
            {
                ActionBar?.SetTitle(Resource.String.StepEditSaved);
                SupportActionBar?.SetTitle(Resource.String.StepEditSaved);
            }
        }
    }
}