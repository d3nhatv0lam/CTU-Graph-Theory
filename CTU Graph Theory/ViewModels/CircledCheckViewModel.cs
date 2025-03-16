using CTU_Graph_Theory.Algorithms.Base;
using CTU_Graph_Theory.Interfaces;
using CTU_Graph_Theory.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CTU_Graph_Theory.Algorithms;
using ReactiveUI;

namespace CTU_Graph_Theory.ViewModels
{
    public class CircledCheckViewModel :ViewModelBase, IAlgorithmViewModel , IAllVertexRun
    {
        private CircledCheck _circledCheck;
        private string _isHasCricleString;
        public string AlgorithmName
        {
            get => _circledCheck.AlgorithmName;
        }




        public ObservableCollection<StringPseudoCode> Pseudocodes
        {
            get => _circledCheck.Pseudocodes;
        }
        public bool IsSetCompletedAlgorithm { get; set; }

        public string IsHasCricleString
        {
            get => _isHasCricleString;
            set => this.RaiseAndSetIfChanged(ref _isHasCricleString, value);
        }

        public CircledCheckViewModel()
        {
            _circledCheck = new CircledCheck();
            IsSetCompletedAlgorithm = false;
            _circledCheck.ReturnIsCricleCheck += _circledCheck_OnReturnIsCircleCheck;
        }

        private void _circledCheck_OnReturnIsCircleCheck(object? sender, bool? e)
        {
            IsHasCricleString = (e == null) ? "": (e == true) ? "Đồ thị có chu trình" : "Đồ thị không có chu trình";
        }

        public void RunAlgorithm(CustomGraph graph, Vertex startVertex)
        {
            _circledCheck.RunAlgorithm(graph, startVertex);
        }

        public void RunAlgorithmWithAllVertex(CustomGraph graph, ObservableCollection<Vertex> vertices)
        {
            _circledCheck.RunAlgorithmWithAllVertex(graph, vertices);
        }

        public void PauseAlgorithm()
        {
            _circledCheck.PauseAlgorithm();
        }
        public void ContinueAlgorithm(CustomGraph graph)
        {
            _circledCheck.ContinueAlgorithm(graph);
        }

        public void ContinueAlgorithmWithAllVertex(CustomGraph graph)
        {
            _circledCheck.ContinueAlgorithmWithAllVertex(graph);
        }

        public void StopAlgorithm(CustomGraph graph)
        {
            _circledCheck.StopAlgorithm(graph);
        }
        public void SetRunSpeed(int speedUp)
        {
            _circledCheck.SetRunSpeed(speedUp);
        }
        public void SetCompletedAlgorithm(EventHandler returnIsRunningState)
        {
            _circledCheck.CompletedAlgorithm += returnIsRunningState;
            IsSetCompletedAlgorithm = true;
        }
    }
}
