using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using CVZ_Core.Modalities;


namespace CVZ_Core.GUI.Modalities
{
    public partial class ImageModalityCtrl : UserControl
    {
        private IModality m = null;
        private System.Threading.Thread tRefresh;
        private int tPeriod;

        public ImageModalityCtrl()
        {
            InitializeComponent();
            this.AutoSize = true;
        }

        ~ImageModalityCtrl()
        {
            tRefresh.Abort();
        }

        public void LinkToModality(IModality mod, int refreshPeriod = 100)
        {
            m = mod;
            labelName.Text = mod.name;
            tPeriod = refreshPeriod;
            tRefresh = new System.Threading.Thread(refreshValues);
            tRefresh.Start();
        }

        protected virtual void refreshValues()
        {
            while (tRefresh.IsAlive)
            {
                if (m != null)
                {
                    try
                    {
                        pictureBoxReal.Image = m.GetAsBmp(m.GetRealValue);
				    pictureBoxPerceived.Image = m.GetAsBmp(m.PerceivedValue);
				    pictureBoxPredicted.Image = m.GetAsBmp(m.PredictedValue);
                    }
                        catch (Exception e)
                    {
                            Console.WriteLine("Modalities display exception: " + e.Message);
                    }
                }
                System.Threading.Thread.Sleep(tPeriod);
            }
        }
    }
}
