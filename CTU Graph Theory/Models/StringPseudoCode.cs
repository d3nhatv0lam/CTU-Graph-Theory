using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CTU_Graph_Theory.Models
{
    public class StringPseudoCode : ReactiveObject
    {
        //public static int AlignmentPerLine = 20;
        //private int _alignmentCount;

        //public int AlignmentCount
        //{
        //    get => _alignmentCount;
        //    set => this.RaiseAndSetIfChanged(ref _alignmentCount, value);
        //}
        //public Thickness Alignment
        //{
        //    get => new Thickness(AlignmentCount * AlignmentPerLine, 0, 0, 0);
        //}

        private string _code;
        private bool _isSelectionCode;

        public string Code
        {
            get => _code;
            set => this.RaiseAndSetIfChanged(ref _code, value);
        }
        public bool IsSelectionCode
        {
            get => _isSelectionCode;
            set => this.RaiseAndSetIfChanged(ref _isSelectionCode, value);
        }

        public StringPseudoCode()
        {
            //AlignmentCount = 0;
            Code = string.Empty;
            IsSelectionCode = false;
        } 

        public StringPseudoCode(string code)
        {
            //AlignmentCount = 0;
            Code = code;
            IsSelectionCode = false;
        }
    }
}
