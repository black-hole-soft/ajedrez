/*
 * Creado por SharpDevelop.
 * Usuario: ggonzalez
 * Fecha: 06/11/2008
 * Hora: 09:19 a.m.
 * 
 * Para cambiar esta plantilla use Herramientas | Opciones | Codificación | Editar Encabezados Estándar
 */
using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Ajedrez
{
	public class Casilla
	{
		public Button btn;
		public Pieza pza;
		public Casilla(String r,bool c)
		{
			pza=new Pieza(r,c);
		}
		public Casilla(){}
	}
}
