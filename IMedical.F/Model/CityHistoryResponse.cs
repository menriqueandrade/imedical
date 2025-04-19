using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using IMedicalB.Dto;

namespace IMedical.F.Model
{
    public class CityHistoryResponse
    {
        public List<CityHistoryDto> History { get; set; }
    }
}