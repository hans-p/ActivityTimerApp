using Android.App;
using Android.Content;
using Android.OS;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;
using TimerApp.Model;
using TimerApp.RecipeTimer;

namespace TimerApp.RecipePreview
{
    [Activity(Label = "RecipePreviewActivity")]
    public class RecipePreviewActivity : AppCompatActivity
    {
        StepAdapter stepAdapter;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.Recipe);

            var session = Recipes.SessionList[Intent.GetIntExtra("SessionId", 0)];

            FindViewById<TextView>(Resource.Id.titleTextView).Text = session.Recipe.Title;
            FindViewById<TextView>(Resource.Id.descriptionTextView).Text = session.Recipe.Description;
            FindViewById<TextView>(Resource.Id.timeTextView).Text = session.Recipe.Time.ToString("hh\\:mm\\:ss");
            FindViewById<TextView>(Resource.Id.categoriesTextView).Text = string.Join(", ", session.Recipe.Categories);

            stepAdapter = new StepAdapter(this);
            stepAdapter.AddRange(session.Recipe.Steps);

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
            if (item.ItemId == Resource.Id.MenuRecipeStartItem)
            {
                if (!IsFinishing)
                {
                    var intent = new Intent(this, typeof(TimerActivity));
                    intent.PutExtra("SessionId", Intent.GetIntExtra("SessionId", 0));
                    StartActivity(intent);
                }
            }
            return base.OnOptionsItemSelected(item);
        }
    }
}