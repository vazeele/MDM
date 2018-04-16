using SmartBiz.MDMAPI.API;
using SmartBiz.MDMAPI.API.Queries;
using SmartBiz.MDMAPI.Common.Entities;
using SmartBiz.PCSAPI.Common;
using SmartBiz.PCSAPI.Common.Enums;
using SmartBiz.Services.Base.Exception;
using SmartBiz.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;

namespace SmartBiz.MDMAPI.Service.ServiceContract
{
    /// <summary>
    /// Class MdmService.
    /// </summary>

    public class MdmService : IMdmService
    {
        /// <summary>
        /// The logger instance
        /// </summary>
        private readonly Logger _logger;

        /// <summary>
        /// The strings for client windows credentials
        /// </summary>
        private string username;
        private string from;
        private System.DateTime date;

        /// <summary>
        /// Initializes a new instance of the <see cref="PcsApiService"/> class.
        /// </summary>
        public MdmService()
        {
            _logger = Logger.GetLogger();
            string loginUser = OperationContext.Current.ServiceSecurityContext.WindowsIdentity.Name;
            this.username = loginUser.Substring(loginUser.IndexOf("\\") + 1);
            this.from = loginUser.Substring(0, loginUser.IndexOf("\\"));
            this.date = DateTime.Today;

        }
        public List<string> GetReportNames()
        {
            string loginUser = OperationContext.Current.ServiceSecurityContext.WindowsIdentity.Name;
            var api = new FM100();
            return api.getReportNames(loginUser);
        }
        /// <summary>
        /// Pings the service.
        /// </summary>
        /// <returns>Message.</returns>
        public bool PingService()
        {
            return true;
        }

       

        #region AgentBrokerSalesMan

        public ApiAck CreateAgentBrokerSalesMan(ERP_AgentBrokerSalesMan newAgentBrokerSalesMan)
        {
            ApiAck ack = null;
            newAgentBrokerSalesMan.EnteredUser = username;
            newAgentBrokerSalesMan.EnteredFrom = from;
            newAgentBrokerSalesMan.EnteredDate = date;

            try
            {
                var api = new FM100();
                ack = api.CreateAgentBrokerSalesMan(newAgentBrokerSalesMan);
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return ack;
        }

        public ERP_AgentBrokerSalesMan ReadAgentBrokerSalesManbyKey(string AgentBrokerSalesManCode, int AgentBrokerSalesManFlag)
        {
            ERP_AgentBrokerSalesMan result = null;
            try
            {
                var api = new FM100();
                result = api.ReadAgentBrokerSalesManbyKey(AgentBrokerSalesManCode, AgentBrokerSalesManFlag);
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return result;
        }

        public ERP_AgentBrokerSalesMan[] ReadAllAgentBrokerSalesMan()
        {
            ERP_AgentBrokerSalesMan[] result = null;
            try
            {
                var api = new FM100();
                result = api.ReadAllAgentBrokerSalesMan();
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return result;
        }

        public IQueryable<ERP_AgentBrokerSalesMan> ReadAgentBrokerSalesMan()
        {
            IQueryable<ERP_AgentBrokerSalesMan> result = null;
            try
            {
                var api = new FM100();
                result = api.ReadAgentBrokerSalesMan();
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return result;
        }

        public ApiAck UpdateAgentBrokerSalesMan(ERP_AgentBrokerSalesMan modifiedAgentBrokerSalesMan)
        {
            ApiAck result = null;
            modifiedAgentBrokerSalesMan.ModifiedUser = username;
            modifiedAgentBrokerSalesMan.ModifiedFrom = from;
            modifiedAgentBrokerSalesMan.ModifiedDate = date;
            try
            {
                var api = new FM100();
                result = api.UpdateAgentBrokerSalesMan(modifiedAgentBrokerSalesMan);
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return result;
        }

        public ApiAck DeleteAgentBrokerSalesMan(ERP_AgentBrokerSalesMan deletingAgentBrokerSalesMan)
        {
            ApiAck result = null;
            try
            {
                var api = new FM100();
                result = api.DeleteAgentBrokerSalesMan(deletingAgentBrokerSalesMan);
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return result;
        }

        public ResultDTO<ERP_AgentBrokerSalesMan> QueryAgentBrokerSalesMan(AgentBrokerSalesManQuery query, int pageSize, int pageNumber)
        {
            ResultDTO<ERP_AgentBrokerSalesMan> result = null;
            try
            {
                var api = new FM100();
                result = api.QueryAgentBrokerSalesMan(query, pageSize, pageNumber);
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return result;
        }

        public bool AgentBrokerSalesManExists(ERP_AgentBrokerSalesMan AgentBrokerSalesManexists)
        {
            bool result = false;
            try
            {
                var api = new FM100();
                result = api.AgentBrokerSalesManExists(AgentBrokerSalesManexists);
            }
            catch (Exception ex)
            {
                result = false;
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return result;
        }

        #endregion AgentBrokerSalesMan

        #region Document

        public ApiAck CreateDocument(ERP_Document newDocument)
        {
            ApiAck ack = null;
            newDocument.EnteredUser = username;
            newDocument.EnteredFrom = from;
            newDocument.EnteredDate = date;
            try
            {
                var api = new FM100();
                ack = api.CreateDocument(newDocument);
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return ack;
        }

        public ResultDTO<ERP_Document> QueryDocument(DocumentQuery query, int pageSize, int pageNumber)
        {
            ResultDTO<ERP_Document> result = null;
            try
            {
                var api = new FM100();
                result = api.QueryDocument(query, pageSize, pageNumber);
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return result;
        }

        public bool DocumentExists(ERP_Document Documentexists)
        {
            bool result = false;
            try
            {
                var api = new FM100();
                result = api.DocumentExists(Documentexists);
            }
            catch (Exception ex)
            {
                result = false;
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return result;
        }

        public ERP_Document ReadDocumentbyKey(string DocCode)
        {
            ERP_Document result = null;
            try
            {
                var api = new FM100();
                result = api.ReadDocumentbyKey(DocCode);
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return result;
        }

        public ERP_Document[] ReadAllDocument()
        {
            ERP_Document[] result = null;
            try
            {
                var api = new FM100();
                result = api.ReadAllDocument();
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return result;
        }

        public IQueryable<ERP_Document> ReadDocument()
        {
            IQueryable<ERP_Document> result = null;
            try
            {
                var api = new FM100();
                result = api.ReadDocument();
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return result;
        }

        public ApiAck UpdateDocument(ERP_Document modifiedDocument)
        {
            ApiAck result = null;
            modifiedDocument.ModifiedUser = username;
            modifiedDocument.ModifiedFrom = from;
            modifiedDocument.ModifiedDate = date;
            try
            {
                var api = new FM100();
                result = api.UpdateDocument(modifiedDocument);
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return result;
        }

        public ApiAck DeleteDocument(ERP_Document deletingDocument)
        {
            ApiAck result = null;
            try
            {
                var api = new FM100();
                result = api.DeleteDocument(deletingDocument);
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return result;
        }

        #endregion Document

        #region DocumentAttributes

        public ApiAck CreateDocumentAttributes(ERP_DocumentAttributes newDocumentAttributes)
        {
            ApiAck ack = null;
            newDocumentAttributes.EnteredUser = username;
            newDocumentAttributes.EnteredFrom = from;
            newDocumentAttributes.EnteredDate = date;            
            try
            {
                var api = new FM100();
                ack = api.CreateDocumentAttributes(newDocumentAttributes);
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return ack;
        }

        public ResultDTO<ERP_DocumentAttributes> QueryDocumentAttributes(DocumentAttributesQuery query, int pageSize, int pageNumber)
        {
            ResultDTO<ERP_DocumentAttributes> result = null;
            try
            {
                var api = new FM100();
                result = api.QueryDocumentAttributes(query, pageSize, pageNumber);
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return result;
        }

        public bool DocumentAttributesExists(ERP_DocumentAttributes DocumentAttributesexists)
        {
            bool result = false;
            try
            {
                var api = new FM100();
                result = api.DocumentAttributesExists(DocumentAttributesexists);
            }
            catch (Exception ex)
            {
                result = false;
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return result;
        }

        public ERP_DocumentAttributes ReadDocumentAttributesbyKey(string DocCode, string TxCode)
        {
            ERP_DocumentAttributes result = null;
            try
            {
                var api = new FM100();
                result = api.ReadDocumentAttributesbyKey(DocCode, TxCode);
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return result;
        }

        public ERP_DocumentAttributes[] ReadAllDocumentAttributes()
        {
            ERP_DocumentAttributes[] result = null;
            try
            {
                var api = new FM100();
                result = api.ReadAllDocumentAttributes();
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return result;
        }

        public IQueryable<ERP_DocumentAttributes> ReadDocumentAttributes()
        {
            IQueryable<ERP_DocumentAttributes> result = null;
            try
            {
                var api = new FM100();
                result = api.ReadDocumentAttributes();
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return result;
        }

        public ApiAck UpdateDocumentAttributes(ERP_DocumentAttributes modifiedDocumentAttributes)
        {
            ApiAck result = null;
            modifiedDocumentAttributes.ModifiedUser = username;
            modifiedDocumentAttributes.ModifiedFrom = from;
            modifiedDocumentAttributes.ModifiedDate = date;
            try
            {
                var api = new FM100();
                result = api.UpdateDocumentAttributes(modifiedDocumentAttributes);
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return result;
        }

        public ApiAck DeleteDocumentAttributes(ERP_DocumentAttributes deletingDocumentAttributes)
        {
            ApiAck result = null;
            try
            {
                var api = new FM100();
                result = api.DeleteDocumentAttributes(deletingDocumentAttributes);
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return result;
        }

        #endregion DocumentAttributes

        #region DocumentAttributesBankInfo

        public ApiAck CreateDocumentAttributesBankInfo(ERP_DocumentAttributesBankInfo newDocumentAttributesBankInfo)
        {
            ApiAck ack = null;
            newDocumentAttributesBankInfo.EnteredUser = username;
            newDocumentAttributesBankInfo.EnteredFrom = from;
            newDocumentAttributesBankInfo.EnteredDate = date;
            try
            {
                var api = new FM100();
                ack = api.CreateDocumentAttributesBankInfo(newDocumentAttributesBankInfo);
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return ack;
        }

        public ResultDTO<ERP_DocumentAttributesBankInfo> QueryDocumentAttributesBankInfo(DocumentAttributesBankInfoQuery query, int pageSize, int pageNumber)
        {
            ResultDTO<ERP_DocumentAttributesBankInfo> result = null;
            try
            {
                var api = new FM100();
                result = api.QueryDocumentAttributesBankInfo(query, pageSize, pageNumber);
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return result;
        }

        public bool DocumentAttributesBankInfoExists(ERP_DocumentAttributesBankInfo DocumentAttributesBankInfoexists)
        {
            bool result = false;
            try
            {
                var api = new FM100();
                result = api.DocumentAttributesBankInfoExists(DocumentAttributesBankInfoexists);
            }
            catch (Exception ex)
            {
                result = false;
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return result;
        }

        public ERP_DocumentAttributesBankInfo ReadDocumentAttributesBankInfobyKey(string DocCode, string TxCode)
        {
            ERP_DocumentAttributesBankInfo result = null;
            try
            {
                var api = new FM100();
                result = api.ReadDocumentAttributesBankInfobyKey(DocCode, TxCode);
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return result;
        }

        public ERP_DocumentAttributesBankInfo[] ReadAllDocumentAttributesBankInfo()
        {
            ERP_DocumentAttributesBankInfo[] result = null;
            try
            {
                var api = new FM100();
                result = api.ReadAllDocumentAttributesBankInfo();
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return result;
        }

        public IQueryable<ERP_DocumentAttributesBankInfo> ReadDocumentAttributesBankInfo()
        {
            IQueryable<ERP_DocumentAttributesBankInfo> result = null;
            try
            {
                var api = new FM100();
                result = api.ReadDocumentAttributesBankInfo();
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return result;
        }

        public ApiAck UpdateDocumentAttributesBankInfo(ERP_DocumentAttributesBankInfo modifiedDocumentAttributesBankInfo)
        {
            ApiAck result = null;
            modifiedDocumentAttributesBankInfo.ModifiedUser = username;
            modifiedDocumentAttributesBankInfo.ModifiedFrom = from;
            modifiedDocumentAttributesBankInfo.ModifiedDate = date;
            try
            {
                var api = new FM100();
                result = api.UpdateDocumentAttributesBankInfo(modifiedDocumentAttributesBankInfo);
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return result;
        }

        public ApiAck DeleteDocumentAttributesBankInfo(ERP_DocumentAttributesBankInfo deletingDocumentAttributesBankInfo)
        {
            ApiAck result = null;
            try
            {
                var api = new FM100();
                result = api.DeleteDocumentAttributesBankInfo(deletingDocumentAttributesBankInfo);
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return result;
        }

        #endregion DocumentAttributesBankInfo

        #region DocumentAttributesRef

        public ApiAck CreateDocumentAttributesRef(ERP_DocumentAttributesRef newDocumentAttributesRef)
        {
            ApiAck ack = null;
            newDocumentAttributesRef.EnteredUser = username;
            newDocumentAttributesRef.EnteredFrom = from;
            newDocumentAttributesRef.EnteredDate = date;
            try
            {
                var api = new FM100();
                ack = api.CreateDocumentAttributesRef(newDocumentAttributesRef);
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return ack;
        }

        public ResultDTO<ERP_DocumentAttributesRef> QueryDocumentAttributesRef(DocumentAttributesRefQuery query, int pageSize, int pageNumber)
        {
            ResultDTO<ERP_DocumentAttributesRef> result = null;
            try
            {
                var api = new FM100();
                result = api.QueryDocumentAttributesRef(query, pageSize, pageNumber);
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return result;
        }

        public bool DocumentAttributesRefExists(ERP_DocumentAttributesRef DocumentAttributesRefexists)
        {
            bool result = false;
            try
            {
                var api = new FM100();
                result = api.DocumentAttributesRefExists(DocumentAttributesRefexists);
            }
            catch (Exception ex)
            {
                result = false;
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return result;
        }

        public ERP_DocumentAttributesRef ReadDocumentAttributesRefbyKey(string DocCode, string TxCode, int RefNo)
        {
            ERP_DocumentAttributesRef result = null;
            try
            {
                var api = new FM100();
                result = api.ReadDocumentAttributesRefbyKey(DocCode, TxCode, RefNo);
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return result;
        }

        public ERP_DocumentAttributesRef[] ReadAllDocumentAttributesRef()
        {
            ERP_DocumentAttributesRef[] result = null;
            try
            {
                var api = new FM100();
                result = api.ReadAllDocumentAttributesRef();
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return result;
        }

        public IQueryable<ERP_DocumentAttributesRef> ReadDocumentAttributesRef()
        {
            IQueryable<ERP_DocumentAttributesRef> result = null;
            try
            {
                var api = new FM100();
                result = api.ReadDocumentAttributesRef();
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return result;
        }

        public ApiAck UpdateDocumentAttributesRef(ERP_DocumentAttributesRef modifiedDocumentAttributesRef)
        {
            ApiAck result = null;
            modifiedDocumentAttributesRef.ModifiedUser = username;
            modifiedDocumentAttributesRef.ModifiedFrom = from;
            modifiedDocumentAttributesRef.ModifiedDate = date;
            try
            {
                var api = new FM100();
                result = api.UpdateDocumentAttributesRef(modifiedDocumentAttributesRef);
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return result;
        }

        public ApiAck DeleteDocumentAttributesRef(ERP_DocumentAttributesRef deletingDocumentAttributesRef)
        {
            ApiAck result = null;
            try
            {
                var api = new FM100();
                result = api.DeleteDocumentAttributesRef(deletingDocumentAttributesRef);
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return result;
        }

        #endregion DocumentAttributesRef

        #region CostCenter

        public ApiAck CreateCostCenter(FIN_CostCenter newCostCenter)
        {
            ApiAck ack = null;
            newCostCenter.EnteredUser = username;
            newCostCenter.EnteredFrom = from;
            newCostCenter.EnteredDate = date;
            try
            {
                var api = new FM100();
                ack = api.CreateCostCenter(newCostCenter);
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return ack;
        }

        public ResultDTO<FIN_CostCenter> QueryCostCenter(CostCenterQuery query, int pageSize, int pageNumber)
        {
            ResultDTO<FIN_CostCenter> result = null;
            try
            {
                var api = new FM100();
                result = api.QueryCostCenter(query, pageSize, pageNumber);
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return result;
        }

        public bool CostCenterExists(FIN_CostCenter CostCenterexists)
        {
            bool result = false;
            try
            {
                var api = new FM100();
                result = api.CostCenterExists(CostCenterexists);
            }
            catch (Exception ex)
            {
                result = false;
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return result;
        }

        public FIN_CostCenter ReadCostCenterbyKey(string CostCenterCode)
        {
            FIN_CostCenter result = null;
            try
            {
                var api = new FM100();
                result = api.ReadCostCenterbyKey(CostCenterCode);
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return result;
        }

        public FIN_CostCenter[] ReadAllCostCenter()
        {
            FIN_CostCenter[] result = null;
            try
            {
                var api = new FM100();
                result = api.ReadAllCostCenter();
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return result;
        }

        public IQueryable<FIN_CostCenter> ReadCostCenter()
        {
            IQueryable<FIN_CostCenter> result = null;
            try
            {
                var api = new FM100();
                result = api.ReadCostCenter();
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return result;
        }

        public ApiAck UpdateCostCenter(FIN_CostCenter modifiedCostCenter)
        {
            ApiAck result = null;
            modifiedCostCenter.ModifiedUser = username;
            modifiedCostCenter.ModifiedFrom = from;
            modifiedCostCenter.ModifiedDate = date;
            try
            {
                var api = new FM100();
                result = api.UpdateCostCenter(modifiedCostCenter);
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return result;
        }

        public ApiAck DeleteCostCenter(FIN_CostCenter deletingCostCenter)
        {
            ApiAck result = null;
            try
            {
                var api = new FM100();
                result = api.DeleteCostCenter(deletingCostCenter);
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return result;
        }

        #endregion CostCenter

        #region CostCenterwiseConfiguration

        public ApiAck CreateCostCenterwiseConfiguration(FIN_CostCenterwiseConfiguration newCostCenterwiseConfiguration)
        {
            ApiAck ack = null;
            newCostCenterwiseConfiguration.EnteredUser = username;
            newCostCenterwiseConfiguration.EnteredFrom = from;
            newCostCenterwiseConfiguration.EnteredDate = date;
            try
            {
                var api = new FM100();
                ack = api.CreateCostCenterwiseConfiguration(newCostCenterwiseConfiguration);
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return ack;
        }

        public ResultDTO<FIN_CostCenterwiseConfiguration> QueryCostCenterwiseConfiguration(CostCenterwiseConfigurationQuery query, int pageSize, int pageNumber)
        {
            ResultDTO<FIN_CostCenterwiseConfiguration> result = null;
            try
            {
                var api = new FM100();
                result = api.QueryCostCenterwiseConfiguration(query, pageSize, pageNumber);
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return result;
        }

        public bool CostCenterwiseConfigurationExists(FIN_CostCenterwiseConfiguration CostCenterwiseConfigurationexists)
        {
            bool result = false;
            try
            {
                var api = new FM100();
                result = api.CostCenterwiseConfigurationExists(CostCenterwiseConfigurationexists);
            }
            catch (Exception ex)
            {
                result = false;
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return result;
        }

        public FIN_CostCenterwiseConfiguration ReadCostCenterwiseConfigurationbyKey(int RevNo)
        {
            FIN_CostCenterwiseConfiguration result = null;
            try
            {
                var api = new FM100();
                result = api.ReadCostCenterwiseConfigurationbyKey(RevNo);
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return result;
        }

        public FIN_CostCenterwiseConfiguration[] ReadAllCostCenterwiseConfiguration()
        {
            FIN_CostCenterwiseConfiguration[] result = null;
            try
            {
                var api = new FM100();
                result = api.ReadAllCostCenterwiseConfiguration();
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return result;
        }

        public IQueryable<FIN_CostCenterwiseConfiguration> ReadCostCenterwiseConfiguration()
        {
            IQueryable<FIN_CostCenterwiseConfiguration> result = null;
            try
            {
                var api = new FM100();
                result = api.ReadCostCenterwiseConfiguration();
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return result;
        }

        public ApiAck UpdateCostCenterwiseConfiguration(FIN_CostCenterwiseConfiguration modifiedCostCenterwiseConfiguration)
        {
            ApiAck result = null;
            modifiedCostCenterwiseConfiguration.ModifiedUser = username;
            modifiedCostCenterwiseConfiguration.ModifiedFrom = from;
            modifiedCostCenterwiseConfiguration.ModifiedDate = date;
            try
            {
                var api = new FM100();
                result = api.UpdateCostCenterwiseConfiguration(modifiedCostCenterwiseConfiguration);
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return result;
        }

        public ApiAck DeleteCostCenterwiseConfiguration(FIN_CostCenterwiseConfiguration deletingCostCenterwiseConfiguration)
        {
            ApiAck result = null;
            try
            {
                var api = new FM100();
                result = api.DeleteCostCenterwiseConfiguration(deletingCostCenterwiseConfiguration);
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return result;
        }

        #endregion CostCenterwiseConfiguration

        #region UnitDefinition

        public ApiAck CreateUnitDefinition(ERP_UnitDefinition newUnitDefinition)
        {
            ApiAck ack = null;
            newUnitDefinition.EnteredUser = username;
            newUnitDefinition.EnteredFrom = from;
            newUnitDefinition.EnteredDate = date;
            try
            {
                var api = new FM100();
                ack = api.CreateUnitDefinition(newUnitDefinition);
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return ack;
        }

        public ERP_UnitDefinition ReadUnitDefinitionbyKey(string UnitCode)
        {
            ERP_UnitDefinition result = null;
            try
            {
                var api = new FM100();
                result = api.ReadUnitDefinitionbyKey(UnitCode);
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return result;
        }

        public ERP_UnitDefinition[] ReadAllUnitDefinition()
        {
            ERP_UnitDefinition[] result = null;
            try
            {
                var api = new FM100();
                result = api.ReadAllUnitDefinition();
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return result;
        }

        public IQueryable<ERP_UnitDefinition> ReadUnitDefinition()
        {
            IQueryable<ERP_UnitDefinition> result = null;
            try
            {
                var api = new FM100();
                result = api.ReadUnitDefinition();
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return result;
        }

        public ApiAck UpdateUnitDefinition(ERP_UnitDefinition modifiedUnitDefinition)
        {
            ApiAck result = null;
            modifiedUnitDefinition.ModifiedUser = username;
            modifiedUnitDefinition.ModifiedFrom = from;
            modifiedUnitDefinition.ModifiedDate = date;
            try
            {
                var api = new FM100();
                result = api.UpdateUnitDefinition(modifiedUnitDefinition);
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return result;
        }

        public ApiAck DeleteUnitDefinition(ERP_UnitDefinition deletingUnitDefinition)
        {
            ApiAck result = null;
            try
            {
                var api = new FM100();
                result = api.DeleteUnitDefinition(deletingUnitDefinition);
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return result;
        }

        public ResultDTO<ERP_UnitDefinition> QueryUnitDefinition(UnitDefinitionQuery query, int pageSize, int pageNumber)
        {
            ResultDTO<ERP_UnitDefinition> result = null;
            try
            {
                var api = new FM100();
                result = api.QueryUnitDefinition(query, pageSize, pageNumber);
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return result;
        }

        public bool UnitDefinitionExists(ERP_UnitDefinition UnitDefinitionexists)
        {
            bool result = false;
            try
            {
                var api = new FM100();
                result = api.UnitDefinitionExists(UnitDefinitionexists);
            }
            catch (Exception ex)
            {
                result = false;
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return result;
        }

        #endregion UnitDefinition

        #region UnitConversion

        public ApiAck CreateUnitConversion(ERP_UnitConversion newUnitConversion)
        {
            ApiAck ack = null;
            newUnitConversion.EnteredUser = username;
            newUnitConversion.EnteredFrom = from;
            newUnitConversion.EnteredDate = date;
            try
            {
                var api = new FM100();
                ack = api.CreateUnitConversion(newUnitConversion);
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return ack;
        }

        public ERP_UnitConversion ReadUnitConversionbyKey(string FromUnitCode, string ToUnitCode)
        {
            ERP_UnitConversion result = null;
            try
            {
                var api = new FM100();
                result = api.ReadUnitConversionbyKey(FromUnitCode, ToUnitCode);
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return result;
        }

        public ERP_UnitConversion[] ReadAllUnitConversion()
        {
            ERP_UnitConversion[] result = null;
            try
            {
                var api = new FM100();
                result = api.ReadAllUnitConversion();
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return result;
        }

        public IQueryable<ERP_UnitConversion> ReadUnitConversion()
        {
            IQueryable<ERP_UnitConversion> result = null;
            try
            {
                var api = new FM100();
                result = api.ReadUnitConversion();
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return result;
        }

        public ApiAck UpdateUnitConversion(ERP_UnitConversion modifiedUnitConversion)
        {
            ApiAck result = null;
            modifiedUnitConversion.ModifiedUser = username;
            modifiedUnitConversion.ModifiedFrom = from;
            modifiedUnitConversion.ModifiedDate = date;
            try
            {
                var api = new FM100();
                result = api.UpdateUnitConversion(modifiedUnitConversion);
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return result;
        }

        public ApiAck DeleteUnitConversion(ERP_UnitConversion deletingUnitConversion)
        {
            ApiAck result = null;
            try
            {
                var api = new FM100();
                result = api.DeleteUnitConversion(deletingUnitConversion);
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return result;
        }

        public ResultDTO<ERP_UnitConversion> QueryUnitConversion(UnitConversionQuery query, int pageSize, int pageNumber)
        {
            ResultDTO<ERP_UnitConversion> result = null;
            try
            {
                var api = new FM100();
                result = api.QueryUnitConversion(query, pageSize, pageNumber);
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return result;
        }

        public bool UnitConversionExists(ERP_UnitConversion UnitConversionexists)
        {
            bool result = false;
            try
            {
                var api = new FM100();
                result = api.UnitConversionExists(UnitConversionexists);
            }
            catch (Exception ex)
            {
                result = false;
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return result;
        }

        #endregion UnitConversion

        #region Region

        public ApiAck CreateRegion(FIN_Region newRegion)
        {
            ApiAck ack = null;
            newRegion.EnteredUser = username;
            newRegion.EnteredFrom = from;
            newRegion.EnteredDate = date;
            try
            {
                var api = new FM100();
                ack = api.CreateRegion(newRegion);
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return ack;
        }

        public FIN_Region ReadRegionbyKey(string RegionCode)
        {
            FIN_Region result = null;
            try
            {
                var api = new FM100();
                result = api.ReadRegionbyKey(RegionCode);
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return result;
        }

        public FIN_Region[] ReadAllRegion()
        {
            FIN_Region[] result = null;
            try
            {
                var api = new FM100();
                result = api.ReadAllRegion();
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return result;
        }

        public IQueryable<FIN_Region> ReadRegion()
        {
            IQueryable<FIN_Region> result = null;
            try
            {
                var api = new FM100();
                result = api.ReadRegion();
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return result;
        }

        public ApiAck UpdateRegion(FIN_Region modifiedRegion)
        {
            ApiAck result = null;
            modifiedRegion.ModifiedUser = username;
            modifiedRegion.ModifiedFrom = from;
            modifiedRegion.ModifiedDate = date;
            try
            {
                var api = new FM100();
                result = api.UpdateRegion(modifiedRegion);
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return result;
        }

        public ApiAck DeleteRegion(FIN_Region deletingRegion)
        {
            ApiAck result = null;
            try
            {
                var api = new FM100();
                result = api.DeleteRegion(deletingRegion);
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return result;
        }

        public bool RegionExists(FIN_Region Regionexists)
        {
            bool result = false;
            try
            {
                var api = new FM100();
                result = api.RegionExists(Regionexists);
            }
            catch (Exception ex)
            {
                result = false;
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return result;
        }

        public ResultDTO<FIN_Region> QueryRegion(RegionQuery query, int pageSize, int pageNumber)
        {
            ResultDTO<FIN_Region> result = null;
            try
            {
                var api = new FM100();
                result = api.QueryRegion(query, pageSize, pageNumber);
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return result;
        }

        #endregion Region

        #region Area

        public ApiAck CreateArea(FIN_Area newArea)
        {
            ApiAck ack = null;
            newArea.EnteredUser = username;
            newArea.EnteredFrom = from;
            newArea.EnteredDate = date;
            try
            {
                var api = new FM100();
                ack = api.CreateArea(newArea);
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return ack;
        }

        public FIN_Area ReadAreabyKey(string AreaCode, string RegionCode)
        {
            FIN_Area result = null;
            try
            {
                var api = new FM100();
                result = api.ReadAreabyKey(AreaCode, RegionCode);
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return result;
        }

        public FIN_Area[] ReadAllArea()
        {
            FIN_Area[] result = null;
            try
            {
                var api = new FM100();
                result = api.ReadAllArea();
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return result;
        }

        public IQueryable<FIN_Area> ReadArea()
        {
            IQueryable<FIN_Area> result = null;
            try
            {
                var api = new FM100();
                result = api.ReadArea();
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return result;
        }

        public ApiAck UpdateArea(FIN_Area modifiedArea)
        {
            ApiAck result = null;
            modifiedArea.ModifiedUser = username;
            modifiedArea.ModifiedFrom = from;
            modifiedArea.ModifiedDate = date;
            try
            {
                var api = new FM100();
                result = api.UpdateArea(modifiedArea);
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return result;
        }

        public ApiAck DeleteArea(FIN_Area deletingArea)
        {
            ApiAck result = null;
            try
            {
                var api = new FM100();
                result = api.DeleteArea(deletingArea);
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return result;
        }

        public bool AreaExists(FIN_Area Areaexists)
        {
            bool result = false;
            try
            {
                var api = new FM100();
                result = api.AreaExists(Areaexists);
            }
            catch (Exception ex)
            {
                result = false;
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return result;
        }

        public ResultDTO<FIN_Area> QueryArea(AreaQuery query, int pageSize, int pageNumber)
        {
            ResultDTO<FIN_Area> result = null;
            try
            {
                var api = new FM100();
                result = api.QueryArea(query, pageSize, pageNumber);
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return result;
        }

        #endregion Area

        #region Currency

        public ApiAck CreateCurrency(FIN_Currency new_Currency)
        {
            ApiAck ack = null;
            new_Currency.EnteredUser = username;
            new_Currency.EnteredFrom = from;
            new_Currency.EnteredDate = date;
            try
            {
                var api = new FM100();
                ack = api.CreateCurrency(new_Currency);
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return ack;
        }

        public FIN_Currency ReadCurrencyByKey(string CurrencyCode)
        {
            FIN_Currency result = null;
            try
            {
                var api = new FM100();
                result = api.ReadCurrencyByKey(CurrencyCode);
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return result;
        }

        public FIN_Currency[] ReadAllCurrency()
        {
            FIN_Currency[] result = null;
            try
            {
                var api = new FM100();
                result = api.ReadAllCurrency();
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return result;
        }

        public ApiAck UpdateCurrency(FIN_Currency modified_Currency)
        {
            ApiAck result = null;
            modified_Currency.ModifiedUser = username;
            modified_Currency.ModifiedFrom = from;
            modified_Currency.ModifiedDate = date;
            try
            {
                var api = new FM100();
                result = api.UpdateCurrency(modified_Currency);
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return result;
        }

        public ApiAck DeleteCurrency(FIN_Currency delete_Currency)
        {
            ApiAck result = null;
            try
            {
                var api = new FM100();
                result = api.DeleteCurrency(delete_Currency);
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return result;
        }

        public bool ExistCurrency(FIN_Currency exist_Currency)
        {
            bool result = false;
            try
            {
                var api = new FM100();
                result = api.ExistCurrency(exist_Currency);
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return result;
        }

        public ResultDTO<FIN_Currency> QueryCurrency(CurrencyQuery query, int pageSize, int pageNumber)
        {
            ResultDTO<FIN_Currency> result = null;
            try
            {
                var api = new FM100();
                result = api.QueryCurrency(query, pageSize, pageNumber);
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return result;
        }

        #endregion Currency

        #region GeneralLedgerAccount

        public ApiAck CreateGeneralLedgerAccount(FIN_GeneralLedgerAccount newGeneralLedgerAccount)
        {
            ApiAck ack = null;
            newGeneralLedgerAccount.EnteredUser = username;
            newGeneralLedgerAccount.EnteredFrom = from;
            newGeneralLedgerAccount.EnteredDate = date;
            try
            {
                var api = new FM100();
                ack = api.CreateGeneralLedgerAccount(newGeneralLedgerAccount);
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return ack;
        }

        public FIN_GeneralLedgerAccount ReadGeneralLedgerAccountbyKey(string AccountNo)
        {
            FIN_GeneralLedgerAccount result = null;
            try
            {
                var api = new FM100();
                result = api.ReadGeneralLedgerAccountbyKey(AccountNo);
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return result;
        }

        public FIN_GeneralLedgerAccount[] ReadAllGeneralLedgerAccount()
        {
            FIN_GeneralLedgerAccount[] result = null;
            try
            {
                var api = new FM100();
                result = api.ReadAllGeneralLedgerAccount();
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return result;
        }

        public IQueryable<FIN_GeneralLedgerAccount> ReadGeneralLedgerAccount()
        {
            IQueryable<FIN_GeneralLedgerAccount> result = null;
            try
            {
                var api = new FM100();
                result = api.ReadGeneralLedgerAccount();
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return result;
        }

        public ApiAck UpdateGeneralLedgerAccount(FIN_GeneralLedgerAccount modifiedGeneralLedgerAccount)
        {
            ApiAck result = null;
            modifiedGeneralLedgerAccount.ModifiedUser = username;
            modifiedGeneralLedgerAccount.ModifiedFrom = from;
            modifiedGeneralLedgerAccount.ModifiedDate = date;
            try
            {
                var api = new FM100();
                result = api.UpdateGeneralLedgerAccount(modifiedGeneralLedgerAccount);
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return result;
        }

        public ApiAck DeleteGeneralLedgerAccount(FIN_GeneralLedgerAccount deletingGeneralLedgerAccount)
        {
            ApiAck result = null;
            try
            {
                var api = new FM100();
                result = api.DeleteGeneralLedgerAccount(deletingGeneralLedgerAccount);
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return result;
        }

        public bool GeneralLedgerAccountExists(FIN_GeneralLedgerAccount existsGeneralLedgerAccount)
        {
            bool result = false;
            try
            {
                var api = new FM100();
                result = api.GeneralLedgerAccountExists(existsGeneralLedgerAccount);
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return result;
        }

        public ResultDTO<FIN_GeneralLedgerAccount> QueryGeneralLedgerAccount(GeneralLedgerAccountQuery query, int pageSize, int pageNumber)
        {
            ResultDTO<FIN_GeneralLedgerAccount> result = null;
            try
            {
                var api = new FM100();
                result = api.QueryGeneralLedgerAccount(query, pageSize, pageNumber);
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return result;
        }

        #endregion GeneralLedgerAccount

        #region Employee

        public ResultDTO<HR_EMPLOYEE> QueryEmployee(EmployeeQuery query, int pageSize, int pageNumber)
        {
            ResultDTO<HR_EMPLOYEE> result = null;
            try
            {
                var api = new HR100();
                result = api.QueryEmployee(query, pageSize, pageNumber);
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return result;
        }

        public ApiAck CreateEmployee(HR_EMPLOYEE newEmployee)
        {
            ApiAck ack = null;
            try
            {
                var api = new HR100();
                ack = api.CreateEmployee(newEmployee);
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return ack;
        }

        public HR_EMPLOYEE ReadEmployeebyKey(string EMP_NUMBER)
        {
            HR_EMPLOYEE result = null;
            try
            {
                var api = new HR100();
                result = api.ReadEmployeebyKey(EMP_NUMBER);
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return result;
        }

        public HR_EMPLOYEE[] ReadAllEmployee()
        {
            HR_EMPLOYEE[] result = null;
            try
            {
                var api = new HR100();
                result = api.ReadAllEmployee();
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return result;
        }

        public IQueryable<HR_EMPLOYEE> ReadEmployee()
        {
            IQueryable<HR_EMPLOYEE> result = null;
            try
            {
                var api = new HR100();
                result = api.ReadEmployee();
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return result;
        }

        public ApiAck UpdateEmployee(HR_EMPLOYEE modifiedEmployee)
        {
            ApiAck result = null;
            try
            {
                var api = new HR100();
                result = api.UpdateEmployee(modifiedEmployee);
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return result;
        }

        public ApiAck DeleteEmployee(HR_EMPLOYEE deletingEmployee)
        {
            ApiAck result = null;
            try
            {
                var api = new HR100();
                result = api.DeleteEmployee(deletingEmployee);
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return result;
        }

        public bool EmployeeExists(HR_EMPLOYEE existsEmployee)
        {
            bool result = false;
            try
            {
                var api = new HR100();
                result = api.EmployeeExists(existsEmployee);
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return result;
        }

        #endregion Employee

        #region Employee_Personal_Details

        public ResultDTO<HR_EMPLOYEE_PD> QueryEmployeePD(EmployeePDQuery query, int pageSize, int pageNumber)
        {
            ResultDTO<HR_EMPLOYEE_PD> result = null;
            try
            {
                var api = new HR100();
                result = api.QueryEmployeePD(query, pageSize, pageNumber);
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return result;
        }

        public ApiAck CreateEmployeePD(HR_EMPLOYEE_PD newEmployeePD)
        {
            ApiAck ack = null;
            try
            {
                var api = new HR100();
                ack = api.CreateEmployeePD(newEmployeePD);
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return ack;
        }

        public HR_EMPLOYEE_PD ReadEmployeePDbyKey(string EMP_NUMBER_PD)
        {
            HR_EMPLOYEE_PD result = null;
            try
            {
                var api = new HR100();
                result = api.ReadEmployeePDbyKey(EMP_NUMBER_PD);
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return result;
        }

        public HR_EMPLOYEE_PD[] ReadAllEmployeePD()
        {
            HR_EMPLOYEE_PD[] result = null;
            try
            {
                var api = new HR100();
                result = api.ReadAllEmployeePD();
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return result;
        }

        public IQueryable<HR_EMPLOYEE_PD> ReadEmployeePD()
        {
            IQueryable<HR_EMPLOYEE_PD> result = null;
            try
            {
                var api = new HR100();
                result = api.ReadEmployeePD();
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return result;
        }

        public ApiAck UpdateEmployeePD(HR_EMPLOYEE_PD modifiedEmployeePD)
        {
            ApiAck result = null;
            try
            {
                var api = new HR100();
                result = api.UpdateEmployeePD(modifiedEmployeePD);
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return result;
        }

        public ApiAck DeleteEmployeePD(HR_EMPLOYEE_PD deletingEmployeePD)
        {
            ApiAck result = null;
            try
            {
                var api = new HR100();
                result = api.DeleteEmployeePD(deletingEmployeePD);
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return result;
        }

        public bool EmployeePDExists(HR_EMPLOYEE_PD existsEmployeePD)
        {
            bool result = false;
            try
            {
                var api = new HR100();
                result = api.EmployeePDExists(existsEmployeePD);
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return result;
        }

        #endregion Employee_Personal_Details

        #region Employee_Job_Info

        public ResultDTO<HR_EMPLOYEE_JI> QueryEmployeeJI(EmployeeJIQuery query, int pageSize, int pageNumber)
        {
            ResultDTO<HR_EMPLOYEE_JI> result = null;
            try
            {
                var api = new HR100();
                result = api.QueryEmployeeJI(query, pageSize, pageNumber);
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return result;
        }

        public ApiAck CreateEmployeeJI(HR_EMPLOYEE_JI newEmployeeJI)
        {
            ApiAck ack = null;
            try
            {
                var api = new HR100();
                ack = api.CreateEmployeeJI(newEmployeeJI);
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return ack;
        }

        public HR_EMPLOYEE_JI ReadEmployeeJIbyKey(string EMP_NUMBER_JI)
        {
            HR_EMPLOYEE_JI result = null;
            try
            {
                var api = new HR100();
                result = api.ReadEmployeeJIbyKey(EMP_NUMBER_JI);
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return result;
        }

        public HR_EMPLOYEE_JI[] ReadAllEmployeeJI()
        {
            HR_EMPLOYEE_JI[] result = null;
            try
            {
                var api = new HR100();
                result = api.ReadAllEmployeeJI();
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return result;
        }

        public IQueryable<HR_EMPLOYEE_JI> ReadEmployeeJI()
        {
            IQueryable<HR_EMPLOYEE_JI> result = null;
            try
            {
                var api = new HR100();
                result = api.ReadEmployeeJI();
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return result;
        }

        public ApiAck UpdateEmployeeJI(HR_EMPLOYEE_JI modifiedEmployeeJI)
        {
            ApiAck result = null;
            try
            {
                var api = new HR100();
                result = api.UpdateEmployeeJI(modifiedEmployeeJI);
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return result;
        }

        public ApiAck DeleteEmployeeJI(HR_EMPLOYEE_JI deletingEmployeeJI)
        {
            ApiAck result = null;
            try
            {
                var api = new HR100();
                result = api.DeleteEmployeeJI(deletingEmployeeJI);
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return result;
        }

        public bool EmployeeJIExists(HR_EMPLOYEE_JI existsEmployeeJI)
        {
            bool result = false;
            try
            {
                var api = new HR100();
                result = api.EmployeeJIExists(existsEmployeeJI);
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return result;
        }

        #endregion Employee_Job_Info

        #region Employee_Job_Status

        public ResultDTO<HR_EMPLOYEE_JS> QueryEmployeeJS(EmployeeJSQuery query, int pageSize, int pageNumber)
        {
            ResultDTO<HR_EMPLOYEE_JS> result = null;
            try
            {
                var api = new HR100();
                result = api.QueryEmployeeJS(query, pageSize, pageNumber);
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return result;
        }

        public ApiAck CreateEmployeeJS(HR_EMPLOYEE_JS newEmployeeJS)
        {
            ApiAck ack = null;
            try
            {
                var api = new HR100();
                ack = api.CreateEmployeeJS(newEmployeeJS);
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return ack;
        }

        public HR_EMPLOYEE_JS ReadEmployeeJSbyKey(string EMP_NUMBER_JS)
        {
            HR_EMPLOYEE_JS result = null;
            try
            {
                var api = new HR100();
                result = api.ReadEmployeeJSbyKey(EMP_NUMBER_JS);
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return result;
        }

        public HR_EMPLOYEE_JS[] ReadAllEmployeeJS()
        {
            HR_EMPLOYEE_JS[] result = null;
            try
            {
                var api = new HR100();
                result = api.ReadAllEmployeeJS();
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return result;
        }

        public IQueryable<HR_EMPLOYEE_JS> ReadEmployeeJS()
        {
            IQueryable<HR_EMPLOYEE_JS> result = null;
            try
            {
                var api = new HR100();
                result = api.ReadEmployeeJS();
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return result;
        }

        public ApiAck UpdateEmployeeJS(HR_EMPLOYEE_JS modifiedEmployeeJS)
        {
            ApiAck result = null;
            try
            {
                var api = new HR100();
                result = api.UpdateEmployeeJS(modifiedEmployeeJS);
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return result;
        }

        public ApiAck DeleteEmployeeJS(HR_EMPLOYEE_JS deletingEmployeeJS)
        {
            ApiAck result = null;
            try
            {
                var api = new HR100();
                result = api.DeleteEmployeeJS(deletingEmployeeJS);
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return result;
        }

        public bool EmployeeJSExists(HR_EMPLOYEE_JS existsEmployeeJS)
        {
            bool result = false;
            try
            {
                var api = new HR100();
                result = api.EmployeeJSExists(existsEmployeeJS);
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return result;
        }

        #endregion Employee_Job_Status

        #region Employee_Tax_Details

        public ResultDTO<HR_EMPLOYEE_TD> QueryEmployeeTD(EmployeeTDQuery query, int pageSize, int pageNumber)
        {
            ResultDTO<HR_EMPLOYEE_TD> result = null;
            try
            {
                var api = new HR100();
                result = api.QueryEmployeeTD(query, pageSize, pageNumber);
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return result;
        }

        public ApiAck CreateEmployeeTD(HR_EMPLOYEE_TD newEmployeeTD)
        {
            ApiAck ack = null;
            try
            {
                var api = new HR100();
                ack = api.CreateEmployeeTD(newEmployeeTD);
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return ack;
        }

        public HR_EMPLOYEE_TD ReadEmployeeTDbyKey(string EMP_NUMBER_TD)
        {
            HR_EMPLOYEE_TD result = null;
            try
            {
                var api = new HR100();
                result = api.ReadEmployeeTDbyKey(EMP_NUMBER_TD);
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return result;
        }

        public HR_EMPLOYEE_TD[] ReadAllEmployeeTD()
        {
            HR_EMPLOYEE_TD[] result = null;
            try
            {
                var api = new HR100();
                result = api.ReadAllEmployeeTD();
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return result;
        }

        public IQueryable<HR_EMPLOYEE_TD> ReadEmployeeTD()
        {
            IQueryable<HR_EMPLOYEE_TD> result = null;
            try
            {
                var api = new HR100();
                result = api.ReadEmployeeTD();
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return result;
        }

        public ApiAck UpdateEmployeeTD(HR_EMPLOYEE_TD modifiedEmployeeTD)
        {
            ApiAck result = null;
            try
            {
                var api = new HR100();
                result = api.UpdateEmployeeTD(modifiedEmployeeTD);
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return result;
        }

        public ApiAck DeleteEmployeeTD(HR_EMPLOYEE_TD deletingEmployeeTD)
        {
            ApiAck result = null;
            try
            {
                var api = new HR100();
                result = api.DeleteEmployeeTD(deletingEmployeeTD);
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return result;
        }

        public bool EmployeeTDExists(HR_EMPLOYEE_TD existsEmployeeTD)
        {
            bool result = false;
            try
            {
                var api = new HR100();
                result = api.EmployeeTDExists(existsEmployeeTD);
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return result;
        }

        #endregion Employee_Tax_Details

        #region Employee_Permanent_Contacts

        public ResultDTO<HR_EMPLOYEE_PC> QueryEmployeePC(EmployeePCQuery query, int pageSize, int pageNumber)
        {
            ResultDTO<HR_EMPLOYEE_PC> result = null;
            try
            {
                var api = new HR100();
                result = api.QueryEmployeePC(query, pageSize, pageNumber);
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return result;
        }

        public ApiAck CreateEmployeePC(HR_EMPLOYEE_PC newEmployeePC)
        {
            ApiAck ack = null;
            try
            {
                var api = new HR100();
                ack = api.CreateEmployeePC(newEmployeePC);
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return ack;
        }

        public HR_EMPLOYEE_PC ReadEmployeePCbyKey(string EMP_NUMBER_PC)
        {
            HR_EMPLOYEE_PC result = null;
            try
            {
                var api = new HR100();
                result = api.ReadEmployeePCbyKey(EMP_NUMBER_PC);
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return result;
        }

        public HR_EMPLOYEE_PC[] ReadAllEmployeePC()
        {
            HR_EMPLOYEE_PC[] result = null;
            try
            {
                var api = new HR100();
                result = api.ReadAllEmployeePC();
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return result;
        }

        public IQueryable<HR_EMPLOYEE_PC> ReadEmployeePC()
        {
            IQueryable<HR_EMPLOYEE_PC> result = null;
            try
            {
                var api = new HR100();
                result = api.ReadEmployeePC();
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return result;
        }

        public ApiAck UpdateEmployeePC(HR_EMPLOYEE_PC modifiedEmployeePC)
        {
            ApiAck result = null;
            try
            {
                var api = new HR100();
                result = api.UpdateEmployeePC(modifiedEmployeePC);
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return result;
        }

        public ApiAck DeleteEmployeePC(HR_EMPLOYEE_PC deletingEmployeePC)
        {
            ApiAck result = null;
            try
            {
                var api = new HR100();
                result = api.DeleteEmployeePC(deletingEmployeePC);
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return result;
        }

        public bool EmployeePCExists(HR_EMPLOYEE_PC existsEmployeePC)
        {
            bool result = false;
            try
            {
                var api = new HR100();
                result = api.EmployeePCExists(existsEmployeePC);
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return result;
        }

        #endregion Employee_Permanent_Contacts

        #region Employee_Contacts_during_working_days

        public ResultDTO<HR_EMPLOYEE_CDWD> QueryEmployeeCDWD(EmployeeCDWDQuery query, int pageSize, int pageNumber)
        {
            ResultDTO<HR_EMPLOYEE_CDWD> result = null;
            try
            {
                var api = new HR100();
                result = api.QueryEmployeeCDWD(query, pageSize, pageNumber);
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return result;
        }

        public ApiAck CreateEmployeeCDWD(HR_EMPLOYEE_CDWD newEmployeeCDWD)
        {
            ApiAck ack = null;
            try
            {
                var api = new HR100();
                ack = api.CreateEmployeeCDWD(newEmployeeCDWD);
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return ack;
        }

        public HR_EMPLOYEE_CDWD ReadEmployeeCDWDbyKey(string EMP_NUMBER_CDWD)
        {
            HR_EMPLOYEE_CDWD result = null;
            try
            {
                var api = new HR100();
                result = api.ReadEmployeeCDWDbyKey(EMP_NUMBER_CDWD);
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return result;
        }

        public HR_EMPLOYEE_CDWD[] ReadAllEmployeeCDWD()
        {
            HR_EMPLOYEE_CDWD[] result = null;
            try
            {
                var api = new HR100();
                result = api.ReadAllEmployeeCDWD();
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return result;
        }

        public IQueryable<HR_EMPLOYEE_CDWD> ReadEmployeeCDWD()
        {
            IQueryable<HR_EMPLOYEE_CDWD> result = null;
            try
            {
                var api = new HR100();
                result = api.ReadEmployeeCDWD();
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return result;
        }

        public ApiAck UpdateEmployeeCDWD(HR_EMPLOYEE_CDWD modifiedEmployeeCDWD)
        {
            ApiAck result = null;
            try
            {
                var api = new HR100();
                result = api.UpdateEmployeeCDWD(modifiedEmployeeCDWD);
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return result;
        }

        public ApiAck DeleteEmployeeCDWD(HR_EMPLOYEE_CDWD deletingEmployeeCDWD)
        {
            ApiAck result = null;
            try
            {
                var api = new HR100();
                result = api.DeleteEmployeeCDWD(deletingEmployeeCDWD);
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return result;
        }

        public bool EmployeeCDWDExists(HR_EMPLOYEE_CDWD existsEmployeeCDWD)
        {
            bool result = false;
            try
            {
                var api = new HR100();
                result = api.EmployeeCDWDExists(existsEmployeeCDWD);
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return result;
        }

        #endregion Employee_Contacts_during_working_days

        #region Employee_Official_contacts

        public ResultDTO<HR_EMPLOYEE_OC> QueryEmployeeOC(EmployeeOCQuery query, int pageSize, int pageNumber)
        {
            ResultDTO<HR_EMPLOYEE_OC> result = null;
            try
            {
                var api = new HR100();
                result = api.QueryEmployeeOC(query, pageSize, pageNumber);
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return result;
        }

        public ApiAck CreateEmployeeOC(HR_EMPLOYEE_OC newEmployeeOC)
        {
            ApiAck ack = null;
            try
            {
                var api = new HR100();
                ack = api.CreateEmployeeOC(newEmployeeOC);
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return ack;
        }

        public HR_EMPLOYEE_OC ReadEmployeeOCbyKey(string EMP_NUMBER_OC)
        {
            HR_EMPLOYEE_OC result = null;
            try
            {
                var api = new HR100();
                result = api.ReadEmployeeOCbyKey(EMP_NUMBER_OC);
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return result;
        }

        public HR_EMPLOYEE_OC[] ReadAllEmployeeOC()
        {
            HR_EMPLOYEE_OC[] result = null;
            try
            {
                var api = new HR100();
                result = api.ReadAllEmployeeOC();
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return result;
        }

        public IQueryable<HR_EMPLOYEE_OC> ReadEmployeeOC()
        {
            IQueryable<HR_EMPLOYEE_OC> result = null;
            try
            {
                var api = new HR100();
                result = api.ReadEmployeeOC();
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return result;
        }

        public ApiAck UpdateEmployeeOC(HR_EMPLOYEE_OC modifiedEmployeeOC)
        {
            ApiAck result = null;
            try
            {
                var api = new HR100();
                result = api.UpdateEmployeeOC(modifiedEmployeeOC);
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return result;
        }

        public ApiAck DeleteEmployeeOC(HR_EMPLOYEE_OC deletingEmployeeOC)
        {
            ApiAck result = null;
            try
            {
                var api = new HR100();
                result = api.DeleteEmployeeOC(deletingEmployeeOC);
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return result;
        }

        public bool EmployeeOCExists(HR_EMPLOYEE_OC existsEmployeeOC)
        {
            bool result = false;
            try
            {
                var api = new HR100();
                result = api.EmployeeOCExists(existsEmployeeOC);
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return result;
        }

        #endregion Employee_Official_contacts

        #region Employee_EIM_Work_Station_Details

        public ResultDTO<HR_EMPLOYEE_EIM_WS> QueryEmployeeEIMWS(EmployeeEIMWSQuery query, int pageSize, int pageNumber)
        {
            ResultDTO<HR_EMPLOYEE_EIM_WS> result = null;
            try
            {
                var api = new HR100();
                result = api.QueryEmployeeEIMWS(query, pageSize, pageNumber);
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return result;
        }

        public ApiAck CreateEmployeeEIMWS(HR_EMPLOYEE_EIM_WS newEmployeeEIMWS)
        {
            ApiAck ack = null;
            try
            {
                var api = new HR100();
                ack = api.CreateEmployeeEIMWS(newEmployeeEIMWS);
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return ack;
        }

        public HR_EMPLOYEE_EIM_WS ReadEmployeeEIMWSbyKey(string EMP_NUMBER_EIM_WS)
        {
            HR_EMPLOYEE_EIM_WS result = null;
            try
            {
                var api = new HR100();
                result = api.ReadEmployeeEIMWSbyKey(EMP_NUMBER_EIM_WS);
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return result;
        }

        public HR_EMPLOYEE_EIM_WS[] ReadAllEmployeeEIMWS()
        {
            HR_EMPLOYEE_EIM_WS[] result = null;
            try
            {
                var api = new HR100();
                result = api.ReadAllEmployeeEIMWS();
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return result;
        }

        public IQueryable<HR_EMPLOYEE_EIM_WS> ReadEmployeeEIMWS()
        {
            IQueryable<HR_EMPLOYEE_EIM_WS> result = null;
            try
            {
                var api = new HR100();
                result = api.ReadEmployeeEIMWS();
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return result;
        }

        public ApiAck UpdateEmployeeEIMWS(HR_EMPLOYEE_EIM_WS modifiedEmployeeEIMWS)
        {
            ApiAck result = null;
            try
            {
                var api = new HR100();
                result = api.UpdateEmployeeEIMWS(modifiedEmployeeEIMWS);
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return result;
        }

        public ApiAck DeleteEmployeeEIMWS(HR_EMPLOYEE_EIM_WS deletingEmployeeEIMWS)
        {
            ApiAck result = null;
            try
            {
                var api = new HR100();
                result = api.DeleteEmployeeEIMWS(deletingEmployeeEIMWS);
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return result;
        }

        public bool EmployeeEIMWSExists(HR_EMPLOYEE_EIM_WS existsEmployeeEIMWS)
        {
            bool result = false;
            try
            {
                var api = new HR100();
                result = api.EmployeeEIMWSExists(existsEmployeeEIMWS);
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return result;
        }

        #endregion Employee_EIM_Work_Station_Details

        #region Nationality

        public ResultDTO<HR_NATIONALITY> QueryNationality(NationalityQuery query, int pageSize, int pageNumber)
        {
            ResultDTO<HR_NATIONALITY> result = null;
            try
            {
                var api = new HR100();
                result = api.QueryNationality(query, pageSize, pageNumber);
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return result;
        }

        #endregion Nationality

        #region Religion

        public ResultDTO<HR_RELIGION> QueryReligion(ReligionQuery query, int pageSize, int pageNumber)
        {
            ResultDTO<HR_RELIGION> result = null;
            try
            {
                var api = new HR100();
                result = api.QueryReligion(query, pageSize, pageNumber);
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return result;
        }

        #endregion Religion

        #region Salary Grade

        public ResultDTO<HR_SALARY_GRADE> QuerySalaryGrade(SalaryGradeQuery query, int pageSize, int pageNumber)
        {
            ResultDTO<HR_SALARY_GRADE> result = null;
            try
            {
                var api = new HR100();
                result = api.QuerySalaryGrade(query, pageSize, pageNumber);
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return result;
        }

        #endregion Salary Grade

        #region Corporate Title

        public ResultDTO<HR_CORPORATE_TITLE> QueryCorporateTitle(CorporateTitleQuery query, int pageSize, int pageNumber)
        {
            ResultDTO<HR_CORPORATE_TITLE> result = null;
            try
            {
                var api = new HR100();
                result = api.QueryCorporateTitle(query, pageSize, pageNumber);
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return result;
        }

        #endregion Corporate Title

        #region Designation

        public ResultDTO<HR_DESIGNATION> QueryDesignation(DesignationQuery query, int pageSize, int pageNumber)
        {
            ResultDTO<HR_DESIGNATION> result = null;
            try
            {
                var api = new HR100();
                result = api.QueryDesignation(query, pageSize, pageNumber);
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return result;
        }

        #endregion Designation

        #region JDCategory

        public ResultDTO<HR_JD_CATEGORY> QueryJDCategory(JDCategoryQuery query, int pageSize, int pageNumber)
        {
            ResultDTO<HR_JD_CATEGORY> result = null;
            try
            {
                var api = new HR100();
                result = api.QueryJDCategory(query, pageSize, pageNumber);
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return result;
        }

        #endregion JDCategory

        #region FixedTxnAttributes

        public ResultDTO<ERP_FixedTxnAttributes> QueryFixedTransactionAttributes(FixedTxnAttributesQuery query, int pageSize, int pageNumber)
        {
            ResultDTO<ERP_FixedTxnAttributes> result = null;
            try
            {
                var api = new FM100();
                result = api.QueryFixedTransactionAttributes(query, pageSize, pageNumber);
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return result;
        }

        public ApiAck CreateFixedTxnAttributes(ERP_FixedTxnAttributes newFixedTxnAttributes)
        {
            ApiAck ack = null;
            newFixedTxnAttributes.EnteredUser = username;
            newFixedTxnAttributes.EnteredFrom = from;
            newFixedTxnAttributes.EnteredDate = date;
            try
            {
                var api = new FM100();
                ack = api.CreateFixedTxnAttributes(newFixedTxnAttributes);
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return ack;
        }

        public ERP_FixedTxnAttributes ReadFixedTxnAttributesbyKey(string DocCode, string TxCode, string GLCode)
        {
            ERP_FixedTxnAttributes result = null;
            try
            {
                var api = new FM100();
                result = api.ReadFixedTxnAttributesbyKey(DocCode, TxCode, GLCode);
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return result;
        }

        public ERP_FixedTxnAttributes[] ReadAllFixedTxnAttributes()
        {
            ERP_FixedTxnAttributes[] result = null;
            try
            {
                var api = new FM100();
                result = api.ReadAllFixedTxnAttributes();
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return result;
        }

        public IQueryable<ERP_FixedTxnAttributes> ReadFixedTxnAttributes()
        {
            IQueryable<ERP_FixedTxnAttributes> result = null;
            try
            {
                var api = new FM100();
                result = api.ReadFixedTxnAttributes();
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return result;
        }

        public ApiAck UpdateFixedTxnAttributes(ERP_FixedTxnAttributes modifiedFixedTxnAttributes)
        {
            ApiAck result = null;
            modifiedFixedTxnAttributes.ModifiedUser = username;
            modifiedFixedTxnAttributes.ModifiedFrom = from;
            modifiedFixedTxnAttributes.ModifiedDate = date;
            try
            {
                var api = new FM100();
                result = api.UpdateFixedTxnAttributes(modifiedFixedTxnAttributes);
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return result;
        }

        public ApiAck DeleteFixedTxnAttributes(ERP_FixedTxnAttributes deletingFixedTxnAttributes)
        {
            ApiAck result = null;
            try
            {
                var api = new FM100();
                result = api.DeleteFixedTxnAttributes(deletingFixedTxnAttributes);
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return result;
        }

        public bool FixedTxnAttributesExists(ERP_FixedTxnAttributes existsFixedTxnAttributes)
        {
            bool result = false;
            try
            {
                var api = new FM100();
                result = api.FixedTxnAttributesExists(existsFixedTxnAttributes);
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return result;
        }

        #endregion FixedTxnAttributes

        #region LastTransactionInfo

        public ResultDTO<ERP_LastTransactionInfo> QueryLastTransactionInfo(LastTransactionInfoQuery query, int pageSize, int pageNumber)
        {
            ResultDTO<ERP_LastTransactionInfo> result = null;
            try
            {
                var api = new FM100();
                result = api.QueryLastTransactionInfo(query, pageSize, pageNumber);
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return result;
        }

        public ApiAck CreateLastTransactionInfo(ERP_LastTransactionInfo newLastTransactionInfo)
        {
            ApiAck ack = null;
            newLastTransactionInfo.EnteredUser = username;
            newLastTransactionInfo.EnteredFrom = from;
            newLastTransactionInfo.EnteredDate = date;
            try
            {
                var api = new FM100();
                ack = api.CreateLastTransactionInfo(newLastTransactionInfo);
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return ack;
        }

        public ERP_LastTransactionInfo ReadLastTransactionInfobyKey(string ProductCode, string SubSystemCode, string DocCode, string TxCode)
        {
            ERP_LastTransactionInfo result = null;
            try
            {
                var api = new FM100();
                result = api.ReadLastTransactionInfobyKey(ProductCode, SubSystemCode, DocCode, TxCode);
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return result;
        }

        public ResultDTO<ERP_LastTransactionInfo> ReadAllLastTransactionInfo(int pageSize, int pageNumber)
        {
            ResultDTO<ERP_LastTransactionInfo> result = null;
            try
            {
                var api = new FM100();
                result = api.ReadAllLastTransactionInfo(pageSize, pageNumber);
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return result;
        }

        public IQueryable<ERP_LastTransactionInfo> ReadLastTransactionInfo()
        {
            IQueryable<ERP_LastTransactionInfo> result = null;
            try
            {
                var api = new FM100();
                result = api.ReadLastTransactionInfo();
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return result;
        }

        public ApiAck UpdateLastTransactionInfo(ERP_LastTransactionInfo modifiedLastTransactionInfo)
        {
            ApiAck result = null;
            modifiedLastTransactionInfo.ModifiedUser = username;
            modifiedLastTransactionInfo.ModifiedFrom = from;
            modifiedLastTransactionInfo.ModifiedDate = date;
            try
            {
                var api = new FM100();
                result = api.UpdateLastTransactionInfo(modifiedLastTransactionInfo);
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return result;
        }

        public ApiAck DeleteLastTransactionInfo(ERP_LastTransactionInfo deletingLastTransactionInfo)
        {
            ApiAck result = null;
            try
            {
                var api = new FM100();
                result = api.DeleteLastTransactionInfo(deletingLastTransactionInfo);
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return result;
        }

        public bool LastTransactionInfoExists(ERP_LastTransactionInfo existsLastTransactionInfo)
        {
            bool result = false;
            try
            {
                var api = new FM100();
                result = api.LastTransactionInfoExists(existsLastTransactionInfo);
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return result;
        }

        #endregion LastTransactionInfo

        #region AccountSubType

        public ResultDTO<FIN_AccountSubType> QueryAccountSubType(AccountSubTypeQuery query, int pageSize, int pageNumber)
        {
            ResultDTO<FIN_AccountSubType> result = null;
            try
            {
                var api = new FM100();
                result = api.QueryAccountSubType(query, pageSize, pageNumber);
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return result;
        }

        public ApiAck CreateAccountSubType(FIN_AccountSubType newAccountSubType)
        {
            ApiAck ack = null;
            newAccountSubType.EnteredUser = username;
            newAccountSubType.EnteredFrom = from;
            newAccountSubType.EnteredDate = date;
            try
            {
                var api = new FM100();
                ack = api.CreateAccountSubType(newAccountSubType);
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return ack;
        }

        public FIN_AccountSubType ReadAccountSubTypebyKey(int AccountType, int AccountSubType)
        {
            FIN_AccountSubType result = null;
            try
            {
                var api = new FM100();
                result = api.ReadAccountSubTypebyKey(AccountType, AccountSubType);
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return result;
        }

        public FIN_AccountSubType[] ReadAllAccountSubType()
        {
            FIN_AccountSubType[] result = null;
            try
            {
                var api = new FM100();
                result = api.ReadAllAccountSubType();
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return result;
        }

        public IQueryable<FIN_AccountSubType> ReadAccountSubType()
        {
            IQueryable<FIN_AccountSubType> result = null;
            try
            {
                var api = new FM100();
                result = api.ReadAccountSubType();
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return result;
        }

        public ApiAck UpdateAccountSubType(FIN_AccountSubType modifiedAccountSubType)
        {
            ApiAck result = null;
            modifiedAccountSubType.ModifiedUser = username;
            modifiedAccountSubType.ModifiedFrom = from;
            modifiedAccountSubType.ModifiedDate = date;
            try
            {
                var api = new FM100();
                result = api.UpdateAccountSubType(modifiedAccountSubType);
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return result;
        }

        public ApiAck DeleteAccountSubType(FIN_AccountSubType deletingAccountSubType)
        {
            ApiAck result = null;
            try
            {
                var api = new FM100();
                result = api.DeleteAccountSubType(deletingAccountSubType);
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return result;
        }

        public bool AccountSubTypeExists(FIN_AccountSubType existsAccountSubType)
        {
            bool result = false;
            try
            {
                var api = new FM100();
                result = api.AccountSubTypeExists(existsAccountSubType);
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return result;
        }

        #endregion AccountSubType

        #region AccountSubTypeCategory

        public ResultDTO<FIN_AccountSubTypeCategory> QueryAccountSubTypeCategory(AccountSubTypeCategoryQuery query, int pageSize, int pageNumber)
        {
            ResultDTO<FIN_AccountSubTypeCategory> result = null;
            try
            {
                var api = new FM100();
                result = api.QueryAccountSubTypeCategory(query, pageSize, pageNumber);
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return result;
        }

        public ApiAck CreateAccountSubTypeCategory(FIN_AccountSubTypeCategory newAccountSubTypeCategory)
        {
            ApiAck ack = null;
            newAccountSubTypeCategory.EnteredUser = username;
            newAccountSubTypeCategory.EnteredFrom = from;
            newAccountSubTypeCategory.EnteredDate = date;
            try
            {
                var api = new FM100();
                ack = api.CreateAccountSubTypeCategory(newAccountSubTypeCategory);
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return ack;
        }

        public FIN_AccountSubTypeCategory ReadAccountSubTypeCategorybyKey(int AccountType, int AccountSubType, int AccountSubCatType)
        {
            FIN_AccountSubTypeCategory result = null;
            try
            {
                var api = new FM100();
                result = api.ReadAccountSubTypeCategorybyKey(AccountType, AccountSubType, AccountSubCatType);
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return result;
        }

        public FIN_AccountSubTypeCategory[] ReadAllAccountSubTypeCategory()
        {
            FIN_AccountSubTypeCategory[] result = null;
            try
            {
                var api = new FM100();
                result = api.ReadAllAccountSubTypeCategory();
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return result;
        }

        public IQueryable<FIN_AccountSubTypeCategory> ReadAccountSubTypeCategory()
        {
            IQueryable<FIN_AccountSubTypeCategory> result = null;
            try
            {
                var api = new FM100();
                result = api.ReadAccountSubTypeCategory();
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return result;
        }

        public ApiAck UpdateAccountSubTypeCategory(FIN_AccountSubTypeCategory modifiedAccountSubTypeCategory)
        {
            ApiAck result = null;
            modifiedAccountSubTypeCategory.ModifiedUser = username;
            modifiedAccountSubTypeCategory.ModifiedFrom = from;
            modifiedAccountSubTypeCategory.ModifiedDate = date;
            try
            {
                var api = new FM100();
                result = api.UpdateAccountSubTypeCategory(modifiedAccountSubTypeCategory);
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return result;
        }

        public ApiAck DeleteAccountSubTypeCategory(FIN_AccountSubTypeCategory deletingAccountSubTypeCategory)
        {
            ApiAck result = null;
            try
            {
                var api = new FM100();
                result = api.DeleteAccountSubTypeCategory(deletingAccountSubTypeCategory);
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return result;
        }

        public bool AccountSubTypeCategoryExists(FIN_AccountSubTypeCategory existsAccountSubTypeCategory)
        {
            bool result = false;
            try
            {
                var api = new FM100();
                result = api.AccountSubTypeCategoryExists(existsAccountSubTypeCategory);
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return result;
        }

        #endregion AccountSubTypeCategory

        #region AccountType

        public ResultDTO<FIN_AccountType> QueryAccountType(AccountTypeQuery query, int pageSize, int pageNumber)
        {
            ResultDTO<FIN_AccountType> result = null;
            try
            {
                var api = new FM100();
                result = api.QueryAccountType(query, pageSize, pageNumber);
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return result;
        }

        public ApiAck CreateAccountType(FIN_AccountType newAccountType)
        {
            ApiAck ack = null;
            newAccountType.EnteredUser = username;
            newAccountType.EnteredFrom = from;
            newAccountType.EnteredDate = date;
            try
            {
                var api = new FM100();
                ack = api.CreateAccountType(newAccountType);
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return ack;
        }

        public FIN_AccountType ReadAccountTypebyKey(int AccountType)
        {
            FIN_AccountType result = null;
            try
            {
                var api = new FM100();
                result = api.ReadAccountTypebyKey(AccountType);
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return result;
        }

        public ResultDTO<FIN_AccountType> ReadAllAccountType(int pageSize, int pageNumber)
        {
            ResultDTO<FIN_AccountType> result = null;
            try
            {
                var api = new FM100();
                result = api.ReadAllAccountType(pageSize, pageNumber);
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return result;
        }

        public IQueryable<FIN_AccountType> ReadAccountType()
        {
            IQueryable<FIN_AccountType> result = null;
            try
            {
                var api = new FM100();
                result = api.ReadAccountType();
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return result;
        }

        public ApiAck UpdateAccountType(FIN_AccountType modifiedAccountType)
        {
            ApiAck result = null;
            modifiedAccountType.ModifiedUser = username;
            modifiedAccountType.ModifiedFrom = from;
            modifiedAccountType.ModifiedDate = date;
            try
            {
                var api = new FM100();
                result = api.UpdateAccountType(modifiedAccountType);
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return result;
        }

        public ApiAck DeleteAccountType(FIN_AccountType deletingAccountType)
        {
            ApiAck result = null;
            try
            {
                var api = new FM100();
                result = api.DeleteAccountType(deletingAccountType);
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return result;
        }

        public bool AccountTypeExists(FIN_AccountType existsAccountType)
        {
            bool result = false;
            try
            {
                var api = new FM100();
                result = api.AccountTypeExists(existsAccountType);
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return result;
        }

        #endregion AccountType

        #region Bank

        public ApiAck CreateBank(FIN_Bank newBank)
        {
            ApiAck ack = null;
            newBank.EnteredUser = username;
            newBank.EnteredFrom = from;
            newBank.EnteredDate = date;
            try
            {
                var api = new FM100();
                ack = api.CreateBank(newBank);
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return ack;
        }

        public ResultDTO<FIN_Bank> QueryBank(BankQuery query, int pageSize, int pageNumber)
        {
            ResultDTO<FIN_Bank> result = null;
            try
            {
                var api = new FM100();
                result = api.QueryBank(query, pageSize, pageNumber);
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return result;
        }

        public FIN_Bank ReadBankbyKey(string BankCode)
        {
            FIN_Bank result = null;
            try
            {
                var api = new FM100();
                result = api.ReadBankbyKey(BankCode);
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return result;
        }

        public FIN_Bank[] ReadAllBank()
        {
            FIN_Bank[] result = null;
            try
            {
                var api = new FM100();
                result = api.ReadAllBank();
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return result;
        }

        public IQueryable<FIN_Bank> ReadBank()
        {
            IQueryable<FIN_Bank> result = null;
            try
            {
                var api = new FM100();
                result = api.ReadBank();
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return result;
        }

        public ApiAck UpdateBank(FIN_Bank modifiedBank)
        {
            ApiAck result = null;
            modifiedBank.ModifiedUser = username;
            modifiedBank.ModifiedFrom = from;
            modifiedBank.ModifiedDate = date;
            try
            {
                var api = new FM100();
                result = api.UpdateBank(modifiedBank);
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return result;
        }

        public ApiAck DeleteBank(FIN_Bank deletingBank)
        {
            ApiAck result = null;
            try
            {
                var api = new FM100();
                result = api.DeleteBank(deletingBank);
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return result;
        }

        public bool BankExists(FIN_Bank existsBank)
        {
            bool result = false;
            try
            {
                var api = new FM100();
                result = api.BankExists(existsBank);
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return result;
        }

        #endregion Bank

        #region BankAccount

        public ApiAck CreateBankAccount(FIN_BankAccount newBankAccount)
        {
            ApiAck ack = null;
            newBankAccount.EnteredUser = username;
            newBankAccount.EnteredFrom = from;
            newBankAccount.EnteredDate = date;
            try
            {
                var api = new FM100();
                ack = api.CreateBankAccount(newBankAccount);
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return ack;
        }

        public ResultDTO<FIN_BankAccount> QueryBankAccount(BankAccountQuery query, int pageSize, int pageNumber)
        {
            ResultDTO<FIN_BankAccount> result = null;
            try
            {
                var api = new FM100();
                result = api.QueryBankAccount(query, pageSize, pageNumber);
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return result;
        }

        public FIN_BankAccount ReadBankAccountbyKey(string BankCode, string BankBranchCode, int AccountSEQNo)
        {
            FIN_BankAccount result = null;
            try
            {
                var api = new FM100();
                result = api.ReadBankAccountbyKey(BankCode, BankBranchCode, AccountSEQNo);
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return result;
        }

        public FIN_BankAccount[] ReadAllBankAccount()
        {
            FIN_BankAccount[] result = null;
            try
            {
                var api = new FM100();
                result = api.ReadAllBankAccount();
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return result;
        }

        public IQueryable<FIN_BankAccount> ReadBankAccount()
        {
            IQueryable<FIN_BankAccount> result = null;
            try
            {
                var api = new FM100();
                result = api.ReadBankAccount();
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return result;
        }

        public ApiAck UpdateBankAccount(FIN_BankAccount modifiedBankAccount)
        {
            ApiAck result = null;
            modifiedBankAccount.ModifiedUser = username;
            modifiedBankAccount.ModifiedFrom = from;
            modifiedBankAccount.ModifiedDate = date;
            try
            {
                var api = new FM100();
                result = api.UpdateBankAccount(modifiedBankAccount);
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return result;
        }

        public ApiAck DeleteBankAccount(FIN_BankAccount deletingBankAccount)
        {
            ApiAck result = null;
            try
            {
                var api = new FM100();
                result = api.DeleteBankAccount(deletingBankAccount);
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return result;
        }

        public bool BankAccountExists(FIN_BankAccount existsBankAccount)
        {
            bool result = false;
            try
            {
                var api = new FM100();
                result = api.BankAccountExists(existsBankAccount);
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return result;
        }

        #endregion BankAccount

        #region BankBranch

        public ApiAck CreateBankBranch(FIN_BankBranch newBankBranch)
        {
            ApiAck ack = null;
            newBankBranch.EnteredUser = username;
            newBankBranch.EnteredFrom = from;
            newBankBranch.EnteredDate = date;
            try
            {
                var api = new FM100();
                ack = api.CreateBankBranch(newBankBranch);
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return ack;
        }

        public ResultDTO<FIN_BankBranch> QueryBankBranch(BankBranchQuery query, int pageSize, int pageNumber)
        {
            ResultDTO<FIN_BankBranch> result = null;
            try
            {
                var api = new FM100();
                result = api.QueryBankBranch(query, pageSize, pageNumber);
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return result;
        }

        public FIN_BankBranch ReadBankBranchbyKey(string BankCode, string BankBranchCode)
        {
            FIN_BankBranch result = null;
            try
            {
                var api = new FM100();
                result = api.ReadBankBranchbyKey(BankCode, BankBranchCode);
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return result;
        }

        public FIN_BankBranch[] ReadAllBankBranch()
        {
            FIN_BankBranch[] result = null;
            try
            {
                var api = new FM100();
                result = api.ReadAllBankBranch();
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return result;
        }

        public IQueryable<FIN_BankBranch> ReadBankBranch()
        {
            IQueryable<FIN_BankBranch> result = null;
            try
            {
                var api = new FM100();
                result = api.ReadBankBranch();
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return result;
        }

        public ApiAck UpdateBankBranch(FIN_BankBranch modifiedBankBranch)
        {
            ApiAck result = null;
            modifiedBankBranch.ModifiedUser = username;
            modifiedBankBranch.ModifiedFrom = from;
            modifiedBankBranch.ModifiedDate = date;
            try
            {
                var api = new FM100();
                result = api.UpdateBankBranch(modifiedBankBranch);
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return result;
        }

        public ApiAck DeleteBankBranch(FIN_BankBranch deletingBankBranch)
        {
            ApiAck result = null;
            try
            {
                var api = new FM100();
                result = api.DeleteBankBranch(deletingBankBranch);
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return result;
        }

        public bool BankBranchExists(FIN_BankBranch existsBankBranch)
        {
            bool result = false;
            try
            {
                var api = new FM100();
                result = api.BankBranchExists(existsBankBranch);
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return result;
        }

        #endregion BankBranch

        #region ControledTransaction

        public ResultDTO<FIN_ControledTransaction> QueryControlledTransaction(ControlledTransactionQuery query, int pageSize, int pageNumber)
        {
            ResultDTO<FIN_ControledTransaction> result = null;
            try
            {
                var api = new FM100();
                result = api.QueryControlledTransaction(query, pageSize, pageNumber);
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return result;
        }

        public ApiAck CreateControledTransaction(FIN_ControledTransaction newControledTransaction)
        {
            ApiAck ack = null;
            newControledTransaction.EnteredUser = username;
            newControledTransaction.EnteredFrom = from;
            newControledTransaction.EnteredDate = date;
            try
            {
                var api = new FM100();
                ack = api.CreateControledTransaction(newControledTransaction);
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return ack;
        }

        public FIN_ControledTransaction ReadControledTransactionbyKey(string DocCode, string TxCode, string GLAccountNo, string CostCenterCode, string CurrencyCode)
        {
            FIN_ControledTransaction result = null;
            try
            {
                var api = new FM100();
                result = api.ReadControledTransactionbyKey(DocCode, TxCode, GLAccountNo, CostCenterCode, CurrencyCode);
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return result;
        }

        public FIN_ControledTransaction[] ReadAllControledTransaction()
        {
            FIN_ControledTransaction[] result = null;
            try
            {
                var api = new FM100();
                result = api.ReadAllControledTransaction();
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return result;
        }

        public IQueryable<FIN_ControledTransaction> ReadControledTransaction()
        {
            IQueryable<FIN_ControledTransaction> result = null;
            try
            {
                var api = new FM100();
                result = api.ReadControledTransaction();
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return result;
        }

        public ApiAck UpdateControledTransaction(FIN_ControledTransaction modifiedControledTransaction)
        {
            ApiAck result = null;
            modifiedControledTransaction.ModifiedUser = username;
            modifiedControledTransaction.ModifiedFrom = from;
            modifiedControledTransaction.ModifiedDate = date;
            try
            {
                var api = new FM100();
                result = api.UpdateControledTransaction(modifiedControledTransaction);
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return result;
        }

        public ApiAck DeleteControledTransaction(FIN_ControledTransaction deletingControledTransaction)
        {
            ApiAck result = null;
            try
            {
                var api = new FM100();
                result = api.DeleteControledTransaction(deletingControledTransaction);
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return result;
        }

        public bool ControledTransactionExists(FIN_ControledTransaction existsControledTransaction)
        {
            bool result = false;
            try
            {
                var api = new FM100();
                result = api.ControledTransactionExists(existsControledTransaction);
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return result;
        }

        #endregion ControledTransaction

        #region CustomerSupplierInfo

        public ResultDTO<FIN_CustomerSupplier_Info> QueryCustomerSupplier(CustomerSupplierQuery query, int pageSize, int pageNumber)
        {
            ResultDTO<FIN_CustomerSupplier_Info> result = null;
            try
            {
                var api = new FM100();
                result = api.QueryCustomerSupplier(query, pageSize, pageNumber);
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return result;
        }
        public ApiAck CreateCustomerSupplierInfo(FIN_CustomerSupplier_Info newCustomerSupplierInfo)
        {
            ApiAck ack = null;
            newCustomerSupplierInfo.EnteredUser = username;
            newCustomerSupplierInfo.EnteredFrom = from;
            newCustomerSupplierInfo.EnteredDate = date;
            try
            {
                var api = new FM100();
                ack = api.CreateCustomerSupplierInfo(newCustomerSupplierInfo);
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return ack;
        }

        public FIN_CustomerSupplier_Info ReadCustomerSupplierInfobyKey(int CustSupFlag, string CusSupCode)
        {
            FIN_CustomerSupplier_Info result = null;
            try
            {
                var api = new FM100();
                result = api.ReadCustomerSupplierInfobyKey(CustSupFlag, CusSupCode);
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return result;
        }

        public FIN_CustomerSupplier_Info[] ReadAllCustomerSupplierInfo()
        {
            FIN_CustomerSupplier_Info[] result = null;
            try
            {
                var api = new FM100();
                result = api.ReadAllCustomerSupplierInfo();
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return result;
        }

        public IQueryable<FIN_CustomerSupplier_Info> ReadCustomerSupplierInfo()
        {
            IQueryable<FIN_CustomerSupplier_Info> result = null;
            try
            {
                var api = new FM100();
                result = api.ReadCustomerSupplierInfo();
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return result;
        }

        public ApiAck UpdateCustomerSupplierInfo(FIN_CustomerSupplier_Info modifiedCustomerSupplierInfo)
        {
            ApiAck result = null;
            modifiedCustomerSupplierInfo.ModifiedUser = username;
            modifiedCustomerSupplierInfo.ModifiedFrom = from;
            modifiedCustomerSupplierInfo.ModifiedDate = date;
            try
            {
                var api = new FM100();
                result = api.UpdateCustomerSupplierInfo(modifiedCustomerSupplierInfo);
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return result;
        }

        public ApiAck DeleteCustomerSupplierInfo(FIN_CustomerSupplier_Info deletingCustomerSupplierInfo)
        {
            ApiAck result = null;
            try
            {
                var api = new FM100();
                result = api.DeleteCustomerSupplierInfo(deletingCustomerSupplierInfo);
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return result;
        }

        public bool CustomerSupplierInfoExists(FIN_CustomerSupplier_Info existsCustomerSupplierInfo)
        {
            bool result = false;
            try
            {
                var api = new FM100();
                result = api.CustomerSupplierInfoExists(existsCustomerSupplierInfo);
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return result;
        }

        #endregion CustomerSupplierInfo

        #region CustomerSupplierBank

        public ResultDTO<FIN_CustomerSupplierBank> QueryCustomerSupplierBank(CustomerSupplierBankQuery query, int pageSize, int pageNumber)
        {
            ResultDTO<FIN_CustomerSupplierBank> result = null;
            try
            {
                var api = new FM100();
                result = api.QueryCustomerSupplierBank(query, pageSize, pageNumber);
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return result;
        }

        public ApiAck CreateCustomerSupplierBank(FIN_CustomerSupplierBank newCustomerSupplierBank)
        {
            ApiAck ack = null;
            newCustomerSupplierBank.EnteredUser = username;
            newCustomerSupplierBank.EnteredFrom = from;
            newCustomerSupplierBank.EnteredDate = date;
            try
            {
                var api = new FM100();
                ack = api.CreateCustomerSupplierBank(newCustomerSupplierBank);
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return ack;
        }

        public FIN_CustomerSupplierBank ReadCustomerSupplierBankbyKey(string CusSupCode, string BankCode, string BranchCode)
        {
            FIN_CustomerSupplierBank result = null;
            try
            {
                var api = new FM100();
                result = api.ReadCustomerSupplierBankbyKey(CusSupCode, BankCode, BranchCode);
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return result;
        }

        public FIN_CustomerSupplierBank[] ReadAllCustomerSupplierBank()
        {
            FIN_CustomerSupplierBank[] result = null;
            try
            {
                var api = new FM100();
                result = api.ReadAllCustomerSupplierBank();
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return result;
        }

        public IQueryable<FIN_CustomerSupplierBank> ReadCustomerSupplierBank()
        {
            IQueryable<FIN_CustomerSupplierBank> result = null;
            try
            {
                var api = new FM100();
                result = api.ReadCustomerSupplierBank();
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return result;
        }

        public ApiAck UpdateCustomerSupplierBank(FIN_CustomerSupplierBank modifiedCustomerSupplierBank)
        {
            ApiAck result = null;
            modifiedCustomerSupplierBank.ModifiedUser  = username;
            modifiedCustomerSupplierBank.ModifiedFrom = from;
            modifiedCustomerSupplierBank.ModifiedDate = date;
            try
            {
                var api = new FM100();
                result = api.UpdateCustomerSupplierBank(modifiedCustomerSupplierBank);
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return result;
        }

        public ApiAck DeleteCustomerSupplierBank(FIN_CustomerSupplierBank deletingCustomerSupplierBank)
        {
            ApiAck result = null;
            try
            {
                var api = new FM100();
                result = api.DeleteCustomerSupplierBank(deletingCustomerSupplierBank);
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return result;
        }

        public bool CustomerSupplierBankExists(FIN_CustomerSupplierBank existsCustomerSupplierBank)
        {
            bool result = false;
            try
            {
                var api = new FM100();
                result = api.CustomerSupplierBankExists(existsCustomerSupplierBank);
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return result;
        }

        #endregion CustomerSupplierBank

        #region CustSupPeriodBalance

        public ApiAck CreateCustSupPeriodBalance(FIN_CustSupPeriodBalance newCustSupPeriodBalance)
        {
            ApiAck ack = null;
            newCustSupPeriodBalance.EnteredUser = username;
            newCustSupPeriodBalance.EnteredFrom = from;
            newCustSupPeriodBalance.EnteredDate = date;
            try
            {
                var api = new FM100();
                ack = api.CreateCustSupPeriodBalance(newCustSupPeriodBalance);
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return ack;
        }

        public FIN_CustSupPeriodBalance ReadCustSupPeriodBalancebyKey(int CustSupFlag, string CusSupCode, int FinancialYear, int AccountingPeriod)
        {
            FIN_CustSupPeriodBalance result = null;
            try
            {
                var api = new FM100();
                result = api.ReadCustSupPeriodBalancebyKey(CustSupFlag, CusSupCode, FinancialYear, AccountingPeriod);
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return result;
        }

        public FIN_CustSupPeriodBalance[] ReadAllCustSupPeriodBalance()
        {
            FIN_CustSupPeriodBalance[] result = null;
            try
            {
                var api = new FM100();
                result = api.ReadAllCustSupPeriodBalance();
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return result;
        }

        public IQueryable<FIN_CustSupPeriodBalance> ReadCustSupPeriodBalance()
        {
            IQueryable<FIN_CustSupPeriodBalance> result = null;
            try
            {
                var api = new FM100();
                result = api.ReadCustSupPeriodBalance();
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return result;
        }

        public ApiAck UpdateCustSupPeriodBalance(FIN_CustSupPeriodBalance modifiedCustSupPeriodBalance)
        {
            ApiAck result = null;
            modifiedCustSupPeriodBalance.ModifiedUser = username;
            modifiedCustSupPeriodBalance.ModifiedFrom = from;
            modifiedCustSupPeriodBalance.ModifiedDate = date;
            try
            {
                var api = new FM100();
                result = api.UpdateCustSupPeriodBalance(modifiedCustSupPeriodBalance);
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return result;
        }

        public ApiAck DeleteCustSupPeriodBalance(FIN_CustSupPeriodBalance deletingCustSupPeriodBalance)
        {
            ApiAck result = null;
            try
            {
                var api = new FM100();
                result = api.DeleteCustSupPeriodBalance(deletingCustSupPeriodBalance);
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return result;
        }

        public bool CustSupPeriodBalanceExists(FIN_CustSupPeriodBalance existsCustSupPeriodBalance)
        {
            bool result = false;
            try
            {
                var api = new FM100();
                result = api.CustSupPeriodBalanceExists(existsCustSupPeriodBalance);
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return result;
        }

        #endregion CustSupPeriodBalance

        #region GeneralLedgerSummary

        public ApiAck CreateGeneralLedgerSummary(FIN_GeneralLedgerSummary newGeneralLedgerSummary)
        {
            ApiAck ack = null;
            newGeneralLedgerSummary.EnteredUser = username;
            newGeneralLedgerSummary.EnteredFrom = from;
            newGeneralLedgerSummary.EnteredDate = date;
            try
            {
                var api = new FM100();
                ack = api.CreateGeneralLedgerSummary(newGeneralLedgerSummary);
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return ack;
        }

        public FIN_GeneralLedgerSummary ReadGeneralLedgerSummarybyKey(string AccountNo, string CurrencyCode, int FinancialYear, short AccountingPeriod)
        {
            FIN_GeneralLedgerSummary result = null;
            try
            {
                var api = new FM100();
                result = api.ReadGeneralLedgerSummarybyKey(AccountNo, CurrencyCode, FinancialYear, AccountingPeriod);
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return result;
        }

        public FIN_GeneralLedgerSummary[] ReadAllGeneralLedgerSummary()
        {
            FIN_GeneralLedgerSummary[] result = null;
            try
            {
                var api = new FM100();
                result = api.ReadAllGeneralLedgerSummary();
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return result;
        }

        public IQueryable<FIN_GeneralLedgerSummary> ReadGeneralLedgerSummary()
        {
            IQueryable<FIN_GeneralLedgerSummary> result = null;
            try
            {
                var api = new FM100();
                result = api.ReadGeneralLedgerSummary();
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return result;
        }

        public ApiAck UpdateGeneralLedgerSummary(FIN_GeneralLedgerSummary modifiedGeneralLedgerSummary)
        {
            ApiAck result = null;
            modifiedGeneralLedgerSummary.ModifiedUser = username;
            modifiedGeneralLedgerSummary.ModifiedFrom = from;
            modifiedGeneralLedgerSummary.ModifiedDate = date;
            try
            {
                var api = new FM100();
                result = api.UpdateGeneralLedgerSummary(modifiedGeneralLedgerSummary);
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return result;
        }

        public ApiAck DeleteGeneralLedgerSummary(FIN_GeneralLedgerSummary deletingGeneralLedgerSummary)
        {
            ApiAck result = null;
            try
            {
                var api = new FM100();
                result = api.DeleteGeneralLedgerSummary(deletingGeneralLedgerSummary);
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return result;
        }

        public bool GeneralLedgerSummaryExists(FIN_GeneralLedgerSummary existsGeneralLedgerSummary)
        {
            bool result = false;
            try
            {
                var api = new FM100();
                result = api.GeneralLedgerSummaryExists(existsGeneralLedgerSummary);
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return result;
        }

        #endregion GeneralLedgerSummary

        #region PrimaryTransaction

        public ResultDTO<FIN_PrimaryTransaction> QueryPrimaryTransaction(PrimaryTransactionQuery query)
        {
            ResultDTO<FIN_PrimaryTransaction> result = null;
            try
            {
                var api = new FM100();
                result = api.QueryPrimaryTransaction(query);
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return result;
        }

        public ApiAck CreatePrimaryTransaction(FIN_PrimaryTransaction newPrimaryTransaction)
        {
            ApiAck ack = null;
            newPrimaryTransaction.EnteredUser = username;
            newPrimaryTransaction.EnteredFrom = from;
            newPrimaryTransaction.EnteredDate = date;
            try
            {
                var api = new FM100();
                ack = api.CreatePrimaryTransaction(newPrimaryTransaction);
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return ack;
        }

        public FIN_PrimaryTransaction ReadPrimaryTransactionbyKey(string DocCode, string TxCode, int TxSerial, int VoucherNumber, int FinancialYear)
        {
            FIN_PrimaryTransaction result = null;
            try
            {
                var api = new FM100();
                result = api.ReadPrimaryTransactionbyKey(DocCode, TxCode, TxSerial, VoucherNumber, FinancialYear);
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return result;
        }

        public FIN_PrimaryTransaction[] ReadAllPrimaryTransaction()
        {
            FIN_PrimaryTransaction[] result = null;
            try
            {
                var api = new FM100();
                result = api.ReadAllPrimaryTransaction();
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return result;
        }

        public IQueryable<FIN_PrimaryTransaction> ReadPrimaryTransaction()
        {
            IQueryable<FIN_PrimaryTransaction> result = null;
            try
            {
                var api = new FM100();
                result = api.ReadPrimaryTransaction();
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return result;
        }

        public ApiAck UpdatePrimaryTransaction(FIN_PrimaryTransaction modifiedPrimaryTransaction)
        {
            ApiAck result = null;
            modifiedPrimaryTransaction.ModifiedUser = username;
            modifiedPrimaryTransaction.ModifiedFrom = from;
            modifiedPrimaryTransaction.ModifiedDate = date;
            try
            {
                var api = new FM100();
                result = api.UpdatePrimaryTransaction(modifiedPrimaryTransaction);
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return result;
        }

        public ApiAck DeletePrimaryTransaction(FIN_PrimaryTransaction deletingPrimaryTransaction)
        {
            ApiAck result = null;
            try
            {
                var api = new FM100();
                result = api.DeletePrimaryTransaction(deletingPrimaryTransaction);
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return result;
        }

        public bool PrimaryTransactionExists(FIN_PrimaryTransaction existsPrimaryTransaction)
        {
            bool result = false;
            try
            {
                var api = new FM100();
                result = api.PrimaryTransactionExists(existsPrimaryTransaction);
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return result;
        }

        #endregion PrimaryTransaction

        #region ProfitLostType

        public ResultDTO<FIN_ProfitLostType> QueryProfitLostType(ProfitLostQuery query, int pageSize, int pageNumber)
        {
            ResultDTO<FIN_ProfitLostType> result = null;
            try
            {
                var api = new FM100();
                result = api.QueryProfitLostType(query, pageSize, pageNumber);
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return result;
        }

        public ApiAck CreateProfitLostType(FIN_ProfitLostType newProfitLostType)
        {
            ApiAck ack = null;
            newProfitLostType.EnteredUser = username;
            newProfitLostType.EnteredFrom = from;
            newProfitLostType.EnteredDate = date;
            try
            {
                var api = new FM100();
                ack = api.CreateProfitLostType(newProfitLostType);
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return ack;
        }

        public FIN_ProfitLostType ReadProfitLostTypebyKey(int TypeID)
        {
            FIN_ProfitLostType result = null;
            try
            {
                var api = new FM100();
                result = api.ReadProfitLostTypebyKey(TypeID);
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return result;
        }

        public FIN_ProfitLostType[] ReadAllProfitLostType()
        {
            FIN_ProfitLostType[] result = null;
            try
            {
                var api = new FM100();
                result = api.ReadAllProfitLostType();
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return result;
        }

        public IQueryable<FIN_ProfitLostType> ReadProfitLostType()
        {
            IQueryable<FIN_ProfitLostType> result = null;
            try
            {
                var api = new FM100();
                result = api.ReadProfitLostType();
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return result;
        }

        public ApiAck UpdateProfitLostType(FIN_ProfitLostType modifiedProfitLostType)
        {
            ApiAck result = null;
            modifiedProfitLostType.ModifiedUser = username;
            modifiedProfitLostType.ModifiedFrom = from;
            modifiedProfitLostType.ModifiedDate = date;
            try
            {
                var api = new FM100();
                result = api.UpdateProfitLostType(modifiedProfitLostType);
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return result;
        }

        public ApiAck DeleteProfitLostType(FIN_ProfitLostType deletingProfitLostType)
        {
            ApiAck result = null;
            try
            {
                var api = new FM100();
                result = api.DeleteProfitLostType(deletingProfitLostType);
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return result;
        }

        public bool ProfitLostTypeExists(FIN_ProfitLostType existsProfitLostType)
        {
            bool result = false;
            try
            {
                var api = new FM100();
                result = api.ProfitLostTypeExists(existsProfitLostType);
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return result;
        }

        #endregion ProfitLostType

        #region SecondaryTransaction

        public ApiAck CreateSecondaryTransaction(FIN_SecondaryTransaction newSecondaryTransaction)
        {
            ApiAck ack = null;
            newSecondaryTransaction.EnteredUser = username;
            newSecondaryTransaction.EnteredFrom = from;
            newSecondaryTransaction.EnteredDate = date;
            try
            {
                var api = new FM100();
                ack = api.CreateSecondaryTransaction(newSecondaryTransaction);
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return ack;
        }

        public FIN_SecondaryTransaction ReadSecondaryTransactionbyKey(string DocCode, string TxCode, int TxSerial, int VoucherNumber, int FinancialYear, int TxnSeqNo)
        {
            FIN_SecondaryTransaction result = null;
            try
            {
                var api = new FM100();
                result = api.ReadSecondaryTransactionbyKey(DocCode, TxCode, TxSerial, VoucherNumber, FinancialYear, TxnSeqNo);
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return result;
        }

        public FIN_SecondaryTransaction[] ReadAllSecondaryTransaction()
        {
            FIN_SecondaryTransaction[] result = null;
            try
            {
                var api = new FM100();
                result = api.ReadAllSecondaryTransaction();
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return result;
        }

        public IQueryable<FIN_SecondaryTransaction> ReadSecondaryTransaction()
        {
            IQueryable<FIN_SecondaryTransaction> result = null;
            try
            {
                var api = new FM100();
                result = api.ReadSecondaryTransaction();
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return result;
        }

        public ApiAck UpdateSecondaryTransaction(FIN_SecondaryTransaction modifiedSecondaryTransaction)
        {
            ApiAck result = null;
            modifiedSecondaryTransaction.ModifiedUser = username;
            modifiedSecondaryTransaction.ModifiedFrom = from;
            modifiedSecondaryTransaction.ModifiedDate = date;
            try
            {
                var api = new FM100();
                result = api.UpdateSecondaryTransaction(modifiedSecondaryTransaction);
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return result;
        }

        public ApiAck DeleteSecondaryTransaction(FIN_SecondaryTransaction deletingSecondaryTransaction)
        {
            ApiAck result = null;
            try
            {
                var api = new FM100();
                result = api.DeleteSecondaryTransaction(deletingSecondaryTransaction);
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return result;
        }

        public bool SecondaryTransactionExists(FIN_SecondaryTransaction existsSecondaryTransaction)
        {
            bool result = false;
            try
            {
                var api = new FM100();
                result = api.SecondaryTransactionExists(existsSecondaryTransaction);
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return result;
        }

        #endregion SecondaryTransaction

        #region SpecialAccountType

        public ResultDTO<FIN_SpecialAccountType> QuerySpecialAccountType(SpecialAccountQuery query, int pageSize, int pageNumber)
        {
            ResultDTO<FIN_SpecialAccountType> result = null;
            try
            {
                var api = new FM100();
                result = api.QuerySpecialAccountType(query, pageSize, pageNumber);
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return result;
        }

        public ApiAck CreateSpecialAccountType(FIN_SpecialAccountType newSpecialAccountType)
        {
            ApiAck ack = null;
            newSpecialAccountType.EnteredUser = username;
            newSpecialAccountType.EnteredFrom = from;
            newSpecialAccountType.EnteredDate = date;
            try
            {
                var api = new FM100();
                ack = api.CreateSpecialAccountType(newSpecialAccountType);
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return ack;
        }

        public FIN_SpecialAccountType ReadSpecialAccountTypebyKey(int TypeID)
        {
            FIN_SpecialAccountType result = null;
            try
            {
                var api = new FM100();
                result = api.ReadSpecialAccountTypebyKey(TypeID);
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return result;
        }

        public FIN_SpecialAccountType[] ReadAllSpecialAccountType()
        {
            FIN_SpecialAccountType[] result = null;
            try
            {
                var api = new FM100();
                result = api.ReadAllSpecialAccountType();
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return result;
        }

        public IQueryable<FIN_SpecialAccountType> ReadSpecialAccountType()
        {
            IQueryable<FIN_SpecialAccountType> result = null;
            try
            {
                var api = new FM100();
                result = api.ReadSpecialAccountType();
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return result;
        }

        public ApiAck UpdateSpecialAccountType(FIN_SpecialAccountType modifiedSpecialAccountType)
        {
            ApiAck result = null;
            modifiedSpecialAccountType.ModifiedUser = username;
            modifiedSpecialAccountType.ModifiedFrom = from;
            modifiedSpecialAccountType.ModifiedDate = date;
            try
            {
                var api = new FM100();
                result = api.UpdateSpecialAccountType(modifiedSpecialAccountType);
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return result;
        }

        public ApiAck DeleteSpecialAccountType(FIN_SpecialAccountType deletingSpecialAccountType)
        {
            ApiAck result = null;
            try
            {
                var api = new FM100();
                result = api.DeleteSpecialAccountType(deletingSpecialAccountType);
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return result;
        }

        public bool SpecialAccountTypeExists(FIN_SpecialAccountType existsSpecialAccountType)
        {
            bool result = false;
            try
            {
                var api = new FM100();
                result = api.SpecialAccountTypeExists(existsSpecialAccountType);
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return result;
        }

        #endregion SpecialAccountType

        #region TxnReference

        public ApiAck CreateTxnReference(FIN_TxnReference newTxnReference)
        {
            ApiAck ack = null;
            newTxnReference.EnteredUser = username;
            newTxnReference.EnteredFrom = from;
            newTxnReference.EnteredDate = date;
            try
            {
                var api = new FM100();
                ack = api.CreateTxnReference(newTxnReference);
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return ack;
        }

        public ResultDTO<FIN_TxnReference> QueryTxnReference(TransactionReferenceQuery query, int pageSize, int pageNumber)
        {
            ResultDTO<FIN_TxnReference> ack = null;
            try
            {
                var api = new FM100();
                ack = api.QueryTxnReference(query, pageSize, pageNumber);
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return ack;
        }

        public FIN_TxnReference ReadTxnReferencebyKey(string DocCode, string TxCode, short RefSeq)
        {
            FIN_TxnReference result = null;
            try
            {
                var api = new FM100();
                result = api.ReadTxnReferencebyKey(DocCode, TxCode, RefSeq);
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return result;
        }

        public FIN_TxnReference[] ReadAllTxnReference()
        {
            FIN_TxnReference[] result = null;
            try
            {
                var api = new FM100();
                result = api.ReadAllTxnReference();
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return result;
        }

        public IQueryable<FIN_TxnReference> ReadTxnReference()
        {
            IQueryable<FIN_TxnReference> result = null;
            try
            {
                var api = new FM100();
                result = api.ReadTxnReference();
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return result;
        }

        public ApiAck UpdateTxnReference(FIN_TxnReference modifiedTxnReference)
        {
            ApiAck result = null;
            modifiedTxnReference.ModifiedUser = username;
            modifiedTxnReference.ModifiedFrom = from;
            modifiedTxnReference.ModifiedDate = date;
            try
            {
                var api = new FM100();
                result = api.UpdateTxnReference(modifiedTxnReference);
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return result;
        }

        public ApiAck DeleteTxnReference(FIN_TxnReference deletingTxnReference)
        {
            ApiAck result = null;
            try
            {
                var api = new FM100();
                result = api.DeleteTxnReference(deletingTxnReference);
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return result;
        }

        public bool TxnReferenceExists(FIN_TxnReference existsTxnReference)
        {
            bool result = false;
            try
            {
                var api = new FM100();
                result = api.TxnReferenceExists(existsTxnReference);
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return result;
        }

        #endregion TxnReference

        #region EMPATTACHMENT

        public ApiAck CreateEMPATTACHMENT(HR_EMP_ATTACHMENT newEMPATTACHMENT)
        {
            ApiAck ack = null;
            try
            {
                var api = new HR100();
                ack = api.CreateEMPATTACHMENT(newEMPATTACHMENT);
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return ack;
        }

        public HR_EMP_ATTACHMENT ReadEMPATTACHMENTbyKey(string EMP_NUMBER, decimal EATTACH_ID)
        {
            HR_EMP_ATTACHMENT result = null;
            try
            {
                var api = new HR100();
                result = api.ReadEMPATTACHMENTbyKey(EMP_NUMBER, EATTACH_ID);
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return result;
        }

        public HR_EMP_ATTACHMENT[] ReadAllEMPATTACHMENT()
        {
            HR_EMP_ATTACHMENT[] result = null;
            try
            {
                var api = new HR100();
                result = api.ReadAllEMPATTACHMENT();
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return result;
        }

        public IQueryable<HR_EMP_ATTACHMENT> ReadEMPATTACHMENT()
        {
            IQueryable<HR_EMP_ATTACHMENT> result = null;
            try
            {
                var api = new HR100();
                result = api.ReadEMPATTACHMENT();
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return result;
        }

        public ApiAck UpdateEMPATTACHMENT(HR_EMP_ATTACHMENT modifiedEMPATTACHMENT)
        {
            ApiAck result = null;
            try
            {
                var api = new HR100();
                result = api.UpdateEMPATTACHMENT(modifiedEMPATTACHMENT);
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return result;
        }

        public ApiAck DeleteEMPATTACHMENT(HR_EMP_ATTACHMENT deletingEMPATTACHMENT)
        {
            ApiAck result = null;
            try
            {
                var api = new HR100();
                result = api.DeleteEMPATTACHMENT(deletingEMPATTACHMENT);
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return result;
        }

        public bool EMPATTACHMENTExists(HR_EMP_ATTACHMENT existsEMPATTACHMENT)
        {
            bool result = false;
            try
            {
                var api = new HR100();
                result = api.EMPATTACHMENTExists(existsEMPATTACHMENT);
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return result;
        }

        #endregion EMPATTACHMENT

        #region EMPBANK

        public ResultDTO<HR_EMP_BANK> QueryEMPBANK(EmpBankQuery query, int pageSize, int pageNumber)
        {
            ResultDTO<HR_EMP_BANK> result = null;
            try
            {
                var api = new HR100();
                result = api.QueryEMPBANK(query, pageSize, pageNumber);
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return result;
        }

        public ApiAck CreateEMPBANK(HR_EMP_BANK newEMPBANK)
        {
            ApiAck ack = null;
            try
            {
                var api = new HR100();
                ack = api.CreateEMPBANK(newEMPBANK);
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return ack;
        }

        public HR_EMP_BANK ReadEMPBANKbyKey(string EMP_NUMBER, string BBRANCH_CODE, string BANK_CODE)
        {
            HR_EMP_BANK result = null;
            try
            {
                var api = new HR100();
                result = api.ReadEMPBANKbyKey(EMP_NUMBER, BBRANCH_CODE, BANK_CODE);
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return result;
        }

        public HR_EMP_BANK[] ReadAllEMPBANK()
        {
            HR_EMP_BANK[] result = null;
            try
            {
                var api = new HR100();
                result = api.ReadAllEMPBANK();
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return result;
        }

        public IQueryable<HR_EMP_BANK> ReadEMPBANK()
        {
            IQueryable<HR_EMP_BANK> result = null;
            try
            {
                var api = new HR100();
                result = api.ReadEMPBANK();
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return result;
        }

        public ApiAck UpdateEMPBANK(HR_EMP_BANK modifiedEMPBANK)
        {
            ApiAck result = null;
            try
            {
                var api = new HR100();
                result = api.UpdateEMPBANK(modifiedEMPBANK);
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return result;
        }

        public ApiAck DeleteEMPBANK(HR_EMP_BANK deletingEMPBANK)
        {
            ApiAck result = null;
            try
            {
                var api = new HR100();
                result = api.DeleteEMPBANK(deletingEMPBANK);
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return result;
        }

        public bool EMPBANKExists(HR_EMP_BANK existsEMPBANK)
        {
            bool result = false;
            try
            {
                var api = new HR100();
                result = api.EMPBANKExists(existsEMPBANK);
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return result;
        }

        #endregion EMPBANK

        #region EMPPASSPORT

        public ResultDTO<HR_EMP_PASSPORT> QueryEMPPASSPORT(EmpPassportQuery query, int pageSize, int pageNumber)
        {
            ResultDTO<HR_EMP_PASSPORT> result = null;
            try
            {
                var api = new HR100();
                result = api.QueryEMPPASSPORT(query, pageSize, pageNumber);
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return result;
        }
        public ApiAck CreateEMPPASSPORT(HR_EMP_PASSPORT newEMPPASSPORT)
        {
            ApiAck ack = null;
            try
            {
                var api = new HR100();
                ack = api.CreateEMPPASSPORT(newEMPPASSPORT);
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return ack;
        }

        public HR_EMP_PASSPORT ReadEMPPASSPORTbyKey(string EMP_NUMBER, decimal EP_SEQNO)
        {
            HR_EMP_PASSPORT result = null;
            try
            {
                var api = new HR100();
                result = api.ReadEMPPASSPORTbyKey(EMP_NUMBER, EP_SEQNO);
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return result;
        }

        public HR_EMP_PASSPORT[] ReadAllEMPPASSPORT()
        {
            HR_EMP_PASSPORT[] result = null;
            try
            {
                var api = new HR100();
                result = api.ReadAllEMPPASSPORT();
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return result;
        }

        public IQueryable<HR_EMP_PASSPORT> ReadEMPPASSPORT()
        {
            IQueryable<HR_EMP_PASSPORT> result = null;
            try
            {
                var api = new HR100();
                result = api.ReadEMPPASSPORT();
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return result;
        }

        public ApiAck UpdateEMPPASSPORT(HR_EMP_PASSPORT modifiedEMPPASSPORT)
        {
            ApiAck result = null;
            try
            {
                var api = new HR100();
                result = api.UpdateEMPPASSPORT(modifiedEMPPASSPORT);
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return result;
        }

        public ApiAck DeleteEMPPASSPORT(HR_EMP_PASSPORT deletingEMPPASSPORT)
        {
            ApiAck result = null;
            try
            {
                var api = new HR100();
                result = api.DeleteEMPPASSPORT(deletingEMPPASSPORT);
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return result;
        }

        public bool EMPPASSPORTExists(HR_EMP_PASSPORT existsEMPPASSPORT)
        {
            bool result = false;
            try
            {
                var api = new HR100();
                result = api.EMPPASSPORTExists(existsEMPPASSPORT);
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return result;
        }

        #endregion EMPPASSPORT

        # region HR Dependent

        public ApiAck CreateHRDependent(HR_EMP_RELATIONINFO newHRDependent)
        {
            ApiAck ack = null;
            try
            {
                var api = new HR100();
                ack = api.CreateHRDependent(newHRDependent);
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return ack;
        }

        public IEnumerable<HR_EMP_RELATIONINFO> QueryHRDependent(HRDependentQuery query)
        {
            IEnumerable<HR_EMP_RELATIONINFO> result = null;
            try
            {
                var api = new HR100();
                result = api.QueryHRDependent(query);
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return result;
        }

        public bool HRDependentExists(HR_EMP_RELATIONINFO findHRDependent)
        {
            bool result = false;
            try
            {
                var api = new HR100();
                result = api.HRDependentExists(findHRDependent);
            }
            catch (Exception ex)
            {
                result = false;
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return result;
        }

        public HR_EMP_RELATIONINFO ReadHRDependentbyKey(string EmpID, decimal DependentID)
        {
            HR_EMP_RELATIONINFO result = null;
            try
            {
                var api = new HR100();
                result = api.ReadHRDependentbyKey(EmpID, DependentID);
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return result;
        }

        public HR_EMP_RELATIONINFO[] ReadAllHRDependent()
        {
            HR_EMP_RELATIONINFO[] result = null;
            try
            {
                var api = new HR100();
                result = api.ReadAllHRDependent();
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return result;
        }

        public ApiAck UpdateHRDependent(HR_EMP_RELATIONINFO modifiedHRDependent)
        {
            ApiAck result = null;
            try
            {
                var api = new HR100();
                result = api.UpdateHRDependent(modifiedHRDependent);
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return result;
        }

        public ApiAck DeleteHRDependent(HR_EMP_RELATIONINFO deletingHRDependent)
        {
            ApiAck result = null;
            try
            {
                var api = new HR100();
                result = api.DeleteHRDependent(deletingHRDependent);
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return result;
        }

        #endregion

        #region Handling Exceptions

        /// <summary>
        /// Handles the generic exception.
        /// </summary>
        /// <param name="ex">The ex.</param>
        /// <param name="customMessage">The custom message.</param>
        /// <exception cref="SmartBiz.PCSAPI.Service.Exception.ServiceException"></exception>
        private void HandleGenericException(System.Exception ex, string customMessage)
        {
            _logger.Log(customMessage);
            _logger.Log(ex.Message);

            var exception = new ServiceException(customMessage, EExceptionType.Application);
            throw new FaultException<ServiceException>(exception, ex.Message);
        }

        /// <summary>
        /// Handles the validation exception.
        /// </summary>
        /// <param name="customMessage">The custom message.</param>
        /// <exception cref="SmartBiz.PCSAPI.Service.Exception.ServiceException"></exception>
        private void HandleValidationException(string customMessage)
        {
            _logger.Log(customMessage);

            var exception = new ServiceException(customMessage, EExceptionType.Validation);
            throw new FaultException<ServiceException>(exception, customMessage);
        }

        /// <summary>
        /// Handles the security exception.
        /// </summary>
        /// <param name="customMessage">The custom message.</param>
        /// <exception cref="SmartBiz.PCSAPI.Service.Exception.ServiceException"></exception>
        private void HandleSecurityException(string customMessage)
        {
            _logger.Log(customMessage);

            var exception = new ServiceException(customMessage, EExceptionType.Security);
            throw new FaultException<ServiceException>(exception, customMessage);
        }

        #endregion

        #region CurrencyExchangeRate

        public ResultDTO<FIN_CurrencyExchangeRate> QueryCurrencyExchangeRate(CurrencyExchangeRateQuery query, int pageSize, int pageNumber)
        {
            ResultDTO<FIN_CurrencyExchangeRate> result = null;
            try
            {
                var api = new FM100();
                result = api.QueryCurrencyExchangeRate(query, pageSize, pageNumber);
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return result;
        }

        public ApiAck CreateCurrencyExchangeRate(FIN_CurrencyExchangeRate new_CurrencyExchangeRate)
        {
            ApiAck ack = null;
            new_CurrencyExchangeRate.EnteredUser = username;
            new_CurrencyExchangeRate.EnteredFrom = from;
            new_CurrencyExchangeRate.EnteredDate = date;
            try
            {
                var api = new FM100();
                ack = api.CreateCurrencyExchangeRate(new_CurrencyExchangeRate);
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return ack;
        }

        public FIN_CurrencyExchangeRate ReadCurrencyExchangeRateByKey(string CurrencyCode, short CalenderYear, byte CalenderMonth)
        {
            FIN_CurrencyExchangeRate result = null;
            try
            {
                var api = new FM100();
                result = api.ReadCurrencyExchangeRateByKey(CurrencyCode, CalenderYear, CalenderMonth);
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return result;
        }

        public FIN_CurrencyExchangeRate[] ReadAllCurrencyExchangeRate()
        {
            FIN_CurrencyExchangeRate[] result = null;
            try
            {
                var api = new FM100();
                result = api.ReadAllCurrencyExchangeRate();
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return result;
        }

        public ApiAck UpdateCurrencyExchangeRate(FIN_CurrencyExchangeRate modified_CurrencyExchangeRate)
        {
            ApiAck result = null;
            modified_CurrencyExchangeRate.ModifiedUser = username;
            modified_CurrencyExchangeRate.ModifiedFrom = from;
            modified_CurrencyExchangeRate.ModifiedDate = date;
            try
            {
                var api = new FM100();
                result = api.UpdateCurrencyExchangeRate(modified_CurrencyExchangeRate);
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return result;
        }

        public ApiAck DeleteCurrencyExchangeRate(FIN_CurrencyExchangeRate delete_CurrencyExchangeRate)
        {
            ApiAck result = null;
            try
            {
                var api = new FM100();
                result = api.DeleteCurrencyExchangeRate(delete_CurrencyExchangeRate);
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return result;
        }

        public bool ExistCurrencyExchangeRate(FIN_CurrencyExchangeRate exist_CurrencyExchangeRate)
        {
            bool result = false;
            try
            {
                var api = new FM100();
                result = api.ExistCurrencyExchangeRate(exist_CurrencyExchangeRate);
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return result;
        }

        #endregion

        #region ProcessControl

        public ResultDTO<ERP_ProcessControl> QueryProcessControl(ProcessControlQuery query, int pageSize, int pageNumber)
        {
            ResultDTO<ERP_ProcessControl> result = null;
            try
            {
                var api = new FM100();
                result = api.QueryProcessControl(query, pageSize, pageNumber);
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return result;
        }

        public ApiAck CreateProcessControl(ERP_ProcessControl new_ProcessControl)
        {
            ApiAck ack = null;
            new_ProcessControl.EnteredUser = username;
            new_ProcessControl.EnteredFrom = from;
            new_ProcessControl.EnteredDate = date;
            try
            {
                var api = new FM100();
                ack = api.CreateProcessControl(new_ProcessControl);
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return ack;
        }

        public ERP_ProcessControl ReadProcessControlByKey(string ProductCode)
        {
            ERP_ProcessControl result = null;
            try
            {
                var api = new FM100();
                result = api.ReadProcessControlByKey(ProductCode);
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return result;
        }

        public ERP_ProcessControl[] ReadAllProcessControl()
        {
            ERP_ProcessControl[] result = null;
            try
            {
                var api = new FM100();
                result = api.ReadAllProcessControl();
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return result;
        }

        public ApiAck UpdateProcessControl(ERP_ProcessControl modified_ProcessControl)
        {
            ApiAck result = null;
            modified_ProcessControl.ModifiedUser = username;
            modified_ProcessControl.ModifiedFrom = from;
            modified_ProcessControl.ModifiedDate = date;
            try
            {
                var api = new FM100();
                result = api.UpdateProcessControl(modified_ProcessControl);
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return result;
        }

        public ApiAck DeleteProcessControl(ERP_ProcessControl delete_ProcessControl)
        {
            ApiAck result = null;
            try
            {
                var api = new FM100();
                result = api.DeleteProcessControl(delete_ProcessControl);
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return result;
        }

        public bool ExistProcessControl(ERP_ProcessControl exist_ProcessControl)
        {
            bool result = false;
            try
            {
                var api = new FM100();
                result = api.ExistProcessControl(exist_ProcessControl);
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return result;
        }

        #endregion

        #region Period

        public ResultDTO<ERP_Period> QueryPeriod(PeriodQuery query, int pageSize, int pageNumber)
        {
            ResultDTO<ERP_Period> result = null;
            try
            {
                var api = new FM100();
                result = api.QueryPeriod(query, pageSize, pageNumber);
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return result;
        }

        public ApiAck CreatePeriod(ERP_Period new_Period)
        {
            ApiAck ack = null;
            new_Period.EnteredUser = username;
            new_Period.EnteredFrom = from;
            new_Period.EnteredDate = date;
            try
            {
                var api = new FM100();
                ack = api.CreatePeriod(new_Period);
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return ack;
        }

        public ERP_Period ReadPeriodByKey(string ProductCode, int FinancialYear, int AccountingPeriod)
        {
            ERP_Period result = null;
            try
            {
                var api = new FM100();
                result = api.ReadPeriodByKey(ProductCode, FinancialYear, AccountingPeriod);
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return result;
        }

        public ERP_Period[] ReadAllPeriod()
        {
            ERP_Period[] result = null;
            try
            {
                var api = new FM100();
                result = api.ReadAllPeriod();
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return result;
        }

        public ApiAck UpdatePeriod(ERP_Period modified_Period)
        {
            ApiAck result = null;
            modified_Period.ModifiedUser = username;
            modified_Period.ModifiedFrom = from;
            modified_Period.ModifiedDate = date;
            try
            {
                var api = new FM100();
                result = api.UpdatePeriod(modified_Period);
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return result;
        }

        public ApiAck DeletePeriod(ERP_Period delete_Period)
        {
            ApiAck result = null;
            try
            {
                var api = new FM100();
                result = api.DeletePeriod(delete_Period);
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return result;
        }

        public bool ExistPeriod(ERP_Period exist_Period)
        {
            bool result = false;
            try
            {
                var api = new FM100();
                result = api.ExistPeriod(exist_Period);
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return result;
        }

        #endregion

        #region Emergency

        public ResultDTO<HR_EMP_EMERGENCY> QueryEmergency(EmergencyQuery query, int pageSize, int pageNumber)
        {
            ResultDTO<HR_EMP_EMERGENCY> result = null;
            try
            {
                var api = new HR100();
                result = api.QueryEmergency(query, pageSize, pageNumber);
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return result;
        }

        public ApiAck CreateEmergency(HR_EMP_EMERGENCY new_Emergency)
        {
            ApiAck ack = null;
            try
            {
                var api = new HR100();
                ack = api.CreateEmergency(new_Emergency);
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return ack;
        }

        public HR_EMP_EMERGENCY ReadEmergencyByKey(string emp_number)
        {
            HR_EMP_EMERGENCY result = null;
            try
            {
                var api = new HR100();
                result = api.ReadEmergencyByKey(emp_number);
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return result;
        }

        public HR_EMP_EMERGENCY[] ReadAllEmergency()
        {
            HR_EMP_EMERGENCY[] result = null;
            try
            {
                var api = new HR100();
                result = api.ReadAllEmergency();
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return result;
        }

        public ApiAck UpdateEmergency(HR_EMP_EMERGENCY modified_Emergency)
        {
            ApiAck result = null;
            try
            {
                var api = new HR100();
                result = api.UpdateEmergency(modified_Emergency);
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return result;
        }

        public ApiAck DeleteEmergency(HR_EMP_EMERGENCY delete_Emergency)
        {
            ApiAck result = null;
            try
            {
                var api = new HR100();
                result = api.DeleteEmergency(delete_Emergency);
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return result;
        }

        public bool ExistEmergency(HR_EMP_EMERGENCY exist_Emergency)
        {
            bool result = false;
            try
            {
                var api = new HR100();
                result = api.ExistEmergency(exist_Emergency);
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return result;
        }

        #endregion

        #region Transport

        public ResultDTO<HR_EMP_TRANSPORT> QueryTransport(TransportQuery query, int pageSize, int pageNumber)
        {
            ResultDTO<HR_EMP_TRANSPORT> result = null;
            try
            {
                var api = new HR100();
                result = api.QueryTransport(query, pageSize, pageNumber);
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return result;
        }

        public ApiAck CreateTransport(HR_EMP_TRANSPORT new_Transport)
        {
            ApiAck ack = null;
            try
            {
                var api = new HR100();
                ack = api.CreateTransport(new_Transport);
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return ack;
        }

        public HR_EMP_TRANSPORT ReadTransportByKey(string emp_number)
        {
            HR_EMP_TRANSPORT result = null;
            try
            {
                var api = new HR100();
                result = api.ReadTransportByKey(emp_number);
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return result;
        }

        public HR_EMP_TRANSPORT[] ReadAllTransport()
        {
            HR_EMP_TRANSPORT[] result = null;
            try
            {
                var api = new HR100();
                result = api.ReadAllTransport();
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return result;
        }

        public ApiAck UpdateTransport(HR_EMP_TRANSPORT modified_Transport)
        {
            ApiAck result = null;
            try
            {
                var api = new HR100();
                result = api.UpdateTransport(modified_Transport);
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return result;
        }

        public ApiAck DeleteTransport(HR_EMP_TRANSPORT delete_Transport)
        {
            ApiAck result = null;
            try
            {
                var api = new HR100();
                result = api.DeleteTransport(delete_Transport);
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return result;
        }

        public bool ExistTransport(HR_EMP_TRANSPORT exist_Transport)
        {
            bool result = false;
            try
            {
                var api = new HR100();
                result = api.ExistTransport(exist_Transport);
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return result;
        }

        #endregion

        #region Training

        public ResultDTO<HR_EMP_TRAININGS> QueryTraining(TrainingQuery query, int pageSize, int pageNumber)
        {
            ResultDTO<HR_EMP_TRAININGS> result = null;
            try
            {
                var api = new HR100();
                result = api.QueryTraining(query, pageSize, pageNumber);
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return result;
        }

        public ApiAck CreateTraining(HR_EMP_TRAININGS new_Training)
        {
            ApiAck ack = null;
            try
            {
                var api = new HR100();
                ack = api.CreateTraining(new_Training);
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return ack;
        }

        public HR_EMP_TRAININGS ReadTrainingByKey(string emp_number, int tn_id)
        {
            HR_EMP_TRAININGS result = null;
            try
            {
                var api = new HR100();
                result = api.ReadTrainingByKey(emp_number, tn_id);
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return result;
        }

        public HR_EMP_TRAININGS[] ReadAllTraining()
        {
            HR_EMP_TRAININGS[] result = null;
            try
            {
                var api = new HR100();
                result = api.ReadAllTraining();
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return result;
        }

        public ApiAck UpdateTraining(HR_EMP_TRAININGS modified_Training)
        {
            ApiAck result = null;
            try
            {
                var api = new HR100();
                result = api.UpdateTraining(modified_Training);
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return result;
        }

        public ApiAck DeleteTraining(HR_EMP_TRAININGS delete_Training)
        {
            ApiAck result = null;
            try
            {
                var api = new HR100();
                result = api.DeleteTraining(delete_Training);
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return result;
        }

        public bool ExistTraining(HR_EMP_TRAININGS exist_Training)
        {
            bool result = false;
            try
            {
                var api = new HR100();
                result = api.ExistTraining(exist_Training);
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return result;
        }

        #endregion

        #region Warning

        public ResultDTO<HR_EMP_WARNINGS> QueryWarning(WarningQuery query, int pageSize, int pageNumber)
        {
            ResultDTO<HR_EMP_WARNINGS> result = null;
            try
            {
                var api = new HR100();
                result = api.QueryWarning(query, pageSize, pageNumber);
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return result;
        }

        public ApiAck CreateWarning(HR_EMP_WARNINGS new_Warning)
        {
            ApiAck ack = null;
            try
            {
                var api = new HR100();
                ack = api.CreateWarning(new_Warning);
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return ack;
        }

        public HR_EMP_WARNINGS ReadWarningByKey(string emp_number, int wrn_id)
        {
            HR_EMP_WARNINGS result = null;
            try
            {
                var api = new HR100();
                result = api.ReadWarningByKey(emp_number, wrn_id);
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return result;
        }

        public HR_EMP_WARNINGS[] ReadAllWarning()
        {
            HR_EMP_WARNINGS[] result = null;
            try
            {
                var api = new HR100();
                result = api.ReadAllWarning();
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return result;
        }

        public ApiAck UpdateWarning(HR_EMP_WARNINGS modified_Warning)
        {
            ApiAck result = null;
            try
            {
                var api = new HR100();
                result = api.UpdateWarning(modified_Warning);
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return result;
        }

        public ApiAck DeleteWarning(HR_EMP_WARNINGS delete_Warning)
        {
            ApiAck result = null;
            try
            {
                var api = new HR100();
                result = api.DeleteWarning(delete_Warning);
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return result;
        }

        public bool ExistWarning(HR_EMP_WARNINGS exist_Warning)
        {
            bool result = false;
            try
            {
                var api = new HR100();
                result = api.ExistWarning(exist_Warning);
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return result;
        }

        #endregion

        #region EMPQUALIFICATION

        public ResultDTO<HR_EMP_QUALIFICATION> QueryEMPQUALIFICATION(HRQualificationQuery query, int pageSize, int pageNumber)
        {
            ResultDTO<HR_EMP_QUALIFICATION> result = null;
            try
            {
                var api = new HR100();
                result = api.QueryEMPQUALIFICATION(query, pageSize, pageNumber);
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return result;
        }

        public ApiAck CreateEMPQUALIFICATION(HR_EMP_QUALIFICATION newEMPQUALIFICATION)
        {
            ApiAck ack = null;
            try
            {
                var api = new HR100();
                ack = api.CreateEMPQUALIFICATION(newEMPQUALIFICATION);
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return ack;
        }

        public HR_EMP_QUALIFICATION ReadEMPQUALIFICATIONbyKey(string EMP_NUMBER, string QUALIFI_CODE)
        {
            HR_EMP_QUALIFICATION result = null;
            try
            {
                var api = new HR100();
                result = api.ReadEMPQUALIFICATIONbyKey(EMP_NUMBER, QUALIFI_CODE);
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return result;
        }

        public HR_EMP_QUALIFICATION[] ReadAllEMPQUALIFICATION()
        {
            HR_EMP_QUALIFICATION[] result = null;
            try
            {
                var api = new HR100();
                result = api.ReadAllEMPQUALIFICATION();
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return result;
        }

        public IQueryable<HR_EMP_QUALIFICATION> ReadEMPQUALIFICATION()
        {
            IQueryable<HR_EMP_QUALIFICATION> result = null;
            try
            {
                var api = new HR100();
                result = api.ReadEMPQUALIFICATION();
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return result;
        }

        public ApiAck UpdateEMPQUALIFICATION(HR_EMP_QUALIFICATION modifiedEMPQUALIFICATION)
        {
            ApiAck result = null;
            try
            {
                var api = new HR100();
                result = api.UpdateEMPQUALIFICATION(modifiedEMPQUALIFICATION);
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return result;
        }

        public ApiAck DeleteEMPQUALIFICATION(HR_EMP_QUALIFICATION deletingEMPQUALIFICATION)
        {
            ApiAck result = null;
            try
            {
                var api = new HR100();
                result = api.DeleteEMPQUALIFICATION(deletingEMPQUALIFICATION);
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return result;
        }

        public bool EMPQUALIFICATIONExists(HR_EMP_QUALIFICATION existsEMPQUALIFICATION)
        {
            bool result = false;
            try
            {
                var api = new HR100();
                result = api.EMPQUALIFICATIONExists(existsEMPQUALIFICATION);
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return result;
        }

        #endregion

        #region EMPWORKEXPERIENCE

        public ResultDTO<HR_EMP_WORK_EXPERIENCE> QueryEMPWORKEXPERIENCE(HRWorkExperienceQuery query, int pageSize, int pageNumber)
        {
            ResultDTO<HR_EMP_WORK_EXPERIENCE> result = null;
            try
            {
                var api = new HR100();
                result = api.QueryEMPWORKEXPERIENCE(query, pageSize, pageNumber);
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return result;
        }

        public ApiAck CreateEMPWORK(HR_EMP_WORK_EXPERIENCE newEMPWORK)
        {
            ApiAck ack = null;
            try
            {
                var api = new HR100();
                ack = api.CreateEMPWORK(newEMPWORK);
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return ack;
        }

        public HR_EMP_WORK_EXPERIENCE ReadEMPWORKbyKey(string EMP_NUMBER)
        {
            HR_EMP_WORK_EXPERIENCE result = null;
            try
            {
                var api = new HR100();
                result = api.ReadEMPWORKbyKey(EMP_NUMBER);
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return result;
        }

        public HR_EMP_WORK_EXPERIENCE[] ReadAllEMPWORK()
        {
            HR_EMP_WORK_EXPERIENCE[] result = null;
            try
            {
                var api = new HR100();
                result = api.ReadAllEMPWORK();
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return result;
        }

        public IQueryable<HR_EMP_WORK_EXPERIENCE> ReadEMPWORK()
        {
            IQueryable<HR_EMP_WORK_EXPERIENCE> result = null;
            try
            {
                var api = new HR100();
                result = api.ReadEMPWORK();
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return result;
        }

        public ApiAck UpdateEMPWORK(HR_EMP_WORK_EXPERIENCE modifiedEMPWORK)
        {
            ApiAck result = null;
            try
            {
                var api = new HR100();
                result = api.UpdateEMPWORK(modifiedEMPWORK);
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return result;
        }

        public ApiAck DeleteEMPWORK(HR_EMP_WORK_EXPERIENCE deletingEMPWORK)
        {
            ApiAck result = null;
            try
            {
                var api = new HR100();
                result = api.DeleteEMPWORK(deletingEMPWORK);
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return result;
        }

        public bool EMPWORKExists(HR_EMP_WORK_EXPERIENCE existsEMPWORK)
        {
            bool result = false;
            try
            {
                var api = new HR100();
                result = api.EMPWORKExists(existsEMPWORK);
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return result;
        }

        #endregion

        #region HR Cash Benefit

        public ResultDTO<HR_CASH_BENEFIT> QueryHRCashBenefits(HRCashBenefitQuery query, int pageSize, int pageNumber)
        {
            ResultDTO<HR_CASH_BENEFIT> result = null;
            try
            {
                var api = new HR100();
                result = api.QueryHRCashBenefits(query, pageSize, pageNumber);
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return result;
        }

        public ApiAck CreateHRCashBenefit(HR_CASH_BENEFIT new_HR_Cash_Benifit)
        {
            ApiAck ack = null;
            try
            {
                var api = new HR100();
                ack = api.CreateHRCashBenefit(new_HR_Cash_Benifit);
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return ack;
        }

        public HR_CASH_BENEFIT ReadHRCashBenefitbyKey(string HRCashBenefit)
        {
            HR_CASH_BENEFIT result = null;
            try
            {
                var api = new HR100();
                result = api.ReadHRCashBenefitbyKey(HRCashBenefit);
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return result;
        }

        public ResultDTO<HR_CASH_BENEFIT> ReadAllHRCashBenefit(int pageSize, int pageNumber)
        {
            ResultDTO<HR_CASH_BENEFIT> result = null;
            try
            {
                var api = new HR100();
                result = api.ReadAllHRCashBenefit(pageSize, pageNumber);
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return result;
        }

        public IQueryable<HR_CASH_BENEFIT> ReadHRCashBenefit()
        {
            IQueryable<HR_CASH_BENEFIT> result = null;
            try
            {
                var api = new HR100();
                result = api.ReadHRCashBenefit();
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return result;
        }

        public ApiAck UpdateHRCashBenefit(HR_CASH_BENEFIT modifiedHRCashBenefit)
        {
            ApiAck result = null;
            try
            {
                var api = new HR100();
                result = api.UpdateHRCashBenefit(modifiedHRCashBenefit);
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return result;
        }

        public ApiAck DeleteHRCashBenefit(HR_CASH_BENEFIT deletingHRCashBenefit)
        {
            ApiAck result = null;
            try
            {
                var api = new HR100();
                result = api.DeleteHRCashBenefit(deletingHRCashBenefit);
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return result;
        }

        public bool HRCashBenefitExists(HR_CASH_BENEFIT existsHRCashBenefit)
        {
            bool result = false;
            try
            {
                var api = new HR100();
                result = api.HRCashBenefitExists(existsHRCashBenefit);
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return result;
        }

        #endregion

        #region HR Non Cash Benefit

        public ResultDTO<HR_NONCASH_BENEFIT> QueryHRNonCashBenefits(HRNonCashBenefitQuery query, int pageSize, int pageNumber)
        {
            ResultDTO<HR_NONCASH_BENEFIT> result = null;
            try
            {
                var api = new HR100();
                result = api.QueryHRNonCashBenefits(query, pageSize, pageNumber);
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return result;
        }

        public ApiAck CreateHRNonCashBenefit(HR_NONCASH_BENEFIT new_HR_Non_Cash_Benifit)
        {
            ApiAck ack = null;
            try
            {
                var api = new HR100();
                ack = api.CreateHRNonCashBenefit(new_HR_Non_Cash_Benifit);
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return ack;
        }

        public HR_NONCASH_BENEFIT ReadHRNonCashBenefitbyKey(string HRNonCashBenefit)
        {
            HR_NONCASH_BENEFIT result = null;
            try
            {
                var api = new HR100();
                result = api.ReadHRNonCashBenefitbyKey(HRNonCashBenefit);
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return result;
        }

        public ResultDTO<HR_NONCASH_BENEFIT> ReadAllHRNonCashBenefit(int pageSize, int pageNumber)
        {
            ResultDTO<HR_NONCASH_BENEFIT> result = null;
            try
            {
                var api = new HR100();
                result = api.ReadAllHRNonCashBenefit(pageSize, pageNumber);
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return result;
        }

        public IQueryable<HR_NONCASH_BENEFIT> ReadHRNonCashBenefit()
        {
            IQueryable<HR_NONCASH_BENEFIT> result = null;
            try
            {
                var api = new HR100();
                result = api.ReadHRNonCashBenefit();
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return result;
        }

        public ApiAck UpdateHRNonCashBenefit(HR_NONCASH_BENEFIT modifiedHRNonCashBenefit)
        {
            ApiAck result = null;
            try
            {
                var api = new HR100();
                result = api.UpdateHRNonCashBenefit(modifiedHRNonCashBenefit);
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return result;
        }

        public ApiAck DeleteHRNonCashBenefit(HR_NONCASH_BENEFIT deletingHRNonCashBenefit)
        {
            ApiAck result = null;
            try
            {
                var api = new HR100();
                result = api.DeleteHRNonCashBenefit(deletingHRNonCashBenefit);
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return result;
        }

        public bool HRNonCashBenefitExists(HR_NONCASH_BENEFIT existsHRNonCashBenefit)
        {
            bool result = false;
            try
            {
                var api = new HR100();
                result = api.HRNonCashBenefitExists(existsHRNonCashBenefit);
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return result;
        }

        #endregion

        #region HR Employee Cash Benefits

        public ResultDTO<HR_EMP_CASH_BENEFIT> QueryHREmpCashBenefits(HREmpCashBenefitsQuery query, int pageSize, int pageNumber)
        {
            ResultDTO<HR_EMP_CASH_BENEFIT> result = null;
            try
            {
                var api = new HR100();
                result = api.QueryHREmpCashBenefits(query, pageSize, pageNumber);
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return result;
        }

        public ApiAck CreateHREmpCashBenefits(HR_EMP_CASH_BENEFIT new_FIN_HREmpCashBenefits)
        {
            ApiAck ack = null;
            try
            {
                var api = new HR100();
                ack = api.CreateHREmpCashBenefits(new_FIN_HREmpCashBenefits);
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return ack;
        }

        public HR_EMP_CASH_BENEFIT ReadHREmpCashBenefitsbyKey(string EmpCode, string BenCode)
        {
            HR_EMP_CASH_BENEFIT result = null;
            try
            {
                var api = new HR100();
                result = api.ReadHREmpCashBenefitsbyKey(EmpCode, BenCode);
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return result;
        }

        public ResultDTO<HR_EMP_CASH_BENEFIT> ReadAllHREmpCashBenefit(int pageSize, int pageNumber)
        {
            ResultDTO<HR_EMP_CASH_BENEFIT> result = null;
            try
            {
                var api = new HR100();
                result = api.ReadAllHREmpCashBenefit(pageSize, pageNumber);
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return result;
        }

        public IQueryable<HR_EMP_CASH_BENEFIT> ReadHREmpCashBenefits()
        {
            IQueryable<HR_EMP_CASH_BENEFIT> result = null;
            try
            {
                var api = new HR100();
                result = api.ReadHREmpCashBenefits();
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return result;
        }

        public ApiAck UpdateHREmpCashBenefits(HR_EMP_CASH_BENEFIT modifiedHREmpCashBenefits)
        {
            ApiAck result = null;
            try
            {
                var api = new HR100();
                result = api.UpdateHREmpCashBenefits(modifiedHREmpCashBenefits);
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return result;
        }

        public ApiAck DeleteHREmpCashBenefits(HR_EMP_CASH_BENEFIT deletingHREmpCashBenefits)
        {
            ApiAck result = null;
            try
            {
                var api = new HR100();
                result = api.DeleteHREmpCashBenefits(deletingHREmpCashBenefits);
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return result;
        }

        public bool HREmpCashBenefitsExists(HR_EMP_CASH_BENEFIT existsHREmpCashBenefits)
        {
            bool result = false;
            try
            {
                var api = new HR100();
                result = api.HREmpCashBenefitsExists(existsHREmpCashBenefits);
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return result;
        }

        #endregion

        #region HR Employee Non Cash Benefits

        public ResultDTO<HR_EMP_NONCASH_BENEFIT> QueryHREmpNonCashBenefits(HREmpNonCashBenefitsQuery query, int pageSize, int pageNumber)
        {
            ResultDTO<HR_EMP_NONCASH_BENEFIT> result = null;
            try
            {
                var api = new HR100();
                result = api.QueryHREmpNonCashBenefits(query, pageSize, pageNumber);
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return result;
        }

        public ApiAck CreateHREmpNonCashBenefits(HR_EMP_NONCASH_BENEFIT new_FIN_HREmpNonCashBenefits)
        {
            ApiAck ack = null;
            try
            {
                var api = new HR100();
                ack = api.CreateHREmpNonCashBenefits(new_FIN_HREmpNonCashBenefits);
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return ack;
        }

        public HR_EMP_NONCASH_BENEFIT ReadHREmpNonCashBenefitsbyKey(string EmpCode, string NBenCode)
        {
            HR_EMP_NONCASH_BENEFIT result = null;
            try
            {
                var api = new HR100();
                result = api.ReadHREmpNonCashBenefitsbyKey(EmpCode, NBenCode);
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return result;
        }

        public ResultDTO<HR_EMP_NONCASH_BENEFIT> ReadAllHREmpNonCashBenefit(int pageSize, int pageNumber)
        {
            ResultDTO<HR_EMP_NONCASH_BENEFIT> result = null;
            try
            {
                var api = new HR100();
                result = api.ReadAllHREmpNonCashBenefit(pageSize, pageNumber);
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return result;
        }

        public IQueryable<HR_EMP_NONCASH_BENEFIT> ReadHREmpNonCashBenefits()
        {
            IQueryable<HR_EMP_NONCASH_BENEFIT> result = null;
            try
            {
                var api = new HR100();
                result = api.ReadHREmpNonCashBenefits();
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return result;
        }

        public ApiAck UpdateHREmpNonCashBenefits(HR_EMP_NONCASH_BENEFIT modifiedHREmpNonCashBenefits)
        {
            ApiAck result = null;
            try
            {
                var api = new HR100();
                result = api.UpdateHREmpNonCashBenefits(modifiedHREmpNonCashBenefits);
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return result;
        }

        public ApiAck DeleteHREmpNonCashBenefits(HR_EMP_NONCASH_BENEFIT deletingHREmpNonCashBenefits)
        {
            ApiAck result = null;
            try
            {
                var api = new HR100();
                result = api.DeleteHREmpNonCashBenefits(deletingHREmpNonCashBenefits);
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return result;
        }

        public bool HREmpNonCashBenefitsExists(HR_EMP_NONCASH_BENEFIT existsHREmpNonCashBenefits)
        {
            bool result = false;
            try
            {
                var api = new HR100();
                result = api.HREmpNonCashBenefitsExists(existsHREmpNonCashBenefits);
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return result;
        }

        #endregion

        #region Basic Salary

        public ResultDTO<HR_EMP_BASICSALARY> QueryBasicSalary(BasicSalaryQuery query, int pageSize, int pageNumber)
        {
            ResultDTO<HR_EMP_BASICSALARY> result = null;
            try
            {
                var api = new HR100();
                result = api.QueryBasicSalary(query, pageSize, pageNumber);
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return result;
        }

        public ApiAck CreateBasicSalary(HR_EMP_BASICSALARY new_BasicSalary)
        {
            ApiAck ack = null;
            try
            {
                var api = new HR100();
                ack = api.CreateBasicSalary(new_BasicSalary);
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return ack;
        }

        public HR_EMP_BASICSALARY ReadBasicSalaryByKey(string emp_number, int sal_dtl_year, string sal_grd_code, string currency_id)
        {
            HR_EMP_BASICSALARY result = null;
            try
            {
                var api = new HR100();
                result = api.ReadBasicSalaryByKey(emp_number, sal_dtl_year, sal_grd_code, currency_id);
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return result;
        }

        public HR_EMP_BASICSALARY[] ReadAllBasicSalary()
        {
            HR_EMP_BASICSALARY[] result = null;
            try
            {
                var api = new HR100();
                result = api.ReadAllBasicSalary();
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return result;
        }

        public ApiAck UpdateBasicSalary(HR_EMP_BASICSALARY modified_BasicSalary)
        {
            ApiAck result = null;
            try
            {
                var api = new HR100();
                result = api.UpdateBasicSalary(modified_BasicSalary);
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return result;
        }

        public ApiAck DeleteBasicSalary(HR_EMP_BASICSALARY delete_BasicSalary)
        {
            ApiAck result = null;
            try
            {
                var api = new HR100();
                result = api.DeleteBasicSalary(delete_BasicSalary);
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return result;
        }

        public bool ExistBasicSalary(HR_EMP_BASICSALARY exist_BasicSalary)
        {
            bool result = false;
            try
            {
                var api = new HR100();
                result = api.ExistBasicSalary(exist_BasicSalary);
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return result;
        }

        #endregion

        #region Reporting

        public ResultDTO<HR_EMP_REPORTTO> QueryReporting(ReportingQuery query, int pageSize, int pageNumber)
        {
            ResultDTO<HR_EMP_REPORTTO> result = null;
            try
            {
                var api = new HR100();
                result = api.QueryReporting(query, pageSize, pageNumber);
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return result;
        }

        public ApiAck CreateReporting(HR_EMP_REPORTTO new_Reporting)
        {
            ApiAck ack = null;
            try
            {
                var api = new HR100();
                ack = api.CreateReporting(new_Reporting);
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return ack;
        }

        public HR_EMP_REPORTTO ReadReportingByKey(string emp_number,string emp_number2,Int16 reporting_mode)
        {
            HR_EMP_REPORTTO result = null;
            try
            {
                var api = new HR100();
                result = api.ReadReportingByKey(emp_number,emp_number2,reporting_mode);
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return result;
        }

        public HR_EMP_REPORTTO[] ReadAllReporting()
        {
            HR_EMP_REPORTTO[] result = null;
            try
            {
                var api = new HR100();
                result = api.ReadAllReporting();
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return result;
        }

        public ApiAck UpdateReporting(HR_EMP_REPORTTO modified_Reporting)
        {
            ApiAck result = null;
            try
            {
                var api = new HR100();
                result = api.UpdateReporting(modified_Reporting);
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return result;
        }

        public ApiAck DeleteReporting(HR_EMP_REPORTTO delete_Reporting)
        {
            ApiAck result = null;
            try
            {
                var api = new HR100();
                result = api.DeleteReporting(delete_Reporting);
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return result;
        }

        public bool ExistReporting(HR_EMP_REPORTTO exist_Reporting)
        {
            bool result = false;
            try
            {
                var api = new HR100();
                result = api.ExistReporting(exist_Reporting);
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return result;
        }

        #endregion

        #region Job Specification

        public ResultDTO<HR_EMP_JOBSPEC> QueryJobSpecification(JobSpecificationQuery query, int pageSize, int pageNumber)
        {
            ResultDTO<HR_EMP_JOBSPEC> result = null;
            try
            {
                var api = new HR100();
                result = api.QueryJobSpecification(query, pageSize, pageNumber);
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return result;
        }

        public ApiAck CreateJobSpecification(HR_EMP_JOBSPEC new_JobSpecification)
        {
            ApiAck ack = null;
            try
            {
                var api = new HR100();
                ack = api.CreateJobSpecification(new_JobSpecification);
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return ack;
        }

        public HR_EMP_JOBSPEC ReadJobSpecificationByKey(string emp_number, string jdcat_code)
        {
            HR_EMP_JOBSPEC result = null;
            try
            {
                var api = new HR100();
                result = api.ReadJobSpecificationByKey(emp_number,jdcat_code);
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return result;
        }

        public HR_EMP_JOBSPEC[] ReadAllJobSpecification()
        {
            HR_EMP_JOBSPEC[] result = null;
            try
            {
                var api = new HR100();
                result = api.ReadAllJobSpecification();
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return result;
        }

        public ApiAck UpdateJobSpecification(HR_EMP_JOBSPEC modified_JobSpecification)
        {
            ApiAck result = null;
            try
            {
                var api = new HR100();
                result = api.UpdateJobSpecification(modified_JobSpecification);
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return result;
        }

        public ApiAck DeleteJobSpecification(HR_EMP_JOBSPEC delete_JobSpecification)
        {
            ApiAck result = null;
            try
            {
                var api = new HR100();
                result = api.DeleteJobSpecification(delete_JobSpecification);
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return result;
        }

        public bool ExistJobSpecification(HR_EMP_JOBSPEC exist_JobSpecification)
        {
            bool result = false;
            try
            {
                var api = new HR100();
                result = api.ExistJobSpecification(exist_JobSpecification);
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return result;
        }

        #endregion

        #region Currency2

        public ResultDTO<HR_CURRENCY> QueryCurrency2(CurrencyHRQuery query, int pageSize, int pageNumber)
        {
            ResultDTO<HR_CURRENCY> result = null;
            try
            {
                var api = new HR100();
                result = api.QueryCurrency2(query, pageSize, pageNumber);
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return result;
        }
        #endregion

        #region HR Bank

        public ResultDTO<HR_BANK> QueryHRBank(HRBankQuery query, int pageSize, int pageNumber)
        {
            ResultDTO<HR_BANK> result = null;
            try
            {
                var api = new HR100();
                result = api.QueryHRBank(query, pageSize, pageNumber);
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return result;
        }
        #endregion

        #region HR Bank Branch

        public ResultDTO<HR_BRANCH> QueryHRBankBranch(HRBankBranchQuery query, int pageSize, int pageNumber)
        {
            ResultDTO<HR_BRANCH> result = null;
            try
            {
                var api = new HR100();
                result = api.QueryHRBankBranch(query, pageSize, pageNumber);
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return result;
        }
        #endregion

        #region HR Country

        public ResultDTO<HR_COUNTRY> QueryHRCountry(HRCountryQuery query, int pageSize, int pageNumber)
        {
            ResultDTO<HR_COUNTRY> result = null;
            try
            {
                var api = new HR100();
                result = api.QueryHRCountry(query, pageSize, pageNumber);
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return result;
        }
        #endregion

        #region HR Language

        public ResultDTO<HR_LANGUAGE> QueryHRLanguage(HRLanguageQuery query, int pageSize, int pageNumber)
        {
            ResultDTO<HR_LANGUAGE> result = null;
            try
            {
                var api = new HR100();
                result = api.QueryHRLanguage(query, pageSize, pageNumber);
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return result;
        }

        public ApiAck CreateHRLanguage(HR_LANGUAGE new_HR_Language)
        {
            ApiAck ack = null;
            try
            {
                var api = new HR100();
                ack = api.CreateHRLanguage(new_HR_Language);
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return ack;
        }

        public HR_LANGUAGE ReadHRLanguagebyKey(string HRLanguage)
        {
            HR_LANGUAGE result = null;
            try
            {
                var api = new HR100();
                result = api.ReadHRLanguagebyKey(HRLanguage);
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return result;
        }

        public ResultDTO<HR_LANGUAGE> ReadAllHRLanguage(int pageSize, int pageNumber)
        {
            ResultDTO<HR_LANGUAGE> result = null;
            try
            {
                var api = new HR100();
                result = api.ReadAllHRLanguage(pageSize, pageNumber);
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return result;
        }

        public IQueryable<HR_LANGUAGE> ReadHRLanguage()
        {
            IQueryable<HR_LANGUAGE> result = null;
            try
            {
                var api = new HR100();
                result = api.ReadHRLanguage();
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return result;
        }

        public ApiAck UpdateHRLanguage(HR_LANGUAGE modifiedHRLanguage)
        {
            ApiAck result = null;
            try
            {
                var api = new HR100();
                result = api.UpdateHRLanguage(modifiedHRLanguage);
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return result;
        }

        public ApiAck DeleteHRLanguage(HR_LANGUAGE deletingHRLanguage)
        {
            ApiAck result = null;
            try
            {
                var api = new HR100();
                result = api.DeleteHRLanguage(deletingHRLanguage);
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return result;
        }

        public bool HRLanguageExists(HR_LANGUAGE existsHRLanguage)
        {
            bool result = false;
            try
            {
                var api = new HR100();
                result = api.HRLanguageExists(existsHRLanguage);
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return result;
        }

        #endregion

        #region HR Emp Language

        public ResultDTO<HR_EMP_LANGUAGE> QueryHREmpLanguage(HREmpLanguageQuery query, int pageSize, int pageNumber)
        {
            ResultDTO<HR_EMP_LANGUAGE> result = null;
            try
            {
                var api = new HR100();
                result = api.QueryHREmpLanguage(query, pageSize, pageNumber);
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return result;
        }

        public ApiAck CreateHREmpLanguage(HR_EMP_LANGUAGE new_HR_Emp_Language)
        {
            ApiAck ack = null;
            try
            {
                var api = new HR100();
                ack = api.CreateHREmpLanguage(new_HR_Emp_Language);
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return ack;
        }

        public HR_EMP_LANGUAGE ReadHREmpLanguagebyKey(string EmployeeCode, string LanguageCode)
        {
            HR_EMP_LANGUAGE result = null;
            try
            {
                var api = new HR100();
                result = api.ReadHREmpLanguagebyKey(EmployeeCode, LanguageCode);
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return result;
        }

        public ResultDTO<HR_EMP_LANGUAGE> ReadAllHREmpLanguage(int pageSize, int pageNumber)
        {
            ResultDTO<HR_EMP_LANGUAGE> result = null;
            try
            {
                var api = new HR100();
                result = api.ReadAllHREmpLanguage(pageSize, pageNumber);
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return result;
        }

        public IQueryable<HR_EMP_LANGUAGE> ReadHREmpLanguage()
        {
            IQueryable<HR_EMP_LANGUAGE> result = null;
            try
            {
                var api = new HR100();
                result = api.ReadHREmpLanguage();
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return result;
        }

        public ApiAck UpdateHREmpLanguage(HR_EMP_LANGUAGE modifiedHREmpLanguage)
        {
            ApiAck result = null;
            try
            {
                var api = new HR100();
                result = api.UpdateHREmpLanguage(modifiedHREmpLanguage);
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return result;
        }

        public ApiAck DeleteHREmpLanguage(HR_EMP_LANGUAGE deletingHREmpLanguage)
        {
            ApiAck result = null;
            try
            {
                var api = new HR100();
                result = api.DeleteHREmpLanguage(deletingHREmpLanguage);
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return result;
        }

        public bool HREmpLanguageExists(HR_EMP_LANGUAGE existsHREmpLanguage)
        {
            bool result = false;
            try
            {
                var api = new HR100();
                result = api.HREmpLanguageExists(existsHREmpLanguage);
            }
            catch (Exception ex)
            {
                HandleGenericException(ex, "An exception has caught while performing the requested operation.");
            }
            return result;
        }

        #endregion
    }
}