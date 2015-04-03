using System;
using System.Drawing;
using System.Windows.Forms;

namespace Game_Checkers
{
    public partial class Game : Form
    {
        
        Graphics _graphics;
        Bitmap _bitmap;
        Pen _pen;
        //шахова дошка
        Rectangle[,] _rectangle;
        //прямокутник жовтий
        Rectangle _rec;
        //закраска для чорнох квадратів
        Brush _brushBlack;
        //закраска для білих квадратів
        Brush _brushWhite;
        //закраска для червоних шашок
        Brush _brushCheckers1;
        //закраска для синіх шашок
        Brush _brushCheckers2;
        //координати ходів і прості змінні
        int _x, _y, _xPrew, _yPrew, _iNext, _jNext, _i, _j,_iDeath,_jDeath;
        //колір шашок в комірках
        string[,] _color;
        bool _one,_faith;
        //конструктор гри
        public Game()
        {
            _faith = false;
            _one = true;
            InitializeComponent();
            pictureBox_Initialization();
            Color_in_cell();
            Chessboard();
        }

        public void pictureBox_Initialization()
        {
            _bitmap = new Bitmap(480,480);
            //створюємо графіку для BitMap 
            _graphics = Graphics.FromImage(_bitmap);
            //колір ліній квадрата(жовтий)
            _pen = new Pen(Color.Yellow, 3);
            //закраска для чорної клітки
            _brushBlack = new SolidBrush(Color.Black);
            //закраска для білої клітки
            _brushWhite = new SolidBrush(Color.White);
            //закраска для червоної шашки
            _brushCheckers1 = new SolidBrush(Color.Red);
            //закраска для синьої шашки
            _brushCheckers2 = new SolidBrush(Color.Blue);
            //створення нового обєкта(дошки)
            _rectangle = new Rectangle[8, 8];
            //колір шашки в комірці
            _color = new string[8, 8];
        }

        //метод який реалізує початковий колір шашок в комірках
        public void Color_in_cell()
        {
            for (int i = 0; i < 8; i++)
                for (int j = 0; j < 8; j++)
                {
                    if (i < 3 && (i + j) % 2 == 0) _color[i, j] = "Red";
                    else
                        if (i > 4 && (i + j) % 2 == 0) _color[i, j] = "Blue";
                        else
                            _color[i, j] = "White";
                }
        }

        public void Chessboard()
        {
            _x = 0;
            _y = 0;
            //ініціалізація стану дошки
            for (int i = 0; i < 8; i++)
                for (int j = 0; j < 8; j++)
                {
                    if (j == 0)
                    {
                        _x = 0;
                        _y = i * 60;
                    }
                    else _x += 60;
                    _rectangle[i, j] = new Rectangle(_x, _y, 60, 60);                  
                }
        }
        // запуск гри
        public void Start_Click(object sender, EventArgs e)
        {
            Chessboard();
            Color_in_cell();
            //ініціалізація стану дошки і шашок в pictureBox1
            Rectangle_initialization();
            pictureBox1.Image = _bitmap;
        }

        //ініціалізація стану дошки і шашок
        public void Rectangle_initialization()
        {           
            //ініціалізація стану дошки
            for (int i = 0; i < 8; i++)
                for (int j = 0; j < 8; j++)
                {                  
                    if ((i + j) % 2 == 0) _graphics.FillRectangle(_brushBlack, _rectangle[i, j]); 
                    else 
                        _graphics.FillRectangle(_brushWhite, _rectangle[i,j]);                
                }
            //ініціалізація стану шашок
            for (int i = 0; i < 8; i++)
                for (int j = 0; j < 8; j++)
                    if (_color[i, j].CompareTo("Red") == 0) _graphics.FillEllipse(_brushCheckers1, _rectangle[i, j]); else
            if (_color[i, j].CompareTo("Blue") == 0) _graphics.FillEllipse(_brushCheckers2, _rectangle[i, j]);
        }
        //вихід з гри
        private void Exit_Click(object sender, EventArgs e)
        {
            Close();
        }

        //Хід шашок, удар
        private void Mouse_Click(object sender, MouseEventArgs e)
        {
            pictureBox1.Image = _bitmap;
            //координати кліку мишки
            _x = (int) e.X;
            _y = (int) e.Y;
            
            _x = (_x / 60) * 60;
            _y = (_y / 60) * 60;
            //прямокутник жовтий(показує в яку клітку клікнули)
            _rec = new Rectangle(_x,_y,58,58);
            //координати комірки rectangle[i,j] попереднього кліку
            _j = _xPrew/60;
            _i = _yPrew/60;
            //координати комірки rectangle[i_next,j_next] теперішнього кліку
            _iNext = _y/60;
            _jNext = _x/60;
            //координати комірки rectangle[i_death,j_death] яку вдарили
            _iDeath = 0;
            _jDeath = 0;
            if (_i - 2 == _iNext && _j - 2 == _jNext) { _iDeath = _i - 1; _jDeath = _j - 1; }
            if (_i - 2 == _iNext && _j + 2 == _jNext) { _iDeath = _i - 1; _jDeath = _j + 1; }
            if (_i + 2 == _iNext && _j - 2 == _jNext) { _iDeath = _i + 1; _jDeath = _j - 1; }
            if (_i + 2 == _iNext && _j + 2 == _jNext) { _iDeath = _i + 1; _jDeath = _j + 1; }
            

            //прибираємо жовтий колір з попереднього ходу
            if ((_i + _j) % 2 == 0) _graphics.FillRectangle(_brushBlack, _rectangle[_i, _j]); else _graphics.FillRectangle(_brushWhite, _rectangle[_i, _j]);
            if (_color[_i, _j].CompareTo("Red") == 0) _graphics.FillEllipse(_brushCheckers1, _rectangle[_i, _j]);
            else
                if (_color[_i, _j].CompareTo("Blue") == 0) _graphics.FillEllipse(_brushCheckers2, _rectangle[_i, _j]);

            //ставимо жовтий колір
            if (_color[_iNext, _jNext].CompareTo("White") != 0) _graphics.DrawRectangle(_pen, _rec); else
            
            //хід шашки
            if (_color[_i, _j].CompareTo("White") != 0 && Math.Abs(_jNext - _j) == 1 && Math.Abs(_iNext - _i) == 1
                && _color[_iNext, _jNext].CompareTo("White") == 0 && ((_color[_i, _j].CompareTo("Red") == 0 && (_i - _iNext < 0)) 
                || (_color[_i, _j].CompareTo("Blue") == 0 && (_i - _iNext > 0))) && ((_color[_i,_j].CompareTo("Red") == 0 && _one) || (_color[_i,_j].CompareTo("Blue") == 0 && !_one)) )
            {
                if (_one) _one = false; else _one = true;
                _color[_iNext, _jNext] = _color[_i,_j];
                if (_color[_i, _j].CompareTo("Red") == 0) _graphics.FillEllipse(_brushCheckers1, _rectangle[_iNext, _jNext]);
                else
                    _graphics.FillEllipse(_brushCheckers2, _rectangle[_iNext, _jNext]);
                _color[_i, _j] = "White";
                _graphics.FillRectangle(_brushBlack,_rectangle[_i,_j]);
            } else
                //удар противника
                if (_color[_i, _j].CompareTo("White") != 0 && _color[_iNext, _jNext].CompareTo("White") == 0 
                    && Math.Abs(_jNext - _j) == 2 && Math.Abs(_iNext - _i) == 2
                    && _color[_i, _j].CompareTo(_color[_iDeath, _jDeath]) != 0 && _color[_iNext, _jNext].CompareTo(_color[_iDeath, _jDeath]) != 0
                    && ((_color[_i, _j].CompareTo("Red") == 0 && _one) || (_color[_i, _j].CompareTo("Blue") == 0 && !_one)))
            {
                if (_one) _one = false; else _one = true;
                _color[_iNext, _jNext] = _color[_i, _j];
                if (_color[_i, _j].CompareTo("Red") == 0) _graphics.FillEllipse(_brushCheckers1, _rectangle[_iNext, _jNext]);
                else
                    _graphics.FillEllipse(_brushCheckers2, _rectangle[_iNext, _jNext]);
                _color[_i, _j] = "White";
                _color[_iDeath, _jDeath] = "White";
                _graphics.FillRectangle(_brushBlack, _rectangle[_i, _j]);
                _graphics.FillRectangle(_brushBlack, _rectangle[_iDeath, _jDeath]);
            }
             else
                    //показуємо куди нажав ігрок
                    _graphics.DrawRectangle(_pen, _rec);
            //присвоєння поперднім координатам теперішні(кліка мишки)
            _xPrew = _x;
            _yPrew = _y;
        }    
    }
}
