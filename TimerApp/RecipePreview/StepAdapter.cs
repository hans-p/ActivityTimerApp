using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using TimerApp.Model;

namespace TimerApp.RecipePreview
{
    class StepAdapter : BaseAdapter<Step>
    {
        Context Context;
        List<Step> steps;

        public StepAdapter(Context context)
        {
            Context = context;
            steps = new List<Step>();
        }

        public StepAdapter(IntPtr handle, JniHandleOwnership transfer) : base(handle, transfer)
        {
        }

        public override Step this[int position]
        {
            get
            {
                return steps[position];
            }
        }

        public override int Count
        {
            get
            {
                return steps.Count;
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
                convertView = View.Inflate(Context, Resource.Layout.StepListAdapterView, null);
            }
            var step = GetItem(position);
            convertView.FindViewById<TextView>(Resource.Id.titleTextView).Text = step.Title;
            convertView.FindViewById<TextView>(Resource.Id.timeTextView).Text = step.IsTimed ? step.Time.ToString("hh\\:mm\\:ss") : "Untimed";

            return convertView;
        }

        new public Step GetItem(int position)
        {
            return steps[position];
        }

        public void Add(Step step)
        {
            steps.Add(step);
            NotifyDataSetChanged();
        }

        public void AddRange(IList<Step> steps)
        {
            this.steps.AddRange(steps);
            NotifyDataSetChanged();
        }

        public void Remove(Step step)
        {
            if (steps.Contains(step))
            {
                steps.Remove(step);
                NotifyDataSetChanged();
            }
        }
    }
}