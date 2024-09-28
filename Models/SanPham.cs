namespace QuocThinh.Models
{
    public class SanPham
    {
        public int ID { get; set; }
        public string Ten { get; set; }
        public string? Anh { get; set; }
        public int Gia { get; set; }
        public int SoLuong { get; set; }
        public string? MoTa { get; set; }
        public string? MauSac { get; set; }
        public string? ThongSo { get; set; }
        public int IDLoai { get; set; }
    }
}
