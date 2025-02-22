using ReactiveUI;

namespace CTU_Graph_Theory.ViewModels;

public class ViewModelBase : ReactiveObject
{
    public string Name { get; protected set; }
}
