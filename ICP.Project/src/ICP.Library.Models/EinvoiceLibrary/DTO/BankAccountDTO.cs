namespace ICP.Library.Models.EinvoiceLibrary.DTO
{
    public class BankAccountDTO : BaseDTO
    {
        public string AccountNo { get; set; }

        public string BankNo { get; set; }

        public string CardEncrypt { get; set; }

        public string CardNo { get; set; }

        public string RocID { get; set; }

        public string UserIdType { get; set; }

        public string WinnerName { get; set; }

        public string WinnerPhone { get; set; }
    }
}
