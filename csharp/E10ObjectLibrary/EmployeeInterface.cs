using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Erp.Proxy.BO;
using Erp.BO;
using Ice.Core;
using Erp.Contracts;

namespace ObjectLibrary
{
    public class EmployeeInterface : EpicorExtension<EmpBasicImpl, EmpBasicSvcContract>
    {
        #region Public Methods

        #region Update Methods

        public void ClockIn(string server, string port, string username, string password, string employeeID, int shift)
        {
            OpenSession(server, port, username, password, Session.LicenseType.Default);
            try
            {
                string opMessage = "";
                BusinessObject.CheckShift(employeeID, shift, out opMessage);
                BusinessObject.ClockIn(employeeID, shift);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                CloseSession();
            }
        }

        public void ClockOut(string server, string port, string username, string password, string employeeID)
        {
            OpenSession(server, port, username, password, Session.LicenseType.Default);
            try
            {
                BusinessObject.ClockOut(ref employeeID);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                CloseSession();
            }
        }
        #endregion

        #endregion
    }
}
