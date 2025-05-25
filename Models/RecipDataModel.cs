using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace wongnok_dev.Models
{
    public class RecipDataModel
    {
        public string RecipeID { get; set; }
        public string MenuName { get; set; }
        public string Ingredient { get; set; }
        public string Step { get; set; }
        public string Times { get; set; }
        public string Levels { get; set; }
        public string ImageRecipePath { get; set; }
        public string Rating { get; set; }
        public string UserOwner { get; set; }
        public string Voter { get; set; }

    }
}
