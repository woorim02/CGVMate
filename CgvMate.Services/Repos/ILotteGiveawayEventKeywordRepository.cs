using CgvMate.Data.Entities.LotteCinema;

namespace CgvMate.Services.Repos;

public interface ILotteGiveawayEventKeywordRepository
{
    // 새로운 이벤트 키워드 추가
    Task AddKeywordAsync(string keyword);

    // 이벤트 키워드를 키워드로 삭제
    Task DeleteKeywordAsync(string keyword);

    // 모든 이벤트 키워드 조회
    Task<IEnumerable<string>> GetAllKeywordsAsync();

    Task<bool> Exists(string keyword);
}
