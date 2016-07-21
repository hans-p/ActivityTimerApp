using Android.App;
using Android.OS;
using Android.Widget;
using TimerApp.Model;

namespace TimerApp.RecipePreview
{
    [Activity(Label = "RecipePreviewActivity")]
    public class RecipePreviewActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.Recipe);

            var recipe = Recipes.RecipeList[Intent.GetIntExtra("RecipeId", 0)];

            FindViewById<TextView>(Resource.Id.titleTextView).Text = recipe.Title;
            FindViewById<TextView>(Resource.Id.descriptionTextView).Text = recipe.Description;
            FindViewById<TextView>(Resource.Id.timeTextView).Text = recipe.Time.ToString("hh\\:mm\\:ss");
            FindViewById<TextView>(Resource.Id.categoriesTextView).Text = string.Join(", ", recipe.Categories);
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
            throw new System.NotImplementedException();
        }
    }
}