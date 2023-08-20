﻿using DataAccessLayer.Common.Models;

namespace CarRentalSystemAPI.Dtos
{
    public class DriverRequestDto : PagingModel<DriverDto>
    {
        public string? columnName { get; set; }

        public string? searchTerm { get; set; }

        public string? sortBy { get; set; }

        public bool isAscending { get; set; }
    }
}
