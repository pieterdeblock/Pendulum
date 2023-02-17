using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using System.Windows.Media.Imaging;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wpf3DUtils;
using Pendulum_Pieter.Models;
using Microsoft.Toolkit.Mvvm.Input;
using System.Diagnostics;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using System.Windows.Threading;

namespace Pendulum_Pieter.Presentation
{
    public class MainViewModel : ObservableObject
    {
        private readonly IWorld _world;
        private readonly ILogic _logic;
        private readonly ICameraController _cameraController;
        private readonly Model3DGroup _model3dGroup = new();
        private readonly Model3DGroup _sphereGroup = new();
        private GeometryModel3D _beam;
        private DispatcherTimer _dispatcherTimer = new();
        private Model3DGroup _axesGroup;
        private Model3DGroup _support3DGroup = new();
        private Model3DGroup _slinger3DGroup = new();
        private Model3DGroup _Beam3DGroup = new();
        private Model3DGroup _ball3DGroup = new();

        private Stopwatch sw = new();
        private Slinger[] _slingers = Array.Empty<Slinger>();

        private bool _showAxes;
        private int _selectedSlider = 1;
        private int _sliderValue = 1;
        private int _calculationTime = 0;
        private int _selectedSliderAngle = 5;
        private int _angleValue = 5;

        private string _colour;

        public ProjectionCamera Camera => _cameraController.Camera;
        public ProjectionCamera Camera2 => _cameraController.Camera;
        public Model3D Visual3dContent => _model3dGroup;

        public IRelayCommand StartCommand { get; private set; }
        public IRelayCommand PauseCommand { get; private set; }
        public IRelayCommand ResetCommand { get; private set; }
        public IRelayCommand CenterCamera { get; private set; }
        #region Propperties
        public int SelectedSlider
        {
            get => _selectedSlider;
            set 
            {
                SetProperty(ref _selectedSlider, value);
                SliderValue = _selectedSlider;
            }
        }
        public int SliderValue
        {
            get => _sliderValue;
            set
            {
                SetProperty(ref _sliderValue, value);
                CreateSlingerArray();
            }

        }
        public int SelectedSliderAngle
        {
            get => _selectedSliderAngle;
            set
            {
                SetProperty(ref _selectedSliderAngle, value);
                AngleValue = _selectedSliderAngle;
            }
        }
        public int AngleValue
        {
            get => _angleValue;
            set
            {
                SetProperty(ref _angleValue, value);
                CreateSlingerArray();
            }
        }
        public int CalculationTime
        {
            get => _calculationTime;
            set => SetProperty(ref _calculationTime, value);
        }
        public string ComboboxColour
        {
            get => _colour;
            set
            {
                _colour = value;
                InitialiseBalls();
            }
        }
        #endregion Propperties

        #region ColorBrush
        private readonly SolidColorBrush[] _colorBrushList = new SolidColorBrush[]
        {
            new SolidColorBrush(Colors.Crimson),
            new SolidColorBrush(Colors.MediumBlue),
            new SolidColorBrush(Colors.Green),
            new SolidColorBrush(Colors.DarkOrange),
            new SolidColorBrush(Colors.Olive),
            new SolidColorBrush(Colors.DarkCyan),
            new SolidColorBrush(Colors.Brown),
            new SolidColorBrush(Colors.SteelBlue),
            new SolidColorBrush(Colors.Gold),
            new SolidColorBrush(Colors.MistyRose),
            new SolidColorBrush(Colors.PaleTurquoise),
            new SolidColorBrush(Colors.PeachPuff),
            new SolidColorBrush(Colors.Salmon),
            new SolidColorBrush(Colors.Silver),
        };
        #endregion ColorBrush
        public bool? ShowAxes
        {
            get => _showAxes;
            set
            {
                if (value == _showAxes) return;
                _showAxes = value ?? false;
                if (_showAxes)
                {
                    _model3dGroup.Children.Add(_axesGroup);
                }
                else
                {
                    _model3dGroup.Children.Remove(_axesGroup);
                }
            }
        }
        public int SphereCount => _world.SpherePositions.Count;

        public MainViewModel(IWorld world, ILogic logic, ICameraController cameraController)
        {
            _world = world;
            _logic = logic;
            _cameraController = cameraController;

            Init3DPresentation();
            CreateSlingerArray();
            InitDispatchTimer();

            StartCommand = new RelayCommand(_dispatcherTimer.Start);
            PauseCommand = new RelayCommand(_dispatcherTimer.Stop);
            ResetCommand = new RelayCommand(CreateSlingerArray);
            //CenterCamera = new RelayCommand(BottomCamera);
        }
        private void BottomCamera()
        {
            _cameraController.PositionCamera(0, Math.PI, Math.PI);
        }
        private void InitDispatchTimer()
        {
            sw.Start();
            _dispatcherTimer = new System.Windows.Threading.DispatcherTimer();
            _dispatcherTimer.Interval = TimeSpan.FromMilliseconds(16.7);
        }
        private async void help(object sender, EventArgs e)
        {
            await dispatcherTimer_Tick();
        }
        private async Task dispatcherTimer_Tick()
        {
            CalculationTime = (int)sw.Elapsed.TotalSeconds;
            await Task.Run(() =>
            {
                Parallel.For(0, _slingers.Length, i =>
                {
                    (_slingers[i].Angle, _slingers[i].SpeedChange) = _logic.BerekenHoek(_slingers[i].Angle, _slingers[i].EndPoint.Y, 0.1, _slingers[i].SpeedChange);
                });
            });
            UpdateBollen();
            UpdateSlinger();
            TransformSlingers();
        }

        #region World Presentation
        ///<summary>X = Red line, Y = Gray line, Z = Blue Line. Use Decimal numbers </summary>
        private void InitSupport()
        {
            if (_support3DGroup != null) _support3DGroup.Children.Clear();
            _support3DGroup.Children.Add(Models3D.CreateLine(_world.Origin + (new Vector3D(_slingers[_slingers.Length - 1].AnchorPoint.X + 0.1 , 0.75, 0.5)), _world.Origin + (new Vector3D(0, 0.75, 0.5)), 15, Brushes.Black));
            _model3dGroup.Children.Add(_support3DGroup);
        }

        private void CreateSlingerArray()
        {
            _slingers = new Slinger[_selectedSlider];
            for (int i = 0; i < _selectedSlider; i++)
            {
                _slingers[i] = new Slinger { AnchorPoint = new Point3D(i * 50, 0, 0), EndPoint = new Point3D(5 , -10 * (i+4) , 0), Angle = AngleValue };
            }
            if (_slinger3DGroup != null) _model3dGroup.Children.Remove(_slinger3DGroup);
            InitSupport();
            InitialiseSlinger();
            InitialiseBalls();
            _model3dGroup.Children.Add(_slinger3DGroup);
        }

        private void InitialiseSlinger()
        {
            if (_Beam3DGroup != null) _Beam3DGroup.Children.Clear();
            for (int i = 0; i < _slingers.Length; i++)
            {
                var beam = Models3D.CreateLine(_world.Origin, _world.Origin + new Vector3D(0, _slingers[i].EndPoint.Y, 0), _world.WorldSize / 200, Brushes.Gray);
                var transform = new Transform3DGroup();
                transform.Children.Add(new RotateTransform3D(new AxisAngleRotation3D(new Vector3D(1, 0, 0), -_slingers[i].Angle)));
                transform.Children.Add(new TranslateTransform3D(_slingers[i].AnchorPoint - _world.Origin));
                beam.Transform = transform;
                _Beam3DGroup.Children.Add(beam);
            }
            _slinger3DGroup.Children.Add(_Beam3DGroup);
        }

        private void InitialiseBalls()
        {
            if (_ball3DGroup != null) _ball3DGroup.Children.Clear();
            for (int i = 0; i < _slingers.Length; i++)
            {
                var matGroup = new MaterialGroup();
                matGroup.Children.Add(GetColour());
                var ball = Models3D.CreateSphere(matGroup);
                var transform = new Transform3DGroup();
                transform.Children.Add(new ScaleTransform3D(_world.WorldSize / 50, _world.WorldSize / 50, _world.WorldSize/50));
                transform.Children.Add(new TranslateTransform3D(new Vector3D(0, _slingers[i].EndPoint.Y, 0)));
                transform.Children.Add(new RotateTransform3D(new AxisAngleRotation3D(new Vector3D(1, 0, 0), -_slingers[i].Angle)));
                transform.Children.Add(new TranslateTransform3D(new Vector3D(_slingers[i].AnchorPoint.X, 3, _slingers[i].EndPoint.Z)));
                ball.Transform = transform;
                _ball3DGroup.Children.Add(ball);
            }
            _slinger3DGroup.Children.Add(_ball3DGroup);
        }
        private DiffuseMaterial GetColour()
        {
            switch (_colour)
            {
                case "System.Windows.Controls.ComboBoxItem: Gelijke Kleuren":
                    return(new DiffuseMaterial(_colorBrushList[1]));
                case "System.Windows.Controls.ComboBoxItem: Verschillende Kleuren":
                    Random rnd = new Random();
                    return (new DiffuseMaterial(_colorBrushList[rnd.Next(0, 14)]));
            }
            return (new DiffuseMaterial(_colorBrushList[1]));
        }

        private void UpdateSlinger()
        {
            for (int i = 0; i < _slingers.Length; i++)
            {
                var transform = new Transform3DGroup();
                transform.Children.Add(new RotateTransform3D(new AxisAngleRotation3D(new Vector3D(1, 0, 0), -_slingers[i].Angle)));
                transform.Children.Add(new TranslateTransform3D(_slingers[i].AnchorPoint - _world.Origin));
                _Beam3DGroup.Children[i].Transform = transform;
            }
        }

        private async void UpdateBollen()
        {
            for(int i = 0; i < _slingers.Length; i++)
            {
                var transform = new Transform3DGroup();
                transform.Children.Add(new ScaleTransform3D(_world.WorldSize / 50, _world.WorldSize / 50, _world.WorldSize / 50));
                transform.Children.Add(new TranslateTransform3D(new Vector3D(0, _slingers[i].EndPoint.Y, 0)));
                transform.Children.Add(new RotateTransform3D(new AxisAngleRotation3D(new Vector3D(1, 0, 0), -_slingers[i].Angle)));
                transform.Children.Add(new TranslateTransform3D(new Vector3D(_slingers[i].AnchorPoint.X, 3, _slingers[i].EndPoint.Z)));
                _ball3DGroup.Children[i].Transform = transform;
            }
        }
        private void TransformSlingers() 
        {
            for (int i = 0; i < _slingers.Length; i++)
            {
                var transform = new Transform3DGroup();
                transform.Children.Add(new RotateTransform3D(new AxisAngleRotation3D(new Vector3D(1, 0, 0), -_slingers[i].Angle)));
                transform.Children.Add(new TranslateTransform3D(_slingers[i].AnchorPoint - _world.Origin));
                _Beam3DGroup.Children[i].Transform = transform;
            }
        }

        #endregion World Presentation
        #region Presentation setup

        private void Init3DPresentation()
        {
            SetupCamera();
            SetUpLights();
            CreateAxesGroup();
            ShowAxes = true;
        }

        private void SetUpLights()
        {
            _model3dGroup.Children.Add(new AmbientLight(Colors.Gray));
            var direction = new Vector3D(1.5, -3, -5);
            _model3dGroup.Children.Add(new DirectionalLight(Colors.Gray, direction));
        }

        private void CreateAxesGroup()
        {
            double xLength = Math.Abs(_world.Bounds.p2.X - _world.Bounds.p1.X) / 2;
            double yLength = Math.Abs(_world.Bounds.p2.Y - _world.Bounds.p1.Y) / 2;
            double zLength = Math.Abs(_world.Bounds.p2.Z - _world.Bounds.p1.Z) / 2;
            double thickness = (_world.Bounds.p2 - _world.Bounds.p1).Length / 500;
            _axesGroup = new Model3DGroup();
            _axesGroup.Children.Add(Models3D.CreateLine(new Point3D(xLength, 0, 0), new Point3D(0, 0, 0), thickness, Brushes.Red));
            _axesGroup.Children.Add(Models3D.CreateLine(new Point3D(0, yLength, 0), new Point3D(0, 0, 0), thickness, Brushes.Green));
            _axesGroup.Children.Add(Models3D.CreateLine(new Point3D(0, 0, zLength), new Point3D(0, 0, 0), thickness, Brushes.Blue));
            _axesGroup.Freeze();
        }

        #endregion Presentation setup
        #region Camera control

        private void SetupCamera()
        {
            double l1 = (_world.Bounds.p1 - _world.Origin).Length;
            double l2 = (_world.Bounds.p2 - _world.Origin).Length;
            double radius = 2.3 * Math.Max(l1, l2);
            _cameraController.PositionCamera(radius, Math.PI / 10, 2.0 * Math.PI / 5);
        }

        public void ProcessKey(Key key)
        {
            _cameraController.ControlByKey(key);
        }

        public void Zoom(int Delta)
        {
            _cameraController.Zoom(Delta);
        }

        public void ControlByMouse(Vector vector)
        {
            _cameraController.ControlByMouse(vector);
        }
        #endregion Camera control
    }
}
