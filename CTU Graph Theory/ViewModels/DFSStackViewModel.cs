using CTU_Graph_Theory.Interfaces;
using CTU_Graph_Theory.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CTU_Graph_Theory.ViewModels
{
    class DFSStackViewModel: ViewModelBase, IAlgorithms
    {
        public string AlgorithmName
        {
            get => "BFS";
        }
        public bool IsSetCompletedAlgorithm { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public ObservableCollection<StringPseudoCode> Pseudocodes => throw new NotImplementedException();

        public DFSStackViewModel()
        {

        }

        public void TransferGraph(CustomGraph graph, Vertex vertex)
        {
            return;
        }

        public void RunAlgorithm()
        {

        }

        public void PauseAlgorithm()
        {
            throw new NotImplementedException();
        }

        public void SetRunSpeed(int speed)
        {
            throw new NotImplementedException();
        }

        public void SetCompletedAlgorithm(EventHandler returnIsRunningState)
        {
            throw new NotImplementedException();
        }
    }
}
