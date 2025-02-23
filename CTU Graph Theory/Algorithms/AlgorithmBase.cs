using CTU_Graph_Theory.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CTU_Graph_Theory.Algorithms
{
    public class AlgorithmBase
    {
        // base
        private Vertex _startVetex = null;
        private int _baseSpeed = 2100;
        private int _fastSpeedPerLevel = 400; 
   
        public string AlgorithmName { get; protected set; }
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

        // Animation
        protected CancellationTokenSource? cts = new();
        // giây
        public int RunSpeed { get; set; } = 2000;

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
        
        public AlgorithmBase() { }

        protected virtual void FillPseudoCode() { }
        protected virtual void StartVetexChanged(CustomGraph graph) { }
        public virtual void RunAlgorithm(CustomGraph graph) { }
        public virtual void PauseAlgorithm() 
        {
            cts?.Cancel();
            cts = new CancellationTokenSource();
        }

        public void SetRunSpeed(int speedUp)
        {
            RunSpeed = _baseSpeed - speedUp*_fastSpeedPerLevel;
        }

    }
}
