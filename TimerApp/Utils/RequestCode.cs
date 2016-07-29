using System;
using System.Collections.Generic;
using TimerApp.RecipeEdit;

namespace TimerApp.Utils
{
    public static class RequestCode
    {
        static List<Type> activitytypes = new List<Type>
        {
            typeof(RecipeEditActivity),
            typeof(StepEditActivity)
        };

        public static int Get(Type type)
        {
            if (activitytypes.Contains(type))
            {
                return activitytypes.IndexOf(type) + 1;
            }
            return 0;
        }
    }
}