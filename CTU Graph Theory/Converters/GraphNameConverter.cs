using Avalonia.Data.Converters;
using Avalonia.Media.TextFormatting.Unicode;
using CTU_Graph_Theory.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace CTU_Graph_Theory.Converters
{
    public class GraphNameConverter : IMultiValueConverter
    {
        public object? Convert(IList<object?> values, Type targetType, object? parameter, CultureInfo culture)
        {
            string graphName = "Tên đồ thị: ";
            if (values?.Count == 2 
                && values[0] is CustomGraph.GraphType graphType 
                && values[1] is bool IsDirectedType)
            {
                switch (graphType) 
                {
                    case CustomGraph.GraphType.SimpleGraph:
                        graphName = "Đơn đồ thị";
                        break;
                    case CustomGraph.GraphType.MultiGraph:
                        graphName = "Đa đồ thị";
                        break;
                    case CustomGraph.GraphType.PseudoGraph:
                        graphName = "Giả đồ thị";
                        break;
                }
                graphName += " ";
                switch (IsDirectedType) 
                {
                    case false:
                        graphName += "vô hướng";
                        break;
                    case true:
                        graphName += "có hướng";
                        break;
                }
                return graphName;
            }
            return graphName;
            //throw new NotImplementedException();
        }
    }
}
