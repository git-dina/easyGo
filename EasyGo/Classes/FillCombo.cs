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
        static public List<Customer> suppliersList;

        static public async Task<IEnumerable<Customer>> RefreshAgents()
        {
            suppliersList = await customer.Get();
            return suppliersList;
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
