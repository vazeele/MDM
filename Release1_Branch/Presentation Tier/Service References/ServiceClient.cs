//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using SmartBiz.MDMAPI.Service.ServiceContract;
//using System.ServiceModel;
//namespace SmartBiz.MDM.Presentation.MDMServiceReference
//{
//    public class MdmServiceClient
//    {
//        private IMdmService _service;

//        public IMdmService GetService()
//        {
//            return _service ?? (_service = GetServiceInstance("http://localhost:8733/Design_Time_Addresses/SmartBiz.MDMAPI.Service/MdmService/"));
//        }

//        private static IMdmService GetServiceInstance(string address)
//        {
//            BasicHttpBinding binding = new BasicHttpBinding();
//            EndpointAddress endpoint = new EndpointAddress(address);

//            return ChannelFactory<IMdmService>.CreateChannel(binding, endpoint);
//        }
//    }


//}
