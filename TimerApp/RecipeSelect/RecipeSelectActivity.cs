using Android.App;
using Android.Content;
using Android.OS;
using Android.Views;
using Android.Widget;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TimerApp.Model;
using TimerApp.RecipePreview;

namespace TimerApp.RecipeSelect
{
    [Activity(Label = "RecipeSelectActivity", MainLauncher = true)]
    public class RecipeSelectActivity : Activity, AdapterView.IOnItemClickListener
    {
        RecipeAdapter recipeAdapter;
        RecipeFilterAutocompleteAdapter recipeFilterAutocompleteAdapter;
        MultiAutoCompleteTextView recipeListFilterTextView;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.RecipeList);

            recipeAdapter = new RecipeAdapter(this);
            loadRecipes();

            var recipeListView = FindViewById<ListView>(Resource.Id.recipeListView);
            recipeListView.Adapter = recipeAdapter;
            recipeListView.OnItemClickListener = this;

            recipeFilterAutocompleteAdapter = new RecipeFilterAutocompleteAdapter(this);

            recipeListFilterTextView = FindViewById<MultiAutoCompleteTextView>(Resource.Id.recipeListFilterTextView);
            recipeListFilterTextView.SetTokenizer(new MultiAutoCompleteTextView.CommaTokenizer());
            recipeListFilterTextView.Adapter = recipeFilterAutocompleteAdapter;
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
                e.Text.ToString().Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(x => x.Trim())
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .ToList());
        }

        #region AdapterView.IOnItemClickListener implementation

        public void OnItemClick(AdapterView parent, View view, int position, long id)
        {
            if (!IsFinishing)
            {
                var session = new Session(recipeAdapter.GetItem(position));
                Recipes.SessionList.Add(session);
                var sessionPosition = Recipes.SessionList.IndexOf(session);

                var intent = new Intent(this, typeof(RecipePreviewActivity));
                intent.PutExtra("SessionId", sessionPosition);
                StartActivity(intent);
            }
        }

        #endregion

        async void loadRecipes()
        {
            string content;
            using (var s = new StreamReader(Assets.Open("DefaultRecipes.json")))
            {
                content = await s.ReadToEndAsync();
            }
            var json = (JArray)JObject.Parse(content).GetValue("Recipes");
            var recipes = json.ToObject<List<Recipe>>();
            Recipes.RecipeList.AddRange(recipes);
            recipeAdapter.UpdateRecipes(Recipes.RecipeList);
            recipeFilterAutocompleteAdapter.Update(Recipes.Categories);
        }
    }
}