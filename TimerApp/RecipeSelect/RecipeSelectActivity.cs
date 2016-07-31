using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TimerApp.Database;
using TimerApp.Model;
using TimerApp.RecipeEdit;
using TimerApp.RecipePreview;
using TimerApp.Utils;

namespace TimerApp.RecipeSelect
{
    [Activity(Label = "@string/ApplicationName", Theme = "@style/AppTheme", MainLauncher = true)]
    public class RecipeSelectActivity : AppCompatActivity, AdapterView.IOnItemClickListener
    {
        RecipeAdapter recipeAdapter;
        RecipeFilterAutocompleteAdapter recipeFilterAutocompleteAdapter;
        MultiAutoCompleteTextView recipeListFilterTextView;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.RecipeList);

            recipeAdapter = new RecipeAdapter(this);

            var recipeListView = FindViewById<ListView>(Resource.Id.recipeListView);
            recipeListView.Adapter = recipeAdapter;
            recipeListView.OnItemClickListener = this;

            recipeFilterAutocompleteAdapter = new RecipeFilterAutocompleteAdapter(this);

            recipeListFilterTextView = FindViewById<MultiAutoCompleteTextView>(Resource.Id.recipeListFilterTextView);
            recipeListFilterTextView.SetTokenizer(new MultiAutoCompleteTextView.CommaTokenizer());
            recipeListFilterTextView.Adapter = recipeFilterAutocompleteAdapter;

            loadRecipes();
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.MenuRecipeList, menu);
            return base.OnCreateOptionsMenu(menu);
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            if (item.ItemId == Resource.Id.MenuAddTimerItem)
            {
                var intent = new Intent(this, typeof(RecipeEditActivity));
                intent.PutExtra(Recipe.IntentKey, new Recipe().Serialize());
                StartActivityForResult(intent, RequestCode.Get(typeof(RecipeEditActivity)));
            }
            return base.OnOptionsItemSelected(item);
        }

        protected async override void OnActivityResult(int requestCode, [GeneratedEnum] Result resultCode, Intent data)
        {
            if (requestCode == RequestCode.Get(typeof(RecipeEditActivity)))
            {
                if (resultCode == Result.Ok)
                {
                    var recipe = Recipe.DeSerialize(data.GetStringExtra(Recipe.IntentKey));
                    await SQLiteManager.Update(recipe);
                    recipeAdapter.Add(recipe);
                    recipeFilterAutocompleteAdapter.Update(recipeAdapter.AllFilters);
                }
            }
            else if (requestCode == RequestCode.Get(typeof(RecipePreviewActivity)))
            {
                if (resultCode == Result.Ok)
                {
                    var recipe = Recipe.DeSerialize(data.GetStringExtra(Recipe.IntentKey));
                    if (data.GetIntExtra(RecipePreviewActivity.ResultIntentKey, -1) == (int)RecipePreviewActivity.RecipeResult.Delete)
                    {
                        await SQLiteManager.Delete(recipe);
                        recipeAdapter.Remove(recipe);
                        recipeFilterAutocompleteAdapter.Update(recipeAdapter.AllFilters);
                    }
                    else if (data.GetIntExtra(RecipePreviewActivity.ResultIntentKey, -1) == (int)RecipePreviewActivity.RecipeResult.Reload)
                    {
                        recipeAdapter.Replace(recipe);
                        recipeFilterAutocompleteAdapter.Update(recipeAdapter.AllFilters);
                    }
                }
            }
            base.OnActivityResult(requestCode, resultCode, data);
        }

        protected override void OnResume()
        {
            base.OnResume();

            recipeListFilterTextView.TextChanged += RecipeListFilterTextView_TextChanged;
        }

        protected override void OnPause()
        {
            recipeListFilterTextView.TextChanged -= RecipeListFilterTextView_TextChanged;

            base.OnPause();
        }

        private void RecipeListFilterTextView_TextChanged(object sender, Android.Text.TextChangedEventArgs e)
        {
            recipeAdapter.UpdateFilters(
                e.Text.ToString()
                .Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(x => x.Trim())
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .ToList());
        }

        #region AdapterView.IOnItemClickListener implementation

        public void OnItemClick(AdapterView parent, View view, int position, long id)
        {
            if (!IsFinishing)
            {
                var intent = new Intent(this, typeof(RecipePreviewActivity));
                intent.PutExtra(Recipe.IntentKey, recipeAdapter.GetItem(position).Serialize());
                StartActivityForResult(intent, RequestCode.Get(typeof(RecipePreviewActivity)));
            }
        }

        #endregion

        async void loadRecipes()
        {
            await SQLiteManager.CreateDatabase();
            var recipes = await SQLiteManager.GetRecipes();

            if (recipes == null || recipes.Count == 0)
            {
                // load default recipes
                string content;
                using (var s = new StreamReader(Assets.Open("DefaultRecipes.json")))
                {
                    content = await s.ReadToEndAsync();
                }
                var json = (JArray)JObject.Parse(content).GetValue("Recipes");
                recipes = json.ToObject<List<Recipe>>();
                foreach (var recipe in recipes)
                {
                    await SQLiteManager.Update(recipe);
                }
            }
            recipeAdapter.UpdateRecipes(recipes);
            recipeFilterAutocompleteAdapter.Update(recipeAdapter.AllFilters);
        }
    }
}