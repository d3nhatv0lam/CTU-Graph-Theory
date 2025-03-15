using CTU_Graph_Theory.Models;
using Microsoft.Msagl.Core;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CTU_Graph_Theory.Algorithms.Base
{
    public abstract class AbstractAlgorithm: ReactiveObject
    {
        // field area
        private const int BASE_SPEED = 2100;
        private const int SPEED_FER_LEVEL = 400;
        private int _runSpeed;
        protected CancellationTokenSource cts;

        // property area
        protected int TimeDelayOfLineCode
        {
            get => _runSpeed / 2 * Convert.ToInt32(!IsStopAlgorithm && !IsPauseAlgorithm) ;
        }
   
        protected bool IsStopAlgorithm { get; set; }
        protected bool IsPauseAlgorithm {  get; set; } 
        //public Vertex? StartVertex { get; set; }
        public string AlgorithmName { get; protected set; }
        public ObservableCollection<StringPseudoCode> Pseudocodes { get; }
        public Queue<Vertex> QueueVertices { get; }

        // Abstract funtion area
        protected abstract void FillPseudoCode();

        // Funtion area
        protected void CleanGraphForAlgorithm(CustomGraph graph)
        {
            graph.UnVisitAndClearParentAll();
        }

        public void SetRunSpeed(int speedUp)
        {
            _runSpeed = BASE_SPEED - speedUp * SPEED_FER_LEVEL;
            _runSpeed = int.Max(_runSpeed, 0);
        }

        public virtual void PauseAlgorithm()
        {
            cts.Cancel();
            cts = new CancellationTokenSource();
            IsPauseAlgorithm = true;
        }
        public virtual void StopAlgorithm(CustomGraph graph)
        {
            IsStopAlgorithm = true;
            PauseAlgorithm();
            CleanGraphForAlgorithm(graph);
            QueueVertices.Clear();
            QueueVertices.TrimExcess();
        }

        public virtual void CleanGraphAfterStop(CustomGraph graph)
        {
            if (IsStopAlgorithm) CleanGraphForAlgorithm(graph);
        }
        

        // abstract funtion base
        protected void BaseRunAlgorithm(CustomGraph graph)
        {
            IsStopAlgorithm = false;
            IsPauseAlgorithm = false;
            CleanGraphForAlgorithm(graph);
        }

        //protected void BaseRunAlgorithmWithAllVertex(CustomGraph graph, ObservableCollection<Vertex> vertices)
        //{
        //    BaseRunAlgorithm(graph);
        //    QueueVertices.Clear();
        //    foreach (var vertex in vertices)
        //        QueueVertices.Enqueue(vertex);
        //    // run algorithm
        //}

        protected void BaseContinueAlgorithm(CustomGraph graph)
        {
            IsStopAlgorithm = false;
            IsPauseAlgorithm = false;
        }

        //protected void BaseContinueAlgorithmWithAllVertex(CustomGraph graph)
        //{
        //    BaseContinueAlgorithm(graph);
        //}

        // Event Area
        private event EventHandler? _completedAlgorithm;
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
            AlgorithmName = "Unnamed";
            Pseudocodes = new ObservableCollection<StringPseudoCode>();
            QueueVertices = new Queue<Vertex>();
            cts = new CancellationTokenSource();
            IsStopAlgorithm = false;
            IsPauseAlgorithm = false;
            _runSpeed = BASE_SPEED;

        }
    }
}
