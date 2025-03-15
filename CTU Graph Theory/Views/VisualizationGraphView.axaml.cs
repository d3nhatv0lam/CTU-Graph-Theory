using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using Avalonia.ReactiveUI;
using Avalonia.Threading;
using AvaloniaGraphControl;
using CTU_Graph_Theory.Models;
using CTU_Graph_Theory.ViewModels;
using ReactiveUI;
using System.ComponentModel;
using System.Diagnostics;
using System.Reactive.Disposables;

namespace CTU_Graph_Theory.Views;

public partial class VisualizationGraphView : ReactiveUserControl<VisualizationGraphViewModel>
{


    public VisualizationGraphView()
    {
        InitializeComponent();

        //this.WhenActivated(disposables =>
        //{
        //    this.Bind(ViewModel, viewModel => viewModel.MainGraph, x => x.MainGraph.Graph).DisposeWith(disposables);
        //});
    }

    // redraw Connection
    // idk why it can't auto redraw?
    private void Connection_PropertyChanged(object? sender, AvaloniaPropertyChangedEventArgs e)
    {
        if (sender is Connection edgeLine)
        {
            ((VisualizationGraphViewModel)this.DataContext)?.UpdateEdgeColor(edgeLine);
        }
    }

    private void TextBlock_PropertyChanged(object? sender, AvaloniaPropertyChangedEventArgs e)
    {
        if (sender is TextBlock textBlock && e.Property.ToString() == nameof(textBlock.Background))
        {
            textBlock.InvalidateMeasure();
            textBlock.InvalidateArrange();
            textBlock.BringIntoView();
        }

    }


}