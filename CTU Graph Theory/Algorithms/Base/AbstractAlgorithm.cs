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
    public abstract class AbstractAlgorithm
    {
        // field area
        private int _baseSpeed = 2100;
        private int _fastSpeedPerLevel = 400;
        private int _runSpeed;

        // property area
        protected int TimeDelayOfLineCode
        {
            get => _runSpeed / 2 * Convert.ToInt32(!IsStopAlgorithm);
        }

        protected CancellationTokenSource? cts;
        protected bool IsStopAlgorithm { get; set; }
        public Vertex? StartVertex { get; set; }
        public abstract string AlgorithmName { get; }
        public ObservableCollection<StringPseudoCode> Pseudocodes { get; }
        public Queue<Vertex> QueueVertices { get; }


        // Abstract funtion area
        protected abstract void FillPseudoCode();
        public abstract void RunAlgorithm(CustomGraph graph);
        public abstract void RunAlgorithmWithAllVertex(CustomGraph graph, ObservableCollection<Vertex> vertices);
        public abstract void ContinueAlgorithm(CustomGraph graph);
        public abstract void ContinueAlgorithmWithAllVertex(CustomGraph graph);

        // Funtion area
        protected void CleanGraphForAlgorithm(CustomGraph graph)
        {
            graph.UnVisitAndClearParentAll();
        }

        public void SetRunSpeed(int speedUp)
        {
            _runSpeed = _baseSpeed - speedUp * _fastSpeedPerLevel;
            _runSpeed = int.Max(_runSpeed, 0);
        }

        public virtual void PauseAlgorithm()
        {
            cts?.Cancel();
            cts = new CancellationTokenSource();
            IsStopAlgorithm = true;
        }
        public virtual void StopAlgorithm(CustomGraph graph)
        {
            IsStopAlgorithm = true;
            PauseAlgorithm();
        }

        public virtual void CleanGraphAfterStopAndPause(CustomGraph graph)
        {
            if (IsStopAlgorithm) CleanGraphForAlgorithm(graph);
        }
        

        // abstract funtion base
        protected void BaseRunAlgorithm(CustomGraph graph)
        {
            IsStopAlgorithm = false;
            CleanGraphForAlgorithm(graph);
        }

        protected void BaseRunAlgorithmWithAllVertex(CustomGraph graph, ObservableCollection<Vertex> vertices)
        {
            BaseRunAlgorithm(graph);
            QueueVertices.Clear();
            foreach (var vertex in vertices)
                QueueVertices.Enqueue(vertex);
            // run algorithm
        }

        protected void BaseContinueAlgorithm(CustomGraph graph)
        {
            IsStopAlgorithm = false;
        }

        protected void BaseContinueAlgorithmWithAllVertex(CustomGraph graph)
        {
            BaseContinueAlgorithm(graph);
        }

        // Event Area
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

        public AbstractAlgorithm() 
        {
            StartVertex = null;
            Pseudocodes = new ObservableCollection<StringPseudoCode>();
            QueueVertices = new Queue<Vertex>();
            cts = new CancellationTokenSource();
            IsStopAlgorithm = false;
            _runSpeed = _baseSpeed;

        }
    }
}
