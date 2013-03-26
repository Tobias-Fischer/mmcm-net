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
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void modelCanvas_AdCVZ(object sender, RoutedEventArgs e)
        {
            Point mousePosition = Mouse.GetPosition(modelCanvas);
            Ctrl_CVZ nCVZ = new Ctrl_CVZ();
            Canvas.SetTop(nCVZ, mousePosition.Y);
            Canvas.SetLeft(nCVZ, mousePosition.X);
            modelCanvas.Children.Add(nCVZ);
        }
    }
}
