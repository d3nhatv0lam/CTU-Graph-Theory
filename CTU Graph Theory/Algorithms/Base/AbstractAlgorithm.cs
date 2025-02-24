using CTU_Graph_Theory.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CTU_Graph_Theory.Algorithms.Base
{
    abstract class AbstractAlgorithm
    {
        abstract public string AlgorithmName { get; protected set; }
        abstract public ObservableCollection<StringPseudoCode> Pseudocodes { get; protected set; }
        abstract protected void FillPseudoCode();
        abstract protected  void StartVetexChanged(CustomGraph graph);
        abstract public void RunAlgorithm(CustomGraph graph);
        abstract public void PauseAlgorithm();
        abstract public void ContinueAlgorithm(CustomGraph graph);

    }
}
