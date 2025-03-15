using CTU_Graph_Theory.Algorithms;
using CTU_Graph_Theory.Interfaces;
using CTU_Graph_Theory.Models;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CTU_Graph_Theory.ViewModels
{
    public class KruskalViewModel : ViewModelBase, IAlgorithmViewModel, IAlgorithmRequirementViewModel, INonStartVertexRun
    {
        private Kruskal _kruskal;
        private Int64? _minWeight;
        public Int64? MinWeight
        {
            get => _minWeight;
            set => this.RaiseAndSetIfChanged(ref _minWeight, value);
        }
        public string AlgorithmName => _kruskal.AlgorithmName;

        public bool IsSetCompletedAlgorithm { get; set; }

        public ObservableCollection<StringPseudoCode> Pseudocodes => _kruskal.Pseudocodes;

        public List<RequestOfAlgorithm> Requirements => _kruskal.Requirements;


        public KruskalViewModel()
        {
            _kruskal = new Kruskal();
            IsSetCompletedAlgorithm = false;
            _kruskal.UpdateMinWeight += _kruskal_UpdateMinWeight;
        }

        private void _kruskal_UpdateMinWeight(object? sender, long e)
        {
            MinWeight = e;
        }

        public bool CheckRequirements(CustomGraph graph)
        {
            return _kruskal.CheckRequirements(graph);
        }

        public void ContinueAlgorithm(CustomGraph graph)
        {
            _kruskal.ContinueAlgorithm(graph);
        }

        public void PauseAlgorithm()
        {
            _kruskal.PauseAlgorithm();
        }

        public void RunAlgorithm(CustomGraph graph)
        {
            _kruskal.RunAlgorithm(graph);
        }

        public void SetCompletedAlgorithm(EventHandler returnIsRunningState)
        {
            if (!IsSetCompletedAlgorithm)
            {
                _kruskal.CompletedAlgorithm += returnIsRunningState;
                IsSetCompletedAlgorithm = true;
            }
        }

        public void SetRunSpeed(int speedUp)
        {
            _kruskal.SetRunSpeed(speedUp);
        }

        public void StopAlgorithm(CustomGraph graph)
        {
            _kruskal.StopAlgorithm(graph);
        }


    }
}
