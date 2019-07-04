using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace Winform_2048
{
    enum Move
    {
        Left,
        Right,
        Up,
        Down
    }

    class Game2048
    {
        int[,] map = new int[4, 4];
        int[,] mapMoveBefore = new int[4, 4];

        bool isWin = false;

        void InitialMap()//初始化map数组
        {
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    map[i, j] = 0;
                    mapMoveBefore[i, j] = 0;
                }
            }

            //随机选择两个数初始值设为2
            Random rd = new Random();
            int x1 = rd.Next(0, 4);
            int y1 = rd.Next(0, 4);
            int x2 = rd.Next(0, 4);
            int y2 = rd.Next(0, 4);
            while( x1==x2 && y1==y2 )//不允许两个点位相同
            {
                x2 = rd.Next(0, 4);
                y2 = rd.Next(0, 4);
            }
            map[x1, y1] = 2;
            map[x2, y2] = 2;
        }

        public Game2048()
        {
            //InitialMap();
            //UpdateMapMoveBefore();            
        }
        
        public void StartGame()
        {
            InitialMap();
            UpdateMapMoveBefore();
            UpdateGrid(map);
        }

        public void UserMove(Keys key)
        {
            if (IsWin())
            {
                return;
            }
            switch (key)
            {
                case Keys.Up: MoveUp(); break;
                case Keys.Down: MoveDown(); break;
                case Keys.Left: MoveLeft(); break;
                case Keys.Right: MoveRight(); break;
                default:break;
            }
            if(IsDataChanged())
            {
                GenerateNewDataAfterMapChanged();//生成随机值
                UpdateGrid(map);
                UpdateMapMoveBefore();
            }
            if(IsWin())
            {                
                SendMessage("congratulation!!! YOU WIN");
            }
        }

        void UpdateMapMoveBefore()
        {
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    mapMoveBefore[i, j] = map[i, j];
                }
            }
        }

        bool IsWin()//判断是否胜利 即查找数组map中是否存在2048
        {
            foreach (var item in map)
            {
                if (item == 2048)
                {
                    return true;
                }
            }
            return false;
        }

        bool IsDataChanged()
        {
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    if (mapMoveBefore[i, j] != map[i, j])
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        void MoveUp()
        {
            for (int j = 0; j < 4; j++)
            {
                List<int> list = Merge(new int[] { map[0, j], map[1, j], map[2, j], map[3, j] });
                for (int i = 0; i < 4; i++)
                {
                    map[i, j] = list[i];
                }
            }
        }

        void MoveDown()
        {
            for (int j = 0; j < 4; j++)
            {
                List<int> list = Merge(new int[] { map[3, j], map[2, j], map[1, j], map[0, j] });
                for (int i = 0; i < 4; i++)
                {
                    map[i, j] = list[3 - i];
                }
            }
        }

        void MoveLeft()
        {
            for (int i = 0; i < 4; i++)
            {
                List<int> list = Merge(new int[] { map[i, 0], map[i, 1], map[i, 2], map[i, 3] });
                for (int j = 0; j < 4; j++)
                {
                    map[i, j] = list[j];
                }
            }
        }

        void MoveRight()
        {
            for (int i = 0; i < 4; i++)
            {
                List<int> list = Merge(new int[] { map[i, 3], map[i, 2], map[i, 1], map[i, 0] });
                for (int j = 0; j < 4; j++)
                {
                    map[i, j] = list[3-j];
                }
            }
        }

        List<int> Merge(int[] array)
        {
            List<int> list = new List<int>();
            for (int i = 0; i < array.Length; i++)
            {
                if(array[i]!=0)
                {
                    list.Add(array[i]);
                }
            }
            
            for (int i = 0,j=1; j < list.Count; i++,j++)
            {
                if(list[i]==list[j])
                {
                    list[i] = 2 * list[i];
                    list[j] = 0;
                }
            }
            list.RemoveAll((item) => item == 0);

            while (list.Count < array.Length)
            {
                list.Add(0);
            }

            return list;
        }

        void GenerateNewDataAfterMapChanged()//随机从map数组中选中一个值为0的数 将其值改为2
        {
            Random rd = new Random();
            //值为0的格子个数
            int numOfZero=0;
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    if(map[i,j]==0)
                    {
                        numOfZero++;
                    }
                }
            }
                          
            int position = rd.Next(1, numOfZero+1);//位置
            int num = 0;
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {                    
                    if(map[i,j]==0)
                    {
                        num++;
                        if(num==position)
                        {
                            map[i, j] = 2;
                        }
                    }
                }
            }
        }

        public event Action<int[,]> UpdateGrid;//更新界面

        public event Action<string> SendMessage;
    }
}
