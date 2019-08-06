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
		public Casilla(){}
        public void colocaPieza()
        {
            if (pza == null)
                btn.Image = null;
            else
            {
                String c;
                if (piezaBlanca())
                    c = "w";
                else
                    c = "g";
                btn.Image = Image.FromFile(c + pza.roll + ".png");
            }
        }
        public bool piezaBlanca()
        {
            if (pza != null)
                if (pza.color)
                    return true;
            return false;
        }
        public bool piezaNegra()
        {
            if (pza != null)
                if (!pza.color)
                    return true;
            return false;
        }
	}
}
