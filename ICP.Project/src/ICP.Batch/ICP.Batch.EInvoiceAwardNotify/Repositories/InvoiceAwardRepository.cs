using ICP.Infrastructure.Core.Extensions;
using ICP.Infrastructure.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ICP.Batch.EInvoiceAwardNotify.Models;
using ICP.Infrastructure.Abstractions.DbUtil;

namespace ICP.Batch.EInvoiceAwardNotify.Repositories
{
    public class InvoiceAwardRepository
    {
        public readonly IDbConnectionPool _dbConnectionPool = null;

        public InvoiceAwardRepository(
            IDbConnectionPool IdbConnectionPool
        )
        {
            _dbConnectionPool = IdbConnectionPool;
        }

        public BaseResult AddInvoiceAward(Invoice_Award model)
        {
            var db = _dbConnectionPool.Create(DatabaseName.ICP_Member);
            string sql = "EXEC ausp_Sch_Invoice_AddInvoiceAward_I";

            #region model
            var args = new
            {
                Type = model.Type,
                TMer_Identifier = model.TMer_Identifier,
                YearMonth = model.YearMonth,
                FileName = model.FileName,
                Main = model.Main,
                Total_Count = model.Total_Count,
                TotalPrize_Amount = model.TotalPrize_Amount,
                AwardBegin_Time = model.AwardBegin_Time,
                AwardEnd_Time = model.AwardEnd_Time,
                Download_Time = model.Download_Time
            };
            #endregion

            sql += db.GenerateParameter(args);

            return db.QuerySingleOrDefault<BaseResult>(sql, args);
        }

        public BaseResult AddInvoiceAwardDetail(Invoice_AwardDetail model)
        {
            var db = _dbConnectionPool.Create(DatabaseName.ICP_Member);
            string sql = "EXEC ausp_Sch_Invoice_AddInvoiceAwardDetail_I";

            #region model
            var args = new
            {
                model.Sys_ID, 
                model.Number_ID, 
                model.Number, 
                model.Mer_Name, 
                model.Mer_Identifier, 
                model.Mer_Address, 
                model.Invoice_Time, 
                model.Sales_Amount, 
                model.Carrier_Type, 
                model.Carrier_Name, 
                model.CarrierId_Clear, 
                model.CarrierId_Hide, 
                model.Random_Number, 
                model.Prize_Type, 
                model.Prize_Amount, 
                model.Customer_Identifier, 
                model.Platform_Deposit_Mark, 
                model.Exception_Code, 
                model.Print_Type, 
                model.Unique_Identifier, 
                model.Create_Time, 
                model.Notify_State, 
                model.Notify_Time
            };
            #endregion

            sql += db.GenerateParameter(args);
            
            return db.QuerySingleOrDefault<BaseResult>(sql,args);
        }

        public BaseResult InvoiceIssueAwardCheck(string createDate, string endDate, string yearMonth)
        {
            var db = _dbConnectionPool.Create(DatabaseName.ICP_Member);
            string sql ="EXEC ausp_EInvoice_InvoiceAwardDetail_Check_SU";

            var args = new
            {
                Create_Date=createDate,
                End_Date=endDate,
                YearMonth=yearMonth
            };
            return db.QuerySingleOrDefault<BaseResult>(sql,args);
        }
    }
}