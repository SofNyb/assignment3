using System.Collections.Generic;
using System.Linq;
using Assignment3.Models;

namespace Assignment3.Services 
{
    public class CategoryService
    {

        private List<Category> categories = new List<Category> //liste med kategorier
        {
            new Category(1, "Beverages"),
            new Category(2, "Condiments"),
            new Category(3, "Confections")
        };

        public List<Category> GetCategories() //metoden, som returnerer listen
        {
            return categories;
        }


        private Category? FindCategoryById(int id) //hjælpermetoden - kan kun bruges inde i CategoryService - finder ud fra id
        {
            return categories.FirstOrDefault(c => c.Cid == id); //LINQ-kald - returner det første element i listen, som matcher ellers returner null
        }

        public Category? GetCategory(int cid) //?-tegn fortæller compiler, at den godt kan returnere null - nullable reference type
        {
            return FindCategoryById(cid);
        }


        public bool UpdateCategory(int cid, string newName)
        {
            var category = FindCategoryById(cid); //hjælpermetode slår kategorien op
            if (category != null) //hvis der bliver fundet en kategori, ændres navnet
            {
                category.Name = newName;
                return true; //hvis det lykkes
            }
            return false; //hvis det ikke lykkes
        }

        public bool DeleteCategory(int cid)
        {
            var category = FindCategoryById(cid); //hjælpermetode slår kategorien op
            if (category != null) //hvis der bliver fundet en kategori, ændres navnet
            {
                categories.Remove(category); //fjern fra listen
                return true; //hvis det lykkes
            }
            return false; //hvis det ikke lykkes
        }

        public bool CreateCategory(int cid, string name)
        {
            if (FindCategoryById(cid) == null)
            {
                categories.Add(new Category(cid, name)); 
                return true;
            }
            return false;
        }

        public Category CreateCategory(string name)
        {
            // Auto-generate ID by finding the next available ID
            int nextId = categories.Count > 0 ? categories.Max(c => c.Cid) + 1 : 1;
            var category = new Category(nextId, name);
            categories.Add(category);
            return category;
        }
    }
}