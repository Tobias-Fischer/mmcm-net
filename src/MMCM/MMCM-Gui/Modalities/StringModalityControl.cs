using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MMCMLibrary.Modalities
{
    public partial class StringModalityControl : UserControl
    {
        private YarpModalityString m = null;
        private System.Threading.Thread tRefresh;
        private int tPeriod;

        public StringModalityControl()
        {
            InitializeComponent();
            this.AutoSize = true;
        }

        ~StringModalityControl()
        {
            tRefresh.Abort();
        }

        public void LinkToModality(YarpModalityString mod, int refreshPeriod = 100)
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
                    this.labelReal.Text = m.vectorToString(m.GetRealValue);
                    this.labelPerceived.Text = m.vectorToString(m.PerceivedValue);
                    this.labelPredicted.Text = m.vectorToString(m.PredictedValue);
                }
                System.Threading.Thread.Sleep(tPeriod);
            }
        }
    }
}
