namespace Ecom.Dto
{
    public class ChuongTrinhMarDto
    {
        public Guid id { get; set; }
        public string ten { get; set; }
        /// <summary>
        /// 1: khuyến mãi
        /// 2: flash sale
        /// 3: mã giảm giá
        /// </summary>
        public int cong_cu { get; set; }
        public string cong_cu_string => GetCongCuString(cong_cu);

        private static string GetCongCuString(int cong_cu)
        {
            return cong_cu switch
            {
                1 => "Khuyến mãi",
                2 => "Flash Sale",
                3 => "Mã giảm giá",
                _ => "Không xác định"
            };
        }

    }
}
