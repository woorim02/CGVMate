using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CgvMate.Services.DTOs.Megabox
{
    internal class EventReqDTO
    {
        public string currentPage { get; set; }
        public string eventDivCd { get; set; }
        public string eventStatCd { get; set; }
        public string eventTitle { get; set; }
        public string eventTyCd { get; set; }
        public string orderReqCd { get; set; }
        public string recordCountPerPage { get; set; }
        public int totCnt = 1;
    }
}
