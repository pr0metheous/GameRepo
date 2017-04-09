using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Media;
using System.IO;
using Engine;

namespace AdventureGame
{
    public partial class HomePage : Form
    {

        Stream str = Properties.Resources.gangsta;
        private SoundPlayer plyr;
        public HomePage()
        {
            InitializeComponent();
            plyr = new SoundPlayer(str);
            startAudio(plyr);
        }

        private void Playbtn_Click(object sender, EventArgs e)
        {
            this.Hide();
            AdventureGame newForm = new AdventureGame(plyr);
            newForm.ShowDialog();
            this.Close();
        }

        public void startAudio(SoundPlayer plyr)
        {
            plyr.PlayLooping();
        }

     

       
    }
}
