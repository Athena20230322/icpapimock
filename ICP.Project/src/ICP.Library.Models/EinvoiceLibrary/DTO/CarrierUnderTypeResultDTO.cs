namespace ICP.Library.Models.EinvoiceLibrary.DTO
{
    public class CarrierUnderTypeResultDTO : BaseResultDTO
    {
        public string CardType { get; set; }

        public string CardNo { get; set; }

        public Carrier[] Carriers { get; set; }
    }

    public class Carrier
    {
        public string CarrierType { get; set; }

        public string CarrierId2 { get; set; }

        public string CarrierName { get; set; }
    }
}
