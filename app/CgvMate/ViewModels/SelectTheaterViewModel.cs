using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;

namespace CgvMate.ViewModels;

public partial class SelectTheaterViewModel : ObservableObject
{
    private readonly CgvService service;

    public ObservableCollection<Area> Areas { get; set; } = new ObservableCollection<Area>();
    public ObservableCollection<Theater> Theaters { get; set; } = new ObservableCollection<Theater>();

    public SelectTheaterViewModel(CgvService service)
    {
        this.service = service;
    }

    public async Task LoadAsync()
    {
        foreach(Area area in await service.GetAreasAsync())
        {
            Areas.Add(area);
        }
        foreach (Theater theater in await service.GetTheatersAsync(Areas[0].AreaCode))
        {
            Theaters.Add(theater);
        }
    }

    [RelayCommand]
    private async Task SelectArea(string areaCode)
    {
        Theaters.Clear();
        foreach (Theater theater in await service.GetTheatersAsync(areaCode))
        {
            Theaters.Add(theater);
        }
    }
}
