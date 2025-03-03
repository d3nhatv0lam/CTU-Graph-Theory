using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CTU_Graph_Theory.Models
{
    public class RequestOfAlgorithm: ReactiveObject
    {
        private bool _isDoneRequest;
        private string _requestString;

        public bool IsDoneRequest
        {
            get => _isDoneRequest;
            set => this.RaiseAndSetIfChanged(ref _isDoneRequest, value);
        }
        public string RequestString
        {
            get => _requestString;
            set => this.RaiseAndSetIfChanged(ref _requestString, value);
        }

        public RequestOfAlgorithm()
        {
            RequestString = string.Empty;
            IsDoneRequest = false;
        }
        public RequestOfAlgorithm(string requestString)
        {
            RequestString = requestString;
            IsDoneRequest = false;
        }

        public void SetRequestDone()
        {
            IsDoneRequest = true;
        }
        public void UnSetRequestDone()
        {
            IsDoneRequest = false;
        }
    }
}
