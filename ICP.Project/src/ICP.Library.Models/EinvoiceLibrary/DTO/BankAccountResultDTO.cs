namespace ICP.Library.Models.EinvoiceLibrary.DTO
{
    public class BankAccountResultDTO : BaseResultDTO
    {
        public string CardType { get; set; }

        public string CardNo { get; set; }

        public string EnableRemit { get; set; }

        public string UpdateAcc { get; set; }

        public string BankNo { get; set; }

        public string AccountNo { get; set; }

        public string RocID { get; set; }

        public string WinnerName { get; set; }

        public string WinnerPhone { get; set; }

        public string UserIdType { get; set; }
    }
}
