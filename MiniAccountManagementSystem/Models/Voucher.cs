namespace MiniAccountManagementSystem.Models
{
    public class Voucher
    {
        public int VoucherId { get; set; }
        public string VoucherType { get; set; }
        public DateTime Date { get; set; }
        public string ReferenceNo { get; set; }
        public List<VoucherEntry> Entries { get; set; }
    }

}
