using System.Collections.Generic;
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

        static public async Task FillUnits(ComboBox cmb)
        {
            if (unitsList is null)
                await RefreshUnits();
            cmb.ItemsSource = unitsList.ToList();
            cmb.SelectedValuePath = "UnitId";
            cmb.DisplayMemberPath = "Name";
        }
        static public async Task FillUnitsWithDefault(ComboBox cmb)
        {
            if (unitsList is null)
                await RefreshUnits();

            var lst = unitsList.ToList();
            var cat = new Unit();
            cat.UnitId = 0;
            cat.Name = "-";
            lst.Insert(0, cat);

            cmb.ItemsSource = lst;
            cmb.SelectedValuePath = "UnitId";
            cmb.DisplayMemberPath = "Name";
        }
        #endregion
        #region Category
        static public Category category = new Category();
        static public List<Category> categoriesList;
        static public List<Category> categoriesFirstLevelList;

        static public async Task<IEnumerable<Category>> RefreshCategoriesList()
        {
            categoriesList = await category.Get();
            if (categoriesList != null)
                categoriesFirstLevelList = categoriesList.Where(x => x.ParentId == null).ToList();
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
        static public async Task FillCategoriesWithDefault(ComboBox cb, int? categoryId =null)
        {
            if (categoriesList == null)
               await RefreshCategoriesList();

            var lst = categoriesList.ToList();

            if (categoryId != null)
                lst = lst.Where(x => x.CategoryId != categoryId).ToList();


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
        static public List<Item> itemsHasUnitsList;

        static public async Task<IEnumerable<Item>> RefreshItems()
        {
            itemsList = await item.Get();
            if (itemsHasUnitsList != null)
                itemsHasUnitsList = itemsList.Where(x => x.ItemUnits != null && x.ItemUnits.Count > 0).ToList();
            return itemsList;
        }

        static public async Task<IEnumerable<Item>> RefreshItemsHasUnits()
        {
            itemsHasUnitsList = await item.GetWithUnits();
            return itemsHasUnitsList;
        }
        #endregion

        #region item units
        static public ItemUnit itemUnit = new ItemUnit();
        static public List<ItemUnit> itemUnitList;

        static public async Task<List<ItemUnit>> RefreshItemUnit()
        {
            itemUnitList = await itemUnit.Get();
            return itemUnitList;
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

        static public async Task fillSuppliersList(ComboBox combo)
        {
            if (suppliersList is null)
                await RefreshSuppliers();

            combo.ItemsSource = suppliersList;
            combo.SelectedValuePath = "SupplierId";
            combo.DisplayMemberPath = "Name";
            combo.SelectedIndex = -1;
        }
        #endregion

        #region Cards
        static public Card card = new Card();
        static public List<Card> cardsList;
        static public async Task<IEnumerable<Card>> RefreshCards()
        {
            cardsList = await card.Get();
            return cardsList;
        }
        #endregion

        #region Branch
        static public Branch branch = new Branch();
        static public List<Branch> branchsList;

        static public async Task<IEnumerable<Branch>> RefreshBranchs()
        {
            branchsList = await branch.Get();
            return branchsList;
        }
        #endregion

        #region Tax Type

        static public List<keyValueString> taxTypesList;
        static public IEnumerable<keyValueString> RefreshTaxTypes()
        {
            itemTypesList = new List<keyValueString>() {
                new keyValueString(){key="rate", value=AppSettings.resourcemanager.GetString("trRate") },
                new keyValueString(){key="value", value=AppSettings.resourcemanager.GetString("trValue") },
            };

            return itemTypesList;
        }

        static public void fillTaxTypes(ComboBox combo)
        {
            if (taxTypesList is null)
                RefreshTaxTypes();

            combo.ItemsSource = taxTypesList;
            combo.SelectedValuePath = "key";
            combo.DisplayMemberPath = "value";
            combo.SelectedIndex = -1;
        }
        #endregion
    }
}
