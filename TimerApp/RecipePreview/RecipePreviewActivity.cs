using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;
using TimerApp.Database;
using TimerApp.Model;
using TimerApp.RecipeEdit;
using TimerApp.RecipeTimer;
using TimerApp.Utils;

namespace TimerApp.RecipePreview
{
    [Activity(Label = "@string/RecipePreview", Theme = "@style/AppTheme")]
    public class RecipePreviewActivity : AppCompatActivity
    {
        Recipe recipe;
        StepAdapter stepAdapter;
        TextView titleTextView, descriptionTextView, timeTextView, categoriesTextView;
        ListView stepListView;

        public static readonly string ResultIntentKey = "RecipePreviewResult";
        public enum RecipeResult
        {
            Delete,
            Reload
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.Recipe);

            titleTextView = FindViewById<TextView>(Resource.Id.titleTextView);
            descriptionTextView = FindViewById<TextView>(Resource.Id.descriptionTextView);
            timeTextView = FindViewById<TextView>(Resource.Id.timeTextView);
            categoriesTextView = FindViewById<TextView>(Resource.Id.categoriesTextView);
            stepListView = FindViewById<ListView>(Resource.Id.stepListView);
            stepAdapter = new StepAdapter(this);
            stepListView.Adapter = stepAdapter;

            recipe = Recipe.DeSerialize(Intent.GetStringExtra(Recipe.IntentKey));
            UpdateFields();

            SupportActionBar.SetDisplayHomeAsUpEnabled(true);
            SupportActionBar.SetDisplayShowHomeEnabled(true);
        }

        public override bool OnSupportNavigateUp()
        {
            if (!IsFinishing)
            {
                var intent = new Intent();
                intent.PutExtra(Recipe.IntentKey, recipe.Serialize());
                intent.PutExtra(RecipePreviewActivity.ResultIntentKey, (int)RecipeResult.Reload);
                SetResult(Result.Ok, intent);
                Finish();
                return true;
            }
            return false;
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.MenuRecipe, menu);
            return base.OnCreateOptionsMenu(menu);
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            if (item.ItemId == Resource.Id.MenuEditItem)
            {
                var intent = new Intent(this, typeof(RecipeEditActivity));
                intent.PutExtra(Recipe.IntentKey, recipe.Serialize());
                StartActivityForResult(intent, RequestCode.Get(typeof(RecipeEditActivity)));
            }
            else if (item.ItemId == Resource.Id.MenuDeleteItem)
            {
                var intent = new Intent();
                intent.PutExtra(Recipe.IntentKey, recipe.Serialize());
                intent.PutExtra(RecipePreviewActivity.ResultIntentKey, (int)RecipeResult.Delete);
                SetResult(Result.Ok, intent);
                Finish();
            }
            else if (item.ItemId == Resource.Id.MenuStartTimerItem)
            {
                if (!IsFinishing)
                {
                    var intent = new Intent(this, typeof(TimerActivity));
                    intent.PutExtra(Session.IntentKey, new Session(recipe).Serialize());
                    StartActivity(intent);
                }
            }
            return base.OnOptionsItemSelected(item);
        }

        protected async override void OnActivityResult(int requestCode, [GeneratedEnum] Result resultCode, Intent data)
        {
            if (requestCode == RequestCode.Get(typeof(RecipeEditActivity)))
            {
                if (resultCode == Result.Ok)
                {
                    recipe = Recipe.DeSerialize(data.GetStringExtra(Recipe.IntentKey));
                    await SQLiteManager.Update(recipe);
                    UpdateFields();
                }
            }
            base.OnActivityResult(requestCode, resultCode, data);
        }

        void UpdateFields()
        {
            titleTextView.Text = recipe.Title;
            descriptionTextView.Text = recipe.Description;
            timeTextView.Text = recipe.Time.ToString("hh\\:mm\\:ss");
            categoriesTextView.Text = string.Join(", ", recipe.Categories);
            stepAdapter.Update(recipe.Steps);
        }
    }
}