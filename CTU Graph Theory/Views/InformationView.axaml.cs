using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using CTU_Graph_Theory.ViewModels;

namespace CTU_Graph_Theory.Views;

public partial class InformationView : ReactiveUserControl<InformationViewModel>
{
    public InformationView()
    {
        InitializeComponent();
    }
}