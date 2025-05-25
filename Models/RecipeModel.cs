using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace wongnok_dev.Models
{
    public class RecipeModel
    {
        public string MenuName { get; set; }
        public string Ingredient { get; set; }
        public string Step { get; set; }
        public string Time { get; set; }
        public string Level { get; set; }
        public IFormFile ImageRecipe { get; set; }
        public string ImgName { get; set; }
        public string UserID { get; set; }
        public string RecipeID { get; set; }
    }
}
