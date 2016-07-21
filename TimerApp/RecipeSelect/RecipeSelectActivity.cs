using Android.App;
using Android.Content;
using Android.OS;
using Android.Views;
using Android.Widget;
using TimerApp.Model;
using TimerApp.RecipePreview;

namespace TimerApp.RecipeSelect
{
    [Activity(Label = "RecipeSelectActivity", MainLauncher = true)]
    public class RecipeSelectActivity : Activity, AdapterView.IOnItemClickListener
    {
        RecipeAdapter recipeAdapter;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.RecipeList);

            recipeAdapter = new RecipeAdapter(this);
            recipeAdapter.AddRange(Recipes.RecipeList);

            var listView = FindViewById<ListView>(Resource.Id.recipeListView);
            listView.Adapter = recipeAdapter;
            listView.OnItemClickListener = this;
        }

        public void OnItemClick(AdapterView parent, View view, int position, long id)
        {
            if (!IsFinishing)
            {
                var intent = new Intent(this, typeof(RecipePreviewActivity));
                intent.PutExtra("RecipeId", position);
                StartActivity(intent);
            }
        }
    }
}