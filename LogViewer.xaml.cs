/*
 * Creado por SharpDevelop.
 * Usuario: Pingu
 * Fecha: 11/11/2015
 * Hora: 15:58
 * 
 * Para cambiar esta plantilla use Herramientas | Opciones | Codificación | Editar Encabezados Estándar
 */
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;

namespace DeveloperTime
{
	/// <summary>
	/// Interaction logic for LogViewer.xaml
	/// </summary>
	public partial class LogViewer : Window
	{
        private ProjectTimer proyctoActual;

		private LogViewer()
		{
			InitializeComponent();
            
		}

        public LogViewer(ProjectTimer proyctoActual):this()
        {
            // TODO: Complete member initialization
            this.proyctoActual = proyctoActual;
            Title = proyctoActual.NombreProycto;
            StringElement strElement;
            foreach (long[] tiempo in proyctoActual)
            {
                strElement = new StringElement(String.Format("Tiempo: {0}  fecha: {1}", new object[] { new TimeSpan(tiempo[0]), new DateTime(tiempo[1]) }));
                strElement.MouseDoubleClick += Seleccionado;
                stkLog.Children.Add(strElement);
            }
        }

        private void Seleccionado(object sender, MouseButtonEventArgs e)
        {
            int pos = 0;
            if (MessageBox.Show("¿Quieres borrarlo?", "Atencion", MessageBoxButton.YesNo, MessageBoxImage.Information) == MessageBoxResult.Yes)
            {
                pos = stkLog.Children.IndexOf(sender as UIElement);
                proyctoActual.QuitarTiempo(pos);
                stkLog.Children.RemoveAt(pos);
            }
        }
	}
}