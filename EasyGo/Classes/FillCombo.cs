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
