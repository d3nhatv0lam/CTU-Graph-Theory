using CTU_Graph_Theory.Models;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reactive.Linq;

namespace CTU_Graph_Theory.ViewModels;

public class MainViewModel : ViewModelBase
{
    private List<ViewModelBase> _pagesViewModel;

    public List<ViewModelBase> PagesViewModel
    {
        get
        {
            if (_pagesViewModel == null) _pagesViewModel = new List<ViewModelBase>();
            return _pagesViewModel;
        }
    }

    private ViewModelBase _currentViewModel;

    public ViewModelBase CurrentViewModel
    {
        get => _currentViewModel;
        set => this.RaiseAndSetIfChanged(ref _currentViewModel, value);
    }

    private List<ViewLocatorNode> _viewLocatorNodes = new List<ViewLocatorNode>();

    public List<ViewLocatorNode> ViewLocatorNodes
    {
        get => _viewLocatorNodes;
    }

    private ViewLocatorNode _selectedNode;

    public ViewLocatorNode SelectedNode
    {
        get => _selectedNode;
        set => this.RaiseAndSetIfChanged(ref _selectedNode, value);
    }

    public MainViewModel()
    {
        PagesViewModelInit();
        ViewLocatorNodeInit();

        ObservableInit();

        CurrentViewModel = PagesViewModel.First();
    }

    private void PagesViewModelInit()
    {
        PagesViewModel.Add(new InformationViewModel());
        PagesViewModel.Add(new VisualizationGraphViewModel());
    }

    private void ViewLocatorNodeInit()
    {
        int i = 0;
        ViewLocatorNodes.Add(new ViewLocatorNode(i++, Material.Icons.MaterialIconKind.InformationVariantBoxOutline, "Thông tin sản phẩm"));
        ViewLocatorNodes.Add(new ViewLocatorNode(i++, Material.Icons.MaterialIconKind.ChartTimelineVariantShimmer, "Trực quan đồ thị"));
    }

    private void ObservableInit()
    {
        this.WhenAnyValue(x => x.SelectedNode).Subscribe(node => ChangeView(node));
    }

    private void ChangeView(ViewLocatorNode node)
    {
        if (node == null) return;
        if (node.Id < 0 || node.Id >= PagesViewModel.Count) return;

        CurrentViewModel = PagesViewModel[node.Id];
    }




}
