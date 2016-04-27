/*
 * Creado por SharpDevelop.
 * Usuario: Pingu
 * Fecha: 11/11/2015
 * Hora: 12:12
 * 
 * Para cambiar esta plantilla use Herramientas | Opciones | Codificación | Editar Encabezados Estándar
 */
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace DeveloperTime
{
	/// <summary>
	/// Description of ProjectTimer.
	/// </summary>
	public class ProjectTimer:IEnumerable<long[]>
	{
		string nombreProyecto;
		List<long[]> timeList;
		DateTime tiempoActual;
		public ProjectTimer(string nombreProyecto)
		{
			this.nombreProyecto = nombreProyecto;
			timeList = new List<long[]>();
		}
		private ProjectTimer(XmlNode nodo)
			: this(nodo.FirstChild.InnerText)//Nodo Nombre
		{
			XmlNode nodoTiempos=nodo.LastChild,nodoTime;
			for (int i = 0; i < nodoTiempos.ChildNodes.Count; i++)//Nodo Tiempos
				try {
				nodoTime=nodoTiempos.ChildNodes[i];
				timeList.Add(new long[]{Convert.ToInt64(nodoTime.ChildNodes[0].InnerText),Convert.ToInt64(nodoTime.ChildNodes[1].InnerText)});
			} catch {
			}//por si tocan el xml
		}
		public string NombreProycto {
			get{ return nombreProyecto; }
			set{ nombreProyecto = value; }
		}
		public TimeSpan TotalTime {
			get {
				long timeTotal = 0;
				for (int i = 0; i < timeList.Count; i++)
					timeTotal += timeList[i][0];
				return new TimeSpan(timeTotal);
			}
		}

		public bool Valido {
			get{ return !String.IsNullOrEmpty(NombreProycto) && !String.IsNullOrWhiteSpace(NombreProycto); }
			
		}

		public void Inicia()
		{
			tiempoActual = DateTime.Now;
		}
		public void Para()
		{
			timeList.Add(new long[]{(DateTime.Now - tiempoActual).Ticks,DateTime.Now.Ticks});
		}
		public void ParaYInicia()
		{
			Para();
			Inicia();
		}
		public override string ToString()
		{
			return NombreProycto + " " + TotalTime;
		}
        public void QuitarTiempo(int posicion)
        {
            timeList.RemoveAt(posicion);
        }
		public static XmlDocument ToXml(IEnumerable<ProjectTimer> projects)
		{
			XmlDocument xml = new XmlDocument();
			string childNodesTime = "", child;
			StringBuilder parent = new StringBuilder("<Projects>");
			if (projects != null)
				foreach (ProjectTimer project in projects) {
				if (project.Valido) {
					childNodesTime = "";
					child = "";
					for (int i = 0; i < project.timeList.Count; i++)
						childNodesTime += String.Format("\n<Tiempo><TiempoHecho>{0}</TiempoHecho><HoraFin>{1}</HoraFin></Tiempo>",new object[]{project.timeList[i][0],project.timeList[i][1]});
					child = String.Format("<Project><Nombre>{0}</Nombre><Tiempos>{1}</Tiempos></Project>", new object[] {
					                      	project.NombreProycto,
					                      	childNodesTime
					                      });
					parent.Append(child);
				}
			}
			parent.Append("</Projects>");
			xml.LoadXml(parent.ToString());
			xml.Normalize();
			return xml;
		}
		public static ProjectTimer[] ToProjectTimer(XmlDocument xml)
		{
			List<ProjectTimer> projects = new List<ProjectTimer>();
			if (xml != null)
				foreach (XmlNode project in xml.FirstChild.ChildNodes)
					try {
                        projects.Add(new ProjectTimer(project));	//Nodo Project
			} catch {//por si tocan el xml
			}

			return projects.ToArray();
		}

        #region Miembros de IEnumerable<long[]>

        public IEnumerator<long[]> GetEnumerator()
        {
            return timeList.GetEnumerator();
        }

        #endregion

        #region Miembros de IEnumerable

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion
    }
}
