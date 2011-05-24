using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Office.Excel.Server.Udf;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;
using System.Web;
namespace CLIF.Solutions.Code
{
    /**********************************************************************************
     Title:              ExcelUserDefinedFunctions
     Author:             Suresh Thampi
     Project:            CLIF.Solutions.Code
     Date:               04/03/2011
     Copyright:          © Center for e-research(CeRch), King's College London 2010 All Rights Reserved.
     Description:                                     
     ------------------------------------------------------------------------------
     Revision History
     Date             Ref       Author          Reason          Remarks        

     ***********************************************************************************/


    [UdfClass]
    public class ExcelUserDefinedFunctions
    {
        [UdfMethod]
        public double MyDouble(double d)
        {
            return d * 9;
        }
       
        [UdfMethod(IsVolatile = true)]
        public DateTime ReturnDateTimeToday()
        {
            return (DateTime.Today);
        }

        [UdfMethod]
        public string ShowText()
        {
            return "Hello";
        }       
    }
}
