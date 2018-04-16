CREATE TABLE ERP_Document
(
	DocCode varchar(3),
	ProductCode varchar(5) null,
	SubSystemCode varchar(5) null,
	DocType int not null,
	DocName varchar(32) null,
	DocumentMode int not null,
	ChequeSeqFlag int default 4,
	TransferFlag int default 2,
	TransactionMode int not null,
	CashBankFlag int default 3,
	EnteredFrom	varchar(16) null,
	EnteredUser varchar(12) null,
	EnteredDate date null,
	ModifiedFrom varchar(16) null,
	ModifiedUser varchar(12) null,
	ModifiedDate date null,
	
	constraint ERP_Document_PK primary key(DocCode),
	constraint ERP_Document_DocTypeCheck check(DocType in (1,2,3,4)),
	constraint ERP_Document_DocumentModeCheck check(DocumentMode in (1,2,3)),
	constraint ERP_Document_ChequeSeqFlagCheck check(ChequeSeqFlag in (1,2,3,4)),
	constraint ERP_Document_TransferFlagCheck check(TransferFlag in (1,2)),
	constraint ERP_Document_TransactionModeCheck check(TransactionMode in (1,2,3,4)),
	constraint ERP_Document_CashBankFlagCheck check(CashBankFlag in (1,2,3))
);
go
CREATE TABLE ERP_DocumentAttributes
(
	DocCode varchar(3),
	TxCode varchar(3),
	ShortName varchar(32) null,
	LongName varchar(64) null,
	DB_ToGL_Flag int null,
	CR_ToGL_Flag int null,
	FixedToCustSup_Flag int null,
	FixedCustSupCode varchar(8) null,
	ChequePrint_Flag int default(1),
	AmountTally_Flag int default(1),
	FixedBankAcc_Flag int default(1),
	AuthorizeRequired_Flag int default(1),
	EnteredFrom	varchar(16) null,
	EnteredUser varchar(12) null,
	EnteredDate date null,
	ModifiedFrom varchar(16) null,
	ModifiedUser varchar(12) null,
	ModifiedDate date null,
	
	constraint ERP_DocumentAttributes_PK primary key (DocCode,TxCode),
	constraint ERP_DocumentAttributes_FK foreign key (DocCode) references ERP_Document (DocCode),
	constraint ERP_DocumentAttributes_DB_ToGL_FlagCheck check(DB_ToGL_Flag in (1,2,3,4)),
	constraint ERP_DocumentAttributes_CR_ToGL_FlagCheck check(CR_ToGL_Flag in (1,2,3,4)),
	constraint ERP_DocumentAttributes_FixedToCustSup_FlagCheck check(FixedToCustSup_Flag in (1,2)),
	constraint ERP_DocumentAttributes_ChequePrint_FlagCheck check(ChequePrint_Flag in (1,2)),
	constraint ERP_DocumentAttributes_AmountTally_FlagCheck check(AmountTally_Flag in (1,2)),
	constraint ERP_DocumentAttributes_FixedBankAcc_FlagCheck check(FixedBankAcc_Flag in (1,2)),
	constraint ERP_DocumentAttributes_AuthorizeRequired_FlagCheck check(AuthorizeRequired_Flag in (1,2))
);
go
CREATE TABLE ERP_DocumentAttributesRef
(
	DocCode varchar(3),
	TxCode varchar(3),
	RefNo smallint,
	RefValidationCriteria int null,
	RefValidationMethod int null,
	EnteredFrom	varchar(16) null,
	EnteredUser varchar(12) null,
	EnteredDate date null,
	ModifiedFrom varchar(16) null,
	ModifiedUser varchar(12) null,
	ModifiedDate date null,
	constraint ERP_DocumentAttributesRef_PK primary key (DocCode,TxCode,RefNo),
	constraint ERP_DocumentAttributesRef_FK1 foreign key (DocCode,TxCode) references ERP_DocumentAttributes (DocCode,TxCode),
	constraint ERP_DocumentAttributesRef_RefValidationCriteria_Check check(RefValidationCriteria in (1,2,3,4)),
	constraint ERP_DocumentAttributesRef_RefValidationMethod_Check check(RefValidationMethod in (1,2,3,4,5))
);
go
CREATE TABLE FIN_AccountType
(
	AccountType int,
	AccountDescription varchar(32),
	EnteredFrom	varchar(16) null,
	EnteredUser varchar(12) null,
	EnteredDate date null,
	ModifiedFrom varchar(16) null,
	ModifiedUser varchar(12) null,
	ModifiedDate date null,
	
	constraint FIN_AccountType_PK primary key(AccountType),
);
go
CREATE TABLE FIN_AccountSubType
(
	AccountType int,
	AccountSubType int,
	SubTypeDescription varchar(32),
	EnteredFrom	varchar(16) null,
	EnteredUser varchar(12) null,
	EnteredDate date null,
	ModifiedFrom varchar(16) null,
	ModifiedUser varchar(12) null,
	ModifiedDate date null,
	
	constraint FIN_AccountSubType_PK primary key (AccountType,AccountSubType),
	constraint FIN_AccountSubType_FK foreign key(AccountType) references FIN_AccountType(AccountType)
);
go
CREATE TABLE FIN_AccountSubTypeCategory
(
	AccountType int,
	AccountSubType int,
	AccountSubCatType int,
	SubTypeDescription varchar(32),
	EnteredUser varchar(12) null,
	EnteredDate date null,
	ModifiedFrom varchar(16) null,
	ModifiedUser varchar(12) null,
	ModifiedDate date null,
	
	constraint FIN_AccountSubTypeCategory_PK primary key(AccountType,AccountSubType,AccountSubCatType),
	constraint FIN_AccountSubTypeCategory_FK foreign key (AccountType,AccountSubType) references FIN_AccountSubType(AccountType,AccountSubType)
);
go
CREATE TABLE FIN_ProfitLostType
(
	TypeID int,
	ProfitLostAccountName varchar(16),
	EnteredUser varchar(12) null,
	EnteredFrom varchar(16) null,
	EnteredDate date null,
	ModifiedFrom varchar(16) null,
	ModifiedUser varchar(12) null,
	ModifiedDate date null,
	
	constraint FIN_ProfitLostType_PK primary key(TypeID),
	
);	
go
CREATE TABLE FIN_SpecialAccountType
(
	TypeID int,
	SpecialAccountName varchar(16),
	EnteredUser varchar(12) null,
	EnteredFrom varchar(16) null,
	EnteredDate date null,
	ModifiedFrom varchar(16) null,
	ModifiedUser varchar(12) null,
	ModifiedDate date null,
	
	constraint FIN_SpecialAccountType_PK primary key(TypeID),
);
go
CREATE TABLE FIN_GeneralLedgerAccount
(
	AccountNo varchar(4),
	AccountType int,
	AccountSubtype int,
	AccountSubtypeCat int,
	ShortName varchar(32),
	AccountName varchar(64),
	PostableFlag int,
	ProfitLossType int,
	SpecialAccountType int,
	IsBankRelatedFlag int,
	RevalueStatusFlag int,
	EnteredUser varchar(12) null,
	EnteredFrom varchar(16) null,
	EnteredDate date null,
	ModifiedFrom varchar(16) null,
	ModifiedUser varchar(12) null,
	ModifiedDate date null,
	
	constraint FIN_GeneralLedgerAccount_PK primary key (AccountNo),
	constraint FIN_GeneralLedgerAccount_FK1 foreign key (AccountType,AccountSubtype,AccountSubtypeCat) references FIN_AccountSubTypeCategory (AccountType,AccountSubType,AccountSubCatType),
	constraint FIN_GeneralLedgerAccount_FK2 foreign key (ProfitLossType) references FIN_ProfitLostType (TypeID),
	constraint FIN_GeneralLedgerAccount_FK3 foreign key (SpecialAccountType) references FIN_SpecialAccountType (TypeID),
);
go
CREATE TABLE FIN_Currency
(
	CurrencyCode varchar(3) not null,
	CurrencyName varchar(16) null,
	EnteredFrom	varchar(16) null,
	EnteredUser varchar(12) null,
	EnteredDate date null,
	ModifiedFrom varchar(16) null,
	ModifiedUser varchar(12) null,
	ModifiedDate date null,
	constraint FIN_Currency_PK primary key(CurrencyCode)
);
go
CREATE TABLE FIN_CurrencyExchangeRate
(
	CurrencyCode varchar(3) not null,
	CalenderYear smallint not null,
	CalenderMonth tinyint not null,
	StandardRate decimal null,
	BuyRate	decimal null,
	MiddleRate decimal null,
	EnteredFrom	varchar(16) null,
	EnteredUser varchar(12) null,
	EnteredDate date null,
	ModifiedFrom varchar(16) null,
	ModifiedUser varchar(12) null,
	ModifiedDate date null,
	
	constraint FIN_CurrencyExchangeRate_PK primary key(CurrencyCode,CalenderYear,CalenderMonth),
	constraint FIN_CurrencyExchangeRate_FK foreign key(CurrencyCode) references FIN_Currency(CurrencyCode)
);
go
CREATE TABLE FIN_Region
(
	RegionCode varchar(32) not null,
	RegionName varchar(32) not null,
	EnteredFrom	varchar(16) null,
	EnteredUser varchar(12) null,
	EnteredDate date null,
	ModifiedFrom varchar(16) null,
	ModifiedUser varchar(12) null,
	ModifiedDate date null,
	
	constraint FIN_Region_PK primary key (RegionCode)
);
go
CREATE TABLE FIN_Area
(
	AreaCode varchar(7),
	RegionCode varchar(32),
	AreaName varchar(32) not null,
	EnteredFrom	varchar(16) null,
	EnteredUser varchar(12) null,
	EnteredDate date null,
	ModifiedFrom varchar(16) null,
	ModifiedUser varchar(12) null,
	ModifiedDate date null,
	
	constraint FIN_Area_PK primary key (AreaCode,RegionCode),
	constraint FIN_Area_FK foreign key (RegionCode) references FIN_Region (RegionCode)
	
);
go
CREATE TABLE FIN_CostCenter
(
	CostCenterCode varchar(32),
	CostCenterDescription varchar(16),
	EnteredUser varchar(12) null,
	EnteredFrom varchar(16) null,
	EnteredDate date null,
	ModifiedFrom varchar(16) null,
	ModifiedUser varchar(12) null,
	ModifiedDate date null,
	
	constraint FIN_CostCenter_PK primary key (CostCenterCode),

);
go
CREATE TABLE FIN_Bank
(
	BankCode varchar(7),
	BankName varchar(32) null,	
	EnteredFrom	varchar(16) null,
	EnteredUser varchar(12) null,
	EnteredDate date null,
	ModifiedFrom varchar(16) null,
	ModifiedUser varchar(12) null,
	ModifiedDate date null,
	
	constraint FIN_Bank_PK primary key (BankCode)
	
);
go
CREATE TABLE FIN_BankBranch
(
	BankCode varchar(7),
	BankBranchCode varchar(64),
	BranchName varchar(256) null,
	Address1 varchar(512) null,
	Address2 varchar(256) null,
	City varchar(32) null,
	AreaCode varchar(7) null,
	RegionCode varchar(32) null,
	EnteredFrom	varchar(16) null,
	EnteredUser varchar(12) null,
	EnteredDate date null,
	ModifiedFrom varchar(16) null,
	ModifiedUser varchar(12) null,
	ModifiedDate date null,
	
	constraint FIN_BankBranch_PK primary key (BankCode,BankBranchCode),
	constraint FIN_BankBranch_FK1 foreign key (BankCode) references FIN_Bank(BankCode),
	constraint FIN_BankBranch_FK2 foreign key (AreaCode,RegionCode) references FIN_Area(AreaCode,RegionCode),
	
);
go
CREATE TABLE FIN_BankAccount
(
	BankCode varchar(7),
	BankBranchCode varchar(64),	
	AccountSEQNo int,
	GLAccountNo varchar(4) null,
	CurrentACNo varchar(9) null,
	ODLimit decimal null,
	LastChequePrintedDate datetime null,
	LastChequeNumber varchar(3) null,
	ChequeDOCCode varchar(3) null,
	CostCenter varchar(32) null,
	CurrencyCode varchar(3) null,
	Balance decimal default 0.0,
	ChequePrintFlag varchar(4) null,
	NoofDepositperSlip int default 1, 
	EnteredFrom	varchar(16) null,
	EnteredUser varchar(12) null,
	EnteredDate date null,
	ModifiedFrom varchar(16) null,
	ModifiedUser varchar(12) null,
	ModifiedDate date null,
	
	constraint FIN_BankAccount_PK primary key (BankCode,BankBranchCode,AccountSEQNo),
	constraint FIN_BankAccount_FK1 foreign key(BankCode,BankBranchCode) references FIN_BankBranch(BankCode,BankBranchCode),
	constraint FIN_BankAccount_FK2 foreign key(ChequeDOCCode) references ERP_Document(DocCode),
	constraint FIN_BankAccount_FK3 foreign key(CostCenter) references FIN_CostCenter(CostCenterCode),
	constraint FIN_BankAccount_FK4 foreign key(CurrencyCode) references FIN_Currency(CurrencyCode),
	constraint FIN_BankAccount_FK5 foreign key(GLAccountNo) references FIN_GeneralLedgerAccount(AccountNo)
);
go
CREATE TABLE FIN_CustomerSupplier_Info
(
	CustSupFlag int not null,
	CusSupCode varchar(128) not null,
	CustSupName varchar(256) null,
	AddressLine1 varchar(512) null,
	AddressLine2 varchar(512) null,
	City varchar(32) null,
	Country varchar(16) null,
	ForeignLocalFlag int default 1,
	CustSupStatus int default 2,
	ContactPersonsSales varchar(128) null,
	ContactPersonsAccount varchar(128) null,
	TaxFlag int null,
	CustSupType int null,
	TelephoneNo1 varchar(15) null,
	TelephoneNo2 varchar(15) null,
	FaxNo1 varchar(15) null,
	FaxNo2 varchar(15) null,
	EmailAddress varchar(30) null,
	WebSite varchar(30) null,
	TaxRegNo varchar(16) null,
	PaymentMode int null,
	CurrencyCode varchar(3) null,	
	CreditPeriod int null,
	CreditPeriodUnit int null,
	CreditLimit decimal null,
	EnteredFrom	varchar(16) null,
	EnteredUser varchar(12) null,
	EnteredDate date null,
	ModifiedFrom varchar(16) null,
	ModifiedUser varchar(12) null,
	ModifiedDate date null,
	
	constraint FIN_CustomerSupplier_Info_PK primary key(CustSupFlag,CusSupCode),
	constraint FIN_CustomerSupplier_Info_FK foreign key(CurrencyCode) references FIN_Currency(CurrencyCode),
	constraint FIN_CustomerSupplier_Info_CustSupFlag_Check check(CustSupFlag in (1,2)),
	constraint FIN_CustomerSupplier_Info_CustSupStatus_Check check(CustSupStatus in (1,2)),
	constraint FIN_CustomerSupplier_Info_TaxFlag_Check check(TaxFlag in (1,2)),
	constraint FIN_CustomerSupplier_Info_CustSupType_Check check(CustSupType in (1,2,3)),
	constraint FIN_CustomerSupplier_Info_PaymentMode_Check check(PaymentMode in (1,2,3)),
	constraint FIN_CustomerSupplier_Info_CreditPeriodUnit_Check check(CreditPeriodUnit in (1,2,3,4)),
);
go
CREATE TABLE FIN_CustomerSupplierBank
(
	CustSupFlag int,
	CusSupCode varchar(128),
	BankCode varchar(7),
	BranchCode varchar(64),
	AccountNo varchar(16) not null,
	EnteredFrom	varchar(16) null,
	EnteredUser varchar(12) null,
	EnteredDate date null,
	ModifiedFrom varchar(16) null,
	ModifiedUser varchar(12) null,
	ModifiedDate date null,
	
	constraint FIN_CustomerSupplierBank_PK primary key (CusSupCode,BankCode,BranchCode),
	constraint FIN_CustomerSupplierBank_FK1 foreign key (CustSupFlag,CusSupCode) references FIN_CustomerSupplier_Info (CustSupFlag,CusSupCode),
	constraint FIN_CustomerSupplierBank_FK2 foreign key (BankCode,BranchCode) references FIN_BankBranch (BankCode,BankBranchCode),

);
go
CREATE TABLE FIN_CustSupPeriodBalance
(
	CustSupFlag int,
	CusSupCode varchar(128),
	FinancialYear int,
	AccountingPeriod int,
	CurrencyCode varchar(3),
	OpeningBalanceFCY decimal null,
	OpeningBalanceLCY decimal null,
	DebitMovementFCY decimal null,
	DebitMovementLCY decimal null,
	CreditMovementFCY decimal null,
	CreditMovementLCY decimal null,
	ClosingBalanceFCY decimal null,
	ClosingBalanceLCY decimal null,
	EnteredFrom	varchar(16) null,
	EnteredUser varchar(12) null,
	EnteredDate date null,
	ModifiedFrom varchar(16) null,
	ModifiedUser varchar(12) null,
	ModifiedDate date null,
	
	constraint FIN_CustSupPeriodBalance_PK primary key (CustSupFlag,CusSupCode,AccountingPeriod,FinancialYear),
	constraint FIN_CustSupPeriodBalance_FK1 foreign key (CustSupFlag,CusSupCode) references FIN_CustomerSupplier_Info (CustSupFlag,CusSupCode),
	constraint FIN_CustSupPeriodBalance_FK2 foreign key (CurrencyCode) references FIN_Currency (CurrencyCode),
);
go
CREATE TABLE ERP_DocumentAttributesBankInfo
(
	DocCode varchar(3),
	TxCode	varchar(3),
	FixedBankCode varchar(7) NULL,
	FixedBranchCode varchar(64) NULL,
	FixedAccSeqNo INT NULL,
	EnteredFrom	varchar(16) null,
	EnteredUser varchar(12) null,
	EnteredDate date null,
	ModifiedFrom varchar(16) null,
	ModifiedUser varchar(12) null,
	ModifiedDate date null,
	
	constraint ERP_DocumentAttributesBankInfo_PK primary key (DocCode,TxCode),
	constraint ERP_DocumentAttributesBankInfo_FK1 foreign key (DocCode,TxCode) references ERP_DocumentAttributes (DocCode,TxCode),
	constraint ERP_DocumentAttributesBankInfo_FK2 foreign key (FixedBankCode,FixedBranchCode,FixedAccSeqNo) references FIN_BankAccount (BankCode,BankBranchCode,AccountSEQNo)
);
go	
CREATE TABLE FIN_GeneralLedgerSummary
(
	AccountNo varchar(3) not null,
	CurrencyCode varchar(4) not null,
	FinancialYear int not null,
	AccountingPeriod smallint not null,
	OpeningBalanceFCY decimal null,
	OpeningBalanceLCY decimal null,
	DebitMovementFCY decimal null,
	DebitMovementLCY decimal null,
	CreditMovementFCY decimal null,
	CreditMovementLCY decimal null,
	DebitAdjustmentFCY decimal null,
	DebitAdjustmentLCY decimal null,
	CreditAdjustementFCY decimal null,
	CreditAdjustementLCY decimal null,
	ClosingBalanceFCY decimal null,
	ClosingBalanceLCY decimal null,
	EnteredFrom	varchar(16) null,
	EnteredUser varchar(12) null,
	EnteredDate date null,
	ModifiedFrom varchar(16) null,
	ModifiedUser varchar(12) null,
	ModifiedDate date null,

	constraint FIN_GeneralLedgerSummary_PK primary key (AccountNo,CurrencyCode,FinancialYear,AccountingPeriod)
	
);
go
CREATE TABLE ERP_FixedTxnAttributes
(
	DocCode varchar(3),
	TxCode	varchar(3),
	GLCode	varchar(4),
	Cost_Dbt_GLAccCode varchar(4),
	Cost_Dbt_CostCenterCode	varchar(32),
	Cost_Dbt_CurrencyCode varchar(3),
	Cost_Crd_GLAccCode	varchar(4),
	Cost_Crd_CostCenterCode	varchar(32),
	Cost_Crd_CurrencyCode	varchar(3),
	Sales_Dbt_GLAccCode	varchar(4),
	Sales_Dbt_CostCenterCode varchar(32),
	Sales_Dbt_CurrencyCode	varchar(3),
	Sales_Crd_GLAccCode	varchar(4),
	Sales_Crd_CostCenterCode varchar(32),
	Sales_Crd_CurrencyCode	varchar(3),
	EnteredFrom	varchar(16) null,
	EnteredUser varchar(12) null,
	EnteredDate date null,
	ModifiedFrom varchar(16) null,
	ModifiedUser varchar(12) null,
	ModifiedDate date null,
	
	constraint ERP_FixedTxnAttributes_PK primary key(DocCode,TxCode,GLCode),
    constraint DocAttributes_FTA_FK foreign key (DocCode,TxCode) references ERP_DocumentAttributes(DocCode,TxCode),
	constraint GLCode_FTA_FK foreign key (GLCode) references FIN_GeneralLedgerAccount(AccountNo),
	constraint ERP_FixedTxnAttributes_FK1 foreign key (Cost_Dbt_GLAccCode) references FIN_GeneralLedgerAccount(AccountNo),
	constraint ERP_FixedTxnAttributes_FK2 foreign key (Cost_Dbt_CostCenterCode) references FIN_CostCenter(CostCenterCode),
	constraint ERP_FixedTxnAttributes_FK3 foreign key (Cost_Dbt_CurrencyCode) references  FIN_Currency(CurrencyCode),
	constraint ERP_FixedTxnAttributes_FK4 foreign key (Cost_Crd_GLAccCode) references  FIN_GeneralLedgerAccount(AccountNo),
	constraint ERP_FixedTxnAttributes_FK5 foreign key (Cost_Crd_CostCenterCode) references  FIN_CostCenter(CostCenterCode),
	constraint ERP_FixedTxnAttributes_FK6 foreign key (Cost_Crd_CurrencyCode) references  FIN_Currency(CurrencyCode),
	constraint ERP_FixedTxnAttributes_FK7 foreign key (Sales_Dbt_GLAccCode) references FIN_GeneralLedgerAccount(AccountNo),
	constraint ERP_FixedTxnAttributes_FK8 foreign key (Sales_Dbt_CostCenterCode) references FIN_CostCenter(CostCenterCode),
	constraint ERP_FixedTxnAttributes_FK9 foreign key (Sales_Dbt_CurrencyCode) references  FIN_Currency(CurrencyCode),
	constraint ERP_FixedTxnAttributes_FK10 foreign key (Sales_Crd_GLAccCode) references  FIN_GeneralLedgerAccount(AccountNo),
	constraint ERP_FixedTxnAttributes_FK11 foreign key (Sales_Crd_CostCenterCode) references  FIN_CostCenter(CostCenterCode),
	constraint ERP_FixedTxnAttributes_FK12 foreign key (Sales_Crd_CurrencyCode) references  FIN_Currency(CurrencyCode),

);
go
CREATE TABLE ERP_LastTransactionInfo
(
	ProductCode varchar(5),
	SubSystemCode varchar(5),
	DocCode varchar(3),
	TxCode varchar(3),
	LastVoucherNo int,
	LastTxnSerialNo int,
	TransactionType int,
	ExecutedUser varchar(12),
	ExecutedPC varchar(32),
	ExucutedDateTime datetime,
	EnteredFrom	varchar(16) null,
	EnteredUser varchar(12) null,
	EnteredDate date null,
	
	
	constraint ERP_LastTransactionInfo_PK primary key (ProductCode,SubSystemCode,DocCode,TxCode),
	constraint ERP_LastTransactionInfo_FK foreign key (DocCode,TxCode) references ERP_DocumentAttributes(DocCode,TxCode),
	constraint ERP_LastTransactionInfo_TransactionTypeCheck check (TransactionType in (1,2))
);
go
CREATE TABLE FIN_ControledTransaction
(
	DocCode varchar(3),
	TxCode varchar(3),
	GLAccountNo varchar(4),
	CostCenterCode varchar(32),
	CurrencyCode varchar(3),
	CreditDebit_Flag int,
	EnteredFrom	varchar(16) null,
	EnteredUser varchar(12) null,
	EnteredDate date null,
	ModifiedFrom varchar(16) null,
	ModifiedUser varchar(12) null,
	ModifiedDate date null,
	
	constraint FIN_ControledTransaction_PK primary key(DocCode,TxCode,GLAccountNo,CostCenterCode,CurrencyCode),
	constraint DocAttributes_CT_FK foreign key (DocCode,TxCode) references ERP_DocumentAttributes(DocCode,TxCode),
	constraint GLAccountNo_CT_FK foreign key (GLAccountNo) references FIN_GeneralLedgerAccount(AccountNo),
	constraint CostCenterCode_CT_FK foreign key (CostCenterCode) references FIN_CostCenter(CostCenterCode),
	constraint CurrencyCode_CT_FK foreign key(CurrencyCode) references FIN_Currency(CurrencyCode)

);
go
CREATE TABLE FIN_PrimaryTransaction
(
	DocCode varchar(3),
	TxCode varchar(3),
	TxSerial int,
	VoucherNumber int,
	FinancialYear int,
	TransactionStatus int,
	AccountingPeriod int,
	ProcessPeriod int,
	EnteredProcessDate datetime,
	TransactionType int,
	SubModuleCode varchar(8),
	VoucherDate datetime,
	Reference1 varchar(64),
	Reference2 varchar(64),
	Reference3 varchar(64),
	CashBank_Flag int,
	BankTxn_Flag int,
	ARAPTxn_Flag int,
	CusSupTxn_Flag int,
	DueDate int,
	TotalDebitAmt_LCY decimal,
	TotalCreditAmt_LCY decimal,
	EnteredFrom	varchar(16) null,
	EnteredUser varchar(12) null,
	EnteredDate date null,
	ModifiedFrom varchar(16) null,
	ModifiedUser varchar(12) null,
	ModifiedDate date null,
	
	constraint FIN_PrimaryTransaction_PK primary key(DocCode,TxCode,TxSerial,VoucherNumber,FinancialYear)
);
go
CREATE TABLE FIN_SecondaryTransaction
(
	DocCode varchar(3),
	TxCode varchar(3),
	TxSerial int,
	VoucherNumber int,
	FinancialYear int,
	TxnSeqNo int,
	TransactionStatus int,
	GLAccountNo varchar(4),
	CostCenterCode varchar(3),
	CurrencyCode varchar(3),
	ExRate decimal,
	Reference4 varchar(64),
	Reference5 varchar(64),
	Reference6 varchar(64),
	CreditDebit_Flag int,
	AmountLCY decimal,
	AmountFCY decimal,
	BalanceAmountLCY decimal,
	BalanceAmountFCY decimal,
	EnteredFrom	varchar(16) null,
	EnteredUser varchar(12) null,
	EnteredDate date null,
	ModifiedFrom varchar(16) null,
	ModifiedUser varchar(12) null,
	ModifiedDate date null,
	
	constraint FIN_SecondaryTransaction_PK primary key (DocCode,TxCode,TxSerial,VoucherNumber,FinancialYear,TxnSeqNo)

);
go
CREATE TABLE FIN_TxnReference
(
	DocCode varchar(3),
	TxCode varchar(3),
	RefSeq smallint,
	SeqNo int,
	ReferenceText varchar(16),
	EnteredFrom	varchar(16) null,
	EnteredUser varchar(12) null,
	EnteredDate date null,
	ModifiedFrom varchar(16) null,
	ModifiedUser varchar(12) null,
	ModifiedDate date null,
	
	constraint FIN_TxnReference_PK primary key(DocCode,TxCode,RefSeq),
	constraint FIN_TxnReference_FK foreign key (DocCode,TxCode) references ERP_DocumentAttributes(DocCode,TxCode)
);
go
CREATE TABLE ERP_Period
(
	ProductCode varchar(5) NOT NULL,
	FinancialYear int NOT NULL,
	AccountingPeriod int NOT NULL,
	ProcessPeriod int NOT NULL,
	StardDate datetime NOT NULL,
	EndDate datetime NOT NULL,
	EnteredFrom varchar(16) NULL,
	EnteredUser varchar(12) NULL,
	EnteredDate date NULL,
	ModifiedFrom varchar(16) NULL,
	ModifiedUser varchar(12) NULL,
	ModifiedDate date NULL,
	
	CONSTRAINT ERP_Period_PK PRIMARY KEY(ProductCode,FinancialYear,AccountingPeriod)
);
go
CREATE TABLE ERP_ProcessControl
(
	ProductCode varchar(5) NOT NULL,
	ProcessDate datetime NULL,
	PreviousProcessDate datetime NULL,
	FinancialYear int NULL,
	PreviousFinancialYear int NULL,
	ProcessPeriod int NULL,
	PreviousProcessPeriod int NULL,
	AccountingPeriod int NULL,
	PreviousAccountingPeriod int NULL,
	LastRevaluationPeriod int NULL,
	EnteredFrom varchar(16) NULL,
	EnteredUser varchar(12) NULL,
	EnteredDate date NULL,
	ModifiedFrom varchar(16) NULL,
	ModifiedUser varchar(12) NULL,
	ModifiedDate date NULL,
	
	CONSTRAINT ERP_ProcessControl_PK PRIMARY KEY(ProductCode)
);
go
CREATE TABLE ERP_SubModule
(
	ProductCode varchar(5) NOT NULL,
	SubSystemCode varchar(5) NOT NULL,
	SubModuleCode varchar(4) NOT NULL,
	SubModuleDescription varchar(32) NULL,
	SubModuleActiveStatus int NULL,
	EnteredFrom varchar(16) NULL,
	EnteredUser varchar(12) NULL,
	EnteredDate date NULL,
	ModifiedFrom varchar(16) NULL,
	ModifiedUser varchar(12) NULL,
	ModifiedDate date NULL,
	
	CONSTRAINT ERP_SubModule_PK PRIMARY KEY(ProductCode,SubSystemCode,SubModuleCode),
	CONSTRAINT ERP_SubModule_SubModuleActiveStatus_CH CHECK(SubModuleActiveStatus IN (1,2))
);
go
create table ERP_UnitDefinition
(
	UnitCode varchar(3) not null,
	StandardSyntax varchar(4) null,
	UnitDescription varchar(16) null,
	MajorUnitCode varchar (3) null,
	EnteredFrom	varchar(16) null,
	EnteredUser varchar(12) null,
	EnteredDate date null,
	ModifiedFrom varchar(16) null,
	ModifiedUser varchar(12) null,
	ModifiedDate date null,
	constraint ERP_UnitDefinition_PK primary key (UnitCode),
	constraint ERP_UnitDefinition_FK1 foreign key (MajorUnitCode) references ERP_UnitDefinition (UnitCode)
);	
go
create table ERP_UnitConversion
(
	FromUnitCode varchar (3) not null,
	ToUnitCode varchar (3) not null,
	ConversionFactor decimal null,
	EnteredFrom	varchar(16) null,
	EnteredUser varchar(12) null,
	EnteredDate date null,
	ModifiedFrom varchar(16) null,
	ModifiedUser varchar(12) null,
	ModifiedDate date null,
	constraint ERP_UnitConversion_PK primary key (FromUnitCode,ToUnitCode),
	constraint ERP_UnitConversion_FK1 foreign key (FromUnitCode) references ERP_UnitDefinition (UnitCode),
	constraint ERP_UnitConversion_FK2 foreign key (ToUnitCode) references ERP_UnitDefinition (UnitCode),
);
go
create table ERP_AgentBrokerSalesMan
(
	AgentBrokerSalesManFlag int not null,
	AgentBrokerSalesManCode varchar(8) not null,
	AgentBrokerSalesManName varchar (128) null,
	AddressLine1 varchar (512)null,
	AddressLine2 varchar (512) null,
	City varchar (64) null,
	Country varchar (32) null,
	ForeignLocalFlag int default(1),
	ActiveStatus int default(2),
	GLAccount varchar (4) null,
	TaxFlag int not null,
	TelephoneNo1 varchar (30) null,
	TelephoneNo2 varchar (15) null,
	FaxNo1 varchar(15) null,
	FaxNo2 varchar (15) null,
	EmailAddress varchar (30) null,
	WebSite varchar (30) null,
	PaymentMode int default(1),
	CurrencyCode varchar (3) null,
	EnteredFrom	varchar(16) null,
	EnteredUser varchar(12) null,
	EnteredDate date null,
	ModifiedFrom varchar(16) null,
	ModifiedUser varchar(12) null,
	ModifiedDate date null,
	constraint ERP_AgentBrokerSalesMan_PK primary key (AgentBrokerSalesManFlag,AgentBrokerSalesManCode),
	constraint ERP_AgentBrokerSalesMan_FK1 foreign key (GLAccount) references FIN_GeneralLedgerAccount (AccountNo),
	constraint ERP_AgentBrokerSalesMan_FK2 foreign key (CurrencyCode) references FIN_Currency (CurrencyCode),
	CONSTRAINT ERP_AgentBrokerSalesMan_ForeignLocalFlag_CH CHECK(ForeignLocalFlag IN (1,2)),
	CONSTRAINT ERP_AgentBrokerSalesMan_ActiveStatus_CH CHECK(ActiveStatus IN (1,2)),
	CONSTRAINT ERP_AgentBrokerSalesMan_TaxFlag_CH CHECK(TaxFlag IN (1,2)),
	CONSTRAINT ERP_AgentBrokerSalesMan_PaymentMode_CH CHECK(PaymentMode IN (1,2,3)),
	CONSTRAINT ERP_AgentBrokerSalesMan_AgentBrokerSalesManFlag_CH CHECK (AgentBrokerSalesManFlag IN (1,2,3))
);
go
CREATE TABLE FIN_CostCenterwiseConfiguration
(
	RevNo int identity,
	CostCenterCode varchar(32),
	BaseCurrencyCode varchar(3) null,
	AdjustmentAccNo varchar (4) null,
	AdjustmentCostCenter varchar(32) null,
	AdjustmentCurrCode varchar (3) null,
	DateDisplayFormat varchar (16) null,
	APCostCenterCode varchar (32) null,
	DocCodeForInvoice varchar(3) null,
	TxnCodeForInvoice varchar (3) null, 
	DocCodeForRetainedProfit varchar (3) null,
	TxnCodeForRetainedProfit varchar(3) null,
	DocCodeForRevalGainLoss varchar(3) null,
	TxnCodeForRevalGainLoss varchar(3) null,
	BaseGLAccountForRetainedProfit varchar(4) null,
	BaseCostCenterForRetainedProfit varchar(32) null,
	BaseCurrencyCodeForRetainedProfit varchar(3) null,
	PNLAccountCodeForRetainedProfit varchar(4) null,
	PNLCostCenterForRetainedProfit varchar(32) null,
	PNLCurrencyCodeForRetainedProfit varchar(3) null,
	DbtAccountCodeRetainedProfit varchar(4) null,
	DbtCostCenterRetainedProfit varchar(32) null,
	DbtCurrencyCodeForRetainedProfit varchar(3) null,
	CrdAccountCodeRetainedProfit varchar(4) null,
	CrdCostCenterRetainedProfit varchar(32) null,
	CrdCurrencyCodeForRetainedProfit varchar(3) null,
	EnteredFrom	varchar(16) null,
	EnteredUser varchar(12) null,
	EnteredDate date null,
	ModifiedFrom varchar(16) null,
	ModifiedUser varchar(12) null,
	ModifiedDate date null,
	CONSTRAINT FIN_CostCenterRef_PK PRIMARY KEY (RevNo),
	CONSTRAINT FIN_CostCenter_FK0 FOREIGN KEY (CostCenterCode) REFERENCES FIN_CostCenter(CostCenterCode),
	CONSTRAINT FIN_CostCenter_FK1 FOREIGN KEY (BaseCurrencyCode) REFERENCES FIN_Currency(CurrencyCode),
	CONSTRAINT FIN_CostCenter_FK2 FOREIGN KEY (AdjustmentAccNo) REFERENCES FIN_GeneralLedgerAccount(AccountNo),
	CONSTRAINT FIN_CostCenter_FK3 FOREIGN KEY (AdjustmentCostCenter) REFERENCES FIN_CostCenter(CostCenterCode),
	CONSTRAINT FIN_CostCenter_FK4 FOREIGN KEY (AdjustmentCurrCode) REFERENCES FIN_Currency(CurrencyCode),
	CONSTRAINT FIN_CostCenter_FK5 FOREIGN KEY (APCostCenterCode) REFERENCES FIN_CostCenter(CostCenterCode),
	CONSTRAINT FIN_CostCenter_FK6 FOREIGN KEY (DocCodeForInvoice,TxnCodeForInvoice) REFERENCES ERP_DocumentAttributes(DocCode,TxCode),
	CONSTRAINT FIN_CostCenter_FK7 FOREIGN KEY (DocCodeForRetainedProfit,TxnCodeForRetainedProfit) REFERENCES ERP_DocumentAttributes(DocCode,TxCode),
	CONSTRAINT FIN_CostCenter_FK8 FOREIGN KEY (DocCodeForRevalGainLoss,TxnCodeForRevalGainLoss) REFERENCES ERP_DocumentAttributes(DocCode,TxCode),
	CONSTRAINT FIN_CostCenter_FK9 FOREIGN KEY (BaseGLAccountForRetainedProfit) REFERENCES FIN_GeneralLedgerAccount(AccountNo),
	CONSTRAINT FIN_CostCenter_FK10 FOREIGN KEY (BaseCostCenterForRetainedProfit) REFERENCES FIN_CostCenter(CostCenterCode),
	CONSTRAINT FIN_CostCenter_FK11 FOREIGN KEY (BaseCurrencyCodeForRetainedProfit) REFERENCES FIN_Currency(CurrencyCode),
	CONSTRAINT FIN_CostCenter_FK12 FOREIGN KEY (PNLAccountCodeForRetainedProfit) REFERENCES FIN_GeneralLedgerAccount(AccountNo),
	CONSTRAINT FIN_CostCenter_FK13 FOREIGN KEY (PNLCostCenterForRetainedProfit) REFERENCES FIN_CostCenter(CostCenterCode),
	CONSTRAINT FIN_CostCenter_FK14 FOREIGN KEY (PNLCurrencyCodeForRetainedProfit) REFERENCES FIN_Currency(CurrencyCode),
	CONSTRAINT FIN_CostCenter_FK15 FOREIGN KEY (DbtAccountCodeRetainedProfit) REFERENCES FIN_GeneralLedgerAccount(AccountNo),
	CONSTRAINT FIN_CostCenter_FK16 FOREIGN KEY (DbtCostCenterRetainedProfit) REFERENCES FIN_CostCenter(CostCenterCode),
	CONSTRAINT FIN_CostCenter_FK17 FOREIGN KEY (DbtCurrencyCodeForRetainedProfit) REFERENCES FIN_Currency(CurrencyCode),
	CONSTRAINT FIN_CostCenter_FK18 FOREIGN KEY (CrdAccountCodeRetainedProfit) REFERENCES FIN_GeneralLedgerAccount(AccountNo),
	CONSTRAINT FIN_CostCenter_FK19 FOREIGN KEY (CrdCostCenterRetainedProfit) REFERENCES FIN_CostCenter(CostCenterCode),
	CONSTRAINT FIN_CostCenter_FK20 FOREIGN KEY (CrdCurrencyCodeForRetainedProfit) REFERENCES FIN_Currency(CurrencyCode)
);
go
CREATE TABLE HR_EMPLOYEE
(
 EMP_NUMBER varchar(6),
 EMP_TITLE varchar(10),
 EMP_CALLING_NAME varchar(50),
 EMP_SURNAME varchar(50),
 EMP_MAIDEN_NAME varchar(70),
 EMP_MIDDLE_INI varchar(50),
 EMP_NAMES_BY_INI varchar(200),
 EMP_FULLNAME varchar(200),
 EMP_OTHER_NAMES varchar(200),
 constraint HR_EMPLOYEE_PK primary key(EMP_NUMBER),
);
go
CREATE TABLE HR_EMP_ATTACHMENT
(
 EMP_NUMBER varchar(6),
 EATTACH_ID numeric,
 EATTACH_DESC varchar(200),
 EATTACH_ATTACHMENT image,
 
 constraint HR_EMP_ATTACHMENT_PK primary key(EMP_NUMBER,EATTACH_ID),
 constraint HR_EMP_ATTACHMENT_FK foreign key (EMP_NUMBER) references HR_EMPLOYEE (EMP_NUMBER)
 
);
go
create table HR_EMP_RELATIONINFO
(
 EMP_NUMBER varchar(6),
 EREL_DEPENDNO numeric,
 EREL_RELATIONSHIP varchar(20),
 EREL_RELATIONFULLNAME varchar(20),
 EREL_BIRTHDAY datetime,
 EREL_NIC_NUMBER varchar(20),
 EREL_SEX_FLG smallint,
 EREL_TELEPHONE varchar(20),
 EREL_EDU_CENTRE varchar(100),
 EREL_OFFICE_ADDRESS varchar(200),
 EREL_HOUSE_ADDRESS varchar(200),
 EREL_SPOUSE_TELEPHONE varchar(20),
 EREL_WORK_SAME_COMP_FLG smallint,
 EREL_EMP_NUMBER varchar(6),
 EREL_ENTDEATHDONATION_FLG smallint,
 EREL_ENTMEDICALBENIFIT_FLG smallint,
 EREL_LIVINGORNOT_FLG smallint,
 EREL_PF_NOMINEE_FLG smallint,
 EREL_PF_RATIO decimal(5,2),
 
 constraint HR_EMP_RELATIONINFO_PK primary key(EMP_NUMBER,EREL_DEPENDNO),
 constraint HR_EMP_RELATIONINFO_FK1 foreign key (EMP_NUMBER) references HR_EMPLOYEE(EMP_NUMBER),
 constraint HR_EMP_RELATIONINFO_FK2 foreign key (EREL_EMP_NUMBER) references HR_EMPLOYEE(EMP_NUMBER)
 
);
go
create table HR_EMP_PASSPORT(
EMP_NUMBER varchar(6),
EP_SEQNO numeric,
EP_PASSPORTNUMBER varchar(20),
EP_PASSPORTISSUEDDATE datetime,
EP_PLACEPASSPORTISSUED varchar(30),
EP_PASSPORTEXPIREDATE datetime,
EP_COMMENTS varchar(300),
COU_CODE varchar(6),
EP_NO_OF_ENTRIES smallint
constraint HR_EMP_PASSPORT_PK primary key(EMP_NUMBER,EP_SEQNO),
constraint HR_EMP_PASSPORT_EMPNO foreign key (EMP_NUMBER) references HR_EMPLOYEE(EMP_NUMBER)
);
go
create table HR_EMP_BANK(
EMP_NUMBER varchar(6),
BBRANCH_CODE varchar(6),
BANK_CODE varchar(8),
EBANK_ACC_NO varchar(80),
EBANK_ACC_TYPE_FLG smallint,
EBANK_AMOUNT decimal(15,2),
EBANK_ORDER numeric
constraint HR_EMP_BANK_PK primary key(EMP_NUMBER,BBRANCH_CODE,BANK_CODE)
constraint HR_EMP_BANK_EMPNO foreign key (EMP_NUMBER) references HR_EMPLOYEE(EMP_NUMBER)
);
go
create table HR_EMP_EMERGENCY
(
	EMP_NUMBER varchar(6),
	EEMERG_CONT_PER_FULLNAME varchar(200),
	EEMERG_RELATIONSHIP varchar(50),
	EEMERG_PER_ADDRESS varchar(200),
	EEMERG_OFFICIAL_ADDRESS varchar(200),
	EEMERG_RES_TELEPHONE varchar(20),
	EEMERG_OFFICE_TELEPHONE varchar(20),
	EEMERG_MOBILE varchar(20),
	
	constraint HR_EMP_EMERGENCY_pk primary key(EMP_NUMBER),
	constraint HR_EMP_EMERGENCY_fk foreign key(EMP_NUMBER) references HR_EMPLOYEE(EMP_NUMBER) 
);
go
create table HR_EMP_TRANSPORT
(
	EMP_NUMBER varchar(6),
	ETPORT_MODE varchar(20),
	ETPORT_NEAR_STATION varchar(50),
	ETPORT_COMMUTING_MODE smallint,
	ETPORT_COD_EMP_NUMBER varchar(6),
	ETPORT_DISTANCE float,
	ETPORT_DURATION float,
	
	constraint HR_EMP_TRANSPORT_pk primary key(EMP_NUMBER),
	constraint HR_EMP_TRANSPORT_fk foreign key(EMP_NUMBER) references HR_EMPLOYEE(EMP_NUMBER)
);
go
create table HR_EMP_TRAININGS
(
	EMP_NUMBER varchar(6),
	TN_ID int,
	TN_DESC varchar(200),
	TN_TYPE varchar(75),
	TN_STATUS smallint,
	
	constraint HR_EMP_TRAININGS_pk primary key(EMP_NUMBER,TN_ID),
	constraint HR_EMP_TRAININGS_fk foreign key(EMP_NUMBER) references HR_EMPLOYEE(EMP_NUMBER) 
);
go
create table HR_EMP_WARNINGS
(
	EMP_NUMBER varchar(6),
	WRN_ID int,
	WRN_TYPE varchar(75),
	WRN_DESC varchar(200),
	WRN_DATE datetime,
	WRN_CREATED_BY varchar(100),
	
	constraint HR_HR_EMP_WARNINGS_pk primary key(EMP_NUMBER,WRN_ID),
	constraint HR_HR_EMP_WARNINGS_fk foreign key(EMP_NUMBER) references HR_EMPLOYEE(EMP_NUMBER) 
);
go
create table HR_EMP_WORK_EXPERIENCE(

EMP_NUMBER varchar(6),
EEXP_COMPANY varchar(100),
EEXP_ADDRESS1 varchar(50),
EEXP_ADDRESS2 varchar(50),
EEXP_ADDRESS3 varchar(50),
EEXP_DESIG_ON_LEAVE varchar(120),
EEXP_WORK_RELATED_FLG smallint check(EEXP_WORK_RELATED_FLG in (0,1)),
EEXP_FROM_DATE datetime,
EEXP_TO_DATE datetime,
EEXP_YEARS numeric,
EEXP_MONTHS smallint,
EEXP_REASON_FOR_LEAVE varchar(100),
EEXP_CONTACT_PERSON varchar(50),
EEXP_TELEPHONE varchar(20),
EEXP_EMAIL varchar(50),
EEXP_ACCOUNTABILITIES varchar(200),
EEXP_ACHIEVEMENTS varchar(200),

constraint EMP_NUM_HEWE_PK primary key (EMP_NUMBER),
constraint EMP_NO_HEWE_FK foreign key (EMP_NUMBER) references HR_EMPLOYEE(EMP_NUMBER),


);
go
create table HR_EMP_QUALIFICATION(
EMP_NUMBER varchar(6),
QUALIFI_CODE varchar(6),
EQUALIFI_INSTITUTE varchar(100),
EQUALIFI_YEAR numeric(4),
EQUALIFI_STATUS varchar(20),
EQUALIFI_COMMENTS varchar(200),

constraint EMP_NUMBER_QUAL_HEQ_PK primary key (EMP_NUMBER,QUALIFI_CODE),
constraint EMP_NUMBER_HEQ_FK foreign key (EMP_NUMBER) references HR_EMPLOYEE(EMP_NUMBER),
);

go
create table HR_MEMBERSHIP_TYPE(
MEMBTYPE_CODE varchar(6),
MEMBTYPE_NAME varchar(120),
STATUS_FLAG SmallInt check(STATUS_FLAG in(0,1)),
constraint MEMBTYPE_CODE_HMT_PK primary key (MEMBTYPE_CODE)
);
go

create table HR_MEMBERSHIP(
MEMBSHIP_CODE varchar(6),
MEMBTYPE_CODE varchar(6),
MEMBSHIP_NAME varchar(120),
STATUS_FLAG SmallInt,

constraint MEMBSHIP_CODE_HM_PK primary key (MEMBSHIP_CODE),
constraint MEMBTYPE_CODE_HM_FK foreign key (MEMBTYPE_CODE) references HR_MEMBERSHIP_TYPE(MEMBTYPE_CODE)
);
go

create table HR_EMP_MEMBER_DETAIL(
EMP_NUMBER varchar(6),
MEMBSHIP_CODE varchar(6),
MEMBTYPE_CODE varchar(6),
EMEMB_SUBSCRIPT_OWNERSHIP varchar(20),
EMEMB_SUBSCRIPT_AMOUNT decimal(15,2),
EMEMB_COMMENCE_DATE datetime,
EMEMB_RENEWAL_DATE datetime,

constraint NUM_MEMSHIP_MEMTYPE_EMD_PK primary key (EMP_NUMBER,MEMBSHIP_CODE,MEMBTYPE_CODE),
constraint EMP_NUMBER_EMD_FK foreign key (MEMBSHIP_CODE) references HR_EMPLOYEE(EMP_NUMBER),
constraint MEMBSHIP_CODE_EMD_FK foreign key (MEMBSHIP_CODE) references HR_MEMBERSHIP(MEMBSHIP_CODE),
constraint MEMBTYPE_CODE_EMD_FK foreign key (MEMBTYPE_CODE) references HR_MEMBERSHIP_TYPE(MEMBTYPE_CODE),

);
go

CREATE TABLE HR_CASH_BENEFIT
(
	BEN_CODE varchar(6),
	BEN_NAME varchar(120),
	BEN_AMOUNT float,
	STATUS_FLAG smallint,

	constraint HR_CASH_BENEFIT_PK primary key (BEN_CODE),
	constraint HR_CASH_BENEFIT_Check1 check (STATUS_FLAG in (0,1)),

);
go
CREATE TABLE HR_NONCASH_BENEFIT
(
	NBEN_CODE varchar(6),
	NBEN_NAME varchar(120),
	NBEN_ITEM_RETURNABLE_FLG smallint,
	STATUS_FLAG smallint,

	constraint HR_NONCASH_BENEFIT_PK primary key (NBEN_CODE),
	constraint HR_NONCASH_BENEFIT_Check1 check (NBEN_ITEM_RETURNABLE_FLG in (1,2)),
	constraint HR_NONCASH_BENEFIT_Check2 check (STATUS_FLAG in (0,1))
);
go
CREATE TABLE HR_EMP_CASH_BENEFIT
(
	EMP_NUMBER varchar(6),
	BEN_CODE varchar(6),
	EBEN_AMOUNT float,
	EBEN_DATE_ASSIGNED datetime,

	constraint HR_EMP_CASH_BENEFIT_PK primary key (EMP_NUMBER,BEN_CODE),
	constraint HR_EMP_CASH_BENEFIT_FK1 foreign key (EMP_NUMBER) references HR_EMPLOYEE(EMP_NUMBER),
	constraint HR_EMP_CASH_BENEFIT_FK2 foreign key (BEN_CODE) references HR_CASH_BENEFIT(BEN_CODE),
);
go
CREATE TABLE HR_EMP_NONCASH_BENEFIT
(
	EMP_NUMBER varchar(6),
	NBEN_CODE varchar(6),
	ENBEN_ISSUE_DATE datetime,
	ENBEN_QUANTITY float,
	ENBEN_COMMENTS varchar(100),
	ENBEN_ASSES_MGMT_FLG smallint,

	constraint HR_EMP_NONCASH_BENEFIT_PK primary key (EMP_NUMBER,NBEN_CODE),
	constraint HR_EMP_NONCASH_BENEFIT_FK1 foreign key (EMP_NUMBER) references HR_EMPLOYEE(EMP_NUMBER),
	constraint HR_EMP_NONCASH_BENEFIT_FK2 foreign key (NBEN_CODE) references HR_NONCASH_BENEFIT(NBEN_CODE),
);
go

CREATE TABLE HR_NATIONALITY
(
NAT_CODE varchar(6),
NAT_NAME	varchar(120),
constraint HR_NATIONALITY_PK primary key (NAT_CODE)
);
go
create table HR_RELIGION
(
RLG_CODE varchar(6),
RLG_NAME	varchar(50),
constraint HR_RELIGION_PK primary key (RLG_CODE)
);
go
create table HR_CURRENCY
(
CURRENCY_ID  varchar(6),
CURRENCY_NAME varchar(60),
STATUS_FLAG SmallInt,
constraint HR_CURRENCY_PK primary key (CURRENCY_ID),
constraint HR_CURRENCY_CHK check(STATUS_FLAG in (0,1)),
);
go
create table HR_SALARY_GRADE
(
SAL_GRD_CODE varchar(6),
SAL_GRD_NAME varchar(60),
CURRENCY_ID varchar(6),
STATUS_FLAG SmallInt,
constraint HR_SALARY_GRADE_PK primary key (SAL_GRD_CODE),
constraint HR_SALARY_GRADE_CHK check(STATUS_FLAG in (0,1)),
constraint HR_SALARY_GRADE_FK foreign key (CURRENCY_ID) references HR_CURRENCY(CURRENCY_ID)
);
go
create table HR_CORPORATE_TITLE
(
CT_CODE varchar(6),
CT_NAME varchar(120),
CT_TOPLEV_FLG smallint,
SAL_GRD_CODE varchar(6),
STATUS_FLAG SmallInt,
constraint HR_CORPORATE_TITLE_PK primary key (CT_CODE),
constraint HR_CORPORATE_TITLE_CHK1 check(CT_TOPLEV_FLG in (0,1)),
constraint HR_CORPORATE_TITLE_CHK2 check(STATUS_FLAG in (0,1)),
constraint HR_CORPORATE_TITLE_FK foreign key (SAL_GRD_CODE) references HR_SALARY_GRADE(SAL_GRD_CODE)
);
go
create table HR_DESIGNATION
(
DSG_CODE varchar(6),
DSG_NAME varchar(120),
CT_CODE varchar(6),
DSG_SNRMGT_FLG smallint,
STATUS_FLAG SmallInt,
constraint HR_DESIGNATION_PK primary key (DSG_CODE),
constraint HR_DESIGNATION_CHK1 check(DSG_SNRMGT_FLG in (0,1)),
constraint HR_DESIGNATION_CHK2 check(STATUS_FLAG in (0,1)),
constraint HR_DESIGNATION_FK foreign key (CT_CODE) references HR_CORPORATE_TITLE(CT_CODE)
);
go
create table HR_JD_CATEGORY
(
JDCAT_CODE varchar(6),
JDCAT_NAME varchar(100),
STATUS_FLAG SmallInt,
constraint HR_JD_CATEGORY_PK primary key(JDCAT_CODE),
constraint HR_JD_CATEGORY_CHK check(STATUS_FLAG in(0,1))
);
go
create table HR_LOCATION
(
LOC_CODE varchar(6),
LOC_NAME varchar(100),
STATUS_FLAG SmallInt,
constraint HR_LOCATION_PK primary key (LOC_CODE),
constraint HR_LOCATION_CHK check(STATUS_FLAG in(0,1))
)
go
create table HR_COUNTRY
(
COU_CODE varchar(6),
COU_NAME varchar(50),
constraint HR_COUNTRY_PK primary key (COU_CODE)
);
go
create table HR_PROVINCE
(
PROVINCE_CODE varchar(6),
COU_CODE varchar(6),
PROVINCE_NAME varchar(50),
constraint HR_PROVINCE_PK primary key (PROVINCE_CODE),
constraint HR_PROVINCE_FK foreign key (COU_CODE) references HR_COUNTRY(COU_CODE)
);
go
create table HR_DISTRICT
(
DISTRICT_CODE varchar(6),
PROVINCE_CODE varchar(6),
DISTRICT_NAME varchar(50),
constraint HR_DISTRICT_PK primary key (DISTRICT_CODE),
constraint HR_DISTRICT_FK foreign key (PROVINCE_CODE) references HR_PROVINCE(PROVINCE_CODE)
);
go
create table HR_ELECTORATE
(
ELECTORATE_CODE varchar(6),
ELECTORATE_NAME varchar(50),
constraint HR_ELECTORATE_PK primary key(ELECTORATE_CODE)
);
go
CREATE TABLE HR_EMPLOYEE_PD
(
EMP_NUMBER varchar(6) references HR_EMPLOYEE(EMP_NUMBER),
EMP_NIC_NO varchar(20),
EMP_NIC_DATE datetime,
EMP_BIRTHDAY datetime,
EMP_BIRTHPLACE varchar(100),
EMP_GENDER smallint,
EMP_BLOOD_GROUP varchar(4),
NAT_CODE varchar(6),
RLG_CODE varchar(6),
EMP_MARITAL_STATUS varchar(20),
EMP_MARRIED_DATE datetime,
constraint HR_EMPLOYEE_PD_PK primary key(EMP_NUMBER),
CONSTRAINT HR_EMPLOYEE_FK1 FOREIGN KEY (NAT_CODE) REFERENCES HR_NATIONALITY(NAT_CODE),
CONSTRAINT HR_EMPLOYEE_FK2 FOREIGN KEY (RLG_CODE) REFERENCES HR_RELIGION(RLG_CODE),
)
go
CREATE TABLE HR_EMPLOYEE_JI(
EMP_NUMBER varchar(6) references HR_EMPLOYEE(EMP_NUMBER),
EMP_DATE_JOINED datetime,
EMP_CONFIRM_FLG smallint,
EMP_CONFIRM_DATE datetime,
EMP_RESIGN_DATE datetime,
EMP_RETIRE_DATE datetime,
SAL_GRD_CODE varchar(6),
CT_CODE varchar(6),
DSG_CODE varchar(6),
EMP_WORKHOURS real,
constraint HR_EMPLOYEE_JI_PK primary key(EMP_NUMBER),
CONSTRAINT HR_EMPLOYEE_FK3 FOREIGN KEY (SAL_GRD_CODE) REFERENCES HR_SALARY_GRADE(SAL_GRD_CODE),
CONSTRAINT HR_EMPLOYEE_FK4 FOREIGN KEY (CT_CODE) REFERENCES HR_CORPORATE_TITLE(CT_CODE),
CONSTRAINT HR_EMPLOYEE_FK5 FOREIGN KEY (DSG_CODE) REFERENCES HR_DESIGNATION(DSG_CODE),
)
go
CREATE TABLE HR_EMPLOYEE_JS(
EMP_NUMBER varchar(6) references HR_EMPLOYEE(EMP_NUMBER),
EMP_TYPE varchar(50),
STAFFCAT_CODE varchar(6),
CAT_CODE varchar(6),
EMP_CONTARCT_START_DATE datetime,
EMP_CONTRACT_END_DATE datetime,
EMP_CONT_TO_PERM_FLG smallint,
EMP_CONT_TO_PERM_DATE datetime,
EMP_ACTIVE_HRM_FLG smallint,
constraint HR_EMPLOYEE_JS_PK primary key(EMP_NUMBER),
CONSTRAINT HR_EMPLOYEE_FK6 FOREIGN KEY (STAFFCAT_CODE) REFERENCES HR_JD_CATEGORY (JDCAT_CODE),
CONSTRAINT HR_EMPLOYEE_FK7 FOREIGN KEY (CAT_CODE) REFERENCES HR_JD_CATEGORY (JDCAT_CODE),
);
go		
CREATE TABLE HR_EMPLOYEE_TD(
EMP_NUMBER varchar(6) references HR_EMPLOYEE(EMP_NUMBER),
EMP_PAYE_TAX_EXEMPT varchar(20),
EMP_TAXONTAX_FLG smallint,
EMP_TAX_ID_NUMBER varchar(20),
EMP_EPF_ELIGIBLE_FLG smallint,
EMP_EPF_NUMBER varchar(6),
EMP_EPF_PAYMENT_TYPE_FLG smallint,
EMP_EPF_EMPLOYEE_AMOUNT decimal(15,2),
EMP_EPF_EMPLOYER_AMOUNT decimal(15,2),
EMP_ETF_ELIGIBLE_FLG smallint,
EMP_ETF_NUMBER varchar(20),
EMP_ETF_EMPLOYEE_AMOUNT decimal(15,2),
EMP_ETF_DATE datetime,
constraint HR_EMPLOYEE_TD_PK primary key(EMP_NUMBER)
)		
go
CREATE TABLE HR_EMPLOYEE_PC(
EMP_NUMBER varchar(6) references HR_EMPLOYEE(EMP_NUMBER),	
EMP_PER_ADDRESS1 varchar(50),
EMP_PER_ADDRESS2 varchar(50),
EMP_PER_ADDRESS3 varchar(50),
EMP_PER_CITY varchar(30),
EMP_PER_POSTALCODE varchar(20),
EMP_PER_TELEPHONE varchar(30),
EMP_PER_MOBILE	 varchar(20),
EMP_PER_FAX varchar(20),
EMP_PER_EMAIL varchar(50),
EMP_PER_COU_CODE varchar(6),
EMP_PER_PROVINCE_CODE varchar(6),
EMP_PER_DISTRICT_CODE varchar(6),
EMP_PER_ELECTORATE_CODE varchar(6),
constraint HR_EMPLOYEE_PC_PK primary key(EMP_NUMBER),
CONSTRAINT HR_EMPLOYEE_FK9 FOREIGN KEY (EMP_PER_COU_CODE) REFERENCES HR_COUNTRY(COU_CODE),
CONSTRAINT HR_EMPLOYEE_FK10 FOREIGN KEY (EMP_PER_PROVINCE_CODE ) REFERENCES HR_PROVINCE(PROVINCE_CODE),
CONSTRAINT HR_EMPLOYEE_FK11 FOREIGN KEY (EMP_PER_DISTRICT_CODE) REFERENCES HR_DISTRICT(DISTRICT_CODE),
CONSTRAINT HR_EMPLOYEE_FK12 FOREIGN KEY (EMP_PER_ELECTORATE_CODE) REFERENCES HR_ELECTORATE(ELECTORATE_CODE),
)
go
CREATE TABLE HR_EMPLOYEE_CDWD(
EMP_NUMBER varchar(6) references HR_EMPLOYEE(EMP_NUMBER),	
EMP_TEM_ADDRESS1 varchar(50),
EMP_TEM_ADDRESS2 varchar(50),
EMP_TEM_ADDRESS3 varchar(50),
EMP_TEM_CITY varchar(30),
EMP_TEM_POSTALCODE varchar(20),
EMP_TEM_TELEPHONE varchar(30),
EMP_TEM_MOBILE varchar(20),
EMP_TEM_FAX varchar(20),
EMP_TEM_EMAIL varchar(50),
EMP_TEM_COU_CODE varchar(6),
EMP_TEM_PROVINCE_CODE varchar(6),
EMP_TEM_DISTRICT_CODE varchar(6),
EMP_TEM_ELECTORATE_CODE varchar(6),
constraint HR_EMPLOYEE_CDWD_PK primary key(EMP_NUMBER),
CONSTRAINT HR_EMPLOYEE_FK13 FOREIGN KEY (EMP_TEM_COU_CODE) REFERENCES HR_COUNTRY(COU_CODE),
CONSTRAINT HR_EMPLOYEE_FK14 FOREIGN KEY (EMP_TEM_PROVINCE_CODE) REFERENCES HR_PROVINCE(PROVINCE_CODE),
CONSTRAINT HR_EMPLOYEE_FK15 FOREIGN KEY (EMP_TEM_DISTRICT_CODE) REFERENCES HR_DISTRICT(DISTRICT_CODE),
CONSTRAINT HR_EMPLOYEE_FK16 FOREIGN KEY (EMP_TEM_ELECTORATE_CODE) REFERENCES HR_ELECTORATE(ELECTORATE_CODE),
)
go
CREATE TABLE HR_EMPLOYEE_OC(
EMP_NUMBER varchar(6) references HR_EMPLOYEE(EMP_NUMBER),	
EMP_OFFICE_PHONE varchar(20),
EMP_OFFICE_EXTN varchar(10),
EMP_OFFICE_EMAIL varchar(50),
constraint HR_EMPLOYEE_OC_PK primary key(EMP_NUMBER)
)
go
CREATE TABLE HR_EMPLOYEE_EIM_WS(
EMP_NUMBER varchar(6) references HR_EMPLOYEE(EMP_NUMBER),	
EMP_PAYROLLNO varchar(20),
LOC_CODE varchar(6),
constraint HR_EMPLOYEE_EIM_WS_PK primary key(EMP_NUMBER),
CONSTRAINT HR_EMPLOYEE_FK8 FOREIGN KEY (LOC_CODE) REFERENCES HR_LOCATION(LOC_CODE),
);
go
create table HR_LANGUAGE
(
	LANG_CODE varchar(6),
	LANG_Name varchar(20),
	
	
	constraint HR_LANGUAGE_PK primary key(LANG_CODE),
);
go


create table HR_EMP_LANGUAGE
(
	EMP_NUMBER varchar(6),
	LANG_CODE varchar(6),
	ELANG_TYPE smallint, 
	RATING_GRADE_CODE varchar(6), --no fk 
	
	constraint HR_EMP_LANGUAGE_PK primary key(EMP_NUMBER,LANG_CODE,ELANG_TYPE),
	constraint HR_EMP_LANGUAGE_fk1 foreign key(EMP_NUMBER) references HR_EMPLOYEE(EMP_NUMBER),	
	constraint HR_EMP_LANGUAGE_fk2 foreign key(LANG_CODE) references HR_LANGUAGE(LANG_CODE),	
);
go
create table HR_EMP_EXTRA_ACTIVITY
(
	EMP_NUMBER varchar(6),
	EEXTACT_SEQNO int,
	EACAT_CODE varchar(6), --no fk 
	EATYPE_CODE varchar(6), --no fk 
	EEXTACT_ACHIEVEMENT varchar(200),
	
	constraint HR_EMP_EXTRA_ACTIVITY_PK primary key(EMP_NUMBER,EEXTACT_SEQNO),
	constraint HR_EMP_EXTRA_ACTIVITY_fk foreign key(EMP_NUMBER) references HR_EMPLOYEE(EMP_NUMBER),
)
go
create table HR_EMP_BASICSALARY
(
	EMP_NUMBER varchar(6),
	SAL_DTL_YEAR int, --no fk 
	SAL_GRD_CODE varchar(6),
	CURRENCY_ID varchar(6),
	EBSAL_BASIC_SALARY float,
	
	constraint HR_EMP_BASICSALARY_PK primary key(EMP_NUMBER,SAL_DTL_YEAR,SAL_GRD_CODE,CURRENCY_ID),
	constraint HR_EMP_BASICSALARY_fk foreign key(EMP_NUMBER) references HR_EMPLOYEE(EMP_NUMBER),
	constraint HR_EMP_BASICSALARY_fk2 foreign key(SAL_GRD_CODE) references HR_SALARY_GRADE(SAL_GRD_CODE),
	constraint HR_EMP_BASICSALARY_fk3 foreign key(CURRENCY_ID) references HR_CURRENCY(CURRENCY_ID)	
)
go
create table HR_EMP_REPORTTO
(
	EREP_SUP_EMP_NUMBER varchar(6),
	EREP_SUB_EMP_NUMBER varchar(6),
	EREP_REPORTING_MODE smallint,
	EREP_REPORTING_SEQUENCE int,
	
	constraint HR_EMP_REPORTTO_PK primary key(EREP_SUP_EMP_NUMBER,EREP_SUB_EMP_NUMBER,EREP_REPORTING_MODE),
	constraint HR_EMP_REPORTTO_fk foreign key(EREP_SUP_EMP_NUMBER) references HR_EMPLOYEE(EMP_NUMBER),
	constraint HR_EMP_REPORTTO_fk2 foreign key(EREP_SUB_EMP_NUMBER) references HR_EMPLOYEE(EMP_NUMBER)
	
)
go
create table HR_EMP_JOBSPEC
(
	EMP_NUMBER varchar(6),
	JDCAT_CODE varchar(6),
	EJOBSPEC_ATTRIBUTES varchar(500),
		
	constraint HR_EMP_JOBSPEC_PK primary key(EMP_NUMBER,JDCAT_CODE),
	constraint HR_EMP_JOBSPEC_fk foreign key(EMP_NUMBER) references HR_EMPLOYEE(EMP_NUMBER),
	constraint HR_EMP_JOBSPEC_fk2 foreign key(JDCAT_CODE) references HR_JD_CATEGORY(JDCAT_CODE)	
)

