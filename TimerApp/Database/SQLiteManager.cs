using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using TimerApp.Model;

namespace TimerApp.Database
{
    public static class SQLiteManager
    {
        static readonly string dbName = "ActivityTimerApp.db3";
        static readonly string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), dbName);

        public static async Task<bool> CreateDatabase()
        {
            try
            {
                var connection = new SQLiteAsyncConnection(path);
                await connection.CreateTableAsync<Recipe>();
                await connection.CreateTableAsync<Step>();
            }
            catch (SQLiteException)
            {
                return false;
            }
            return true;
        }

        public static async Task<bool> Update(Recipe recipe)
        {
            try
            {
                var connection = new SQLiteAsyncConnection(path);
                await connection.InsertOrReplaceAsync(recipe);
                foreach (var step in recipe.Steps)
                {
                    step.recipeId = recipe._id; // link the step to the recipe
                    await Update(step);
                }
            }
            catch (SQLiteException)
            {
                return false;
            }
            return true;
        }

        public static async Task<bool> Update(Step step)
        {
            try
            {
                var connection = new SQLiteAsyncConnection(path);
                await connection.InsertOrReplaceAsync(step);
            }
            catch (SQLiteException)
            {
                return false;
            }
            return true;
        }

        public static async Task<bool> Delete(Recipe recipe)
        {
            try
            {
                var connection = new SQLiteAsyncConnection(path);
                await connection.DeleteAsync(recipe);
                foreach (var step in recipe.Steps)
                {
                    await Delete(step);
                }
            }
            catch (SQLiteException)
            {
                return false;
            }
            return true;
        }

        public static async Task<bool> Delete(Step step)
        {
            try
            {
                var connection = new SQLiteAsyncConnection(path);
                await connection.DeleteAsync(step);
            }
            catch (SQLiteException)
            {
                return false;
            }
            return true;
        }

        public static async Task<List<Recipe>> GetRecipes()
        {
            List<Recipe> recipes = null;
            try
            {
                var connection = new SQLiteAsyncConnection(path);
                recipes = await connection.Table<Recipe>().ToListAsync();
                var steps = await connection.Table<Step>().ToListAsync();

                foreach (var recipe in recipes)
                {
                    foreach (var step in steps)
                    {
                        if (step.recipeId == recipe._id)
                        {
                            recipe.Steps.Add(step);
                        }
                    }
                    recipe.Steps.OrderBy(x => x._id);
                }
            }
            catch (SQLiteException) { }
            return recipes;
        }
    }
}