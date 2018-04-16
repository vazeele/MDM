using SmartBiz.MDMAPI.API.Queries;
using SmartBiz.MDMAPI.Common.Entities;
using SmartBiz.PCSAPI.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;

namespace SmartBiz.MDMAPI.Service.ServiceContract
{
    /// <summary>
    /// Interface IMdmService - Service Contract
    /// </summary>

    [ServiceContract(Name = "MdmService", Namespace = "http://www.SmartBiz.com/MDMAPI")]
    [ServiceKnownType(typeof(ApiAck))]
    public interface IMdmService
    {
        #region ERP_AgentBrokerSalesMan

        [OperationContract]
        ApiAck CreateAgentBrokerSalesMan(ERP_AgentBrokerSalesMan new_ERP_AgentBrokerSalesMan);

        [OperationContract]
        ERP_AgentBrokerSalesMan ReadAgentBrokerSalesManbyKey(string AgentBrokerSalesManCode, int AgentBrokerSalesManFlag);

        [OperationContract]
        ERP_AgentBrokerSalesMan[] ReadAllAgentBrokerSalesMan();

        [OperationContract]
        IQueryable<ERP_AgentBrokerSalesMan> ReadAgentBrokerSalesMan();

        [OperationContract]
        ApiAck UpdateAgentBrokerSalesMan(ERP_AgentBrokerSalesMan modifiedAgentBrokerSalesMan);

        [OperationContract]
        ApiAck DeleteAgentBrokerSalesMan(ERP_AgentBrokerSalesMan deletingAgentBrokerSalesMan);

        [OperationContract]
        ResultDTO<ERP_AgentBrokerSalesMan> QueryAgentBrokerSalesMan(AgentBrokerSalesManQuery query, int pageSize, int pageNumber);

        [OperationContract]
        bool AgentBrokerSalesManExists(ERP_AgentBrokerSalesMan AgentBrokerSalesManexists);

        #endregion ERP_AgentBrokerSalesMan

        #region ERP_UnitConversion

        [OperationContract]
        ApiAck CreateUnitConversion(ERP_UnitConversion new_ERP_UnitConversion);

        [OperationContract]
        ERP_UnitConversion ReadUnitConversionbyKey(string FromUnitCode, string ToUnitCode);

        [OperationContract]
        ERP_UnitConversion[] ReadAllUnitConversion();

        [OperationContract]
        IQueryable<ERP_UnitConversion> ReadUnitConversion();

        [OperationContract]
        ApiAck UpdateUnitConversion(ERP_UnitConversion modifiedUnitCoversion);

        [OperationContract]
        ApiAck DeleteUnitConversion(ERP_UnitConversion deletingUnitConversion);

        [OperationContract]
        ResultDTO<ERP_UnitConversion> QueryUnitConversion(UnitConversionQuery query, int pageSize, int pageNumber);

        [OperationContract]
        bool UnitConversionExists(ERP_UnitConversion UnitConversionexists);

        #endregion ERP_UnitConversion

        #region ERP_UnitDefinition

        [OperationContract]
        ApiAck CreateUnitDefinition(ERP_UnitDefinition new_ERP_UnitDefinition);

        [OperationContract]
        ERP_UnitDefinition ReadUnitDefinitionbyKey(string UnitDefinitionCode);

        [OperationContract]
        ERP_UnitDefinition[] ReadAllUnitDefinition();

        [OperationContract]
        IQueryable<ERP_UnitDefinition> ReadUnitDefinition();

        [OperationContract]
        ApiAck UpdateUnitDefinition(ERP_UnitDefinition modifiedUnitDefinition);

        [OperationContract]
        ApiAck DeleteUnitDefinition(ERP_UnitDefinition deletingUnitDefinition);

        [OperationContract]
        ResultDTO<ERP_UnitDefinition> QueryUnitDefinition(UnitDefinitionQuery query, int pageSize, int pageNumber);

        [OperationContract]
        bool UnitDefinitionExists(ERP_UnitDefinition UnitDefinitionexists);

        #endregion ERP_UnitDefinition

        #region FIN_Region

        [OperationContract]
        ApiAck CreateRegion(FIN_Region new_FIN_Region);

        [OperationContract]
        FIN_Region ReadRegionbyKey(String RegionCode);

        [OperationContract]
        FIN_Region[] ReadAllRegion();

        [OperationContract]
        IQueryable<FIN_Region> ReadRegion();

        [OperationContract]
        ApiAck UpdateRegion(FIN_Region modifiedRegion);

        [OperationContract]
        ApiAck DeleteRegion(FIN_Region deletingRegion);

        [OperationContract]
        bool RegionExists(FIN_Region Regionexists);

        [OperationContract]
        ResultDTO<FIN_Region> QueryRegion(RegionQuery query, int pageSize, int pageNumber);

        #endregion FIN_Region

        #region FIN_Area

        [OperationContract]
        ApiAck CreateArea(FIN_Area new_FIN_Area);

        [OperationContract]
        FIN_Area ReadAreabyKey(String AreaCode, String RegionCode);

        [OperationContract]
        FIN_Area[] ReadAllArea();

        [OperationContract]
        IQueryable<FIN_Area> ReadArea();

        [OperationContract]
        ApiAck UpdateArea(FIN_Area modifiedArea);

        [OperationContract]
        ApiAck DeleteArea(FIN_Area deletingArea);

        [OperationContract]
        bool AreaExists(FIN_Area Areaexists);

        [OperationContract]
        ResultDTO<FIN_Area> QueryArea(AreaQuery query, int pageSize, int pageNumber);

        #endregion FIN_Area

        #region ERP_Document

        [OperationContract]
        ApiAck CreateDocument(ERP_Document new_ERP_Document);

        [OperationContract]
        ResultDTO<ERP_Document> QueryDocument(DocumentQuery query, int pageSize, int pageNumber);

        [OperationContract]
        bool DocumentExists(ERP_Document Documentexists);

        [OperationContract]
        ERP_Document ReadDocumentbyKey(string DocCode);

        [OperationContract]
        ERP_Document[] ReadAllDocument();

        [OperationContract]
        IQueryable<ERP_Document> ReadDocument();

        [OperationContract]
        ApiAck UpdateDocument(ERP_Document modifiedDocument);

        [OperationContract]
        ApiAck DeleteDocument(ERP_Document deletingDocument);

        #endregion ERP_Document

        #region ERP_DocumentAttributes

        [OperationContract]
        ApiAck CreateDocumentAttributes(ERP_DocumentAttributes new_ERP_DocumentAttributes);

        [OperationContract]
        ResultDTO<ERP_DocumentAttributes> QueryDocumentAttributes(DocumentAttributesQuery query, int pageSize, int pageNumber);

        [OperationContract]
        bool DocumentAttributesExists(ERP_DocumentAttributes ERP_DocumentAttributesexists);

        [OperationContract]
        ERP_DocumentAttributes ReadDocumentAttributesbyKey(string DocCode, string TxCode);

        [OperationContract]
        ERP_DocumentAttributes[] ReadAllDocumentAttributes();

        [OperationContract]
        IQueryable<ERP_DocumentAttributes> ReadDocumentAttributes();

        [OperationContract]
        ApiAck UpdateDocumentAttributes(ERP_DocumentAttributes modifiedDocumentAttributes);

        [OperationContract]
        ApiAck DeleteDocumentAttributes(ERP_DocumentAttributes deletingDocumentAttributes);

        #endregion ERP_DocumentAttributes

        #region ERP_DocumentAttributesBankInfo

        [OperationContract]
        ApiAck CreateDocumentAttributesBankInfo(ERP_DocumentAttributesBankInfo new_ERP_DocumentAttributesBankInfo);

        [OperationContract]
        ResultDTO<ERP_DocumentAttributesBankInfo> QueryDocumentAttributesBankInfo(DocumentAttributesBankInfoQuery query, int pageSize, int pageNumber);

        [OperationContract]
        bool DocumentAttributesBankInfoExists(ERP_DocumentAttributesBankInfo DocumentAttributesBankInfoexists);

        [OperationContract]
        ERP_DocumentAttributesBankInfo ReadDocumentAttributesBankInfobyKey(string DocCode, string TxCode);

        [OperationContract]
        ERP_DocumentAttributesBankInfo[] ReadAllDocumentAttributesBankInfo();

        [OperationContract]
        IQueryable<ERP_DocumentAttributesBankInfo> ReadDocumentAttributesBankInfo();

        [OperationContract]
        ApiAck UpdateDocumentAttributesBankInfo(ERP_DocumentAttributesBankInfo modifiedDocumentAttributesBankInfo);

        [OperationContract]
        ApiAck DeleteDocumentAttributesBankInfo(ERP_DocumentAttributesBankInfo deletingDocumentAttributesBankInfo);

        #endregion ERP_DocumentAttributesBankInfo

        #region ERP_DocumentAttributesRef

        [OperationContract]
        ApiAck CreateDocumentAttributesRef(ERP_DocumentAttributesRef new_ERP_DocumentAttributesRef);

        [OperationContract]
        ResultDTO<ERP_DocumentAttributesRef> QueryDocumentAttributesRef(DocumentAttributesRefQuery query, int pageSize, int pageNumber);

        [OperationContract]
        bool DocumentAttributesRefExists(ERP_DocumentAttributesRef DocumentAttributesRefexists);

        [OperationContract]
        ERP_DocumentAttributesRef ReadDocumentAttributesRefbyKey(string DocCode, string TxCode, int RefNo);

        [OperationContract]
        ERP_DocumentAttributesRef[] ReadAllDocumentAttributesRef();

        [OperationContract]
        IQueryable<ERP_DocumentAttributesRef> ReadDocumentAttributesRef();

        [OperationContract]
        ApiAck UpdateDocumentAttributesRef(ERP_DocumentAttributesRef modifiedDocumentAttributesRef);

        [OperationContract]
        ApiAck DeleteDocumentAttributesRef(ERP_DocumentAttributesRef deletingDocumentAttributesRef);

        #endregion ERP_DocumentAttributesRef

        #region FIN_CostCenter

        [OperationContract]
        ApiAck CreateCostCenter(FIN_CostCenter new_FIN_CostCenter);

        [OperationContract]
        ResultDTO<FIN_CostCenter> QueryCostCenter(CostCenterQuery query, int pageSize, int pageNumber);

        [OperationContract]
        bool CostCenterExists(FIN_CostCenter CostCenterexists);

        [OperationContract]
        FIN_CostCenter ReadCostCenterbyKey(string CostCenterCode);

        [OperationContract]
        FIN_CostCenter[] ReadAllCostCenter();

        [OperationContract]
        IQueryable<FIN_CostCenter> ReadCostCenter();

        [OperationContract]
        ApiAck UpdateCostCenter(FIN_CostCenter modifiedCostCenter);

        [OperationContract]
        ApiAck DeleteCostCenter(FIN_CostCenter deletingCostCenter);

        #endregion FIN_CostCenter

        #region FIN_CostCenterwiseConfiguration

        [OperationContract]
        ApiAck CreateCostCenterwiseConfiguration(FIN_CostCenterwiseConfiguration new_FIN_CostCenterwiseConfiguration);

        [OperationContract]
        ResultDTO<FIN_CostCenterwiseConfiguration> QueryCostCenterwiseConfiguration(CostCenterwiseConfigurationQuery query, int pageSize, int pageNumber);

        [OperationContract]
        bool CostCenterwiseConfigurationExists(FIN_CostCenterwiseConfiguration CostCenterwiseConfigurationexists);

        [OperationContract]
        FIN_CostCenterwiseConfiguration ReadCostCenterwiseConfigurationbyKey(int RevNo);

        [OperationContract]
        FIN_CostCenterwiseConfiguration[] ReadAllCostCenterwiseConfiguration();

        [OperationContract]
        IQueryable<FIN_CostCenterwiseConfiguration> ReadCostCenterwiseConfiguration();

        [OperationContract]
        ApiAck UpdateCostCenterwiseConfiguration(FIN_CostCenterwiseConfiguration modifiedCostCenterwiseConfiguration);

        [OperationContract]
        ApiAck DeleteCostCenterwiseConfiguration(FIN_CostCenterwiseConfiguration deletingCostCenterwiseConfiguration);

        #endregion FIN_CostCenterwiseConfiguration

        # region GeneralLedgerAccount

        [OperationContract]
        ApiAck CreateGeneralLedgerAccount(FIN_GeneralLedgerAccount new_FIN_GeneralLedgerAccount);

        [OperationContract]
        FIN_GeneralLedgerAccount ReadGeneralLedgerAccountbyKey(string AccountNo);

        [OperationContract]
        FIN_GeneralLedgerAccount[] ReadAllGeneralLedgerAccount();

        [OperationContract]
        IQueryable<FIN_GeneralLedgerAccount> ReadGeneralLedgerAccount();

        [OperationContract]
        ApiAck UpdateGeneralLedgerAccount(FIN_GeneralLedgerAccount modifiedGeneralLedgerAccount);

        [OperationContract]
        ApiAck DeleteGeneralLedgerAccount(FIN_GeneralLedgerAccount deletingGeneralLedgerAccount);

        [OperationContract]
        bool GeneralLedgerAccountExists(FIN_GeneralLedgerAccount existsGeneralLedgerAccount);

        [OperationContract]
        ResultDTO<FIN_GeneralLedgerAccount> QueryGeneralLedgerAccount(GeneralLedgerAccountQuery query, int pageSize, int pageNumber);

        #endregion

        #region Currency

        [OperationContract]
        ApiAck CreateCurrency(FIN_Currency new_Currency);

        [OperationContract]
        FIN_Currency ReadCurrencyByKey(string CurrencyCode);

        [OperationContract]
        FIN_Currency[] ReadAllCurrency();

        [OperationContract]
        ApiAck UpdateCurrency(FIN_Currency modified_Currency);

        [OperationContract]
        ApiAck DeleteCurrency(FIN_Currency delete_Currency);

        [OperationContract]
        bool ExistCurrency(FIN_Currency exist_Currency);

        [OperationContract]
        ResultDTO<FIN_Currency> QueryCurrency(CurrencyQuery query, int pageSize, int pageNumber);

        #endregion

        #region Employee

        [OperationContract]
        ApiAck CreateEmployee(HR_EMPLOYEE new_HR_EMPLOYEE);

        [OperationContract]
        HR_EMPLOYEE ReadEmployeebyKey(string EMP_NUMBER);

        [OperationContract]
        HR_EMPLOYEE[] ReadAllEmployee();

        [OperationContract]
        IQueryable<HR_EMPLOYEE> ReadEmployee();

        [OperationContract]
        ApiAck UpdateEmployee(HR_EMPLOYEE modifiedEmployee);

        [OperationContract]
        ApiAck DeleteEmployee(HR_EMPLOYEE deletingEmployee);

        [OperationContract]
        bool EmployeeExists(HR_EMPLOYEE existsEmployee);

        [OperationContract]
        ResultDTO<HR_EMPLOYEE> QueryEmployee(EmployeeQuery query, int pageSize, int pageNumber);

        #endregion

        #region Employee_Personal_Details

        [OperationContract]
        ApiAck CreateEmployeePD(HR_EMPLOYEE_PD new_HR_EMPLOYEE_PD);

        [OperationContract]
        HR_EMPLOYEE_PD ReadEmployeePDbyKey(string EMP_NUMBER_PD);

        [OperationContract]
        HR_EMPLOYEE_PD[] ReadAllEmployeePD();

        [OperationContract]
        IQueryable<HR_EMPLOYEE_PD> ReadEmployeePD();

        [OperationContract]
        ApiAck UpdateEmployeePD(HR_EMPLOYEE_PD modifiedEmployeePD);

        [OperationContract]
        ApiAck DeleteEmployeePD(HR_EMPLOYEE_PD deletingEmployeePD);

        [OperationContract]
        bool EmployeePDExists(HR_EMPLOYEE_PD existsEmployeePD);

        [OperationContract]
        ResultDTO<HR_EMPLOYEE_PD> QueryEmployeePD(EmployeePDQuery query, int pageSize, int pageNumber);

        #endregion

        #region Employee_Job_Info

        [OperationContract]
        ApiAck CreateEmployeeJI(HR_EMPLOYEE_JI new_HR_EMPLOYEE_JI);

        [OperationContract]
        HR_EMPLOYEE_JI ReadEmployeeJIbyKey(string EMP_NUMBER_JI);

        [OperationContract]
        HR_EMPLOYEE_JI[] ReadAllEmployeeJI();

        [OperationContract]
        IQueryable<HR_EMPLOYEE_JI> ReadEmployeeJI();

        [OperationContract]
        ApiAck UpdateEmployeeJI(HR_EMPLOYEE_JI modifiedEmployeeJI);

        [OperationContract]
        ApiAck DeleteEmployeeJI(HR_EMPLOYEE_JI deletingEmployeeJI);

        [OperationContract]
        bool EmployeeJIExists(HR_EMPLOYEE_JI existsEmployeeJI);

        [OperationContract]
        ResultDTO<HR_EMPLOYEE_JI> QueryEmployeeJI(EmployeeJIQuery query, int pageSize, int pageNumber);

        #endregion

        #region Employee_Job_Status

        [OperationContract]
        ApiAck CreateEmployeeJS(HR_EMPLOYEE_JS new_HR_EMPLOYEE_JS);

        [OperationContract]
        HR_EMPLOYEE_JS ReadEmployeeJSbyKey(string EMP_NUMBER_JS);

        [OperationContract]
        HR_EMPLOYEE_JS[] ReadAllEmployeeJS();

        [OperationContract]
        IQueryable<HR_EMPLOYEE_JS> ReadEmployeeJS();

        [OperationContract]
        ApiAck UpdateEmployeeJS(HR_EMPLOYEE_JS modifiedEmployeeJS);

        [OperationContract]
        ApiAck DeleteEmployeeJS(HR_EMPLOYEE_JS deletingEmployeeJS);

        [OperationContract]
        bool EmployeeJSExists(HR_EMPLOYEE_JS existsEmployeeJS);

        [OperationContract]
        ResultDTO<HR_EMPLOYEE_JS> QueryEmployeeJS(EmployeeJSQuery query, int pageSize, int pageNumber);

        #endregion

        #region Employee_Tax_Details

        [OperationContract]
        ApiAck CreateEmployeeTD(HR_EMPLOYEE_TD new_HR_EMPLOYEE_TD);

        [OperationContract]
        HR_EMPLOYEE_TD ReadEmployeeTDbyKey(string EMP_NUMBER_TD);

        [OperationContract]
        HR_EMPLOYEE_TD[] ReadAllEmployeeTD();

        [OperationContract]
        IQueryable<HR_EMPLOYEE_TD> ReadEmployeeTD();

        [OperationContract]
        ApiAck UpdateEmployeeTD(HR_EMPLOYEE_TD modifiedEmployeeTD);

        [OperationContract]
        ApiAck DeleteEmployeeTD(HR_EMPLOYEE_TD deletingEmployeeTD);

        [OperationContract]
        bool EmployeeTDExists(HR_EMPLOYEE_TD existsEmployeeTD);

        [OperationContract]
        ResultDTO<HR_EMPLOYEE_TD> QueryEmployeeTD(EmployeeTDQuery query, int pageSize, int pageNumber);

        #endregion

        #region Employee_Permanent_Contacts

        [OperationContract]
        ApiAck CreateEmployeePC(HR_EMPLOYEE_PC new_HR_EMPLOYEE_PC);

        [OperationContract]
        HR_EMPLOYEE_PC ReadEmployeePCbyKey(string EMP_NUMBER_PC);

        [OperationContract]
        HR_EMPLOYEE_PC[] ReadAllEmployeePC();

        [OperationContract]
        IQueryable<HR_EMPLOYEE_PC> ReadEmployeePC();

        [OperationContract]
        ApiAck UpdateEmployeePC(HR_EMPLOYEE_PC modifiedEmployeePC);

        [OperationContract]
        ApiAck DeleteEmployeePC(HR_EMPLOYEE_PC deletingEmployeePC);

        [OperationContract]
        bool EmployeePCExists(HR_EMPLOYEE_PC existsEmployeePC);

        [OperationContract]
        ResultDTO<HR_EMPLOYEE_PC> QueryEmployeePC(EmployeePCQuery query, int pageSize, int pageNumber);

        #endregion

        #region Employee_Contacts_during_working_days

        [OperationContract]
        ApiAck CreateEmployeeCDWD(HR_EMPLOYEE_CDWD new_HR_EMPLOYEE_CDWD);

        [OperationContract]
        HR_EMPLOYEE_CDWD ReadEmployeeCDWDbyKey(string EMP_NUMBER_CDWD);

        [OperationContract]
        HR_EMPLOYEE_CDWD[] ReadAllEmployeeCDWD();

        [OperationContract]
        IQueryable<HR_EMPLOYEE_CDWD> ReadEmployeeCDWD();

        [OperationContract]
        ApiAck UpdateEmployeeCDWD(HR_EMPLOYEE_CDWD modifiedEmployeeCDWD);

        [OperationContract]
        ApiAck DeleteEmployeeCDWD(HR_EMPLOYEE_CDWD deletingEmployeeCDWD);

        [OperationContract]
        bool EmployeeCDWDExists(HR_EMPLOYEE_CDWD existsEmployeeCDWD);

        [OperationContract]
        ResultDTO<HR_EMPLOYEE_CDWD> QueryEmployeeCDWD(EmployeeCDWDQuery query, int pageSize, int pageNumber);

        #endregion

        #region Employee_Official_contacts

        [OperationContract]
        ApiAck CreateEmployeeOC(HR_EMPLOYEE_OC new_HR_EMPLOYEE_OC);

        [OperationContract]
        HR_EMPLOYEE_OC ReadEmployeeOCbyKey(string EMP_NUMBER_OC);

        [OperationContract]
        HR_EMPLOYEE_OC[] ReadAllEmployeeOC();

        [OperationContract]
        IQueryable<HR_EMPLOYEE_OC> ReadEmployeeOC();

        [OperationContract]
        ApiAck UpdateEmployeeOC(HR_EMPLOYEE_OC modifiedEmployeeOC);

        [OperationContract]
        ApiAck DeleteEmployeeOC(HR_EMPLOYEE_OC deletingEmployeeOC);

        [OperationContract]
        bool EmployeeOCExists(HR_EMPLOYEE_OC existsEmployeeOC);

        [OperationContract]
        ResultDTO<HR_EMPLOYEE_OC> QueryEmployeeOC(EmployeeOCQuery query, int pageSize, int pageNumber);

        #endregion

        #region Employee_EIM_Work_Station_Details

        [OperationContract]
        ApiAck CreateEmployeeEIMWS(HR_EMPLOYEE_EIM_WS new_HR_EMPLOYEE_EIM_WS);

        [OperationContract]
        HR_EMPLOYEE_EIM_WS ReadEmployeeEIMWSbyKey(string EMP_NUMBER_PD);

        [OperationContract]
        HR_EMPLOYEE_EIM_WS[] ReadAllEmployeeEIMWS();

        [OperationContract]
        IQueryable<HR_EMPLOYEE_EIM_WS> ReadEmployeeEIMWS();

        [OperationContract]
        ApiAck UpdateEmployeeEIMWS(HR_EMPLOYEE_EIM_WS modifiedEmployeeEIMWS);

        [OperationContract]
        ApiAck DeleteEmployeeEIMWS(HR_EMPLOYEE_EIM_WS deletingEmployeeEIMWS);

        [OperationContract]
        bool EmployeeEIMWSExists(HR_EMPLOYEE_EIM_WS existsEmployeeEIMWS);

        [OperationContract]
        ResultDTO<HR_EMPLOYEE_EIM_WS> QueryEmployeeEIMWS(EmployeeEIMWSQuery query, int pageSize, int pageNumber);

        #endregion

        #region Nationality

        [OperationContract]
        ResultDTO<HR_NATIONALITY> QueryNationality(NationalityQuery query, int pageSize, int pageNumber);

        #endregion

        #region Religion

        [OperationContract]
        ResultDTO<HR_RELIGION> QueryReligion(ReligionQuery query, int pageSize, int pageNumber);

        #endregion

        #region Salary Grade

        [OperationContract]
        ResultDTO<HR_SALARY_GRADE> QuerySalaryGrade(SalaryGradeQuery query, int pageSize, int pageNumber);

        #endregion

        #region Corporate Title

        [OperationContract]
        ResultDTO<HR_CORPORATE_TITLE> QueryCorporateTitle(CorporateTitleQuery query, int pageSize, int pageNumber);

        #endregion

        #region Designation

        [OperationContract]
        ResultDTO<HR_DESIGNATION> QueryDesignation(DesignationQuery query, int pageSize, int pageNumber);

        #endregion

        #region JDCategory

        [OperationContract]
        ResultDTO<HR_JD_CATEGORY> QueryJDCategory(JDCategoryQuery query, int pageSize, int pageNumber);

        #endregion

        #region ERP_FixedTxnAttributes

        [OperationContract]
        ApiAck CreateFixedTxnAttributes(ERP_FixedTxnAttributes new_ERP_FixedTxnAttributes);

        [OperationContract]
        ERP_FixedTxnAttributes ReadFixedTxnAttributesbyKey(string DocCode, string TxCode, string GLCode);

        [OperationContract]
        ERP_FixedTxnAttributes[] ReadAllFixedTxnAttributes();

        [OperationContract]
        IQueryable<ERP_FixedTxnAttributes> ReadFixedTxnAttributes();

        [OperationContract]
        ApiAck UpdateFixedTxnAttributes(ERP_FixedTxnAttributes modifiedFixedTxnAttributes);

        [OperationContract]
        ApiAck DeleteFixedTxnAttributes(ERP_FixedTxnAttributes deletingFixedTxnAttributes);

        [OperationContract]
        bool FixedTxnAttributesExists(ERP_FixedTxnAttributes existsFixedTxnAttributes);

        [OperationContract]
        ResultDTO<ERP_FixedTxnAttributes> QueryFixedTransactionAttributes(FixedTxnAttributesQuery query, int pageSize, int pageNumber);

        #endregion

        #region ERP_LastTransactionInfo

        [OperationContract]
        ResultDTO<ERP_LastTransactionInfo> QueryLastTransactionInfo(LastTransactionInfoQuery query, int pageSize, int pageNumber);

        [OperationContract]
        ApiAck CreateLastTransactionInfo(ERP_LastTransactionInfo new_ERP_LastTransactionInfo);

        [OperationContract]
        ERP_LastTransactionInfo ReadLastTransactionInfobyKey(string ProductCode, string SubSystemCode, string DocCode, string TxCode);

        [OperationContract]
        ResultDTO<ERP_LastTransactionInfo> ReadAllLastTransactionInfo(int pageSize, int pageNumber);

        [OperationContract]
        IQueryable<ERP_LastTransactionInfo> ReadLastTransactionInfo();

        [OperationContract]
        ApiAck UpdateLastTransactionInfo(ERP_LastTransactionInfo modifiedLastTransactionInfo);

        [OperationContract]
        ApiAck DeleteLastTransactionInfo(ERP_LastTransactionInfo deletingLastTransactionInfo);

        [OperationContract]
        bool LastTransactionInfoExists(ERP_LastTransactionInfo existsLastTransactionInfo);

        #endregion

        #region AccountSubType

        [OperationContract]
        ResultDTO<FIN_AccountSubType> QueryAccountSubType(AccountSubTypeQuery query, int pageSize, int pageNumber);

        [OperationContract]
        ApiAck CreateAccountSubType(FIN_AccountSubType new_FIN_AccountSubType);

        [OperationContract]
        FIN_AccountSubType ReadAccountSubTypebyKey(int AccountType, int AccountSubType);

        [OperationContract]
        FIN_AccountSubType[] ReadAllAccountSubType();

        [OperationContract]
        IQueryable<FIN_AccountSubType> ReadAccountSubType();

        [OperationContract]
        ApiAck UpdateAccountSubType(FIN_AccountSubType modifiedAccountSubType);

        [OperationContract]
        ApiAck DeleteAccountSubType(FIN_AccountSubType deletingAccountSubType);

        [OperationContract]
        bool AccountSubTypeExists(FIN_AccountSubType existsAccountSubType);

        #endregion

        #region AccountSubTypeCategory

        [OperationContract]
        ResultDTO<FIN_AccountSubTypeCategory> QueryAccountSubTypeCategory(AccountSubTypeCategoryQuery query, int pageSize, int pageNumber);

        [OperationContract]
        ApiAck CreateAccountSubTypeCategory(FIN_AccountSubTypeCategory new_FIN_AccountSubTypeCategory);

        [OperationContract]
        FIN_AccountSubTypeCategory ReadAccountSubTypeCategorybyKey(int AccountType, int AccountSubType, int AccountSubCatType);

        [OperationContract]
        FIN_AccountSubTypeCategory[] ReadAllAccountSubTypeCategory();

        [OperationContract]
        IQueryable<FIN_AccountSubTypeCategory> ReadAccountSubTypeCategory();

        [OperationContract]
        ApiAck UpdateAccountSubTypeCategory(FIN_AccountSubTypeCategory modifiedAccountSubTypeCategory);

        [OperationContract]
        ApiAck DeleteAccountSubTypeCategory(FIN_AccountSubTypeCategory deletingAccountSubTypeCategory);

        [OperationContract]
        bool AccountSubTypeCategoryExists(FIN_AccountSubTypeCategory existsAccountSubTypeCategory);

        #endregion

        #region AccountType

        [OperationContract]
        ResultDTO<FIN_AccountType> QueryAccountType(AccountTypeQuery query, int pageSize, int pageNumber);

        [OperationContract]
        ApiAck CreateAccountType(FIN_AccountType new_FIN_AccountType);

        [OperationContract]
        FIN_AccountType ReadAccountTypebyKey(int AccountType);

        [OperationContract]
        ResultDTO<FIN_AccountType> ReadAllAccountType(int pageSize, int pageNumber);

        [OperationContract]
        IQueryable<FIN_AccountType> ReadAccountType();

        [OperationContract]
        ApiAck UpdateAccountType(FIN_AccountType modifiedAccountType);

        [OperationContract]
        ApiAck DeleteAccountType(FIN_AccountType deletingAccountType);

        [OperationContract]
        bool AccountTypeExists(FIN_AccountType existsAccountType);

        #endregion

        #region Bank

        [OperationContract]
        ApiAck CreateBank(FIN_Bank new_FIN_Bank);

        [OperationContract]
        FIN_Bank ReadBankbyKey(string BankCode);

        [OperationContract]
        FIN_Bank[] ReadAllBank();

        [OperationContract]
        IQueryable<FIN_Bank> ReadBank();

        [OperationContract]
        ApiAck UpdateBank(FIN_Bank modifiedBank);

        [OperationContract]
        ApiAck DeleteBank(FIN_Bank deletingBank);

        [OperationContract]
        bool BankExists(FIN_Bank existsBank);

        [OperationContract]
        ResultDTO<FIN_Bank> QueryBank(BankQuery query, int pageSize, int pageNumber);

        #endregion

        #region BankAccount

        [OperationContract]
        ApiAck CreateBankAccount(FIN_BankAccount new_FIN_BankAccount);

        [OperationContract]
        FIN_BankAccount ReadBankAccountbyKey(string BankCode, string BankBranchCode, int AccountSEQNo);

        [OperationContract]
        FIN_BankAccount[] ReadAllBankAccount();

        [OperationContract]
        IQueryable<FIN_BankAccount> ReadBankAccount();

        [OperationContract]
        ApiAck UpdateBankAccount(FIN_BankAccount modifiedBankAccount);

        [OperationContract]
        ApiAck DeleteBankAccount(FIN_BankAccount deletingBankAccount);

        [OperationContract]
        bool BankAccountExists(FIN_BankAccount existsBankAccount);

        [OperationContract]
        ResultDTO<FIN_BankAccount> QueryBankAccount(BankAccountQuery query, int pageSize, int pageNumber);

        #endregion

        #region BankBranch

        [OperationContract]
        ApiAck CreateBankBranch(FIN_BankBranch new_FIN_BankBranch);

        [OperationContract]
        FIN_BankBranch ReadBankBranchbyKey(string BankCode, string BankBranchCode);

        [OperationContract]
        FIN_BankBranch[] ReadAllBankBranch();

        [OperationContract]
        IQueryable<FIN_BankBranch> ReadBankBranch();

        [OperationContract]
        ApiAck UpdateBankBranch(FIN_BankBranch modifiedBankBranch);

        [OperationContract]
        ApiAck DeleteBankBranch(FIN_BankBranch deletingBankBranch);

        [OperationContract]
        bool BankBranchExists(FIN_BankBranch existsBankBranch);

        [OperationContract]
        ResultDTO<FIN_BankBranch> QueryBankBranch(BankBranchQuery query, int pageSize, int pageNumber);

        #endregion

        #region ControlledTransaction

        [OperationContract]
        ResultDTO<FIN_ControledTransaction> QueryControlledTransaction(ControlledTransactionQuery query, int pageSize, int pageNumber);

        [OperationContract]
        ApiAck CreateControledTransaction(FIN_ControledTransaction new_FIN_ControledTransaction);

        [OperationContract]
        FIN_ControledTransaction ReadControledTransactionbyKey(string DocCode, string TxCode, string GLAccountNo, string CostCenterCode, string CurrencyCode);

        [OperationContract]
        FIN_ControledTransaction[] ReadAllControledTransaction();

        [OperationContract]
        IQueryable<FIN_ControledTransaction> ReadControledTransaction();

        [OperationContract]
        ApiAck UpdateControledTransaction(FIN_ControledTransaction modifiedControledTransaction);

        [OperationContract]
        ApiAck DeleteControledTransaction(FIN_ControledTransaction deletingControledTransaction);

        [OperationContract]
        bool ControledTransactionExists(FIN_ControledTransaction existsControledTransaction);

        #endregion

        #region CustomerSupplierInfo

        [OperationContract]
        ResultDTO<FIN_CustomerSupplier_Info> QueryCustomerSupplier(CustomerSupplierQuery query, int pageSize, int pageNumber);
        [OperationContract]
        ApiAck CreateCustomerSupplierInfo(FIN_CustomerSupplier_Info new_FIN_CustomerSupplier_Info);

        [OperationContract]
        FIN_CustomerSupplier_Info ReadCustomerSupplierInfobyKey(int CustSupFlag, string CusSupCode);

        [OperationContract]
        FIN_CustomerSupplier_Info[] ReadAllCustomerSupplierInfo();

        [OperationContract]
        IQueryable<FIN_CustomerSupplier_Info> ReadCustomerSupplierInfo();

        [OperationContract]
        ApiAck UpdateCustomerSupplierInfo(FIN_CustomerSupplier_Info modifiedCustomerSupplierInfo);

        [OperationContract]
        ApiAck DeleteCustomerSupplierInfo(FIN_CustomerSupplier_Info deletingCustomerSupplierInfo);

        [OperationContract]
        bool CustomerSupplierInfoExists(FIN_CustomerSupplier_Info existsCustomerSupplierInfo);

        #endregion

        #region CustomerSupplierBank

        [OperationContract]
        ResultDTO<FIN_CustomerSupplierBank> QueryCustomerSupplierBank(CustomerSupplierBankQuery query, int pageSize, int pageNumber);

        [OperationContract]
        ApiAck CreateCustomerSupplierBank(FIN_CustomerSupplierBank new_FIN_CustomerSupplierBank);

        [OperationContract]
        FIN_CustomerSupplierBank ReadCustomerSupplierBankbyKey(string CusSupCode, string BankCode, string BranchCode);

        [OperationContract]
        FIN_CustomerSupplierBank[] ReadAllCustomerSupplierBank();

        [OperationContract]
        IQueryable<FIN_CustomerSupplierBank> ReadCustomerSupplierBank();

        [OperationContract]
        ApiAck UpdateCustomerSupplierBank(FIN_CustomerSupplierBank modifiedCustomerSupplierBank);

        [OperationContract]
        ApiAck DeleteCustomerSupplierBank(FIN_CustomerSupplierBank deletingCustomerSupplierBank);

        [OperationContract]
        bool CustomerSupplierBankExists(FIN_CustomerSupplierBank existsCustomerSupplierBank);

        #endregion

        #region CustSupPeriodBalance

        [OperationContract]
        ApiAck CreateCustSupPeriodBalance(FIN_CustSupPeriodBalance new_FIN_CustSupPeriodBalance);

        [OperationContract]
        FIN_CustSupPeriodBalance ReadCustSupPeriodBalancebyKey(int CustSupFlag, string CusSupCode, int FinancialYear, int AccountingPeriod);

        [OperationContract]
        FIN_CustSupPeriodBalance[] ReadAllCustSupPeriodBalance();

        [OperationContract]
        IQueryable<FIN_CustSupPeriodBalance> ReadCustSupPeriodBalance();

        [OperationContract]
        ApiAck UpdateCustSupPeriodBalance(FIN_CustSupPeriodBalance modifiedCustSupPeriodBalance);

        [OperationContract]
        ApiAck DeleteCustSupPeriodBalance(FIN_CustSupPeriodBalance deletingCustSupPeriodBalance);

        [OperationContract]
        bool CustSupPeriodBalanceExists(FIN_CustSupPeriodBalance existsCustSupPeriodBalance);

        #endregion

        #region GeneralLedgerSummary

        [OperationContract]
        ApiAck CreateGeneralLedgerSummary(FIN_GeneralLedgerSummary new_FIN_GeneralLedgerSummary);

        [OperationContract]
        FIN_GeneralLedgerSummary ReadGeneralLedgerSummarybyKey(string AccountNo, string CurrencyCode, int FinancialYear, short AccountingPeriod);

        [OperationContract]
        FIN_GeneralLedgerSummary[] ReadAllGeneralLedgerSummary();

        [OperationContract]
        IQueryable<FIN_GeneralLedgerSummary> ReadGeneralLedgerSummary();

        [OperationContract]
        ApiAck UpdateGeneralLedgerSummary(FIN_GeneralLedgerSummary modifiedGeneralLedgerSummary);

        [OperationContract]
        ApiAck DeleteGeneralLedgerSummary(FIN_GeneralLedgerSummary deletingGeneralLedgerSummary);

        [OperationContract]
        bool GeneralLedgerSummaryExists(FIN_GeneralLedgerSummary existsGeneralLedgerSummary);

        [OperationContract]
        ResultDTO<FIN_PrimaryTransaction> QueryPrimaryTransaction(PrimaryTransactionQuery query);

        #endregion

        #region PrimaryTransaction

        [OperationContract]
        ApiAck CreatePrimaryTransaction(FIN_PrimaryTransaction new_FIN_PrimaryTransaction);

        [OperationContract]
        FIN_PrimaryTransaction ReadPrimaryTransactionbyKey(string DocCode, string TxCode, int TxSerial, int VoucherNumber, int FinancialYear);

        [OperationContract]
        FIN_PrimaryTransaction[] ReadAllPrimaryTransaction();

        [OperationContract]
        IQueryable<FIN_PrimaryTransaction> ReadPrimaryTransaction();

        [OperationContract]
        ApiAck UpdatePrimaryTransaction(FIN_PrimaryTransaction modifiedPrimaryTransaction);

        [OperationContract]
        ApiAck DeletePrimaryTransaction(FIN_PrimaryTransaction deletingPrimaryTransaction);

        [OperationContract]
        bool PrimaryTransactionExists(FIN_PrimaryTransaction existsPrimaryTransaction);

        #endregion

        #region ProfitLostType

        [OperationContract]
        ResultDTO<FIN_ProfitLostType> QueryProfitLostType(ProfitLostQuery query, int pageSize, int pageNumber);

        [OperationContract]
        ApiAck CreateProfitLostType(FIN_ProfitLostType new_FIN_ProfitLostType);

        [OperationContract]
        FIN_ProfitLostType ReadProfitLostTypebyKey(int TypeID);

        [OperationContract]
        FIN_ProfitLostType[] ReadAllProfitLostType();

        [OperationContract]
        IQueryable<FIN_ProfitLostType> ReadProfitLostType();

        [OperationContract]
        ApiAck UpdateProfitLostType(FIN_ProfitLostType modifiedProfitLostType);

        [OperationContract]
        ApiAck DeleteProfitLostType(FIN_ProfitLostType deletingProfitLostType);

        [OperationContract]
        bool ProfitLostTypeExists(FIN_ProfitLostType existsProfitLostType);

        #endregion

        #region SecondaryTransaction

        [OperationContract]
        ApiAck CreateSecondaryTransaction(FIN_SecondaryTransaction new_FIN_SecondaryTransaction);

        [OperationContract]
        FIN_SecondaryTransaction ReadSecondaryTransactionbyKey(string DocCode, string TxCode, int TxSerial, int VoucherNumber, int FinancialYear, int TxnSeqNo);

        [OperationContract]
        FIN_SecondaryTransaction[] ReadAllSecondaryTransaction();

        [OperationContract]
        IQueryable<FIN_SecondaryTransaction> ReadSecondaryTransaction();

        [OperationContract]
        ApiAck UpdateSecondaryTransaction(FIN_SecondaryTransaction modifiedSecondaryTransaction);

        [OperationContract]
        ApiAck DeleteSecondaryTransaction(FIN_SecondaryTransaction deletingSecondaryTransaction);

        [OperationContract]
        bool SecondaryTransactionExists(FIN_SecondaryTransaction existsSecondaryTransaction);

        #endregion

        #region SpecialAccountType

        [OperationContract]
        ResultDTO<FIN_SpecialAccountType> QuerySpecialAccountType(SpecialAccountQuery query, int pageSize, int pageNumber);

        [OperationContract]
        ApiAck CreateSpecialAccountType(FIN_SpecialAccountType new_FIN_SpecialAccountType);

        [OperationContract]
        FIN_SpecialAccountType ReadSpecialAccountTypebyKey(int TypeID);

        [OperationContract]
        FIN_SpecialAccountType[] ReadAllSpecialAccountType();

        [OperationContract]
        IQueryable<FIN_SpecialAccountType> ReadSpecialAccountType();

        [OperationContract]
        ApiAck UpdateSpecialAccountType(FIN_SpecialAccountType modifiedSpecialAccountType);

        [OperationContract]
        ApiAck DeleteSpecialAccountType(FIN_SpecialAccountType deletingSpecialAccountType);

        [OperationContract]
        bool SpecialAccountTypeExists(FIN_SpecialAccountType existsSpecialAccountType);

        #endregion

        #region TxnReference

        [OperationContract]
        ApiAck CreateTxnReference(FIN_TxnReference new_FIN_TxnReference);

        [OperationContract]
        ResultDTO<FIN_TxnReference> QueryTxnReference(TransactionReferenceQuery query, int pageSize, int pageNumber);

        [OperationContract]
        FIN_TxnReference ReadTxnReferencebyKey(string DocCode, string TxCode, short RefSeq);

        [OperationContract]
        FIN_TxnReference[] ReadAllTxnReference();

        [OperationContract]
        IQueryable<FIN_TxnReference> ReadTxnReference();

        [OperationContract]
        ApiAck UpdateTxnReference(FIN_TxnReference modifiedTxnReference);

        [OperationContract]
        ApiAck DeleteTxnReference(FIN_TxnReference deletingTxnReference);

        [OperationContract]
        bool TxnReferenceExists(FIN_TxnReference existsTxnReference);

        #endregion

        #region EMPATTACHMENT

        [OperationContract]
        ApiAck CreateEMPATTACHMENT(HR_EMP_ATTACHMENT new_HR_EMP_ATTACHMENT);

        [OperationContract]
        HR_EMP_ATTACHMENT ReadEMPATTACHMENTbyKey(string EMP_NUMBER, decimal EATTACH_ID);

        [OperationContract]
        HR_EMP_ATTACHMENT[] ReadAllEMPATTACHMENT();

        [OperationContract]
        IQueryable<HR_EMP_ATTACHMENT> ReadEMPATTACHMENT();

        [OperationContract]
        ApiAck UpdateEMPATTACHMENT(HR_EMP_ATTACHMENT modifiedEMPATTACHMENT);

        [OperationContract]
        ApiAck DeleteEMPATTACHMENT(HR_EMP_ATTACHMENT deletingEMPATTACHMENT);

        [OperationContract]
        bool EMPATTACHMENTExists(HR_EMP_ATTACHMENT existsEMPATTACHMENT);

        #endregion

        #region EMPBANK

        [OperationContract]
        ResultDTO<HR_EMP_BANK> QueryEMPBANK(EmpBankQuery query, int pageSize, int pageNumber);
        [OperationContract]
        ApiAck CreateEMPBANK(HR_EMP_BANK new_HR_EMP_BANK);

        [OperationContract]
        HR_EMP_BANK ReadEMPBANKbyKey(string EMP_NUMBER, string BBRANCH_CODE, string BANK_CODE);

        [OperationContract]
        HR_EMP_BANK[] ReadAllEMPBANK();

        [OperationContract]
        IQueryable<HR_EMP_BANK> ReadEMPBANK();

        [OperationContract]
        ApiAck UpdateEMPBANK(HR_EMP_BANK modifiedEMPBANK);

        [OperationContract]
        ApiAck DeleteEMPBANK(HR_EMP_BANK deletingEMPBANK);

        [OperationContract]
        bool EMPBANKExists(HR_EMP_BANK existsEMPBANK);

        #endregion

        #region EMPPASSPORT

        [OperationContract]
        ResultDTO<HR_EMP_PASSPORT> QueryEMPPASSPORT(EmpPassportQuery query, int pageSize, int pageNumber);
        [OperationContract]
        ApiAck CreateEMPPASSPORT(HR_EMP_PASSPORT new_HR_EMP_PASSPORT);

        [OperationContract]
        HR_EMP_PASSPORT ReadEMPPASSPORTbyKey(string EMP_NUMBER, decimal EP_SEQNO);

        [OperationContract]
        HR_EMP_PASSPORT[] ReadAllEMPPASSPORT();

        [OperationContract]
        IQueryable<HR_EMP_PASSPORT> ReadEMPPASSPORT();

        [OperationContract]
        ApiAck UpdateEMPPASSPORT(HR_EMP_PASSPORT modifiedEMPPASSPORT);

        [OperationContract]
        ApiAck DeleteEMPPASSPORT(HR_EMP_PASSPORT deletingEMPPASSPORT);

        [OperationContract]
        bool EMPPASSPORTExists(HR_EMP_PASSPORT existsEMPPASSPORT);

        #endregion

        #region HRDependent

        [OperationContract]
        ApiAck CreateHRDependent(HR_EMP_RELATIONINFO newHRDependent);

        [OperationContract]
        IEnumerable<HR_EMP_RELATIONINFO> QueryHRDependent(HRDependentQuery query);

        [OperationContract]
        bool HRDependentExists(HR_EMP_RELATIONINFO findHRDependent);

        [OperationContract]
        HR_EMP_RELATIONINFO ReadHRDependentbyKey(string EmpID, decimal DependentID);

        [OperationContract]
        HR_EMP_RELATIONINFO[] ReadAllHRDependent();

        [OperationContract]
        ApiAck UpdateHRDependent(HR_EMP_RELATIONINFO modifiedHRDependent);

        [OperationContract]
        ApiAck DeleteHRDependent(HR_EMP_RELATIONINFO deletingHRDependent);

        #endregion

        #region CurrencyExchangeRate

        [OperationContract]
        ApiAck CreateCurrencyExchangeRate(FIN_CurrencyExchangeRate new_CurrencyExchangeRate);

        [OperationContract]
        FIN_CurrencyExchangeRate ReadCurrencyExchangeRateByKey(string CurrencyCode, short CalenderYear, byte CalenderMonth);

        [OperationContract]
        FIN_CurrencyExchangeRate[] ReadAllCurrencyExchangeRate();

        [OperationContract]
        ApiAck UpdateCurrencyExchangeRate(FIN_CurrencyExchangeRate modified_CurrencyExchangeRate);

        [OperationContract]
        ApiAck DeleteCurrencyExchangeRate(FIN_CurrencyExchangeRate delete_CurrencyExchangeRate);

        [OperationContract]
        bool ExistCurrencyExchangeRate(FIN_CurrencyExchangeRate exist_CurrencyExchangeRate);

        [OperationContract]
        ResultDTO<FIN_CurrencyExchangeRate> QueryCurrencyExchangeRate(CurrencyExchangeRateQuery query, int pageSize, int pageNumber);

        #endregion

        #region ProcessControl

        [OperationContract]
        ResultDTO<ERP_ProcessControl> QueryProcessControl(ProcessControlQuery query, int pageSize, int pageNumber);

        [OperationContract]
        ApiAck CreateProcessControl(ERP_ProcessControl new_ProcessControl);

        [OperationContract]
        ERP_ProcessControl ReadProcessControlByKey(string ProductCode);

        [OperationContract]
        ERP_ProcessControl[] ReadAllProcessControl();

        [OperationContract]
        ApiAck UpdateProcessControl(ERP_ProcessControl modified_ProcessControl);

        [OperationContract]
        ApiAck DeleteProcessControl(ERP_ProcessControl delete_ProcessControl);

        [OperationContract]
        bool ExistProcessControl(ERP_ProcessControl exist_ProcessControl);

        #endregion

        #region Period

        [OperationContract]
        ResultDTO<ERP_Period> QueryPeriod(PeriodQuery query, int pageSize, int pageNumber);

        [OperationContract]
        ApiAck CreatePeriod(ERP_Period new_Period);

        [OperationContract]
        ERP_Period ReadPeriodByKey(string ProductCode, int FinancialYear, int AccountingPeriod);

        [OperationContract]
        ERP_Period[] ReadAllPeriod();

        [OperationContract]
        ApiAck UpdatePeriod(ERP_Period modified_Period);

        [OperationContract]
        ApiAck DeletePeriod(ERP_Period delete_Period);

        [OperationContract]
        bool ExistPeriod(ERP_Period exist_Period);

        #endregion

        #region Emergency

        [OperationContract]
        ResultDTO<HR_EMP_EMERGENCY> QueryEmergency(EmergencyQuery query, int pageSize, int pageNumber);

        [OperationContract]
        ApiAck CreateEmergency(HR_EMP_EMERGENCY new_Emergency);

        [OperationContract]
        HR_EMP_EMERGENCY ReadEmergencyByKey(string emp_number);

        [OperationContract]
        HR_EMP_EMERGENCY[] ReadAllEmergency();

        [OperationContract]
        ApiAck UpdateEmergency(HR_EMP_EMERGENCY modified_Emergency);

        [OperationContract]
        ApiAck DeleteEmergency(HR_EMP_EMERGENCY delete_Emergency);

        [OperationContract]
        bool ExistEmergency(HR_EMP_EMERGENCY exist_Emergency);

        #endregion

        #region Transport

        [OperationContract]
        ResultDTO<HR_EMP_TRANSPORT> QueryTransport(TransportQuery query, int pageSize, int pageNumber);

        [OperationContract]
        ApiAck CreateTransport(HR_EMP_TRANSPORT new_Transport);

        [OperationContract]
        HR_EMP_TRANSPORT ReadTransportByKey(string emp_number);

        [OperationContract]
        HR_EMP_TRANSPORT[] ReadAllTransport();

        [OperationContract]
        ApiAck UpdateTransport(HR_EMP_TRANSPORT modified_Transport);

        [OperationContract]
        ApiAck DeleteTransport(HR_EMP_TRANSPORT delete_Transport);

        [OperationContract]
        bool ExistTransport(HR_EMP_TRANSPORT exist_Transport);

        #endregion

        #region Training

        [OperationContract]
        ResultDTO<HR_EMP_TRAININGS> QueryTraining(TrainingQuery query, int pageSize, int pageNumber);

        [OperationContract]
        ApiAck CreateTraining(HR_EMP_TRAININGS new_Training);

        [OperationContract]
        HR_EMP_TRAININGS ReadTrainingByKey(string emp_number, int tn_id);

        [OperationContract]
        HR_EMP_TRAININGS[] ReadAllTraining();

        [OperationContract]
        ApiAck UpdateTraining(HR_EMP_TRAININGS modified_Training);

        [OperationContract]
        ApiAck DeleteTraining(HR_EMP_TRAININGS delete_Training);

        [OperationContract]
        bool ExistTraining(HR_EMP_TRAININGS exist_Training);

        #endregion

        #region Warning

        [OperationContract]
        ResultDTO<HR_EMP_WARNINGS> QueryWarning(WarningQuery query, int pageSize, int pageNumber);

        [OperationContract]
        ApiAck CreateWarning(HR_EMP_WARNINGS new_Warning);

        [OperationContract]
        HR_EMP_WARNINGS ReadWarningByKey(string emp_number, int wrn_id);

        [OperationContract]
        HR_EMP_WARNINGS[] ReadAllWarning();

        [OperationContract]
        ApiAck UpdateWarning(HR_EMP_WARNINGS modified_Warning);

        [OperationContract]
        ApiAck DeleteWarning(HR_EMP_WARNINGS delete_Warning);

        [OperationContract]
        bool ExistWarning(HR_EMP_WARNINGS exist_Warning);

        #endregion

        #region EMPQUALIFICATION

        [OperationContract]
        ApiAck CreateEMPQUALIFICATION(HR_EMP_QUALIFICATION new_HR_EMP_QUALIFICATION);

        [OperationContract]
        HR_EMP_QUALIFICATION ReadEMPQUALIFICATIONbyKey(string EMP_NUMBER, string QUALIFI_CODE);

        [OperationContract]
        HR_EMP_QUALIFICATION[] ReadAllEMPQUALIFICATION();

        [OperationContract]
        IQueryable<HR_EMP_QUALIFICATION> ReadEMPQUALIFICATION();

        [OperationContract]
        ApiAck UpdateEMPQUALIFICATION(HR_EMP_QUALIFICATION modifiedEMPQUALIFICATION);

        [OperationContract]
        ApiAck DeleteEMPQUALIFICATION(HR_EMP_QUALIFICATION deletingEMPQUALIFICATION);

        [OperationContract]
        bool EMPQUALIFICATIONExists(HR_EMP_QUALIFICATION existsEMPQUALIFICATION);

        [OperationContract]
        ResultDTO<HR_EMP_QUALIFICATION> QueryEMPQUALIFICATION(HRQualificationQuery query, int pageSize, int pageNumber);

        #endregion

        #region EMPWORKEXPERIENCE

        [OperationContract]
        ApiAck CreateEMPWORK(HR_EMP_WORK_EXPERIENCE new_HR_EMP_WORK_EXPERIENCE);

        [OperationContract]
        HR_EMP_WORK_EXPERIENCE ReadEMPWORKbyKey(string EMP_NUMBER);

        [OperationContract]
        HR_EMP_WORK_EXPERIENCE[] ReadAllEMPWORK();

        [OperationContract]
        IQueryable<HR_EMP_WORK_EXPERIENCE> ReadEMPWORK();

        [OperationContract]
        ApiAck UpdateEMPWORK(HR_EMP_WORK_EXPERIENCE modifiedEMPWORK);

        [OperationContract]
        ApiAck DeleteEMPWORK(HR_EMP_WORK_EXPERIENCE deletingEMPWORK);

        [OperationContract]
        bool EMPWORKExists(HR_EMP_WORK_EXPERIENCE existsEMPWORK);

        [OperationContract]
        ResultDTO<HR_EMP_WORK_EXPERIENCE> QueryEMPWORKEXPERIENCE(HRWorkExperienceQuery query, int pageSize, int pageNumber);

        #endregion

        #region HR Cash benefit

        [OperationContract]
        ResultDTO<HR_CASH_BENEFIT> QueryHRCashBenefits(HRCashBenefitQuery query, int pageSize, int pageNumber);

        [OperationContract]
        ApiAck CreateHRCashBenefit(HR_CASH_BENEFIT new_HR_Cash_Benifit);

        [OperationContract]
        HR_CASH_BENEFIT ReadHRCashBenefitbyKey(string HRCashBenefit);

        [OperationContract]
        ResultDTO<HR_CASH_BENEFIT> ReadAllHRCashBenefit(int pageSize, int pageNumber);

        [OperationContract]
        IQueryable<HR_CASH_BENEFIT> ReadHRCashBenefit();

        [OperationContract]
        ApiAck UpdateHRCashBenefit(HR_CASH_BENEFIT modifiedHRCashBenefit);

        [OperationContract]
        ApiAck DeleteHRCashBenefit(HR_CASH_BENEFIT deletingHRCashBenefit);

        [OperationContract]
        bool HRCashBenefitExists(HR_CASH_BENEFIT existsHRCashBenefit);

        #endregion

        #region HR NonCash benefit

        [OperationContract]
        ResultDTO<HR_NONCASH_BENEFIT> QueryHRNonCashBenefits(HRNonCashBenefitQuery query, int pageSize, int pageNumber);

        [OperationContract]
        ApiAck CreateHRNonCashBenefit(HR_NONCASH_BENEFIT new_HR_Non_Cash_Benifit);

        [OperationContract]
        HR_NONCASH_BENEFIT ReadHRNonCashBenefitbyKey(string HRNonCashBenefit);

        [OperationContract]
        ResultDTO<HR_NONCASH_BENEFIT> ReadAllHRNonCashBenefit(int pageSize, int pageNumber);

        [OperationContract]
        IQueryable<HR_NONCASH_BENEFIT> ReadHRNonCashBenefit();

        [OperationContract]
        ApiAck UpdateHRNonCashBenefit(HR_NONCASH_BENEFIT modifiedHRNonCashBenefit);

        [OperationContract]
        ApiAck DeleteHRNonCashBenefit(HR_NONCASH_BENEFIT deletingHRNonCashBenefit);

        [OperationContract]
        bool HRNonCashBenefitExists(HR_NONCASH_BENEFIT existsHRNonCashBenefit);

        #endregion

        #region HR Employee Cash benefit

        [OperationContract]
        ResultDTO<HR_EMP_CASH_BENEFIT> QueryHREmpCashBenefits(HREmpCashBenefitsQuery query, int pageSize, int pageNumber);

        [OperationContract]
        ApiAck CreateHREmpCashBenefits(HR_EMP_CASH_BENEFIT new_FIN_HREmpCashBenefits);

        [OperationContract]
        HR_EMP_CASH_BENEFIT ReadHREmpCashBenefitsbyKey(string EmpCode, string BenCode);

        [OperationContract]
        ResultDTO<HR_EMP_CASH_BENEFIT> ReadAllHREmpCashBenefit(int pageSize, int pageNumber);

        [OperationContract]
        IQueryable<HR_EMP_CASH_BENEFIT> ReadHREmpCashBenefits();

        [OperationContract]
        ApiAck UpdateHREmpCashBenefits(HR_EMP_CASH_BENEFIT modifiedHREmpCashBenefits);

        [OperationContract]
        ApiAck DeleteHREmpCashBenefits(HR_EMP_CASH_BENEFIT deletingHREmpCashBenefits);

        [OperationContract]
        bool HREmpCashBenefitsExists(HR_EMP_CASH_BENEFIT existsHREmpCashBenefits);

        #endregion

        #region HR Employee NonCash benefit

        [OperationContract]
        ResultDTO<HR_EMP_NONCASH_BENEFIT> QueryHREmpNonCashBenefits(HREmpNonCashBenefitsQuery query, int pageSize, int pageNumber);

        [OperationContract]
        ApiAck CreateHREmpNonCashBenefits(HR_EMP_NONCASH_BENEFIT new_FIN_HREmpNonCashBenefits);

        [OperationContract]
        HR_EMP_NONCASH_BENEFIT ReadHREmpNonCashBenefitsbyKey(string EmpCode, string NBenCode);

        [OperationContract]
        ResultDTO<HR_EMP_NONCASH_BENEFIT> ReadAllHREmpNonCashBenefit(int pageSize, int pageNumber);

        [OperationContract]
        IQueryable<HR_EMP_NONCASH_BENEFIT> ReadHREmpNonCashBenefits();

        [OperationContract]
        ApiAck UpdateHREmpNonCashBenefits(HR_EMP_NONCASH_BENEFIT modifiedHREmpNonCashBenefits);

        [OperationContract]
        ApiAck DeleteHREmpNonCashBenefits(HR_EMP_NONCASH_BENEFIT deletingHREmpNonCashBenefits);

        [OperationContract]
        bool HREmpNonCashBenefitsExists(HR_EMP_NONCASH_BENEFIT existsHREmpNonCashBenefits);

        #endregion

        #region Basic Salary

        [OperationContract]
        ResultDTO<HR_EMP_BASICSALARY> QueryBasicSalary(BasicSalaryQuery query, int pageSize, int pageNumber);

        [OperationContract]
        ApiAck CreateBasicSalary(HR_EMP_BASICSALARY new_BasicSalary);

        [OperationContract]
        HR_EMP_BASICSALARY ReadBasicSalaryByKey(string emp_number, int sal_dtl_year,string sal_grd_code,string currency_id);

        [OperationContract]
        HR_EMP_BASICSALARY[] ReadAllBasicSalary();

        [OperationContract]
        ApiAck UpdateBasicSalary(HR_EMP_BASICSALARY modified_BasicSalary);

        [OperationContract]
        ApiAck DeleteBasicSalary(HR_EMP_BASICSALARY delete_BasicSalary);

        [OperationContract]
        bool ExistBasicSalary(HR_EMP_BASICSALARY exist_BasicSalary);

        #endregion

        #region Reporting

        [OperationContract]
        ResultDTO<HR_EMP_REPORTTO> QueryReporting(ReportingQuery query, int pageSize, int pageNumber);

        [OperationContract]
        ApiAck CreateReporting(HR_EMP_REPORTTO new_Reporting);

        [OperationContract]
        HR_EMP_REPORTTO ReadReportingByKey(string emp_number,string emp_number2,Int16 reporting_mode);

        [OperationContract]
        HR_EMP_REPORTTO[] ReadAllReporting();

        [OperationContract]
        ApiAck UpdateReporting(HR_EMP_REPORTTO modified_Reporting);

        [OperationContract]
        ApiAck DeleteReporting(HR_EMP_REPORTTO delete_Reporting);

        [OperationContract]
        bool ExistReporting(HR_EMP_REPORTTO exist_Reporting);

        #endregion

        #region Job Specification

        [OperationContract]
        ResultDTO<HR_EMP_JOBSPEC> QueryJobSpecification(JobSpecificationQuery query, int pageSize, int pageNumber);

        [OperationContract]
        ApiAck CreateJobSpecification(HR_EMP_JOBSPEC new_JobSpecification);

        [OperationContract]
        HR_EMP_JOBSPEC ReadJobSpecificationByKey(string emp_number, string jdcat_code);

        [OperationContract]
        HR_EMP_JOBSPEC[] ReadAllJobSpecification();

        [OperationContract]
        ApiAck UpdateJobSpecification(HR_EMP_JOBSPEC modified_JobSpecification);

        [OperationContract]
        ApiAck DeleteJobSpecification(HR_EMP_JOBSPEC delete_JobSpecification);

        [OperationContract]
        bool ExistJobSpecification(HR_EMP_JOBSPEC exist_JobSpecification);

        #endregion

        #region Currnecy2

        [OperationContract]
        ResultDTO<HR_CURRENCY> QueryCurrency2(CurrencyHRQuery query, int pageSize, int pageNumber);

        #endregion

        #region HR Currency

        [OperationContract]
        ResultDTO<HR_COUNTRY> QueryHRCountry(HRCountryQuery query, int pageSize, int pageNumber);

        #endregion

        #region HR Bank

        [OperationContract]
        ResultDTO<HR_BANK> QueryHRBank(HRBankQuery query, int pageSize, int pageNumber);

        #endregion

        #region HR Bank Branch

        [OperationContract]
        ResultDTO<HR_BRANCH> QueryHRBankBranch(HRBankBranchQuery query, int pageSize, int pageNumber);

        #endregion

        #region HR Language

        [OperationContract]
        ResultDTO<HR_LANGUAGE> QueryHRLanguage(HRLanguageQuery query, int pageSize, int pageNumber);

        [OperationContract]
        ApiAck CreateHRLanguage(HR_LANGUAGE new_HR_Language);

        [OperationContract]
        HR_LANGUAGE ReadHRLanguagebyKey(string HRLanguage);

        [OperationContract]
        ResultDTO<HR_LANGUAGE> ReadAllHRLanguage(int pageSize, int pageNumber);

        [OperationContract]
        IQueryable<HR_LANGUAGE> ReadHRLanguage();

        [OperationContract]
        ApiAck UpdateHRLanguage(HR_LANGUAGE modifiedHRLanguage);

        [OperationContract]
        ApiAck DeleteHRLanguage(HR_LANGUAGE deletingHRLanguage);

        [OperationContract]
        bool HRLanguageExists(HR_LANGUAGE existsHRLanguage);

        #endregion

        #region HR Employee Language

        [OperationContract]
        ResultDTO<HR_EMP_LANGUAGE> QueryHREmpLanguage(HREmpLanguageQuery query, int pageSize, int pageNumber);

        [OperationContract]
        ApiAck CreateHREmpLanguage(HR_EMP_LANGUAGE new_HR_Emp_Language);

        [OperationContract]
        HR_EMP_LANGUAGE ReadHREmpLanguagebyKey(string EmployeeCode, string LanguageCode);

        [OperationContract]
        ResultDTO<HR_EMP_LANGUAGE> ReadAllHREmpLanguage(int pageSize, int pageNumber);

        [OperationContract]
        IQueryable<HR_EMP_LANGUAGE> ReadHREmpLanguage();

        [OperationContract]
        ApiAck UpdateHREmpLanguage(HR_EMP_LANGUAGE modifiedHREmpLanguage);

        [OperationContract]
        ApiAck DeleteHREmpLanguage(HR_EMP_LANGUAGE deletingHREmpLanguage);

        [OperationContract]
        bool HREmpLanguageExists(HR_EMP_LANGUAGE existsHREmpLanguage);

        #endregion

        [OperationContract]
        List<string> GetReportNames();
    }
}