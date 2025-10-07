using System;
using Assignment3.Models;
using Assignment3.Services;
using Assignment3.Utils;

namespace Assignment3
{
    class Program
    {
        static void Main(string[] args)
        {
            var service = new CategoryService();
            var cat = service.GetCategory(2);

            Console.WriteLine(service.GetCategories().Count);

            if (cat != null)
            {
                Console.WriteLine($"Hello World! {cat.Cid} {cat.Name}");
            }
            else
            {
                Console.WriteLine("Category not found");
            }

            service.UpdateCategory(1, "Hot Beverages");
            var updated = service.GetCategory(1);
            Console.WriteLine($"Updated: {updated.Cid} {updated.Name}");

            service.DeleteCategory(3);
            Console.WriteLine(service.GetCategories().Count);

            Console.WriteLine(service.CreateCategory(4, "Dairy")); // true
            Console.WriteLine(service.CreateCategory(4, "Duplicate")); // false

            foreach (var category in service.GetCategories())
            {
                Console.WriteLine($"{category.Cid}: {category.Name}");
            }

            
        }
    }
}
