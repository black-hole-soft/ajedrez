using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Ajedrez
{
    public partial class frmAjedrez : Form
    {
        public Casilla[,] tablero = new Casilla[8, 8];
        public bool seleccionada=false,turnoBlanca=true;
        public int selectX, selectY,iluminado=0,amenaza=0;
        public Rey reyb = new Rey(3, 6), reyn = new Rey(3, 0);
        
        public frmAjedrez()
        {
            InitializeComponent();
            inicializarTablero();
        }
        public void mosaico(int x, int y, String l) //Marmol Blanco o Negro
        {
            if ((x + y) % 2 == 0)
                tablero[x, y].btn.BackgroundImage = Image.FromFile(l + "wmarbel.png");
            else
                tablero[x, y].btn.BackgroundImage = Image.FromFile(l + "bmarbel.png");
            iluminado++;
            tablero[x, y].btn.BackgroundImage.Tag = l;
        }
        public void onButtonClick(object source, EventArgs e)
        {
            Button b = (Button)source;
            int x=b.Location.X / 69, y=b.Location.Y / 69;
            if (seleccionada)
            {
                seleccionada = false;
                if ((String)tablero[x, y].btn.BackgroundImage.Tag == "l")//Mueve pieza
                {
                    //Posicion del Rey
                    if (selectX == reyb.x && selectY == reyb.y)
                    { reyb.x = x; reyb.y = y; }
                    if (selectX == reyn.x && selectY == reyn.y)
                    { reyn.x = x; reyn.y = y; }
                    //------------------------------------------
                    iluminaPosibles(selectX, selectY, "");
                    if (jaqueAlRey(x, y))
                    {
                    }
                    else
                    {
                        tablero[x, y].pza = tablero[selectX, selectY].pza;
                        tablero[selectX, selectY].pza = null;
                        colocaPieza(selectX, selectY);
                        if (piezaBlanca(x, y) && y == 0 && tablero[x, y].pza.roll == "pawn")
                            tablero[x, y].pza.roll = "queen";
                        if (piezaNegra(x, y) && y == 7 && tablero[x, y].pza.roll == "pawn")
                            tablero[x, y].pza.roll = "queen";
                        colocaPieza(x, y);
                        if (turnoBlanca)
                            turnoBlanca = false;
                        else
                            turnoBlanca = true;
                    }
                }
                else
                    iluminaPosibles(selectX, selectY, "");
            }
            else//Selecciona Pieza
            {
                if (tablero[x, y].pza != null)
                {
                    if ((turnoBlanca && piezaBlanca(x, y)) || (!turnoBlanca && piezaNegra(x, y)))
                    {
                        iluminado = 0;
                        iluminaPosibles(x, y, "l");
                        if (iluminado > 0)
                            seleccionada = true;
                        selectX = x;
                        selectY = y;
                    }
                    #region Messagebox para Turnos 
                    else
                    {
                        String pzas;
                        if (turnoBlanca)
                            pzas = "Blancas";
                        else
                            pzas = "Doradas";
                        MessageBox.Show("Es el turno de las " + pzas, "Turno");
                    }
                    #endregion
                }
            }
        }
        bool jaqueAlRey(int x,int y) 
        {

            return false;
        }

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
            if (piezaBlanca(x, y) && !piezaBlanca(i, j))
                mosaico(i, j, l);
            if (piezaNegra(x, y) && !piezaNegra(i, j))
                mosaico(i, j, l);
        }
        public void iluminaPawn(int x, int y,String l)
        {
            if (piezaBlanca(x, y))
            {
                if (y > 0)
                {
                    if (tablero[x, y - 1].pza == null)
                        mosaico(x, y - 1, l);
                    if (x > 0)
                        if (piezaNegra(x - 1, y - 1))
                            mosaico(x - 1, y - 1, l);
                    if (x < 7)
                        if (piezaNegra(x + 1, y - 1))
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
                        if (piezaBlanca(x - 1, y + 1))
                            mosaico(x - 1, y + 1, l);
                    if (x < 7)
                        if (piezaBlanca(x + 1, y + 1))
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

        public void colocaPieza(int x,int y)
        {
            if (tablero[x, y].pza == null)
                tablero[x, y].btn.Image = null;
            else
            {
                String c;
                if (piezaBlanca(x, y))
                    c = "w";
                else
                    c = "g";
                tablero[x, y].btn.Image = Image.FromFile(c + tablero[x, y].pza.roll + ".png");
            }
        }
        public bool piezaBlanca(int x, int y)
        {
            if (tablero[x, y].pza!=null)
                if (tablero[x, y].pza.color)
                    return true;
            return false;
        }
        public bool piezaNegra(int x, int y)
        {
            if (tablero[x, y].pza != null)
                if (!tablero[x, y].pza.color)
                    return true;
            return false;
        }
        public void inicializarTablero()
        {
            int i,j;
            //Piezaz Negras Arriba
            tablero[0, 0] = new Casilla("rook", false);
            tablero[1, 0] = new Casilla("knight", false);
            tablero[2, 0] = new Casilla("bishop", false);
            tablero[3, 0] = new Casilla("queen", false);
            tablero[4, 0] = new Casilla("king", false);
            tablero[5, 0] = new Casilla("bishop", false);
            tablero[6, 0] = new Casilla("knight", false);
            tablero[7, 0] = new Casilla("rook", false);
            for(i=0; i < 8; i++)
                tablero[i, 1] = new Casilla("pawn", false);
            for (i = 0; i < 8; i++)
                for (j = 2; j < 6; j++)
            	tablero[i, j] = new Casilla();
            //tablero Blancas Abajo
            for (i = 0; i < 8; i++)
                tablero[i, 6] = new Casilla("pawn", true);
            tablero[0, 7] = new Casilla("rook", true);
            tablero[1, 7] = new Casilla("knight", true);
            tablero[2, 7] = new Casilla("bishop", true);
            tablero[3, 7] = new Casilla("queen", true);
            tablero[4, 7] = new Casilla("king", true);
            tablero[5, 7] = new Casilla("bishop", true);
            tablero[6, 7] = new Casilla("knight", true);
            tablero[7, 7] = new Casilla("rook", true);
            
            for (i = 0; i < 8; i++)//Dibujo Tablero
                for (j = 0; j < 8; j++)
                {
            		tablero[i, j].btn=new Button();
                    tablero[i, j].btn.Size = new System.Drawing.Size(70, 70);
                    tablero[i, j].btn.Location = new System.Drawing.Point(i * 69, j * 69);
                    tablero[i, j].btn.FlatAppearance.BorderSize = 0;
                    mosaico(i, j,"");
                    colocaPieza(i,j);
                    tablero[i, j].btn.Click += new EventHandler(onButtonClick);
                    this.Controls.Add(tablero[i, j].btn);
                }
        }

        private void frmAjedrez_Load(object sender, EventArgs e)
        {

        }
    }
}
