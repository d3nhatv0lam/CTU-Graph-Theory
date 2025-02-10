using Avalonia.Controls;
using Avalonia.ReactiveUI;
using CTU_Graph_Theory.ViewModels;
using ReactiveUI;
using System.Reactive.Disposables;

namespace CTU_Graph_Theory.Views;

public partial class MainView : ReactiveUserControl<MainViewModel>
{
    public MainView()
    {
        InitializeComponent();
        //this.WhenActivated(disposables =>
        //{
        //    this.Bind(ViewModel,x => x.CurrentViewModel, x => x.ContentView).DisposeWith(disposables);
        //});
    }
}
