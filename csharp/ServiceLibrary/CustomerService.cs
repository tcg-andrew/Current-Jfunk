#region Usings

using System.Configuration;
using System.ServiceModel;
using ObjectLibrary;
using System.Collections.Generic;
using System;

#endregion

namespace ServiceLibrary
{
    [ServiceBehavior(Namespace = "http://services.it.tcg/epicor/customerservice")]
    public class CustomerService : ICustomerService
    {
        #region Service Behaviors

        public customergetallresult getallcustomers()
        {
            customergetallresult result = new customergetallresult();

            try
            {
                CustomerInterface customerInterface = new CustomerInterface();
                foreach (ObjectLibrary.Customer customer in customerInterface.GetAllCustomers(ConfigurationManager.AppSettings["EpicorDatabaseServer"].ToString(), ConfigurationManager.AppSettings["EpicorDatabase"].ToString(), ConfigurationManager.AppSettings["EpicorUsername"].ToString(), ConfigurationManager.AppSettings["EpicorPassword"].ToString()))
                    result.epicor.Add(new ServiceLibrary.GetAllCustomers.customer(customer));
            }
            catch (Exception ex)
            {
                result.exception = ex.Message;
            }

            return result;
        }

        public customergetsingleresult getcustomerbycustid(string custid)
        {
            customergetsingleresult result = new customergetsingleresult();

            try
            {
                CustomerInterface customerInterface = new CustomerInterface();
                result.epicor.Add(new customer(customerInterface.GetCustomerByCustID(ConfigurationManager.AppSettings["EpicorDatabaseServer"].ToString(), ConfigurationManager.AppSettings["EpicorDatabase"].ToString(), ConfigurationManager.AppSettings["EpicorUsername"].ToString(),
                    ConfigurationManager.AppSettings["EpicorPassword"].ToString(), custid)));
            }
            catch (Exception ex)
            {
                result.exception = ex.Message;
            }

            return result;
        }

        public customergetspecialinstructionresult getspecialinstruction(string custid)
        {
            customergetspecialinstructionresult result = new customergetspecialinstructionresult();

            try
            {
                CustomerInterface customerInterface = new CustomerInterface();
                result.epicor.Add(customerInterface.GetCustomerSpecialInstructions(ConfigurationManager.AppSettings["EpicorDatabaseServer"].ToString(), ConfigurationManager.AppSettings["EpicorDatabase"].ToString(), ConfigurationManager.AppSettings["EpicorUsername"].ToString(),
                    ConfigurationManager.AppSettings["EpicorPassword"].ToString(), custid));
            }
            catch (Exception ex)
            {
                result.exception = ex.Message;
            }

            return result;
        }

        #endregion
    }
}
