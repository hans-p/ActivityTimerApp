using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;
using TimerApp.Model;
using TimerApp.RecipeEdit;
using TimerApp.RecipeTimer;
using TimerApp.Utils;

namespace TimerApp.RecipePreview
{
    [Activity(Label = "@string/RecipePreview")]
    public class RecipePreviewActivity : AppCompatActivity
    {
        Recipe recipe;
        StepAdapter stepAdapter;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.Recipe);

            recipe = Recipe.DeSerialize(Intent.GetStringExtra(Recipe.IntentKey));

            FindViewById<TextView>(Resource.Id.titleTextView).Text = recipe.Title;
            FindViewById<TextView>(Resource.Id.descriptionTextView).Text = recipe.Description;
            FindViewById<TextView>(Resource.Id.timeTextView).Text = recipe.Time.ToString("hh\\:mm\\:ss");
            FindViewById<TextView>(Resource.Id.categoriesTextView).Text = string.Join(", ", recipe.Categories);

            stepAdapter = new StepAdapter(this);
            stepAdapter.Update(recipe.Steps);

            var listView = FindViewById<ListView>(Resource.Id.stepListView);
            listView.Adapter = stepAdapter;

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

        protected override void OnActivityResult(int requestCode, [GeneratedEnum] Result resultCode, Intent data)
        {
            if (requestCode == RequestCode.Get(typeof(RecipeEditActivity)))
            {

            }
            base.OnActivityResult(requestCode, resultCode, data);
        }
    }
}