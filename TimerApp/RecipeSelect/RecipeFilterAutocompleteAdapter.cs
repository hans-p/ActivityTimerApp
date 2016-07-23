using Android.Content;
using Android.Runtime;
using Android.Widget;
using System;
using System.Collections.Generic;

namespace TimerApp.RecipeSelect
{
    class RecipeFilterAutocompleteAdapter : ArrayAdapter<string>
    {
        public RecipeFilterAutocompleteAdapter(Context context) : base(context, Android.Resource.Layout.SimpleDropDownItem1Line)
        {
        }

        public RecipeFilterAutocompleteAdapter(IntPtr handle, JniHandleOwnership transfer) : base(handle, transfer)
        {
        }

        public void Update(IList<string> items)
        {
            Clear();
            foreach (var item in items)
            {
                Add(item);
            }
            NotifyDataSetChanged();
        }
    }
}