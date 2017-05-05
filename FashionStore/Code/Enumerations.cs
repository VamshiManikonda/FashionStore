using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace FashionStore.Code
{
    [Flags]
    public enum UserType : int
    {
        [Description("CRM User")]
        CRM = 0,
        [Description("Domain User")]
        Domain = 1
    }
    public enum GenericCompareOperator
    {
        GreaterThan,
        GreaterThanOrEqual,
        LessThan,
        LessThanOrEqual
    }
    [Flags]
    public enum ProductType
    {
        [Description("Men's wear")]
        M,
        [Description("Women's wear")]
        W
    }

    [Flags]
    public enum ReportFilter
    {
        [Description("With IMEI")]
        WI,
        [Description("Without IMEI")]
        WOI
    }

    [Flags]
    public enum Reports : int
    {
        [Description("Summary")]
        Summary = 0,
        [Description("Cost")]
        Cost = 1,
        [Description("Revenue")]
        Revenue = 2
    }

    [Flags]
    public enum SearchKeys : int
    {
        [Description("QID")]
        QId = 1,
        [Description("Customer Ref")]
        CustomerRef = 2,
        [Description("Account Number")]
        AccountNum = 3,
        [Description("Service Identifier")]
        ServiceIdentifier = 4,
        [Description("Passport")]
        Passport = 5,
        [Description("GCC ID")]
        GCCID = 6,
        [Description("CRN")]
        CRN = 7,
    }

    [Flags]
    public enum CustomerSegments
    {
        [Description("Person")]
        PERSON,
        [Description("Organization")]
        ORGANIZATION
    }

    [Flags]
    public enum Statements
    {
        [Description("By Paper")]
        P,
        [Description("By Email")]
        E
    }

    [Flags]
    public enum BillTypes:int
    {

        [Description("Aggregate Bill")]
        Aggregate =1,
        [Description("Summary Bill")]
        Summary =2,
        [Description("Detail Bill")]
        Detail =4,
    }

    [Flags]
    public enum Languages : int
    {
        [Description("English(UK)")]
        English = 1,
        [Description("Arabic(Qatar)")]
        Arabic = 100
    }

    [Flags]
    public enum Permissions : int
    {
        [Description("Default")]
        Default = 1,
        [Description("Ex-directory")]
        Exdirectory = 2
    }

    [Flags]
    public enum FiberScenarios
    { 
        [Description("New")]
        N,
        [Description("Mozaic to OTV")]
        M,
        [Description("Change")]
        C,
        [Description("Terminate")]
        T,
        [Description("Shifting")]
        S,
        [Description("Re Provision")]
        R
    }
    [Flags]
    public enum FiberLineTypes
    {
        [Description("BWY")]
        BWY,
        [Description("OCB")]
        OCB,
        [Description("ICB")]
        ICB,
         
    }

    [Flags]
    public enum ShahryMiscellaneous:int
    {
        [Description("Sim Replacement")]
        SimReplacement =1,
        [Description("Plan Change")]
        PlanChange = 2,
        [Description("IR Key")]
        IRKey = 3,
        [Description("Access Change")]
        AccessChange = 4,
        [Description("Mobile Internet Pack")]
        Mip = 5,
        [Description("Shahry Promotions")]
        ShahryPromotions = 6,
        [Description("Ooredoo Passport")]
        OoredooPassport = 7
    }

    [Flags]
    public enum LandlineMiscellaneous : int
    {
        [Description("Direct Exchange Line Change Order")]
        DELChangeOrder = 1,
        [Description("IPTV Package Change Order")]
        IPTVChangeOrder = 2,
        [Description("ADSL/IPTV Speed Change")]
        SpeedChange = 3
    }

    [Flags]
    public enum ProductTransTypes : int
    {
        [Description("Product Terminate")]
        ProductTerminate = 1,
        [Description("Product Takeover")]
        ProductTakeover = 2,
        [Description("Bar/Unbar/Suspend/Resume Service")]
        BUSRService = 3,
        [Description("Service Identifier Change")]
        ServiceIdentifierChange = 4
    }
}