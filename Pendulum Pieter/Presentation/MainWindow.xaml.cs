using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Pendulum_Pieter.Presentation;

namespace Pendulum_Pieter
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly MainViewModel _viewModel;
        private Point _lastPoint;

        public MainWindow(MainViewModel vm)
        {
            DataContext = vm;
            _viewModel = vm;
            InitializeComponent();
        }


        #region camera control


        private void WindowKeyDown(object sender, KeyEventArgs e)
        {
            _viewModel.ProcessKey(e.Key);
        }

        private void ViewPortPreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            _viewModel.Zoom(e.Delta);
        }


        private void ViewPortMouseDown(object sender, MouseButtonEventArgs e)
        {
            _lastPoint = e.GetPosition(mainViewPort);
            _ = viewPortControl.CaptureMouse();
            viewPortControl.MouseUp += ViewPortMouseUp;
            viewPortControl.PreviewMouseMove += ViewPortMouseMove;
        }

        private void ViewPortMouseMove(object sender, MouseEventArgs e)
        {
            var newPoint = e.GetPosition(mainViewPort);
            var vector = newPoint - _lastPoint;
            _viewModel.ControlByMouse(vector);
            _lastPoint = newPoint;
        }

        private void ViewPortMouseUp(object sender, MouseButtonEventArgs e)
        {
            viewPortControl.ReleaseMouseCapture();
            viewPortControl.PreviewMouseMove -= ViewPortMouseMove;
            viewPortControl.MouseUp -= ViewPortMouseUp;
        }

        #endregion camera control
    }
}
