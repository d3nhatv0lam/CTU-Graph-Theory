using CTU_Graph_Theory.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CTU_Graph_Theory.Algorithms.Base
{
    public class AlgorithmBase
    {
        // base
        private Vertex _startVetex = null;
        private int _baseSpeed = 2100;
        private int _fastSpeedPerLevel = 400;
        // Animation
        protected CancellationTokenSource? cts = new();

        public  string AlgorithmName { get; protected set; }
        public ObservableCollection<StringPseudoCode> Pseudocodes { get; protected set; }
        public Vertex? StartVertex
        {
            get => _startVetex;
            set
            {
                if (_startVetex != value)
                {
                    _startVetex = value;
                    IsStartVertexChanged = true;
                }
            }
        }
        public bool IsStartVertexChanged { get; protected set; } = false;


        // giây
        protected int RunSpeed { get; set; } = 2000;
        public int IsRunning { get; set; } = 1;
        protected int TimeDelayOfLineCode
        {
            get => RunSpeed / 2 * IsRunning;
        }

        private event EventHandler _completedAlgorithm;
        public event EventHandler CompletedAlgorithm
        {
            add => _completedAlgorithm += value;
            remove => _completedAlgorithm -= value;
        }
        protected void OnCompletedAlgorithm()
        {
            if (_completedAlgorithm != null)
            {
                _completedAlgorithm(this, new EventArgs());
            }
        }

        public AlgorithmBase() 
        {
            Pseudocodes = new();
        }


        // need to override
        protected virtual void StartVetexChanged(CustomGraph graph)
        {
            CleanAlgorithm(graph);
        }
        protected virtual void CleanAlgorithm(CustomGraph graph)
        {
            graph.UnVisitAndClearParentAll();
        }
        protected virtual void FillPseudoCode() { }
        public virtual async void RunAlgorithm(CustomGraph graph) 
        {
            IsRunning = 1;
        }
        public virtual async void ContinueAlgorithm(CustomGraph graph) 
        {
            IsRunning = 1;
        }
        //
        public void PauseAlgorithm()
        {
            IsRunning = 0;
            cts?.Cancel();
            cts = new CancellationTokenSource();
        }


        public void SetRunSpeed(int speedUp)
        {
            RunSpeed = _baseSpeed - speedUp * _fastSpeedPerLevel;
        }
    }
}
