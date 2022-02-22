#region Usings

using System;
using System.ServiceModel;
using System.ServiceModel.Description;

#endregion

namespace FlatWSDL
{
    public class FlatWSDLServiceHost : ServiceHost
    {
        #region Constructors

        public FlatWSDLServiceHost()
        {
        }

        public FlatWSDLServiceHost(Type serviceType, params Uri[] baseAddresses)
            : base(serviceType, baseAddresses)
        {
        }

        public FlatWSDLServiceHost(object singletonInstance, params Uri[] baseAddresses)
            : base(singletonInstance, baseAddresses)
        {
        }

        #endregion

        #region Private Methods

        private void InjectFlatWsdlExtension()
        {
            foreach (ServiceEndpoint endpoint in this.Description.Endpoints)
            {
                endpoint.Behaviors.Add(new FlatWSDL());
            }
        }

        #endregion

        #region Protected Methods

        protected override void ApplyConfiguration()
        {
            base.ApplyConfiguration();
            InjectFlatWsdlExtension();
        }

        #endregion
    }
}
