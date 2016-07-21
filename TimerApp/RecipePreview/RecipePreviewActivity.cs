using Android.App;
using Android.Content;
using Android.OS;
using Android.Widget;
using TimerApp.Model;
using TimerApp.RecipeTimer;

namespace TimerApp.RecipePreview
{
    [Activity(Label = "RecipePreviewActivity")]
    public class RecipePreviewActivity : Activity
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
        }

        protected override void OnResume()
        {
            base.OnResume();

            FindViewById<Button>(Resource.Id.startRecipeButton).Click += StartRecipeButton_Click;
        }

        protected override void OnPause()
        {
            FindViewById<Button>(Resource.Id.startRecipeButton).Click -= StartRecipeButton_Click;

            base.OnPause();
        }

        private void StartRecipeButton_Click(object sender, System.EventArgs e)
        {
            if (!IsFinishing)
            {
                var intent = new Intent(this, typeof(TimerActivity));
                intent.PutExtra("SessionId", Intent.GetIntExtra("SessionId", 0));
                StartActivity(intent);
            }
        }
    }
}