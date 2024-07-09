using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CgvMate.Services.DTOs.LotteCinema;
public class Item
{
    public string EventID { get; set; }
    public int NewSeq { get; set; }
    public string Seq { get; set; }
    public string DcTcktConfigID { get; set; }
    public string CpnCurYn { get; set; }
    public string CpnUsableMaxCnt { get; set; }
    public string CpnUsedCnt { get; set; }
    public string MovieTrailerUrl { get; set; }
    public string MovieDetailUrl { get; set; }
    public string MovieBookUrl { get; set; }
    public string Img1Url { get; set; }
    public string Img1Alt { get; set; }
    public string Img2Url { get; set; }
    public string Img2Alt { get; set; }
    public string Img3Url { get; set; }
    public string Img3Alt { get; set; }
    public string Img4Url { get; set; }
    public string Img4Alt { get; set; }
    public string Img5Url { get; set; }
    public string Img5Alt { get; set; }
    public string MovieCd { get; set; }
    public string MovieNm { get; set; }
    public string MovieImgUrl { get; set; }
    public string MovieImgAlt { get; set; }
    public string EventContents { get; set; }
    public string ProgressStartDate { get; set; }
    public string ProgressEndDate { get; set; }
    public string ProgressStartTime { get; set; }
    public object TimerYN { get; set; }
    public object TimerOnOff { get; set; }
    public string DisplayCouponName { get; set; }
    public string SpeedCpnDownCnt { get; set; }
    public string CpnDownloadMaxCnt { get; set; }
    public int InformationOfferingAgreementYN { get; set; }
    public string InformationOfferingAgreementContents { get; set; }
    public List<object> Items { get; set; }
}

public class ItemGroup
{
    public string EventID { get; set; }
    public int NewSeq { get; set; }
    public object MovieCd { get; set; }
    public object MovieNm { get; set; }
    public object ProgressStartDate { get; set; }
    public object ProgressEndDate { get; set; }
    public object ProgressStartTime { get; set; }
    public object TimerOnOff { get; set; }
    public List<Item> Items { get; set; }
}

public class CuponEventResDto
{
    public List<SpeedEventDetail> SpeedEventDetail { get; set; }
    public string IsOK { get; set; }
    public string ResultMessage { get; set; }
    public object ResultCode { get; set; }
    public object EventResultYn { get; set; }
}

public class SpeedEventDetail
{
    public List<object> ButtonSetting { get; set; }
    public object Items { get; set; }
    public List<ItemGroup> ItemGroup { get; set; }
    public string ConfigDownOrUseCd { get; set; }
    public string EventClassificationCode { get; set; }
    public string EventID { get; set; }
    public string EventName { get; set; }
    public string ProgressStartDate { get; set; }
    public string ProgressEndDate { get; set; }
    public string WinnerAnnouncmentDate { get; set; }
    public string ImgUrl { get; set; }
    public string ImgAlt { get; set; }
    public int ImageDivisionCode { get; set; }
    public object ImageGameTypeDivisionCode { get; set; }
    public string EventContents { get; set; }
    public string EventNotice { get; set; }
    public object WinnerNotice { get; set; }
    public string CinemaID { get; set; }
    public string CinemaName { get; set; }
    public int EventProgressDivisionCode { get; set; }
    public string EventMovieURL { get; set; }
    public string EventMovieImageURL { get; set; }
    public string EventMovieImageAlt { get; set; }
    public string ListImgUrl { get; set; }
    public string ListImgAlt { get; set; }
    public object GoodsShowYN { get; set; }
    public int InformationOfferingAgreementYN { get; set; }
    public string InformationOfferingAgreementContents { get; set; }
}

