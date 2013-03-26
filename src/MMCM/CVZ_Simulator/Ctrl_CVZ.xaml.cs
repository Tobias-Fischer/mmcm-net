using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CVZ_Simulator
{
    /// <summary>
    /// Interaction logic for Ctrl_CVZ.xaml
    /// </summary>
    public partial class Ctrl_CVZ : UserControl
    {
        public event EventHandler deleted;

        public Ctrl_CVZ()
        {
            InitializeComponent();
        }

        #region Context menu

        private void Handler_Delete(object sender, RoutedEventArgs e)
        {
            if (deleted != null)
                deleted(this, null);
        }

        #endregion

        #region Drag and Drop
        private void UserControl_MouseDown(object sender, MouseButtonEventArgs e)
        {

        }
        #endregion
    }
}
