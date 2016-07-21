using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using TimerApp.Model;
using Java.Lang;

namespace TimerApp.RecipeSelect
{
    class RecipeAdapter : BaseAdapter<Recipe>
    {
        Context Context;
        List<Recipe> recipes;

        public RecipeAdapter(Context context)
        {
            Context = context;
            recipes = new List<Recipe>();
        }

        public RecipeAdapter(IntPtr handle, JniHandleOwnership transfer) : base(handle, transfer)
        {
        }

        public override Recipe this[int position]
        {
            get
            {
                return recipes[position];
            }
        }

        public override int Count
        {
            get
            {
                return recipes.Count;
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
            return recipes[position];
        }

        public void Add(Recipe recipe)
        {
            recipes.Add(recipe);
            NotifyDataSetChanged();
        }

        public void AddRange(IList<Recipe> recipes)
        {
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
    }
}