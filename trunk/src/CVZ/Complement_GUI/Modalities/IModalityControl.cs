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
    public partial class IModalityControl : UserControl
    {
        private IModality m = null;
        private System.Threading.Thread tRefresh;
        private int tPeriod;

        public IModalityControl()
        {
            InitializeComponent();
            this.AutoSize = true;
        }

        ~IModalityControl()
        {
            tRefresh.Abort();
        }

        public void LinkToModality(IModality mod, int refreshPeriod = 100)
        {
            for (int i = 0; i < mod.Size; i++)
            {
                this.dataGridViewModalities.Rows.Add();
                this.dataGridViewModalities.Rows[i].HeaderCell.Value = i.ToString();
            }
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
                    for (int i = 0; i < m.Size; i++)
                    {
                        this.dataGridViewModalities["Real", i].Value = m.GetRealValue[i];
                        this.dataGridViewModalities["Perceived", i].Value = m.PerceivedValue[i];
                        this.dataGridViewModalities["Predicted", i].Value = m.PredictedValue[i];
                    }
                }
                System.Threading.Thread.Sleep(tPeriod);
            }
        }
    }
}
