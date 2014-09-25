FORM1:

                       
using

System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Snake_beta_
{
    public partial class Form1 : Form
    {
        Random ranComida = new Random(); // comida aleatoria //
        Graphics tablero; // creacion fondo de juego //
        Snake serpiente = new Snake(); // para llamar a la serpiente //
        Comida comida = new Comida();

        bool izquierda = false; // para indicar cuando pulsa la tecla izquierda //
        bool derecha = false; // para indicar cuando pulsa la tecla derecha //
        bool abajo = false; // para indicar cuando pulsa la tecla abajo //
        bool arriba = false; // para indicar cuando pulsa la tecla arriba //

        int puntuacion = 0; // almacenar puntuacion //

        // para el numero aleatorio //
        int _numero1;

        public Form1()
        {
            InitializeComponent();

            comida.crearNumero(ranComida); // para llamar a la clase comida(aleatoria) //
            _numero1 = generarNumero(); 
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e) // funcion para controlar la serpiente con las teclas //
        {
            if (e.KeyData == Keys.Space) 
            {
                timer1.Enabled = true;
                abajo = false;
                arriba = false;
                derecha = false;
                izquierda = false;
            }

            if (e.KeyData == Keys.Down && arriba == false)
            {
                abajo = true;
                arriba = false;
                derecha = false;
                izquierda = false;
            }

            if (e.KeyData == Keys.Up && abajo == false)
            {
                abajo = false;
                arriba = true;
                derecha = false;
                izquierda = false;
            }

            if (e.KeyData == Keys.Left && derecha == false)
            {
                abajo = false;
                arriba = false;
                derecha = false;
                izquierda = true;
            }

            if (e.KeyData == Keys.Right && izquierda == false)
            {
                abajo = false;
                arriba = false;
                derecha = true;
                izquierda = false;
            }

            if (e.KeyData == Keys.Escape)
            {
                timer1.Stop();
                abajo = false;
                arriba = false;
                izquierda = false;
                derecha = false;
            }
        }

        private void timer1_Tick(object sender, EventArgs e) // timer 1 //
        {
            casilleroPto.Text = Convert.ToString(puntuacion);

            if (abajo) { serpiente.moverAbajo(); }
            if (arriba) { serpiente.moverArriba(); }
            if (izquierda) { serpiente.moverIzquierda(); }
            if (derecha) { serpiente.moverDerecha(); }

            for (int i = 0; i < serpiente.SnakeRec.Length; i++)
            {
                if (serpiente.SnakeRec[i].IntersectsWith(comida._numeroRec))
                {
                    puntuacion += 10;
                    serpiente.crecerAvanzar(generarNumero());
                    _numero1 = generarNumero(); 
                    comida.localizarNumero(ranComida);
                }
            }
            pictureBox1.Refresh();
            this.Invalidate();
            chocarSerpiente();
        }

        public void chocarSerpiente() // funcion que detecta cuando se choca la serpiente contra si misma o alguna pared // 
        {
            for (int i = 1; i < serpiente.SnakeRec.Length; i++) // chocar la serpiente contra si misma //
            {
                if (serpiente.SnakeRec[0].IntersectsWith(serpiente.SnakeRec[i]))
                {
                    reiniciarJuego();
                }
            }

            if (serpiente.SnakeRec[0].X < 0 || serpiente.SnakeRec[0].X > 290) // chocar la serpiente contra eje X //
            {
                reiniciarJuego();
            }

            if (serpiente.SnakeRec[0].Y < 0 || serpiente.SnakeRec[0].Y > 290) // chocar la serpiente contra eje Y //
            {
                reiniciarJuego();
            }
        }

        public void reiniciarJuego() // funcion reiniciar juego //
        {
            timer1.Enabled = false;
            MessageBox.Show("Game Over.Tu puntuacion es de: " + puntuacion + " puntos");
            puntuacion = 0;
            casilleroNivel.Text = "1";
            serpiente = new Snake();
        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e) // tablero desarrollo juego //
        {
            tablero = e.Graphics;
            comida.dibujarNumero(tablero,Convert.ToString(_numero1));
            serpiente.dibujarSnake(tablero);
        }

        private void casilleroPto_TextChanged(object sender, EventArgs e) // para indicar la puntuacion //
        {

        }

        private void casilleroNivel_TextChanged(object sender, EventArgs e) // para indicar el nivel de juego //
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private int generarNumero() // funcion que genera el numero aleatorio //
        {
            Random _numeroAleatorio = new Random();
            int _numero = _numeroAleatorio.Next(1, 7);
            return _numero;
        }
    }
}

SNAKE:

using System;
using System.Collections.Generic;
//using System.Drawing;//
using System.Linq;
using System.Text;

namespace Snake_beta_
{
    public class Snake
    {
        private Rectangle[] serpienteRec; // cuerpo de la serpiente(rectangulo) //
        private SolidBrush brocha; // color de la serpiente //
        private int _x, _y, _ancho, _largo, _i = 0; // _x,_y localizacion serpiente(cuadricula), _width,_height tamaño serpiente(rectangulo) //
        
        public Rectangle[] SnakeRec
        {
            get { return serpienteRec; }
        }

        public Snake() // creacion cuerpo serpiente //
        {
            serpienteRec = new Rectangle[3]; // cuando se inicia la serpiente tiene longitud 3 cuadrados //
            brocha = new SolidBrush(Color.GreenYellow); // color de la serpiente //

            _x = 20; // localizacion serpiente comienzo juego,eje X //
            _y = 0; // localizacion serpiente comienzo juego,eje Y //
            _ancho = 10; // tamaño inicial serpiente //
            _largo = 10; // tamaño inicial serpiente //

            for (int i = 0; i < serpienteRec.Length; i++)
            {
                serpienteRec[i] = new Rectangle(_x, _y, _ancho, _largo);
                _x -= 10;
            }
        }

        public void dibujarSnake(Graphics tablero) // dibuja serpiente(forma y color) //
        {
            foreach (Rectangle rec in serpienteRec)
            {
                tablero.FillRectangle(brocha, rec);
            }
        }

        public void dibujarSnake() // sobrecarge el metodo "dibujarSerpiente" y crea una funcion que mueve la serpiente //
        {
            for (int i = serpienteRec.Length - 1; i > 0; i--)
            {
                serpienteRec[i] = serpienteRec[i - 1]; // para hacer cada rectangulo igual al anterior //
            }
        }

        public void moverAbajo() // funcion mover abajo serpiente //
        {
            dibujarSnake();
            serpienteRec[0].Y += 10;
        }

        public void moverArriba() // funcion mover arriba serpiente //
        {
            dibujarSnake();
            serpienteRec[0].Y -= 10;
        }

        public void moverIzquierda() // funcion mover izquierda serpiente //
        {
            dibujarSnake();
            serpienteRec[0].X -= 10;
        }

        public void moverDerecha() // funcion mover derecha serpiente //
        {
            dibujarSnake();
            serpienteRec[0].X += 10;
        }

        public void crecerAvanzar(int numero) // funcion crecer y avanzar serpiente //
        {
            do
            {
                List<Rectangle> rec = serpienteRec.ToList();
                rec.Add(new Rectangle(serpienteRec[serpienteRec.Length - 1].X, serpienteRec[serpienteRec.Length - 1].Y, _ancho, _largo));
                serpienteRec = rec.ToArray();
                _i++;
            }
            while (_i < numero);
            _i = 0;
        }
    }
}
COMIDA:

using System;
using System.Collections.Generic;
//using System.Drawing;//
using System.Linq;
using System.Text;

namespace Snake_beta_
{
    public class Comida
    {
        private int _x, _y, _ancho, _largo, _x1, _y1;  // _x,_y localizacion comida en la cuadricula; _width,_height tamaño de la comida //
        private string _i;

        private SolidBrush _brocha;
        private SolidBrush _brocha1; 

        public Rectangle _numeroRec; 

        public void crearNumero(Random randNumero) // funcion que genera el numero aleatorio //
        {
            _x = _x1 = randNumero.Next(0, 29) * 10;
            _y = _y1 = randNumero.Next(0, 29) * 10;

           // _brocha = new SolidBrush(Color.Red); //
           // _brocha1 = new SolidBrush(Color.Yellow); //

            _ancho = 10;
            _largo = 10;

            _numeroRec = new Rectangle(_x, _y, _ancho, _largo);
        }

        public void localizarNumero(Random randNumero) // funcion que localiza el numero en el tablero //
        {

            _x = _x1 = randNumero.Next(0, 29) * 10;
            _y = _y1 = randNumero.Next(0, 29) * 10;
        }

        public void dibujarNumero(Graphics tablero,string numero) // Graphics tablero, string numero))
        {
            _i = numero;

            _numeroRec.X = _x;
            _numeroRec.Y = _y;
            tablero.FillRectangle(_brocha, _numeroRec);
            tablero.DrawString(_i, new Font("Arial", 7), _brocha1, _x1, _y1);
        }
    }
}
Se