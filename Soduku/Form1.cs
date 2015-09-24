using System;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;

namespace Soduku
{
    public partial class Form1 : Form
    {
        readonly int[,] _ruta = new int[9,9];
        readonly TextBox[,] _textbox = new TextBox[9,9];
        bool _lösning;
        public Form1()
        {
            InitializeComponent();
            tableLayoutPanel1.RowCount = 9;
            tableLayoutPanel1.ColumnCount = 9;
            tableLayoutPanel1.Top = 20;
            tableLayoutPanel1.Left = 20;
            tableLayoutPanel1.Width = 225;
            tableLayoutPanel1.Height = 225;
            tableLayoutPanel1.CellPaint += tableLayoutPanel1_CellPaint;
            label1.Text = "";
            label1.ForeColor = Color.Red;
            for (int i = 0; i < 9; i++)
            {
                tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 25));
                tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 25));
            }
            Initieratextboxa();
            for (int y = 0; y < 9; y++)
                for (int x = 0; x < 9; x++)
                {
                    _textbox[x, y] = new TextBox {Height = 25, Width = 25, Font = new Font("Arial", 9), Text = ""};
                    tableLayoutPanel1.Controls.Add(_textbox[x, y], x, y);
                }
        }

        public void Initieratextboxa()
        {
            for (int x = 0; x < 9; x++)
                for (int y = 0; y < 9; y++) _ruta[x, y] = 0;
        }

        public bool LäsIn()
        {
            bool fel=false;
            for (int x = 0; x < 9; x++)
                for (int y = 0; y < 9; y++)
                {
                    string s=_textbox[x,y].Text;
                    int svar;
                    if (s == "") _ruta[x, y] = 0;
                    else if (int.TryParse(s,out svar)){
                        if (svar<0) svar=-svar;
                        _ruta[x,y]=svar%10;
                    }
                    else fel=true;
                }
            return fel;
        }

        public bool Kontroll(){
            bool ok=true;
            for (int x = 0; x < 9; x++)
                for (int y = 0; y < 9; y++)
                    if (_ruta[x, y] > 0) ok=Kontroll(x,y,0);
            return ok;
        }

        public bool Kontroll(int x, int y, int nr)
        {
            bool ok = true;
            for (int q = 0; q < 9; q++)
            {
                if (nr == 0 && _ruta[x, y] == _ruta[q, y] && x != q) ok = false;
                if (nr == 0 && _ruta[x, y] == _ruta[x, q] && y != q) ok = false;
                if (nr > 0 && _ruta[q, y] == nr) ok = false;
                if (nr > 0 && _ruta[x, q] == nr) ok = false;
            }
            int i, j;
            if (x < 3) i = 0;
            else if (x < 6) i = 3;
            else i = 6;
            int k = i + 3;
            if (y < 3) j = 0;
            else if (y < 6) j = 3;
            else j = 6;
            int l = j + 3;
            for (int a = i; a < k; a++)
                for (int b = j; b < l; b++)
                {
                    if (nr == 0 && _ruta[a, b] == _ruta[x, y] && a != x && b != y) ok=false;
                    if (nr > 0 && _ruta[a, b] == nr) ok=false;
                }
            return ok;
        }

        public void Lös(int x, int y, int nr)
        {
            int i = 11, klar = 0;
             _ruta[x, y] = nr;
            for (int ya = 0; ya < 9; ya++)
                for (int xa = 0; xa < 9; xa++)
                {
                    if (_ruta[xa, ya] == 0 && i == 11)
                    {
                        x = xa;
                        y = ya;
                        i = 0;
                    }
                    else klar++;
                }
            if (klar == 81)
            {
                _lösning = true;
                for (int a = 0; a < 9; a++)
                    for (int b = 0; b < 9; b++)
                        _textbox[a, b].Text = _ruta[a, b].ToString(CultureInfo.InvariantCulture);
                label1.Text = @"Lösning funnen!";
            }
            else for (i = 1; i < 10; i++) if (Kontroll(x, y, i) && !_lösning) Lös(x, y, i);
            _ruta[x, y] = 0;
        }                                     

        private void button1_Click(object sender, EventArgs e)
        {
            int xs=10;
            int nr=10;
            if (LäsIn()) label1.Text = @"Endast siffror får användas i sodokut!";
            else if (!Kontroll()) label1.Text = @"Det är något fel på siffrorna i sodokut!";
            else
            {
                label1.Text = "";
                _lösning = false;
                for(int y=0;y<9;y++)
                    for (int x=0;x<9;x++)
                        if (_ruta[x, y] == 0 && xs == 10)
                        {
                            xs = x;
                            int ys = y;
                            for (int i = 1; i < 10; i++)
                            {
                                if (Kontroll(xs, ys, i)) nr = i;
                                if (xs < 10 && ys < 10 && !_lösning) Lös(xs, ys, nr);
                            }
                        }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            for(int x=0;x<9;x++)
                for (int y = 0; y < 9; y++)
                {
                    _textbox[x, y].Text = "";
                }
            label1.Text = "";
        }

        void tableLayoutPanel1_CellPaint(object sender, TableLayoutCellPaintEventArgs e)
        {
            var penna = new Pen(Color.Black, 2);
            for (int i = 0; i <= 225; i += 75)
            {
                e.Graphics.DrawLine(penna, i, 0, i, 225);
                e.Graphics.DrawLine(penna, 0, i+1, 225, i+1);
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            var penna = new Pen(Color.Black, 2);
            e.Graphics.DrawLine(penna,20,246,245,246);
            e.Graphics.DrawLine(penna, 20, 20, 19, 246);
            e.Graphics.DrawLine(penna, 245, 20, 245, 246);
        }
    }
}
