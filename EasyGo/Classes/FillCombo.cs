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
        #region Agent
        static public Agent agent = new Agent();
        static public List<Agent> agentsList;

        static public async Task<IEnumerable<Agent>> RefreshAgents()
        {
            agentsList = await agent.Get("");
            return agentsList;
        }
        #endregion

    }
}
