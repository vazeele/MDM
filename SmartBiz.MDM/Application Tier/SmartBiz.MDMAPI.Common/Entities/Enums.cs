using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartBiz.MDMAPI.Common.Entities
{
    public enum TransactionTypesEnum
    {
        Manual = 1,
        Subsystem =2

    }
    public enum CreditDebitFlagEnum
    {
        Credit =1,
        Debit =2
    }
    public enum HRCASHBENEFIT_STATUSFLAG:short
    {
        Inactive =0,
        Active =1
    }
    public enum YesNoEnum
    {
        Yes= 1,
        No =2
    }

    public enum LanguageEnum
    {
        Writing = 1,
        Speaking = 2,
        Reading =3
    }
}
