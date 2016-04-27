using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DeveloperTime
{
    /// <summary>
    /// Lógica de interacción para StringElement.xaml
    /// </summary>
    public partial class StringElement : UserControl
    {
        object obj;
        public StringElement(Object obj)
        {
            InitializeComponent();
            Object = obj;
            
        }
        public object Object
        {
            get { return obj; }
            set { obj = value; txtContent.Text = obj.ToString(); }
        }
    }
}
