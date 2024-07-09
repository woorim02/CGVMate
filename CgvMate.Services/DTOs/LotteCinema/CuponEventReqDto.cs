using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CgvMate.Services.DTOs.LotteCinema;

internal class CuponEventReqDto : ReqDTOBase
{
    public string EventID { get; set; }
    public string MainEventID { get; set; }
}
