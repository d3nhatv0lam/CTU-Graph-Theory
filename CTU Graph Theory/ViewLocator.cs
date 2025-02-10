using Avalonia.Controls;
using Avalonia.Controls.Templates;
using CTU_Graph_Theory.ViewModels;
using System;
using System.Collections.Generic;

namespace CTU_Graph_Theory
{
    public class ViewLocator : IDataTemplate
    {
        private Dictionary<Type, Control> viewCache = new Dictionary<Type,Control>();

        public Control? Build(object? data)
        {
            if (data is null)
                return null;


            var viewModelType = data.GetType();
            // Kiểm tra nếu view đã tồn tại trong cache
            if (viewCache.TryGetValue(viewModelType, out Control? cachedView))
            {
                cachedView.DataContext = data;
                return cachedView;
            }


            var name = data.GetType().FullName!.Replace("ViewModel", "View", StringComparison.Ordinal);
            var type = Type.GetType(name);

            if (type != null)
            {
                var control = (Control)Activator.CreateInstance(type)!;
                control.DataContext = data;

                viewCache[viewModelType] = control; 
                return control;
            }

            return new TextBlock { Text = "Not Found: " + name };
        }

        public bool Match(object? data)
        {
            return data is ViewModelBase;
        }
    }
}
