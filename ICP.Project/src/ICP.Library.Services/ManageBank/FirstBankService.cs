using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Library.Services.ManageBank
{
    using Infrastructure.Core.Models;
    using Library.Models.ManageBank.FirstBank;
    using Library.Repositories.PaymentCenter;
    using Library.Repositories.SystemRepositories;
    using Library.Repositories.ManageBank;
    using Library.Repositories.MemberRepositories;
    using ICP.Infrastructure.Core.Extensions;
    using ICP.Library.Models.Payment;
    using ICP.Library.Repositories.Payment;

    public class FirstBankService
    {
        FirstBankRepository _firstBankRepository;
        CertificateRepository _certificateRepository;
        ConfigKeyValueRepository _configKeyValueRepository;
        MemberConfigRepository _memberConfigRepository;
        PaymentRepository _paymentRepository;

        public FirstBankService(
            FirstBankRepository firstBankRepository,
            CertificateRepository certificateRepository,
            ConfigKeyValueRepository configKeyValueRepository,
            MemberConfigRepository memberConfigRepository,
            PaymentRepository paymentRepository
            )
        {
            _firstBankRepository = firstBankRepository;
            _certificateRepository = certificateRepository;
            _configKeyValueRepository = configKeyValueRepository;
            _memberConfigRepository = memberConfigRepository;
            _paymentRepository = paymentRepository;
        }

        string _fxml_publicKey;
        string fxml_publicKey
        {
            get
            {
                if (_fxml_publicKey == null)
                {
                    var getPublicKeyResult = _certificateRepository.GetPublicKey("managebank_firstbank_fxml");
                    if (!getPublicKeyResult.IsSuccess)
                    {
                        _fxml_publicKey = string.Empty + getPublicKeyResult.RtnData;
                    }
                }
                return _fxml_publicKey;
            }
        }

        private bool _inited;
        private void init_firstBankRepository()
        {
            if (_inited) return;
            _firstBankRepository.fxml_publicKey = fxml_publicKey;
            _firstBankRepository.fxml_url = _configKeyValueRepository.ManageBank_FirstBank_FXML_Url;
            _firstBankRepository.setting = _paymentRepository.GetBankTransferSetting();
            _inited = true;
        }

        /// <summary>
        /// B2B001單/多筆付款資料訊息傳送
        /// </summary>
        /// <param name="model"></param>
        /// <param name="TransferID"></param>
        /// <returns></returns>
        public DataResult<XML<TxResult<B2BResult>>> Exec_B2B001(B2B001 model, long TransferID)
        {
            if (!_memberConfigRepository.ProductMode)
            {
                var result = new DataResult<XML<TxResult<B2BResult>>>();
                result.SetSuccess(new XML<TxResult<B2BResult>>
                {
                    Tx = new TxResult<B2BResult>
                    {
                        TxHeader = new TxHeaderModel
                        {
                            TxID = "B2B001",
                            SvcRqId = string.Empty,
                            CustId = string.Empty,
                            MsgDirection = "RS"
                        },
                        TxRs = new B2BResult
                        {
                            StatusCode = "00",
                            StatusDesc = "TEST"
                        }
                    }
                });
                var tx = result.RtnData.Tx;
                _firstBankRepository.AddFXMLLog(TransferID, tx.TxHeader, string.Empty, tx.TxRs);
                return result;
            }

            init_firstBankRepository();
            return _firstBankRepository.CallApi<XML<TxResult<B2BResult>>, B2B001>(model, TransferID: TransferID);
        }

        /// <summary>
        /// B2B002單/多筆付款處理狀態查詢資料訊息傳送
        /// </summary>
        /// <param name="model"></param>
        /// <param name="TransferID"></param>
        /// <returns></returns>
        public DataResult<XML<TxResult<B2B002Result>>> Exec_B2B002(B2B002 model, long TransferID)
        {
            if (!_memberConfigRepository.ProductMode)
            {
                var result = new DataResult<XML<TxResult<B2B002Result>>>();
                result.SetSuccess(new XML<TxResult<B2B002Result>>
                {
                    Tx = new TxResult<B2B002Result>
                    {
                        TxHeader = new TxHeaderModel
                        {
                            TxID = "B2B002",
                            SvcRqId = string.Empty,
                            CustId = string.Empty,
                            MsgDirection = "RS"
                        },
                        TxRs = new B2B002Result
                        {
                             StatusCode = "20",
                             StatusDesc = "TEST"
                        }
                    }
                });
                var tx = result.RtnData.Tx;
                _firstBankRepository.AddFXMLLog(TransferID, tx.TxHeader, string.Empty, tx.TxRs, RQSvcRqID: model.SvcRqID);
                return result;
            }

            init_firstBankRepository();
            return _firstBankRepository.CallApi<XML<TxResult<B2B002Result>>, B2B002>(model, TransferID: TransferID, RQSvcRqID: model.SvcRqID);
        }
    }
}
