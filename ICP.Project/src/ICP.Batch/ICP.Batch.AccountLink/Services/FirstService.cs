using ICP.Batch.AccountLink.Models.First;
using ICP.Batch.AccountLink.Repositories;
using ICP.Infrastructure.Abstractions.Logging;
using ICP.Infrastructure.Core.Extensions;
using ICP.Infrastructure.Core.Models;
using ICP.Library.Models.AccountLinkApi.Enums;

namespace ICP.Batch.AccountLink.Services
{
    public class FirstService : BaseService
    {
        public readonly string filePattern = "AccountLink_For_{0}-(^0000|0001|9999|[0-9]{4})+(0[1-9]|1[0-2])+(0[1-9]|[12][0-9]|3[01])$"; //檔案格式(AccountLink_For_0006-YYYYMMDD.txt)
        public readonly int[] arrColLength = { 20, 14, 4, 20, 14, 3, 1, 24, 40, 10, 3, 1, 16 }; //每筆記錄欄位的長度陣列
        public readonly string fileExtension = "txt";
        private readonly FirstRepository _firstRepository = null;

        public FirstService(
          ILogger<FirstService> logger,
          FirstRepository firstRepository,
          ACLinkRepository acLinkRepository
          )
        {
            _bankType = BankType.First;
            _logger = logger;
            _firstRepository = firstRepository;
            _acLinkRepository = acLinkRepository;
        }

        /// <summary>
        /// 建立儲存明細資料物件
        /// </summary>
        /// <param name="oBankSource">來源資料</param>
        /// <returns></returns>
        public DataResult<TradeDbReq> BuildRecordRequest(TradeModel oBankSource)
        {
            var result = new DataResult<TradeDbReq>();
            result.SetError();

            TradeDbReq tradeReq = new TradeDbReq();
            tradeReq = CopyData(oBankSource, tradeReq);

            result.SetSuccess(tradeReq);
            return result;
        }

        /// <summary>
        /// 記錄明細資料
        /// </summary>
        /// <param name="reqModel">記錄資料請求</param>
        /// <returns></returns>
        public BaseResult CreateRecord(TradeDbReq reqModel)
        {
            BaseResult result = new BaseResult();
            result.SetError();
            result = _firstRepository.AddFirstBatchLog(reqModel);

            return result;
        }
    }
}
