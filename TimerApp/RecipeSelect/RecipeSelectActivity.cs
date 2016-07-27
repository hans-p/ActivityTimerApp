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
using TimerApp.Model;
using TimerApp.RecipeEdit;
using TimerApp.RecipePreview;
using TimerApp.Utils;

namespace TimerApp.RecipeSelect
{
    [Activity(Label = "@string/RecipeSelectActivity", MainLauncher = true)]
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
            loadRecipes();

            var recipeListView = FindViewById<ListView>(Resource.Id.recipeListView);
            recipeListView.Adapter = recipeAdapter;
            recipeListView.OnItemClickListener = this;

            recipeFilterAutocompleteAdapter = new RecipeFilterAutocompleteAdapter(this);

            recipeListFilterTextView = FindViewById<MultiAutoCompleteTextView>(Resource.Id.recipeListFilterTextView);
            recipeListFilterTextView.SetTokenizer(new MultiAutoCompleteTextView.CommaTokenizer());
            recipeListFilterTextView.Adapter = recipeFilterAutocompleteAdapter;
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

        protected override void OnActivityResult(int requestCode, [GeneratedEnum] Result resultCode, Intent data)
        {
            if (requestCode == RequestCode.Get(typeof(RecipeEditActivity)))
            {
                if (resultCode == Result.Ok)
                {
                    Recipes.AddOrUpdate(Recipe.DeSerialize(data.GetStringExtra(Recipe.IntentKey)));
                    recipeAdapter.UpdateRecipes(Recipes.RecipeList);
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
                var session = new Session(recipeAdapter.GetItem(position));
                Recipes.SessionList.Add(session);
                var sessionPosition = Recipes.SessionList.IndexOf(session);

                var intent = new Intent(this, typeof(RecipePreviewActivity));
                intent.PutExtra("RecipeId", Recipes.RecipeList.IndexOf(recipeAdapter.GetItem(position)));
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
            Recipes.RecipeList.Clear();
            Recipes.AddOrUpdate(recipes);
            recipeAdapter.UpdateRecipes(Recipes.RecipeList);
            recipeFilterAutocompleteAdapter.Update(Recipes.Categories);
        }
    }
}