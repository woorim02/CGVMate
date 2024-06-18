using CgvMate.Data.Entities.LotteCinema;

namespace CgvMate.Services.DTOs.LotteCinema;

internal class CinemaGoodsResDTO
{
    public List<CinemaDivision> CinemaDivisions { get; set; }
    public List<CinemaDivisionGood> CinemaDivisionGoods { get; set; }
    public string IsOK { get; set; }
    public string ResultMessage { get; set; }
    public object ResultCode { get; set; }
    public object EventResultYn { get; set; }
}
