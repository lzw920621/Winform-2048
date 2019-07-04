using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace Winform_2048
{
    public partial class Form1 : Form
    {
        Game2048 game2048 = new Game2048();
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            InitialGrid();
            //this.KeyPreview = true;

            dataGridView1.ClearSelection();//取消默认选定

            MoveEvent += game2048.UserMove;
            game2048.UpdateGrid += this.UpdateGridCallback;
            game2048.SendMessage += MessageCallback;
            game2048.StartGame();
        }

        void InitialGrid()
        {
            this.dataGridView1.Size = new Size(500, 500);//长宽

            this.dataGridView1.Paint += DataGrid_Paint;//绑定重绘事件
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.AllowDrop = false;
            this.dataGridView1.AllowUserToOrderColumns = false;
            this.dataGridView1.ReadOnly = true;
            
            this.dataGridView1.ColumnHeadersVisible = false;
            this.dataGridView1.RowHeadersVisible = false;
            this.dataGridView1.ScrollBars = ScrollBars.None;

            dataGridView1.DefaultCellStyle.SelectionBackColor = Color.White;//设置表格元素选中后背景色设为白色 
            dataGridView1.DefaultCellStyle.SelectionForeColor = SystemColors.ControlText;

            this.dataGridView1.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.dataGridView1.Rows.Add(4);
            for (int i = 0; i < dataGridView1.RowCount; i++)
            {
                dataGridView1.Rows[i].Height = 125;
            }
            for (int i = 0; i < dataGridView1.ColumnCount; i++)
            {
                dataGridView1.Columns[i].Width = 125;
            }

        }

        void DataGrid_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.DrawLine(new Pen(Color.Black), 125, 0, 125, 500);
            e.Graphics.DrawLine(new Pen(Color.Black), 250, 0, 250, 500);
            e.Graphics.DrawLine(new Pen(Color.Black), 375, 0, 375, 500);
            e.Graphics.DrawLine(new Pen(Color.Black), 0, 125, 500, 125);
            e.Graphics.DrawLine(new Pen(Color.Black), 0, 250, 500, 250);
            e.Graphics.DrawLine(new Pen(Color.Black), 0, 375, 500, 375);
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)//取消方向键对控件的焦点的控件，用自己自定义的函数处理各个方向键的处理函数
        {
            if (keyData == Keys.Up || keyData == Keys.Down ||
            keyData == Keys.Left || keyData == Keys.Right)
            {
                MoveEvent(keyData);
                return true;
            }               
            else
            {
                return base.ProcessDialogKey(keyData);
            }               
        }

        event Action<Keys> MoveEvent;//移动事件

        
        void UpdateGridCallback(int[,] map)//回调函数 更新表格
        {
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {                   
                    Color color;
                    switch(map[i,j])
                    {
                        case 2:color =Color.Lime; break;
                        case 4: color = Color.Orange; break;
                        case 8: color = Color.LightPink; break;
                        case 16: color = Color.Blue; break;
                        case 32: color = Color.Brown; break;
                        case 64: color = Color.DarkOrchid; break;
                        case 128: color = Color.Gray; break;
                        case 256: color = Color.DarkOrange; break;
                        case 512: color = Color.DarkBlue; break;
                        case 1024: color = Color.Linen; break;
                        case 2048: color = Color.Red; break;
                        default:color = Color.Black;break;
                    }
                    this.dataGridView1.Rows[i].Cells[j].Style.ForeColor = color;
                    this.dataGridView1.Rows[i].Cells[j].Value = map[i, j];
                    if(dataGridView1.Rows[i].Cells[j].Selected==true)
                    {
                        dataGridView1.DefaultCellStyle.SelectionForeColor = color;
                    }
                }
            }
        }

        void MessageCallback(string msg)
        {
            MessageBox.Show(msg);
        }

        private void btn_Restart_Click(object sender, EventArgs e)
        {
            game2048.StartGame();
        }
    }
    
}
