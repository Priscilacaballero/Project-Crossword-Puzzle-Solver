using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace CRUCIGRAMAFIN
{
    public partial class Form1 : Form
    {
        def def_vent = new def();
        List<id_celdas> idc = new List<id_celdas>();
        public String puzzle_file = Application.StartupPath + "\\puzzles\\cruci_1.pzl";

        public Form1()
        {
            construirlista();
            InitializeComponent();

        }

        private void archivoToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            inicializartab();
            def_vent.SetDesktopLocation(this.Location.X + this.Width + 1, this.Location.Y);
            def_vent.StartPosition = FormStartPosition.Manual;
            def_vent.Show();
            def_vent.defs.AutoResizeColumns();
            //formatcelda(5, 5, "B");

        }
        private void inicializartab()
        {
            tab.BackgroundColor = Color.Black;
            tab.DefaultCellStyle.BackColor = Color.Black;
            for (int i = 0; i < 21; i++)
                tab.Rows.Add();

            // tamano para columnas
            foreach (DataGridViewColumn c in tab.Columns)
                c.Width = tab.Width / tab.Columns.Count;

            // tamano para filas
            foreach (DataGridViewRow r in tab.Rows)
                r.Height = tab.Height / tab.Rows.Count;

            for(int row=0;row< tab.Rows.Count; row++)
            {
                for(int col=0;col< tab.Columns.Count; col++)
                {
                    tab[col, row].ReadOnly = true;
                }        
            }

            foreach(id_celdas i in idc)
            {
                int inic_col = i.X;
                int inic_fil = i.Y;
                char[] word = i.word.ToCharArray();
                for(int j=0; j<word.Length; j++)
                {
                    if (i.direction.ToUpper() == "HORIZ")
                        formatcelda(inic_fil, inic_col+j, word[j].ToString());
                    if (i.direction.ToUpper() == "VERT")
                        formatcelda(inic_fil+j, inic_col, word[j].ToString());
                }
            }
        }

        private void formatcelda(int row, int colum, string letra)
        {
            DataGridViewCell c = tab[colum, row];
            c.Style.BackColor = Color.White;
            c.ReadOnly = false;
            c.Style.SelectionBackColor = Color.Cyan;
            c.Tag = letra;
        }

        private void salirToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void construirlista()
        {
            String linea = "";
            using (StreamReader s = new StreamReader(puzzle_file))
            {
                linea = s.ReadLine();
                while((linea=s.ReadLine() )!= null)
                {
                    String[] l = linea.Split('|');
                    idc.Add(new id_celdas(Int32.Parse(l[0]), Int32.Parse(l[1]), l[2], l[3], l[4], l[5]));
                    def_vent.defs.Rows.Add(new String[] { l[3], l[2], l[5] });
                }

            }
        }
        private void Form1_LocationChanged(object sender, EventArgs e)
        {
            def_vent.SetDesktopLocation(this.Location.X+this.Width+1,this.Location.Y);
        }

        private void tab_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                tab[e.ColumnIndex, e.RowIndex].Value = tab[e.ColumnIndex, e.RowIndex].Value.ToString().ToUpper();
            }
            catch { }

            try
            {
                if (tab[e.ColumnIndex, e.RowIndex].Value.ToString().ToUpper().Equals(tab[e.ColumnIndex, e.RowIndex].Tag.ToString().ToUpper()))
                    tab[e.ColumnIndex, e.RowIndex].Style.ForeColor = Color.DarkGreen;
                else
                    tab[e.ColumnIndex, e.RowIndex].Style.ForeColor = Color.Red;
            }
            catch { }

            try
            {
                if (tab[e.ColumnIndex, e.RowIndex].Value.ToString().Length > 1)
                    tab[e.ColumnIndex, e.RowIndex].Value = tab[e.ColumnIndex, e.RowIndex].Value.ToString().Substring(0, 1);
            }
            catch { }
        }

        private void abrirCrucigramaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd= new OpenFileDialog();
            ofd.Filter = "Archivos Puzzle|*.pzl";
            if(ofd.ShowDialog() == DialogResult.OK)
            {
                puzzle_file = ofd.FileName;
                tab.Rows.Clear();
                def_vent.defs.Rows.Clear();
                idc.Clear();
                construirlista();
                inicializartab();
            }
        }

        private void tab_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            String number = "";
            if(idc.Any(c=>(number=c.number) != "" && c.X==e.ColumnIndex && c.Y == e.RowIndex))
            {
                Rectangle r = new Rectangle(e.CellBounds.X, e.CellBounds.Y,e.CellBounds.Width, e.CellBounds.Height);
                e.Graphics.FillRectangle(Brushes.White, r);
                Font f = new Font(e.CellStyle.Font.FontFamily, 7);
                e.Graphics.DrawString(number, f, Brushes.Black, r);
                e.PaintContent(e.ClipBounds);
                e.Handled = true;
            }

        }
    }

    public class id_celdas
    {
        public int X;
        public int Y;
        public String direction;
        public String number;
        public String word;
        public String clue;
        
        public id_celdas(int x, int y, String d, String n, String w, String c)
        {
            this.X = x;
            this.Y = y;
            this.direction= d;  
            this.number = n;    
            this.word= w;   
            this.clue= c;
        }
    }


}
