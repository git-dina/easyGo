﻿using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System;
using EasyGo.Classes.ApiClasses;

namespace EasyGo.Classes
{
    public class FillCombo
    {

        #region Unit
        static public Unit unit = new Unit();
        static public List<Unit> unitsList;

        static public async Task<IEnumerable<Unit>> RefreshUnits()
        {
            unitsList = await unit.Get();
            return unitsList;
        }
        #endregion
        #region Category
        static public Category category = new Category();
        static public List<Category> categoriesList;

        static public async Task<IEnumerable<Category>> RefreshCategoriesList()
        {
            categoriesList = await category.Get();
            return categoriesList;
        }
        static public async Task FillCategories(ComboBox cb)
        {
            if (categoriesList == null)
                await RefreshCategoriesList();

            cb.ItemsSource = categoriesList.ToList();
            cb.SelectedValuePath = "CategoryId";
            cb.DisplayMemberPath = "Name";
        }
        static public async Task FillCategoriesWithDefault(ComboBox cb)
        {
            if (categoriesList == null)
               await RefreshCategoriesList();

            var lst = categoriesList.ToList();
            var cat = new Category();
            cat.CategoryId = 0;
            cat.Name = "-";
            lst.Insert(0, cat);

            cb.ItemsSource = lst;
            cb.SelectedValuePath = "CategoryId";
            cb.DisplayMemberPath = "Name";
        }
        #endregion

        #region Item Types
        static public List<keyValueString> itemTypesList;
        static public IEnumerable<keyValueString> RefreshItemTypes()
        {
            itemTypesList = new List<keyValueString>() {
                new keyValueString(){key="normal", value=AppSettings.resourcemanager.GetString("trNormal") },
                new keyValueString(){key="service", value=AppSettings.resourcemanager.GetString("trService") },
            };

            return itemTypesList;
        }

        static public void fillItemTypes(ComboBox combo)
        {
            if (itemTypesList is null)
                RefreshItemTypes();

            combo.ItemsSource = itemTypesList;
            combo.SelectedValuePath = "key";
            combo.DisplayMemberPath = "value";
            combo.SelectedIndex = -1;
        }
        #endregion
        #region Item
        static public Item item = new Item();
        static public List<Item> itemsList;

        static public async Task<IEnumerable<Item>> RefreshItems()
        {
            itemsList = await item.Get();
            return itemsList;
        }
        #endregion
        #region User
        static public User user = new User();
        static public List<User> usersList;

        static public async Task<IEnumerable<User>> RefreshUsers()
        {
            usersList = await user.Get();
            return usersList;
        }
        #endregion
        #region Customer
        static public Customer customer = new Customer();
        static public List<Customer> customersList;

        static public async Task<IEnumerable<Customer>> RefreshCustomers()
        {
            customersList = await customer.Get();
            return customersList;
        }
        #endregion
        #region Supplier
        static public Supplier supplier = new Supplier();
        static public List<Supplier> suppliersList;

        static public async Task<IEnumerable<Supplier>> RefreshSuppliers()
        {
            suppliersList = await supplier.Get();
            return suppliersList;
        }
        #endregion

    }
}
