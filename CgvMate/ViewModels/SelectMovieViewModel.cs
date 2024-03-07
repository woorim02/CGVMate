using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;

namespace CgvMate.ViewModels;

public partial class SelectMovieViewModel : ObservableObject
{
    private readonly CgvService service;

    public ObservableCollection<Movie> Movies { get; private set; } = new ObservableCollection<Movie>();

    public SelectMovieViewModel(CgvService service)
    {
        this.service = service;
    }

    public async Task LoadAsync()
    {
        foreach (var movie in await service.GetMoviesAsync()) 
        {
            Movies.Add(movie);
        }
    }

    [RelayCommand]
    public async Task SearchMovie(string keyword)
    {
        var result = await service.SearchMoviesAsync(keyword);
        Movies.Clear();
        if (result == null)
        {
            return;
        }
        foreach(var movie in result)
        {
            Movies.Add(movie);
        }
    }
}
