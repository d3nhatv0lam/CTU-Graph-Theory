using CTU_Graph_Theory.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CTU_Graph_Theory.Interfaces
{
    public interface IAlgorithmRequirementViewModel
    {
        public ObservableCollection<RequestOfAlgorithm> Requirements { get; }
        public bool CheckRequirements(CustomGraph graph);
    }
}
