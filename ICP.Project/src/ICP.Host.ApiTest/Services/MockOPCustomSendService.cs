using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace ICP.Host.ApiTest.Services
{
    using Library.Models.OpenWalletApi.CustomSendApi;
    using Infrastructure.Core.Models;
    using Library.Repositories.OpenWalletApi;
    using Models;

    public class MockOPCustomSendService
    {
        OPCustomApiRepository _oPCustomApiRepository;

        public MockOPCustomSendService(
            OPCustomApiRepository oPCustomApiRepository
            )
        {
            _oPCustomApiRepository = oPCustomApiRepository;
        }

        public BaseResult BindicashAccount(string json)
        {
            var model = JsonConvert.DeserializeObject<MockOPCustomSendResult<BindicashAccountRequest>>(json);

            return _oPCustomApiRepository.BindicashAccount(model.request, model.MID);
        }

        public BaseResult UnBindicashAccount(string json)
        {
            var model = JsonConvert.DeserializeObject<MockOPCustomSendResult<UnBindicashAccountRequest>>(json);

            return _oPCustomApiRepository.UnBindicashAccount(model.request, model.MID);
        }
    }
}