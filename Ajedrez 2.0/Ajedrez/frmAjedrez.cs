#region librerias
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
#endregion
namespace Ajedrez
{
    public partial class frmAjedrez : Form
    {
        public int sec, min, hr;
        public Casilla[,] tablero = new Casilla[8, 8];
        public bool seleccionada, turnoBlanca, amenaza, terminado,tm=true;
        public int selectX, selectY, iluminado;
        public Rey reyb = new Rey(4, 7), reyn = new Rey(4, 0);
        
        public frmAjedrez()
        {
            InitializeComponent();
            inicializarTablero();
        }

        public void onButtonClick(object source, EventArgs e)
        {
            Button b = (Button)source;
            int x = b.Location.X / 69, y = b.Location.Y / 69;
            if (seleccionada)
            {
                seleccionada = false;
                String tag = (String)tablero[x, y].btn.BackgroundImage.Tag;
//----------------------------------------------Mueve Pieza-----------------------------------------------
                if (tag == "l" || tag == "r")
                {
                    bool cr = false;
                    limpiaEnroque();
                    iluminaPosibles(selectX, selectY, "");
                    Pieza aux = tablero[x, y].pza;
                    tablero[x, y].pza = tablero[selectX, selectY].pza;
                    tablero[selectX, selectY].pza = null;
                    //Posicion de los Reyes
                    if (tablero[x, y].pza.color && tablero[x, y].pza.roll == "king")
                    { reyb.x = x; reyb.y = y; cr = true; }
                    else if (!tablero[x, y].pza.color && tablero[x, y].pza.roll == "king")
                    { reyn.x = x; reyn.y = y; cr = true; }
                    tablero[selectX, selectY].colocaPieza();
                    tablero[x, y].colocaPieza();
                    //---------------------
                    if (jaqueAlRey(turnoBlanca))
                    {
                        if (turnoBlanca)
                            MessageBox.Show("El Rey Blanco está en Jaque", "Jaque");
                        else
                            MessageBox.Show("El Rey Dorado está en Jaque", "Jaque");
                        tablero[selectX, selectY].pza = tablero[x, y].pza;
                        tablero[x, y].pza = aux;
                        //Regresar posicion de los Reyes
                        if (cr)
                        {
                            if (tablero[selectX, selectY].pza.color)
                            { reyb.x = selectX; reyb.y = selectY; }
                            else
                            { reyn.x = selectX; reyn.y = selectY; }
                        }
                        tablero[selectX, selectY].colocaPieza();
                        tablero[x, y].colocaPieza();
                    }
                    else
                    {
                        //Comprueba piezaz que ateren el enroque
                        alteranEnroque(x, y);
                        //Peon a Dama
                        if (tablero[x, y].piezaBlanca() && tablero[x, y].pza.roll == "pawn" && y == 0)
                        { tablero[x, y].pza.roll = "queen"; tablero[x, y].colocaPieza(); MessageBox.Show("Peon blanco a Reina", "Promocion"); }
                        else if (tablero[x, y].piezaNegra() && tablero[x, y].pza.roll == "pawn" && y == 7)
                        { tablero[x, y].pza.roll = "queen"; tablero[x, y].colocaPieza(); MessageBox.Show("Peon dorado a Reina", "Promocion"); }
                        //Turno
                        if (turnoBlanca)
                            turnoBlanca = false;
                        else
                            turnoBlanca = true;
                        int i, j, auxX = selectX, auxY = selectY;
                        iluminado = 0;
                        amenaza = false;
                        for (j = 0; j < 8; j++)
                            for (i = 0; i < 8; i++)
                                if (tablero[i, j].pza != null)
                                    if (tablero[i, j].pza.color == turnoBlanca)
                                    {
                                        selectX = i;
                                        selectY = j;
                                        iluminaPosibles(i, j, "j");
                                    }
                        if (jaqueAlRey(turnoBlanca))
                        {
                            if (iluminado == 0)
                            {
                                terminado = true;
                                tm = false;
                                MessageBox.Show("Jaque Mate", "Jaque");
                                DialogResult res = MessageBox.Show("Comenzar Nuevo?", "Juego Terminado");
                                tm = true;
                                if (res.ToString() == "OK")
                                    comenzarNuevo();
                            }
                            else
                            {
                                if (turnoBlanca)
                                    MessageBox.Show("Jaque al Rey Blanco", "Jaque");
                                else
                                    MessageBox.Show("Jaque al Rey Dorado", "Jaque");
                            }
                        }
                        else
                            if (iluminado == 0)
                                MessageBox.Show("Ahogado", "Empate");
                    }
                }
                else
                {
                    limpiaEnroque();
                    iluminaPosibles(selectX, selectY, "");
                    if (tag == "g")
                    {
                        enroque(x, y);
                        //Turno
                        if (turnoBlanca)
                            turnoBlanca = false;
                        else
                            turnoBlanca = true;
                    }
                    if (tag == "y")
                        MessageBox.Show("No es posible enrocar en esta direccion", "Enroque");
                }
            }
//--------------------------------------------------Seleccionar la Pieza----------------------------------
            else
            {
                if (!terminado)
                {
                    if (tablero[x, y].pza != null)
                    {
                        if ((turnoBlanca && tablero[x, y].piezaBlanca()) || (!turnoBlanca && tablero[x, y].piezaNegra()))
                        {
                            iluminado = 0;
                            selectX = x;
                            selectY = y;
                            amenaza = false;
                            iluminaPosibles(x, y, "l");
                            if (iluminado > 0)
                            {
                                seleccionada = true;
                                //Comprueba condiciones para enroque
                                compruebaEnroque();
                            }
                            else
                                if (amenaza)
                                {
                                    if (turnoBlanca)
                                        MessageBox.Show("Rey Blanco quedará en Jaque", "Jaque");
                                    else
                                        MessageBox.Show("Rey Dorado quedará en Jaque", "Jaque");
                                    iluminaPosibles(x, y, "");
                                }
                        }
                        else
                        {
                            if (turnoBlanca)
                                MessageBox.Show("Es el turno de las Blancas", "Turno");
                            else
                                MessageBox.Show("Es el turno de las Doradas", "Turno");
                        }
                    }
                }
            }
        }

        public void mosaico(int x, int y, String l)
        {
            if (l == "l" || l == "j")
            {
                bool cr = false;
                Pieza aux = tablero[x, y].pza;
                tablero[x, y].pza = tablero[selectX, selectY].pza;
                tablero[selectX, selectY].pza = null;
                //Posicion de los Reyes
                if (tablero[x, y].pza != null)
                {
                    if (tablero[x, y].pza.color && tablero[x, y].pza.roll == "king")
                    { reyb.x = x; reyb.y = y; cr = true; }
                    else if (!tablero[x, y].pza.color && tablero[x, y].pza.roll == "king")
                    { reyn.x = x; reyn.y = y; cr = true; }
                }
                if (jaqueAlRey(turnoBlanca))
                {
                    if (l == "j")
                        l = "";
                    else
                        l = "r";
                    amenaza = true;
                }
                else
                    iluminado++;
                tablero[selectX, selectY].pza = tablero[x, y].pza;
                tablero[x, y].pza = aux;
                //Regresar posicion de los Reyes
                if (cr)
                {
                    if (tablero[selectX, selectY].pza.color)
                    { reyb.x = selectX; reyb.y = selectY; }
                    else
                    { reyn.x = selectX; reyn.y = selectY; }
                }
                if (l == "j")
                    l = "";
            }
            if ((x + y) % 2 == 0)
                tablero[x, y].btn.BackgroundImage = Image.FromFile(l + "wmarbel.png");
            else
                tablero[x, y].btn.BackgroundImage = Image.FromFile(l + "bmarbel.png");
            tablero[x, y].btn.BackgroundImage.Tag = l;
        }

        #region Enroque

        public void enroque(int x, int y)
        {
            tablero[x, y].pza = tablero[selectX, selectY].pza;
            tablero[selectX, selectY].pza = null;
            tablero[x, y].colocaPieza();
            tablero[selectX, selectY].colocaPieza();
            if (x == 1 && y == 7)
            {
                tablero[2, 7].pza = tablero[0, 7].pza;
                tablero[0, 7].pza = null;
                tablero[2, 7].colocaPieza();
                tablero[0, 7].colocaPieza();
            }
            if (x == 6 && y == 7)
            {
                tablero[5, 7].pza = tablero[7, 7].pza;
                tablero[7, 7].pza = null;
                tablero[5, 7].colocaPieza();
                tablero[7, 7].colocaPieza();
            }
            if (x == 1 && y == 0)
            {
                tablero[2, 0].pza = tablero[0, 0].pza;
                tablero[0, 0].pza = null;
                tablero[2, 0].colocaPieza();
                tablero[0, 0].colocaPieza();
            }
            if (x == 6 && y == 0)
            {
                tablero[5, 0].pza = tablero[7, 0].pza;
                tablero[7, 0].pza = null;
                tablero[5, 0].colocaPieza();
                tablero[7, 0].colocaPieza();
            }
        }
        public void alteranEnroque(int x, int y)
        {
            if ((tablero[x, y].piezaBlanca() && selectX == 0 && selectY == 7) || (x == 0 && y == 7))
                reyb.LrookM = true;
            if ((tablero[x, y].piezaBlanca() && selectX == 7 && selectY == 7) || (x == 7 && y == 7))
                reyb.RrookM = true;
            if ((tablero[x, y].piezaNegra() && selectX == 0 && selectY == 0) || (x == 0 && y == 0))
                reyn.LrookM = true;
            if ((tablero[x, y].piezaNegra() && selectX == 7 && selectY == 0) || (x == 7 && y == 0))
                reyn.RrookM = true;
            if(tablero[x, y].pza.roll == "king")
            {
                if (tablero[x, y].piezaBlanca())
                    reyb.reyM = true;
                else
                    reyn.reyM = true;
            }
        }
        public void compruebaEnroque()
        {
            if (selectX == reyb.x && selectY == reyb.y && reyb.x == 4 && reyb.y == 7 && !jaqueAlRey(true))
            {
                if (reyb.puedeEnrocarIzq() && tablero[1, 7].pza == null && tablero[2, 7].pza == null && tablero[3, 7].pza == null && !posicionAmenazada(1, 7, true) && !posicionAmenazada(2, 7, true))
                    mosaico(1, 7, "g");
                else
                    mosaico(1, 7, "y");
                if (reyb.puedeEnrocarDer() && tablero[5, 7].pza == null && tablero[6, 7].pza == null && !posicionAmenazada(5, 7, true) && !posicionAmenazada(6, 7, true))
                    mosaico(6, 7, "g");
                else
                    mosaico(6, 7, "y");
            }
            if (selectX == reyn.x && selectY == reyn.y && reyn.x == 4 && reyn.y == 0 && !jaqueAlRey(false))
            {
                if (reyn.puedeEnrocarIzq() && tablero[1, 0].pza == null && tablero[2, 0].pza == null && tablero[3, 0].pza == null && !posicionAmenazada(1, 0, false) && !posicionAmenazada(2, 0, false))
                    mosaico(1, 0, "g");
                else
                    mosaico(1, 0, "y");
                if (reyn.puedeEnrocarDer() && tablero[5, 0].pza == null && tablero[6, 0].pza == null && !posicionAmenazada(5, 0, false) && !posicionAmenazada(6, 0, false))
                    mosaico(6, 0, "g");
                else
                    mosaico(6, 0, "y");
            }
        }
        public void limpiaEnroque()
        {
            mosaico(1, 7, "");
            mosaico(6, 7, "");
            mosaico(1, 0, "");
            mosaico(6, 0, "");
        }

        #endregion

        #region jaques al rey

        public bool jaqueAlRey(bool blanco)
        {
            if ((blanco && posicionAmenazada(reyb.x, reyb.y, blanco)) || (!blanco && posicionAmenazada(reyn.x, reyn.y, blanco)))
                return true;
            return false;
        }
        public bool posicionAmenazada(int x, int y,bool c)
        {
            if (amnzPawn(x, y, c) || amnzKing(x, y, c) || amnzRook(x, y, c) || amnzBishop(x, y, c) || amnzQueen(x, y, c) || amnzKnight(x, y, c))
                return true;
            return false;
        }
        public bool ataca(int x,int y,bool c,String r) 
        {
            if ((c && tablero[x, y].piezaNegra())||(!c && tablero[x, y].piezaBlanca()))
                if (tablero[x, y].pza.roll == r || (tablero[x, y].pza.roll == "queen" && (r == "rook" || r == "bishop")))
                    return true;
            return false;
        }
        public bool amnzPawn(int x, int y, bool c)
        {
            if (c)
            {
                if (y > 0)
                {
                    if (x > 0)
                        if (ataca(x - 1, y - 1, c, "pawn"))
                            return true;
                    if (x < 7)
                        if (ataca(x + 1, y - 1, c, "pawn"))
                            return true;
                }
            }
            else
            {
                if (y < 7)
                {
                    if (x > 0)
                        if (ataca(x - 1, y + 1, c, "pawn"))
                            return true;
                    if (x < 7)
                        if (ataca(x + 1, y + 1, c, "pawn"))
                            return true;
                }
            }
            return false;
        }
        public bool amnzKing(int x, int y,bool c)
        {
            if (x > 0)
            {
                if (y > 0)//Esquina Superior Izquierda
                    if (ataca(x - 1, y - 1, c, "king") || ataca(x, y - 1, c, "king") || ataca(x - 1, y, c, "king"))
                        return true;
                if (y < 7)//Esquina Inferior Izquierda
                    if (ataca(x - 1, y + 1, c, "king") || ataca(x, y + 1, c, "king") || ataca(x - 1, y, c, "king"))
                        return true;
            }
            if (x < 7)
            {
                if (y > 0)//Esquina Superior Derecha
                    if (ataca(x + 1, y - 1, c, "king") || ataca(x, y - 1, c, "king") || ataca(x + 1, y, c, "king"))
                        return true;
                if (y < 7)//Esquina Inferior Derecha
                    if (ataca(x + 1, y + 1, c, "king") || ataca(x + 1, y, c, "king") || ataca(x, y + 1, c, "king"))
                        return true;
            }
            return false;
        }
        public bool amnzRook(int x, int y, bool c)
        {
            int i;
            if (x > 0)
            {
                i = x - 1;
                while (i >= 0 && tablero[i, y].pza == null)
                    i--;
                if (i >= 0 && ataca(i, y, c, "rook"))
                    return true;
            }
            if (x < 7)
            {
                i = x + 1;
                while (i <= 7 && tablero[i, y].pza == null)
                    i++;
                if (i <= 7 && ataca(i, y, c, "rook"))
                    return true;
            }
            if (y > 0)
            {
                i = y - 1;
                while (i >= 0 && tablero[x, i].pza == null)
                    i--;
                if (i >= 0 && ataca(x, i, c, "rook"))
                    return true;
            }
            if (y < 7)
            {
                i = y + 1;
                while (i <= 7 && tablero[x, i].pza == null)
                    i++;
                if (i <= 7 && ataca(x, i, c, "rook"))
                    return true;
            }
            return false;
        }
        public bool amnzBishop(int x, int y, bool c)
        {
            int i, j;
            if (x > 0 && y > 0)
            {
                i = x - 1;
                j = y - 1;
                while (i >= 0 && j >= 0 && tablero[i, j].pza == null)
                {
                    i--;
                    j--;
                }
                if (i >= 0 && j >= 0 && ataca(i, j, c, "bishop"))
                    return true;
            }
            if (x < 7 && y < 7)
            {
                i = x + 1;
                j = y + 1;
                while (i <= 7 && j <= 7 && tablero[i, j].pza == null)
                {
                    i++;
                    j++;
                }
                if (i <= 7 && j <= 7 && ataca(i, j, c, "bishop"))
                    return true;
            }
            if (x < 7 && y > 0)
            {
                i = x + 1;
                j = y - 1;
                while (i <= 7 && j >= 0 && tablero[i, j].pza == null)
                {
                    i++;
                    j--;
                }
                if (i <= 7 && j >= 0 && ataca(i, j, c, "bishop"))
                    return true;
            }
            if (x > 0 && y < 7)
            {
                i = x - 1;
                j = y + 1;
                while (i >= 0 && j <= 7 && tablero[i, j].pza == null)
                {
                    i--;
                    j++;
                }
                if (i >= 0 && j <= 7 && ataca(i, j, c, "bishop"))
                    return true;
            }
            return false;
        }
        public bool amnzQueen(int x, int y, bool c)
        {
            return amnzRook(x, y, c) || amnzBishop(x, y, c);
        }
        public bool amnzKnight(int x, int y, bool c)
        {
            if (x > 1)
            {
                if (y > 1)//Esquina Superior Izquierda
                    if (ataca(x - 2, y - 1, c, "knight") || ataca(x - 1, y - 2, c, "knight"))
                        return true;
                if (y < 6)//Esquina Inferior Izquierda
                    if (ataca(x - 2, y + 1, c, "knight") || ataca(x - 1, y + 2, c, "knight"))
                        return true;
            }
            if (x < 6)
            {
                if (y > 1)//Esquina Superior Derecha
                    if (ataca(x + 1, y - 2, c, "knight") || ataca(x + 2, y - 1, c, "knight"))
                        return true;
                if (y < 6)//Esquina Inferior Derecha
                    if (ataca(x + 2, y + 1, c, "knight") || ataca(x + 1, y + 2, c, "knight"))
                        return true;
            }
            if (x > 0 && y > 1)
                if (ataca(x - 1, y - 2, c, "knight"))
                    return true;
            if (x > 1 && y > 0)
                if (ataca(x - 2, y - 1, c, "knight"))
                    return true;
            if (x > 0 && y < 6)
                if (ataca(x - 1, y + 2, c, "knight"))
                    return true;
            if (x > 1 && y < 7)
                if (ataca(x - 2, y + 1, c, "knight"))
                    return true;

            if (x < 7 && y > 1)
                if (ataca(x + 1, y - 2, c, "knight"))
                    return true;
            if (x < 6 && y > 0)
                if (ataca(x + 2, y - 1, c, "knight"))
                    return true;
            if (x < 7 && y < 6)
                if (ataca(x + 1, y + 2, c, "knight"))
                    return true;
            if (x < 6 && y < 7)
                if (ataca(x + 2, y + 1, c, "knight"))
                    return true;
            return false;
        }

        #endregion

        #region ilumina Posibles Posiciones de cada Pieza

        public void iluminaKing(int x, int y, String l)
        {
            if (x > 0)
            {
                if (y > 0)//Esquina Superior Izquierda
                {
                    iluminar(x, y, x - 1, y - 1, l);
                    iluminar(x, y, x, y - 1, l);
                    iluminar(x, y, x - 1, y, l);
                }
                if (y < 7)//Esquina Inferior Izquierda
                {
                    iluminar(x, y, x - 1, y + 1, l);
                    iluminar(x, y, x, y + 1, l);
                    iluminar(x, y, x - 1, y, l);
                }
            }
            if (x < 7)
            {
                if (y > 0)//Esquina Superior Derecha
                {
                    iluminar(x, y, x + 1, y - 1, l);
                    iluminar(x, y, x, y - 1, l);
                    iluminar(x, y, x + 1, y, l);
                }
                if (y < 7)//Esquina Inferior Derecha
                {
                    iluminar(x, y, x + 1, y + 1, l);
                    iluminar(x, y, x + 1, y, l);
                    iluminar(x, y, x, y + 1, l);
                }
            }
        }
        public void iluminaPosibles(int x,int y,String l) 
        {
            switch (tablero[x, y].pza.roll)
            {
                case "pawn": iluminaPawn(x, y,l); 
                    break;
                case "rook": iluminaRook(x, y,l);
                    break;
                case "knight": iluminaKnight(x, y,l);
                    break;
                case "bishop": iluminaBishop(x, y,l);
                    break;
                case "queen": iluminaQueen(x, y,l);
                    break;
                case "king": iluminaKing(x, y,l);
                    break;
            }
        }
        public void iluminar(int x, int y, int i, int j, String l)
        {
            if (tablero[x, y].piezaBlanca() && !tablero[i, j].piezaBlanca())
                mosaico(i, j, l);
            if (tablero[x, y].piezaNegra() && !tablero[i, j].piezaNegra())
                mosaico(i, j, l);
        }
        public void iluminaPawn(int x, int y,String l)
        {
            if (tablero[x, y].piezaBlanca())
            {
                if (y > 0)
                {
                    if (tablero[x, y - 1].pza == null)
                        mosaico(x, y - 1, l);
                    if (x > 0)
                        if (tablero[x - 1, y - 1].piezaNegra())
                            mosaico(x - 1, y - 1, l);
                    if (x < 7)
                        if (tablero[x + 1, y - 1].piezaNegra())
                            mosaico(x + 1, y - 1, l);
                }
                if (y == 6)
                    if (tablero[x, y - 1].pza == null && tablero[x, y - 2].pza == null)
                        mosaico(x, y - 2, l);
            }
            else
            {
                if (y < 7)
                {
                    if (tablero[x, y + 1].pza == null)
                        mosaico(x, y + 1, l);
                    if (x > 0)
                        if (tablero[x - 1, y + 1].piezaBlanca())
                            mosaico(x - 1, y + 1, l);
                    if (x < 7)
                        if (tablero[x + 1, y + 1].piezaBlanca())
                            mosaico(x + 1, y + 1, l);
                }
                if (y == 1)
                    if (tablero[x, y + 1].pza == null && tablero[x, y + 2].pza == null)
                        mosaico(x, y + 2, l);
            }
        }
        public void iluminaRook(int x, int y, String l)
        {
            int i;
            if (x > 0)
            {
                i = x - 1;
                while ( i >= 0 && tablero[i,y].pza==null)
                {
                    mosaico(i, y, l);
                    i--;
                }
                if (i >= 0 && (String)tablero[i, y].btn.Tag != "l")
                    iluminar(x, y, i, y, l);
            }
            if (x < 7)
            {
                i = x + 1;
                while (i <= 7 && tablero[i, y].pza == null)
                {
                    mosaico(i, y, l);
                    i++;
                }
                if (i <= 7 && (String)tablero[i, y].btn.Tag != "l")
                    iluminar(x, y, i, y, l);
            }
            if (y > 0)
            {
                i = y - 1;
                while (i >= 0 && tablero[x, i].pza == null)
                {
                    mosaico(x, i, l);
                    i--;
                }
                if (i >= 0 && (String)tablero[x, i].btn.Tag != "l")
                    iluminar(x, y, x, i, l);
            }
            if (y < 7)
            {
                i = y + 1;
                while (i <= 7 && tablero[x, i].pza == null)
                {
                    mosaico(x, i, l);
                    i++;
                }
                if (i <= 7 && (String)tablero[x, i].btn.Tag != "l")
                    iluminar(x, y, x, i, l);
            }
        }
        public void iluminaBishop(int x, int y, String l)
        {
            int i,j;
            if (x > 0 && y > 0)
            {
                i = x - 1;
                j = y - 1;
                while (i >= 0 && j >= 0 && tablero[i, j].pza == null)
                {
                    mosaico(i, j, l);
                    i--;
                    j--;
                }
                if (i >= 0 &&j>=0&& (String)tablero[i, j].btn.Tag != "l")
                    iluminar(x, y, i, j, l);
            }
            if (x < 7 && y < 7)
            {
                i = x + 1;
                j = y + 1;
                while (i <= 7&&j<=7 && tablero[i, j].pza == null)
                {
                    mosaico(i, j, l);
                    i++;
                    j++;
                }
                if (i <= 7 && j <= 7 && (String)tablero[i, j].btn.Tag != "l")
                    iluminar(x, y, i, j, l);
            }
            if (x <7 && y > 0)
            {
                i = x + 1;
                j = y - 1;
                while (i <= 7&&j>=0 && tablero[i, j].pza == null)
                {
                    mosaico(i, j, l);
                    i++;
                    j--;
                }
                if (i <= 7 && j >= 0 && (String)tablero[i, j].btn.Tag != "l")
                    iluminar(x, y, i, j, l);
            }
            if (x > 0 && y < 7)
            {
                i = x - 1;
                j = y + 1;
                while (i >= 0&&j<=7 && tablero[i, j].pza == null)
                {
                    mosaico(i, j, l);
                    i--;
                    j++;
                }
                if (i >= 0 && j <= 7 && (String)tablero[i, j].btn.Tag != "l")
                    iluminar(x, y, i, j, l);
            }
        }
        public void iluminaQueen(int x, int y, String l)
        {
            iluminaRook(x, y,l);
            iluminaBishop(x, y,l);
        }
        public void iluminaKnight(int x, int y, String l)
        {
            if (x > 1)
            {
                if (y > 1)//Esquina Superior Izquierda
                {
                    iluminar(x, y, x - 2, y - 1, l);
                    iluminar(x, y, x - 1, y - 2, l);
                }
                if (y < 6)//Esquina Inferior Izquierda
                {
                    iluminar(x, y, x - 2, y + 1, l);
                    iluminar(x, y, x - 1, y + 2, l);
                }
            }
            if (x < 6)
            {
                if (y > 1)//Esquina Superior Derecha
                {
                    iluminar(x, y, x + 1, y - 2, l);
                    iluminar(x, y, x + 2, y - 1, l);
                }
                if (y < 6)//Esquina Inferior Derecha
                {
                    iluminar(x, y, x + 2, y + 1, l);
                    iluminar(x, y, x + 1, y + 2, l);
                }
            }
            if(x > 0 && y > 1)
                iluminar(x, y, x - 1, y - 2, l);
            if (x > 1 && y > 0)
                iluminar(x, y, x - 2, y - 1, l);
            if (x > 0 && y < 6)
                iluminar(x, y, x - 1, y + 2, l);
            if (x > 1 && y < 7)
                iluminar(x, y, x - 2, y + 1, l);
            
            if (x < 7 && y > 1)
                iluminar(x, y, x + 1, y - 2, l);
            if (x < 6 && y > 0)
                iluminar(x, y, x + 2, y - 1, l);
            if (x < 7 && y < 6)
                iluminar(x, y, x + 1, y + 2, l);
            if (x < 6 && y < 7)
                iluminar(x, y, x + 2, y + 1, l);
        }

        #endregion

        public void inicializarTablero()
        {
            int i,j;
            for (i = 0; i < 8; i++)//Dibujo Tablero
                for (j = 0; j < 8; j++)
                {
                    tablero[i, j] = new Casilla();
            		tablero[i, j].btn=new Button();
                    tablero[i, j].btn.Size = new System.Drawing.Size(70, 70);
                    tablero[i, j].btn.Location = new System.Drawing.Point(i * 69, j * 69);
                    tablero[i, j].btn.FlatAppearance.BorderSize = 0;
                    mosaico(i, j,"");
                    tablero[i, j].btn.Click += new EventHandler(onButtonClick);
                    this.Controls.Add(tablero[i, j].btn);
                }
            comenzarNuevo();
        }
        public void comenzarNuevo()
        { 
            int i,j;
            seleccionada = false;
            terminado = false;
            turnoBlanca = true;
            amenaza = false;
            iluminado = 0;
            sec = 0;
            min = 0;
            hr = 0;
            //Piezaz Negras Arriba
            tablero[0, 0].pza = new Pieza("rook", false);
            tablero[1, 0].pza = new Pieza("knight", false);
            tablero[2, 0].pza = new Pieza("bishop", false);
            tablero[3, 0].pza = new Pieza("queen", false);
            tablero[4, 0].pza = new Pieza("king", false);
            tablero[5, 0].pza = new Pieza("bishop", false);
            tablero[6, 0].pza = new Pieza("knight", false);
            tablero[7, 0].pza = new Pieza("rook", false);
            for(i=0; i < 8; i++)
                tablero[i, 1].pza = new Pieza("pawn", false);
            for (i = 0; i < 8; i++)
                for (j = 2; j < 6; j++)
                    tablero[i, j].pza = null;
            //tablero Blancas Abajo
            for (i = 0; i < 8; i++)
                tablero[i, 6].pza = new Pieza("pawn", true);
            tablero[0, 7].pza = new Pieza("rook", true);
            tablero[1, 7].pza = new Pieza("knight", true);
            tablero[2, 7].pza = new Pieza("bishop", true);
            tablero[3, 7].pza = new Pieza("queen", true);
            tablero[4, 7].pza = new Pieza("king", true);
            tablero[5, 7].pza = new Pieza("bishop", true);
            tablero[6, 7].pza = new Pieza("knight", true);
            tablero[7, 7].pza = new Pieza("rook", true);
            for (i = 0; i < 8; i++)
                for (j = 0; j < 8; j++)
                {
                    mosaico(i, j,"");
                    tablero[i, j].colocaPieza();
                }
        }
        
        #region Acciones de diseño
        private void frmAjedrez_Load(object sender, EventArgs e)
        {

        }
        private void btnReiniciar_Click(object sender, EventArgs e)
        {
            comenzarNuevo();
        }
        private void btnRendirse_Click(object sender, EventArgs e)
        {
            tm = false;
            if (turnoBlanca)
                MessageBox.Show("Doradas Gana","Ganador");
            else
                MessageBox.Show("Blancas Gana", "Ganador");
            DialogResult res = MessageBox.Show("Comenzar Nuevo?", "Juego Terminado");
            tm = true;
            comenzarNuevo();
        }
        private void tiempo_Tick(object sender, EventArgs e)
        {
            if(tm)
                sec++;
            if (sec == 60)
            {
                sec = 0;
                min++;
            }
            if (min == 60)
            {
                min = 0;
                hr++;
            }
            String s,m,h;
            if (sec < 10)
                s = "0" + sec.ToString();
            else
                s = sec.ToString();
            if (min < 10)
                m = "0" + min.ToString();
            else
                m = min.ToString();
            if (hr < 10)
                h = "0" + hr.ToString();
            else
                h = hr.ToString();
            btnTiempo.Text = h+":"+m+":"+s;
        }
        #endregion
    }
}
