/*
 * Creado por SharpDevelop.
 * Usuario: Pingu
 * Fecha: 11/11/2015
 * Hora: 11:45
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
using System.Linq;
using System.IO;
using System.Xml;
namespace DeveloperTime
{
	/// <summary>
	/// Interaction logic for Main.xaml
	/// </summary>
	public partial class Main : Window
	{
		readonly string rutaGuardarDatos=Environment.CurrentDirectory+Path.DirectorySeparatorChar+"projectsTimerData.xml";
		ProjectTimer proyctoActual;
		bool iniciado;
		System.Timers.Timer timer;
		DateTime timeInicio;
		public Main()
		{
			InitializeComponent();
			CargarDatos();
			btnOnOff.Click += IniciaOParaProyectoActual;
			iniciado = false;
			timer = new System.Timers.Timer();
			timer.Elapsed += ActualizaTiempo;
			btnNuevo.Click += NuevoProyecto;
			txtNombreProyecto.AcceptsReturn = true;
			txtNombreProyecto.TextChanged += CambiaTextoProyecto;
			projectsList.SelectionChanged += PonProyecto;
            lblTiempoTotal.MouseDoubleClick += AbrirLog;
            btnElimina.Click += Reset;
			Closed+=GuardarDatos;
		}

        private void AbrirLog(object sender, MouseButtonEventArgs e)
        {
            if (proyctoActual != null)
            {
                //abrir una ventana nueva con el log!
                new LogViewer(proyctoActual).ShowDialog();
                lblTiempoTotal.Content = proyctoActual.TotalTime;
            }
        }

		void CargarDatos()
		{
			if(File.Exists(rutaGuardarDatos)){
			XmlDocument xml=new XmlDocument();
			xml.Load(rutaGuardarDatos);
			ProjectTimer[] projects=ProjectTimer.ToProjectTimer(xml);
			for(int i=0;i<projects.Length;i++)
				projectsList.Items.Add(projects[i]);}
		}

		void GuardarDatos(object sender, EventArgs e)
		{
			NuevoProyecto(null,null);//guardo el actual
			ProjectTimer.ToXml(projectsList.Items.OfType<ProjectTimer>()).Save(rutaGuardarDatos);
		}
        void Reset(object sender, RoutedEventArgs e)
        {
            proyctoActual = null;
            txtNombreProyecto.Text = "";
            lblTiempo.Content = "";
            lblTiempoTotal.Content = "";
            timer.Stop();
            btnOnOff.Content = "On";
            iniciado = false;
        }

		void IniciaOParaProyectoActual(object sender, RoutedEventArgs e)
		{

			if (proyctoActual != null) {
				iniciado = !iniciado;
				if (iniciado) {
					btnOnOff.Content = "Off";
					timeInicio = DateTime.Now;
					proyctoActual.Inicia();
					timer.Start();
				} else {
					 btnOnOff.Content = "On";
					proyctoActual.Para();
					timer.Stop();
					lblTiempoTotal.Content=proyctoActual.TotalTime;
                    lblTiempo.Content = "";
				}
			} else
				MessageBox.Show("Lo primero de todo es crear el proyecto", "Atencion", MessageBoxButton.OK, MessageBoxImage.Exclamation);
		}

		void PonProyecto(object sender, SelectionChangedEventArgs e)
		{
			//Pone el que hay y quita el nuevo
			NuevoProyecto(null, null);
			proyctoActual = projectsList.Items[projectsList.SelectedIndex] as ProjectTimer;
			if (proyctoActual != null) {
				txtNombreProyecto.Text = proyctoActual.NombreProycto;
				lblTiempoTotal.Content=proyctoActual.TotalTime;
				projectsList.SelectionChanged-=PonProyecto;
				projectsList.Items.Remove(proyctoActual);
			projectsList.SelectionChanged+=PonProyecto;
			}
		}
		void ActualizaTiempo(object sender, System.Timers.ElapsedEventArgs e)
		{
			Action act = () => lblTiempo.Content = (DateTime.Now - timeInicio).ToString();
			Dispatcher.BeginInvoke(act);
		}

		void CambiaTextoProyecto(object sender, TextChangedEventArgs e)
		{
			if (proyctoActual == null) {
				proyctoActual = new ProjectTimer(txtNombreProyecto.Text);
			} else
				proyctoActual.NombreProycto = txtNombreProyecto.Text;
		}
		void NuevoProyecto(object sender, RoutedEventArgs e)
		{
			if (proyctoActual != null && proyctoActual.Valido) {
				projectsList.Items.Add(proyctoActual);
			}
            Reset(null, null);

		}
	}
}