﻿using backend_v3.Models;
using Ecom.Dto.VanHanh;

namespace Ecom.Interfaces
{
    public interface IDonHangService
    {
        public Task<PaginatedList<DonHangDto>> GetAll(DonHangDto request);
    }
}
