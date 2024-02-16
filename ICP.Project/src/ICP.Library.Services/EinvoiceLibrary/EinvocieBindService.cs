using System;
using System.Text;
using ICP.Infrastructure.Core.Extensions;
using ICP.Infrastructure.Core.Models;
using ICP.Library.Models.EinvoiceLibrary;
using ICP.Library.Repositories.EinvoiceLibrary;
using ICP.Library.Repositories.SystemRepositories;
using Newtonsoft.Json;

namespace ICP.Library.Services.EinvoiceLibrary
{
    public class EinvocieBindService
    {
        private readonly ConfigKeyValueRepository _configKeyValueRepository;
        private EinvoiceRepository _einvoiceRepository;

        public EinvocieBindService(
            ConfigKeyValueRepository configKeyValueRepository, 
            EinvoiceRepository einvoiceRepository)
        {
            _configKeyValueRepository = configKeyValueRepository;
            _einvoiceRepository = einvoiceRepository;
        }

        /// <summary>
        /// 取得電子發票會員載具
        /// </summary>
        /// <param name="MID"></param>
        /// <returns></returns>
        public GetEInvoiceCarrierInfoResultType GetEInvoiceCarrierInfo(long MID)
        {
            return _einvoiceRepository.GetEInvoiceCarrierInfoByICP(MID);
        }

        /// <summary>
        /// 驗證 發票是否能歸戶 並取得token
        /// </summary>
        /// <param name="mid"></param>
        /// <param name="carryNum"></param>
        /// <returns></returns>
        public InvoiceBindReturn InvoiceBindProcess(long mid, string carryNum)
        {
            InvoiceBindReturn RtnModel = new InvoiceBindReturn();
            string token = Guid.NewGuid().ToString();

            RtnModel = _einvoiceRepository.GetInvoiceBindToken(mid, carryNum, token);

            if (RtnModel.RtnCode == 1)
            {
                RtnModel.card_ban = _configKeyValueRepository.Einvoice_ICPIdentifier;
                RtnModel.card_type = _configKeyValueRepository.Einvoice_CardType.ToBase64();
                RtnModel.card_no1 = carryNum.ToBase64();
                RtnModel.card_no2 = RtnModel.card_no1;
                RtnModel.token = token;
                RtnModel.back_url = _configKeyValueRepository.Einvoice_Url_BindCallBack;
                RtnModel.postUrl = _configKeyValueRepository.Einvoice_Url_APMEMBERVAN;
            }

            return RtnModel;
        }

    }
}