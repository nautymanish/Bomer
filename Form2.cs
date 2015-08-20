using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BomberMan
{
   
    public partial class Form2 : Form
    {
        private Button[,] _buttons;
        private int cols, rows, timer;
        Player a, b;
        Timer time;

        public Form2()
        {
            InitializeComponent();

        }
        public Form2(int cols, int rows, int timer)
            : this()
        {
            this.cols = cols;
            this.rows = rows;
            this.timer = timer*1000;
            _buttons = new Button[rows,cols];
            for(int row=0;row<rows;row++)
                for (int col = 0; col < cols; col++)
                {
                    _buttons[row, col] = new Button() { Text = row + "," + col };
                    _buttons[row, col].Click += Form2_Click;
                    this.flowLayoutPanel1.Controls.Add(_buttons[row, col]);
                }
            a = new Player() { IsinAction=true, Buttons=_buttons};
            b = new Player() {IsinAction=false, Buttons=_buttons};
            time = new Timer();
            time.Interval = 1000;
            time.Tick+=time_Tick;
            time.Start();
        }
        /// <summary>
        /// Timer event to check out 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void time_Tick(object sender, EventArgs e)
        {
             label3.Text = "Time left " + timer-- +" seconds";
             if (timer==0)
             {
                 time.Stop();
                 MessageBox.Show(a.Score > b.Score ? "Winner is A" : b.Score>a.Score ?"Winner is B": "Draw");
                 this.Close();
                 return;
             }
        }

        void Form2_Click(object sender, EventArgs e)
        {
            Button tmpBtn = sender as Button;
            if (tmpBtn.Enabled)
            {
               
                tmpBtn.Enabled = false;
                if (a.IsinAction)
                {

                    PerformOperation(a, tmpBtn.Text);
                    tmpBtn.Text = "A";
                    a.IsinAction = false;
                    b.IsinAction = true;
                    label1.Text = "A Score" + a.Score.ToString();
                }
                else
                {
            
                    PerformOperation(b, tmpBtn.Text);
                    tmpBtn.Text = "B";
                    b.IsinAction = false;
                    a.IsinAction = true;
                    label2.Text = "B Score" + b.Score.ToString();
                }

            }
            var result = CheckForButtons();
            if (result == false)
            {
                MessageBox.Show(a.Score > b.Score ? "Winner is A" : b.Score > a.Score ? "Winner is B" : "Draw");
                this.Close();
                return;
            }
        }
        /// <summary>
        /// this would work for both the player keeps the track of the score and moves
        /// </summary>
        /// <param name="playerObject"></param>
        /// <param name="currentPosition"></param>
        private void PerformOperation(Player playerObject, string currentPosition)
        {
            playerObject.Score += 10;

            if (playerObject.Moves.Count == 0)
            {
                playerObject.Moves.Add(currentPosition);

            }
            else
            {
                var data = currentPosition.Split(',');
                int lrow = Convert.ToInt32(data[0]);
                int lcol = Convert.ToInt32(data[1]);

               
                string prev = string.Format("{0},{1}",  lrow - 1, lcol);
                string next = string.Format("{0},{1}", lrow +1, lcol);

                string dprev = string.Format("{0},{1}",  lrow - 1, lcol - 1);
                string dnext = string.Format("{0},{1}", lrow + 1, lcol + 1);

                string nprev = string.Format("{0},{1}", lrow,  cols - 1);
                string nnext = string.Format("{0},{1}", lrow,  lcol + 1);

                string lprev = string.Format("{0},{1}",  lrow - 1,  lcol + 1);
                string lnext = string.Format("{0},{1}",  lrow + 1, lcol - 1);

                string sprev = string.Format("{0},{1}",  lrow - 2, lcol);
                string snext = string.Format("{0},{1}",  lrow + 2, lcol);

                string sdprev = string.Format("{0},{1}", lrow - 2,  lcol - 2);
                string sdnext = string.Format("{0},{1}", lrow + 2, lcol + 2);

                string snprev = string.Format("{0},{1}",  lrow - 2, lcol);
                string snnext = string.Format("{0},{1}",  lrow + 2, lcol);

                string slprev = string.Format("{0},{1}",  lrow - 2,  lcol + 2);
                string slnext = string.Format("{0},{1}",  lrow + 2,  lcol - 2);

                int fcount = playerObject.Moves.FindAll(dataObject => dataObject.Contains(prev) || dataObject.Contains(next)).Count;
                int scount = playerObject.Moves.FindAll(dataObject => dataObject.Contains(dprev) || dataObject.Contains(dnext)).Count;
                int tcount = playerObject.Moves.FindAll(dataObject => dataObject.Contains(nprev) || dataObject.Contains(nnext)).Count;
                int ffcount = playerObject.Moves.FindAll(dataObject => dataObject.Contains(lprev) || dataObject.Contains(lnext)).Count;

                int sfcount = playerObject.Moves.FindAll(dataObject => (dataObject.Contains(sprev) || dataObject.Contains(snext))&&fcount>0).Count;
                int sscount = playerObject.Moves.FindAll(dataObject => (dataObject.Contains(sdprev) || dataObject.Contains(sdnext))&&scount>0).Count;
                int stcount = playerObject.Moves.FindAll(dataObject => (dataObject.Contains(snprev) || dataObject.Contains(snnext))&&tcount>0).Count;
                int sffcount = playerObject.Moves.FindAll(dataObject => (dataObject.Contains(slprev) || dataObject.Contains(slnext))&&ffcount>0).Count;


                if (fcount > 1 || scount > 1 || tcount > 1 || ffcount > 1 || sfcount >= 1 || sscount >= 1 || stcount >= 1 || sffcount >= 1)
                {

                    playerObject.Moves.RemoveAll(dataObject => dataObject.Contains(currentPosition) || dataObject.Contains(prev) || dataObject.Contains(next) 
                                                    || dataObject.Contains(dprev) || dataObject.Contains(dnext) || dataObject.Contains(nprev) || dataObject.Contains(nnext) 
                                                    || dataObject.Contains(lprev) || dataObject.Contains(lnext)
                                                    || dataObject.Contains(sprev) || dataObject.Contains(snext)
                                                    || dataObject.Contains(sdprev) || dataObject.Contains(sdnext) || dataObject.Contains(snprev) || dataObject.Contains(snnext)
                                                    || dataObject.Contains(slprev) || dataObject.Contains(slnext));
                    playerObject.Score -= 45;
                    

                }
                playerObject.Moves.Add(currentPosition);

            }
        }

        private bool CheckForButtons()
        {
            foreach (var btn in _buttons)
            {
                if (btn.Text.Contains(","))
                   return true;
                
                    
            }
            return false;
        }
    }

    class Player
    {
        public Player()
        {
            this.Moves = new List<string>();
        }
        public int Score { get; set; }
        public List<string> Moves { get; set; }
        public bool IsinAction { get; set; }
        public Button[,] Buttons { get; set; }
    }
    
}
