﻿using NorthwindEntities; // Product, Category, Supplier classes
using NorthwindSystem.DAL; // NorthwindContext
using System.Collections.Generic; // List<T>
using System.ComponentModel;
using System.Data.SqlClient;
using System.Linq; // for extension methods such as .ToList()
using System;

namespace NorthwindSystem.BLL
{
    // This class is the public access into our system/application
    // that will be used by the website to provide CRUD maintenance
    // for inventory related data.
    [DataObject] // This class is a source for data
    public class InventoryPurchasingController
    {
        #region Countries
        [DataObjectMethod(DataObjectMethodType.Select)]
        public List<CountryName> ListAllCountries()
        {
            using (var context = new NorthwindContext())
            {
                var result = context.Database.SqlQuery<CountryName>("Countries_List");
                return result.ToList();
            }
        }
        #endregion

        #region Product CRUD
        [DataObjectMethod(DataObjectMethodType.Select)]
        public List<Product> ListAllProducts()
        {
            // This "using" statement is different than the "using" at the top of this file.
            // This "using" statement is to ensure that the connection to the database is properly closed after we are done.
            // The variable context is a NorthwindContext object
            // The NorthwindContext class represents a "virtual" database
            using (var context = new NorthwindContext())
            {
                return context.Products.ToList();
            }
        }

        public Product LookupProduct(int productId)
        {
            using (var context = new NorthwindContext())
            {
                return context.Products.Find(productId);
            }
        }

        [DataObjectMethod(DataObjectMethodType.Select)] // to Read objects
        public List<Product> GetProductsByCategory(int searchId)
        {
            using (NorthwindContext context = new NorthwindContext())
            {
                return context // from the context of where I connect to the Db server...
                         .Database // access the database directly to ...
                           .SqlQuery<Product>("EXEC Products_GetByCategories @cat"
                                              , new SqlParameter("cat", searchId))
                             .ToList();
            }
        }

        public List<Product> GetProductsBySupplier(int searchId)
        {
            using (NorthwindContext context = new NorthwindContext())
            {
                return context // from the context of where I connect to the Db server...
                         .Database // access the database directly to ...
                           .SqlQuery<Product>("EXEC Products_GetBySuppliers @sup"
                                              , new SqlParameter("sup", searchId))
                             .ToList();
            }
        }

        public List<Product> GetProductsByPartialName(string name)
        {
            using (var context = new NorthwindContext())
            {
                var result = context // from the context of where I connect to Db Server
                             .Database // access the database directly to...
                             .SqlQuery<Product>( // run an SQL statement
                    "EXEC Products_GetByPartialProductName @PartialName",
                    new SqlParameter("PartialName", name));
                return result.ToList();
            }
        }

        public int AddProduct(Product item) // we could also just return void
        {
            using (var context = new NorthwindContext())
            {
                Product addedItem = context.Products.Add(item);
                context.SaveChanges(); // do the work to save the changes to the database
                return addedItem.ProductID; // because the PK is an Identity column and generated by the database
            }
        }

        public void UpdateProduct(Product item)
        {
            using (var context = new NorthwindContext())
            {
                // The following approach will update the entire Product object in the database
                var existing = context.Entry(item);
                // Tell the context that this object's data is modified
                existing.State = System.Data.Entity.EntityState.Modified;
                // Save the changes
                context.SaveChanges();
            }
        }

        public void DeleteProduct(int id)
        {
            using (var context = new NorthwindContext())
            {
                // The .Find method will look up the specific Product based on the Primary Key value
                var existing = context.Products.Find(id);
                context.Products.Remove(existing);
                context.SaveChanges();
            }
        }

        // Here is an overloaded method that "chains" to the DeleteProduct(int) version
        public void DeleteProduct(Product item)
        {
            DeleteProduct(item.ProductID);
        }
        #endregion

        #region Category CRUD
        [DataObjectMethod(DataObjectMethodType.Select)] // method is for SELECT
        public List<Category> ListAllCategories()
        {
            using (NorthwindContext context = new NorthwindContext())
            {
                return context.Categories.OrderBy(item => item.CategoryName).ToList();
                //                       \[scary] Lambda stuff [next term]/
            }
        }

        public Category LookupCategory(int categoryid)
        {
            using (var context = new NorthwindContext())
            {
                return context.Categories.Find(categoryid);
            }
        }

        public int AddCategory(Category item)
        {
            using (NorthwindContext dbContext = new NorthwindContext())
            {
                Category newItem = dbContext.Categories.Add(item);
                dbContext.SaveChanges();
                return newItem.CategoryID;
            }
        }

        public void UpdateCategory(Category item)
        {
            using (NorthwindContext dbContext = new NorthwindContext())
            {
                var existing = dbContext.Entry(item);
                // Tell the dbContext that this object's data is modified
                existing.State = System.Data.Entity.EntityState.Modified;
                // Save the changes
                dbContext.SaveChanges();
            }
        }

        public void DeleteCategory(int categoryId)
        {
            using (var context = new NorthwindContext())
            {
                var existing = context.Categories.Find(categoryId);
                context.Categories.Remove(existing);
                context.SaveChanges();
            }
        }
        #endregion

        #region Supplier CRUD
        [DataObjectMethod(DataObjectMethodType.Select)]
        public List<Supplier> ListAllSuppliers()
        {
            using (var context = new NorthwindContext())
            {
                return context.Suppliers.ToList();
            }
        }

        public Supplier LookupSupplier(int supplierId)
        {
            using (var context = new NorthwindContext())
            {
                return context.Suppliers.Find(supplierId);
            }
        }

        public int AddSupplier(Supplier item)
        {
            using (NorthwindContext dbContext = new NorthwindContext())
            {
                Supplier newItem = dbContext.Suppliers.Add(item);
                dbContext.SaveChanges();
                return newItem.SupplierID;
            }
        }

        public void UpdateSupplier(Supplier item)
        {
            using (NorthwindContext dbContext = new NorthwindContext())
            {
                var existing = dbContext.Entry(item);
                // Tell the dbContext that this object's data is modified
                existing.State = System.Data.Entity.EntityState.Modified;
                // Save the changes
                dbContext.SaveChanges();
            }
        }

        public void DeleteSupplier(int SupplierId)
        {
            using (var context = new NorthwindContext())
            {
                var existing = context.Suppliers.Find(SupplierId);
                context.Suppliers.Remove(existing);
                context.SaveChanges();
            }
        }
        #endregion
    }
}
