﻿namespace CgvMate.Data;

#pragma warning disable CS8618 // 생성자를 종료할 때 null을 허용하지 않는 필드에 null이 아닌 값을 포함해야 합니다. null 허용으로 선언해 보세요.
public class TheaterGiveawayInfo
{
    public string GiveawayIndex { get; set; }
    public string TheaterCode { get; init; }
    public string TheaterName { get; init; }
    public string CountTypeCode { get; init; }
    public string EncCount { get; init; }
    public string GiveawayName { get; init; }
    public string GiveawayRemainCount { get; set; }
    public string ReceiveTypeCode { get; init; }

	/// <summary>
	/// 빈 생성자를 직접 호출하지 마세요. json 직렬화 또는 ef core에서 사용하는 클래스입니다.
	/// </summary>
	public TheaterGiveawayInfo()
	{

	}
}
