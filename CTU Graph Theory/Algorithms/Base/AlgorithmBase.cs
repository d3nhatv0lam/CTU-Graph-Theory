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
        private bool _isStopAlgorithm;
        private Vertex? _startVetex = null;
        // Animation
        private int _baseSpeed = 2100;
        private int _fastSpeedPerLevel = 400;
        protected CancellationTokenSource cts = new();

        public string AlgorithmName { get; protected set; }
        public ObservableCollection<StringPseudoCode> Pseudocodes { get; protected set; }
        public Queue<Vertex> QueueVertices { get; protected set; }
        public Vertex? StartVertex
        {
            get => _startVetex;
            set
            {
                if (_startVetex != value)
                {
                    _startVetex = value;
                }
            }
        }
        public bool IsStopAlgorithm
        {
            get => _isStopAlgorithm;
            set => _isStopAlgorithm = value;
        }

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
            QueueVertices = new();
        }


        // need to override
        protected virtual void CleanGraphForAlgorithm(CustomGraph graph)
        {
            graph.UnVisitAndClearParentAll();
        }

        protected virtual void FillPseudoCode() { }
        public virtual async void RunAlgorithm(CustomGraph graph)
        {
            IsStopAlgorithm = false;
            IsRunning = 1;
            CleanGraphForAlgorithm(graph);
        }

        public virtual async void RunAlgorithmWithAllVertex(CustomGraph graph, ObservableCollection<Vertex> vertices)
        {
            IsStopAlgorithm = false;
            IsRunning = 1;
            CleanGraphForAlgorithm(graph);
            QueueVertices.Clear();
            foreach (var vertex in vertices)
                QueueVertices.Enqueue(vertex);
            // run algorith
        }

        public virtual async void ContinueAlgorithmWithAllVertex(CustomGraph graph)
        {
            IsRunning = 1;
        }
        public virtual async void ContinueAlgorithm(CustomGraph graph)
        {
            IsRunning = 1;
        }

        public void PauseAlgorithm()
        {
            cts.Cancel();
            cts = new CancellationTokenSource();
            IsRunning = 0;
        }

        public void StopAlgorithm(CustomGraph graph)
        {
            IsStopAlgorithm = true;
            PauseAlgorithm();
            CleanGraphForAlgorithm(graph);
        }


        public void SetRunSpeed(int speedUp)
        {
            RunSpeed = _baseSpeed - speedUp * _fastSpeedPerLevel;
        }
    }
}
