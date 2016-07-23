using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using TimerApp.Model;

namespace TimerApp.RecipeSelect
{
    class RecipeAdapter : BaseAdapter<Recipe>
    {
        Context Context;
        List<Recipe> recipes = new List<Recipe>();
        List<string> filters = new List<string>();
        List<Recipe> filteredRecipes
        {
            get
            {
                if (filters.Count > 0)
                {
                    // take recipes where any filter entry is contained in the category entry
                    return recipes
                        .Where(r => r.Categories.Any(c => filters.Any(f => c.Contains(f))))
                        .ToList();
                }
                return recipes;
            }
        }

        public RecipeAdapter(Context context)
        {
            Context = context;
        }

        public RecipeAdapter(IntPtr handle, JniHandleOwnership transfer) : base(handle, transfer)
        {
        }

        public override Recipe this[int position]
        {
            get
            {
                return filteredRecipes[position];
            }
        }

        public override int Count
        {
            get
            {
                return filteredRecipes.Count;
            }
        }

        public override long GetItemId(int position)
        {
            return 1L; // unused
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            if (convertView == null)
            {
                convertView = View.Inflate(Context, Resource.Layout.RecipeListAdapterView, null);
            }
            convertView.FindViewById<TextView>(Resource.Id.titleTextView).Text = GetItem(position).Title;

            return convertView;
        }

        new public Recipe GetItem(int position)
        {
            return filteredRecipes[position];
        }

        public void Add(Recipe recipe)
        {
            recipes.Add(recipe);
            NotifyDataSetChanged();
        }

        public void UpdateRecipes(IList<Recipe> recipes)
        {
            this.recipes.Clear();
            this.recipes.AddRange(recipes);
            NotifyDataSetChanged();
        }

        public void Remove(Recipe recipe)
        {
            if (recipes.Contains(recipe))
            {
                recipes.Remove(recipe);
                NotifyDataSetChanged();
            }
        }

        public void UpdateFilters(IList<string> filters)
        {
            this.filters.Clear();
            this.filters.AddRange(filters);
            NotifyDataSetChanged();
        }
    }
}