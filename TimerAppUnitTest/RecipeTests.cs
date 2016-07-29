using NUnit.Framework;
using System;
using System.Collections.Generic;
using TimerApp.Model;

namespace TimerAppUnitTest
{
    [TestFixture]
    public class RecipeTests
    {
        [Test]
        public void RecipeSerializeTest()
        {
            var recipe = new Recipe { Steps = new List<Step> { new Step { }, new Step { }, new Step { } } };
            var serialized = recipe.Serialize();
            Assert.False(string.IsNullOrWhiteSpace(serialized), "Serialized string null or whitespace");
        }

        [Test]
        public void RecipeDeSerializeTest()
        {
            var title = "testRecipe";
            var serialized = "{\"Title\": \"" + title + "\"}";
            var recipe = Recipe.DeSerialize(serialized);
            Assert.True(recipe.Title == title, $"Recipe title not {title}, recipe.title:{recipe.Title}");
        }

        [Test]
        public void RecipeSerializeDeseralizeTest()
        {
            var recipe = new Recipe { _id = 3426236, Title = "test title", Time = TimeSpan.Parse("14:15:16"), Description = "test test 1234 124 description", CategoriesSqlite = "cat,12341245,cat2,test1326", Steps = new List<Step> { new Step { _id = 3126, Time = TimeSpan.Parse("01:02:03"), Title = "step1", ContinuationMode = ContinuationMode.Manual, Instruction = "abcdefasf", recipeId = 3426236 } } };
            var serialized = recipe.Serialize();
            var deserialized = Recipe.DeSerialize(serialized);
            Assert.True(recipe._id == deserialized._id, $"Recipe id:{recipe._id} not equal to deserialized:{deserialized._id}");
            Assert.True(recipe.Title == deserialized.Title, $"Recipe title:{recipe.Title} not equal to deserialized:{deserialized.Title}");
            Assert.True(recipe.Time == deserialized.Time, $"Recipe time:{recipe.Time.ToString()} not equal to deserialized:{deserialized.Time.ToString()}");
            Assert.True(recipe.Description == deserialized.Description, $"Recipe description:{recipe.Description} not equal to deserialized:{deserialized.Description}");
            Assert.True(recipe.Categories.Count == deserialized.Categories.Count, $"Recipe categories count:{recipe.Categories.Count} not equal to deserialized:{deserialized.Categories.Count}");
            Assert.True(recipe.CategoriesSqlite == deserialized.CategoriesSqlite, $"Recipe categories:{recipe.CategoriesSqlite} not equal to deserialized:{deserialized.CategoriesSqlite}");

            Assert.True(recipe.Steps.Count == deserialized.Steps.Count, $"Recipe steps count:{recipe.Steps.Count} not equal to deserialized:{deserialized.Steps.Count}");
            Assert.True(recipe.Steps[0].Title == deserialized.Steps[0].Title, $"Recipe step [0] title:{recipe.Steps[0].Title} not equal to deserialized:{deserialized.Steps[0].Title}");
            Assert.True(recipe.Steps[0].Time == deserialized.Steps[0].Time, $"Recipe step [0] Time:{recipe.Steps[0].Time.ToString()} not equal to deserialized:{deserialized.Steps[0].Time.ToString()}");
            Assert.True(recipe.Steps[0]._id == deserialized.Steps[0]._id, $"Recipe step [0] id:{recipe.Steps[0]._id} not equal to deserialized:{deserialized.Steps[0]._id}");
            Assert.True(recipe.Steps[0].recipeId == deserialized.Steps[0].recipeId, $"Recipe step [0] recipeid:{recipe.Steps[0].recipeId} not equal to deserialized:{deserialized.Steps[0].recipeId}");
            Assert.True(recipe.Steps[0].Instruction == deserialized.Steps[0].Instruction, $"Recipe step [0] instruction:{recipe.Steps[0].Instruction} not equal to deserialized:{deserialized.Steps[0].Instruction}");
            Assert.True(recipe.Steps[0].ContinuationMode == deserialized.Steps[0].ContinuationMode, $"Recipe step [0] continuationmode:{recipe.Steps[0].ContinuationMode} not equal to deserialized:{deserialized.Steps[0].ContinuationMode}");
        }
    }
}