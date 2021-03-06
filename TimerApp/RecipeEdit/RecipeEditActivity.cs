using Android.App;
using Android.Content;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.OS;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;
using Newtonsoft.Json;
using System;
using System.Linq;
using TimerApp.Model;
using TimerApp.RecipePreview;
using Android.Runtime;
using TimerApp.Utils;

namespace TimerApp.RecipeEdit
{
    [Activity(Label = "@string/RecipeEdit", Theme = "@style/AppTheme")]
    public class RecipeEditActivity : AppCompatActivity, AdapterView.IOnItemClickListener
    {
        Recipe recipe;
        StepAdapter stepAdapter;
        EditText titleEditText, timeEditText, categoriesEditText, descriptionEditText;
        Drawable errorDrawable;
        int editedStepIndex;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.RecipeEdit);

            titleEditText = FindViewById<EditText>(Resource.Id.titleEditText);
            timeEditText = FindViewById<EditText>(Resource.Id.timeEditText);
            categoriesEditText = FindViewById<EditText>(Resource.Id.categoriesEditText);
            descriptionEditText = FindViewById<EditText>(Resource.Id.descriptionEditText);

            errorDrawable = (int)Build.VERSION.SdkInt < 23 ? Resources.GetDrawable(Resource.Drawable.Report) : GetDrawable(Resource.Drawable.Report);

            recipe = Recipe.DeSerialize(Intent.GetStringExtra(Recipe.IntentKey));
            updateFields();

            stepAdapter = new StepAdapter(this);
            stepAdapter.Update(recipe.Steps);

            var listView = FindViewById<ListView>(Resource.Id.stepListView);
            listView.Adapter = stepAdapter;
            listView.OnItemClickListener = this;
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.MenuRecipeEdit, menu);
            return base.OnCreateOptionsMenu(menu);
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            if (item.ItemId == Resource.Id.MenuAddStepItem)
            {
                editedStepIndex = recipe.Steps.Count;
                var intent = new Intent(this, typeof(StepEditActivity));
                intent.PutExtra(Step.IntentKey, new Step().Serialize());
                StartActivityForResult(intent, RequestCode.Get(typeof(StepEditActivity)));
            }
            else if (item.ItemId == Resource.Id.MenuCancelItem)
            {
                SetResult(Result.Canceled);
                Finish();
            }
            else if (item.ItemId == Resource.Id.MenuSaveItem)
            {
                if (saveFields())
                {
                    var intent = new Intent();
                    intent.PutExtra(Recipe.IntentKey, recipe.Serialize());
                    SetResult(Result.Ok, intent);
                    Finish();
                }
            }
            return base.OnOptionsItemSelected(item);
        }

        #region AdapterView.IOnItemClickListener implementation

        public void OnItemClick(AdapterView parent, View view, int position, long id)
        {
            if (!IsFinishing)
            {
                editedStepIndex = position;
                var intent = new Intent(this, typeof(StepEditActivity));
                intent.PutExtra(Step.IntentKey, JsonConvert.SerializeObject(recipe.Steps[position]).ToString());
                StartActivityForResult(intent, RequestCode.Get(typeof(StepEditActivity)));
            }
        }

        #endregion

        protected override void OnActivityResult(int requestCode, [GeneratedEnum] Result resultCode, Intent data)
        {
            if (requestCode == RequestCode.Get(typeof(StepEditActivity)))
            {
                if (resultCode == Result.Ok)
                {
                    var step = Step.DeSerialize(data.GetStringExtra(Step.IntentKey));
                    if (recipe.Steps.Count == editedStepIndex)
                    {
                        recipe.Steps.Add(step);
                    }
                    else
                    {
                        recipe.Steps[editedStepIndex] = step;
                    }
                    stepAdapter.Update(recipe.Steps);
                    updateLabel();
                }
            }
            base.OnActivityResult(requestCode, resultCode, data);
        }

        protected override void OnResume()
        {
            base.OnResume();

            titleEditText.TextChanged += TitleEditText_TextChanged;
            timeEditText.TextChanged += TimeEditText_TextChanged;
            categoriesEditText.TextChanged += CategoriesEditText_TextChanged;
            descriptionEditText.TextChanged += DescriptionEditText_TextChanged;
        }


        protected override void OnPause()
        {
            titleEditText.TextChanged -= TitleEditText_TextChanged;
            timeEditText.TextChanged -= TimeEditText_TextChanged;
            categoriesEditText.TextChanged -= CategoriesEditText_TextChanged;
            descriptionEditText.TextChanged -= DescriptionEditText_TextChanged;

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

        private void CategoriesEditText_TextChanged(object sender, Android.Text.TextChangedEventArgs e)
        {
            updateLabel();
        }

        private void DescriptionEditText_TextChanged(object sender, Android.Text.TextChangedEventArgs e)
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

        bool checkSteps()
        {
            if (recipe.Steps.Count == 0)
            {
                var builder = new Android.Support.V7.App.AlertDialog.Builder(this);
                builder.SetTitle("Error");
                builder.SetMessage("Can't create a timer with 0 steps");
                builder.SetCancelable(true);
                if (!IsFinishing)
                {
                    var dialog = builder.Create();
                    dialog.Show();
                }
                return false;
            }
            return true;
        }

        void updateFields()
        {
            titleEditText.Text = recipe.Title;
            timeEditText.Text = recipe.Time.ToString("hh\\:mm\\:ss");
            categoriesEditText.Text = string.Join(", ", recipe.Categories);
            descriptionEditText.Text = recipe.Description;
        }

        bool saveFields()
        {
            if (!checkTitle())
            {
                return false;
            }
            recipe.Title = titleEditText.Text.ToString();

            if (!checkTime())
            {
                return false;
            }
            TimeSpan time;
            if (TimeSpan.TryParse(timeEditText.Text.ToString(), out time))
            {
                recipe.Time = time;
            }

            if (!checkSteps())
            {
                return false;
            }

            recipe.Categories = categoriesEditText.Text.ToString()
                .Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(x => x.Trim())
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .ToList();

            recipe.Description = descriptionEditText.Text.ToString();

            return true;
        }

        void updateLabel()
        {
            //Update the activity title to reflect the status of the recipe
            if (titleEditText.Text != recipe.Title ||
                (recipe.Time.TotalMilliseconds > 0 && timeEditText.Text != recipe.Time.ToString("hh\\:mm\\:ss")) ||
                categoriesEditText.Text != (recipe.Categories.Count > 0 ? string.Join(", ", recipe.Categories) : "") ||
                descriptionEditText.Text != recipe.Description ||
                recipe.Steps.Any(x => !stepAdapter.Contains(x)))
            {
                ActionBar?.SetTitle(Resource.String.RecipeEditUnSaved);
                SupportActionBar?.SetTitle(Resource.String.RecipeEditUnSaved);
            }
            else
            {
                ActionBar?.SetTitle(Resource.String.RecipeEditSaved);
                SupportActionBar?.SetTitle(Resource.String.RecipeEditSaved);
            }
        }
    }
}