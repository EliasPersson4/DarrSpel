using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Dhar_man_Real
{
    public partial class Form1 : Form
    {
        int highscore;
        int points = 0;
        int stoppedTime = 0;
        bool timeIsStopped = false;
        int time = 0;
        public Form1()
        {
            InitializeComponent();
        }
        private void Point_Click(object sender, EventArgs e)
        {
            // så basically varför jag gör såhär istället för att göra 15 funktioner för alla olika knappar är för att detta
            // är så mycker mer effektivt och coolare
            var button = (Button)sender;
            button.Hide();
            points++;
            // vi har inte gått igenom formatted strings men de borde vi fan göra för att de är så effektivt
            label1.Text = $"Progress: {points}/15";
            if (points == 15)
            {
                Timer.Stop();
                moveTimer.Stop();
                MessageBox.Show("You Win :)");
                if (time < highscore)
                {
                    highscore = time;
                    System.IO.File.WriteAllText("highscore.txt", highscore.ToString());
                }
            }
        }
        private void MoveMonsert(object sender, EventArgs e)
        {
            // jag tyckte att de va tråkigt med bara vanliga hinder så jag gjorde så att du aldrig är säker
            // det är ganska simpel tracking, när inte på mus pekare, rör sig mot muspekare

            int mouse_posX = Cursor.Position.X - this.Left;
            int mouse_posY = Cursor.Position.Y - this.Top;
            int cube_posX = Monsert.Location.X + Monsert.Width / 2 + 5;
            int cube_posY = Monsert.Location.Y + Monsert.Height / 2 + 30;

            if (mouse_posX > cube_posX)
            {
                Monsert.Left += 1;
            }
            else if (mouse_posX < cube_posX)
            {
                Monsert.Left -= 1;
            }
            if (mouse_posY > cube_posY)
            {
                Monsert.Top += 1;
            }
            else if (mouse_posY < cube_posY)
            {
                Monsert.Top -= 1;
            }
        }
        private void StoppedTimeTimer(object sender, EventArgs e)
        {
            // du kan stoppa tiden i 7 sekunder, efter det så kan du inte göra det på 4 sekunder
            // jag la till detta för att annars är det omöjligt att klara spelet (det förföljande hindret är för svårt)
            if (timeIsStopped)
            {
                stoppedTime++;
                if (stoppedTime == 7)
                {
                    moveTimer.Start();
                    this.Text = "Ability on cooldown";
                }
                if (stoppedTime == 11)
                {
                    Cooldown_Done();
                }
            }
        }
        private void Cooldown_Done()
        {
            timeIsStopped = !timeIsStopped;
            secondTimer.Stop();
            stoppedTime = 0;
            this.Text = "Ability Ready";
        }
        private void AnyKeyPress(object sender, KeyPressEventArgs e)
        {
            // Jag valde "e" knappen på tangentbordet för alla bra spel använder "e" för abilities
            if (e.KeyChar.ToString() == "e")
            {
                if (!timeIsStopped)
                {
                    moveTimer.Stop();
                    secondTimer.Start();
                    timeIsStopped = true;
                    this.Text = "Ability active";
                }
            }
            if (e.KeyChar.ToString() == "q")
            {
                Close();
            }
        }
        private void On_Fail(object sender, EventArgs e)
        {
            MessageBox.Show("You lose");
            points = 0;
            time = 0;
            // detta loopar igenom alla knappar på formen och resettar allt, mycket mer effektivt än att skriva 15 rader av button1.show() osv
            foreach (Button button in this.Controls.OfType<Button>())
            {
                button.Show();
            }
            Cursor.Position = new Point(60 + Left, 30 + 60 + Top);
            Monsert.Location = new Point(683, 649);
            label1.Text = "points: 0";
            label2.Text = "time: 0";
            moveTimer.Start();
            Cooldown_Done();
        }
        private void On_Load(object sender, EventArgs e)
        {
            // (60 + Left, 30 + 60 + Top) är start positionen
            Cursor.Position = new Point(60 + Left, 30 + 60 + Top);
            label3.Text = $"Highscore: {highscore}";
            highscore = int.Parse(System.IO.File.ReadAllText("highscore.txt"));
        }
        private void Timer_Tick(object sender, EventArgs e)
        {
            time++;
            label2.Text = $"Time: {time}";
        }
    }
}
