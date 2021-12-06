using System.Linq;
using Windows.Kinect;
using Microsoft.Kinect.Face;
using Microsoft.Kinect;
using System.Collections.Generic;
using UnityEngine;
public class KinectFaceDetection : MonoBehaviour
{
    // Provides a Kinect sensor reference.
private KinectSensor _sensor = null;

    // Acquires body frame data.
    private BodyFrameSource _bodySource = null;

    // Reads body frame data.
    private BodyFrameReader _bodyReader = null;

    // Acquires HD face data.
    private HighDefinitionFaceFrameSource _faceSource = null;

    // Reads HD face data.
    private HighDefinitionFaceFrameReader _faceReader = null;

    // Required to access the face vertices.
    private FaceAlignment _faceAlignment = null;

    // Required to access the face model points.
    private FaceModel _faceModel = null;

    // Used to display 1,000 points on screen.
    private List<GameObject> _points = new List<GameObject>();
    // Start is called before the first frame update



    void Start()
    {
        _sensor = KinectSensor.GetDefault();

        if (_sensor != null)
        {
            // Listen for body data.
            //_bodySource = _sensor.BodyFrameSource;
            //_bodyReader = _bodySource.OpenReader();
            //_bodyReader.FrameArrived += BodyReader_FrameArrived;

            // Listen for HD face data.
            _faceSource = HighDefinitionFaceFrameSource.Create(_sensor);
         //   _faceSource = new HighDefinitionFaceFrameSource(_sensor);
            _faceReader = _faceSource.OpenReader();
            _faceReader.FrameArrived += FaceReader_FrameArrived;

            _faceModel = _faceSource.FaceModel;
            FaceAlignment aligment = Microsoft.Kinect.Face.FaceAlignment.Create();
            _faceAlignment = aligment;//_faceModel.CalculateVerticesForAlignment(aligment) ;

            // Start tracking!        
            _sensor.Open();
        }
    }

    private void BodyReader_FrameArrived(object sender, BodyFrameArrivedEventArgs e)
    {
        using (var frame = e.FrameReference.AcquireFrame())
        {
            if (frame != null)
            {
                Body[] bodies = new Body[frame.BodyCount];
                frame.GetAndRefreshBodyData(bodies);

                Body body = bodies.Where(b => b.IsTracked).FirstOrDefault();

                if (!_faceSource.IsTrackingIdValid)
                {
                    if (body != null)
                    {
                        _faceSource.TrackingId = body.TrackingId;
                    }
                }
            }
        }
    }
    private void FaceReader_FrameArrived(object sender, HighDefinitionFaceFrameArrivedEventArgs e)
    {
        
        using (var frame = e.FrameReference.AcquireFrame())
        {
            if (frame != null && frame.IsFaceTracked)
            {
                frame.GetAndRefreshFaceAlignmentResult(_faceAlignment);
               
                UpdateFacePoints();
            }
        }
    }

    private void UpdateFacePoints()
    {
        if (_faceModel == null) return;
       
        var vertices = _faceModel.CalculateVerticesForAlignment(_faceAlignment);
    }
    private void UpdateFacePointsprefab()
    {
        if (_faceModel == null) return;

        var vertices = _faceModel.CalculateVerticesForAlignment(_faceAlignment);

        if (vertices.Count > 0)
        {
            if (_points.Count == 0)
            {
                for (int index = 0; index < vertices.Count; index++)
                {
                    GameObject ellipse = new GameObject();
                    {
                        //Width = 2.0,
                        //Height = 2.0,
                        //Fill = new SolidColorBrush(Colors.Blue)
                    };

                    _points.Add(ellipse);
                }

                foreach (GameObject item in _points)
                {
                    //  canvas.Children.Add(ellipse);
                    GameObject ellipse = GameObject.CreatePrimitive(PrimitiveType.Cube);
                }
            }

            for (int index = 0; index < vertices.Count; index++)
            {
                CameraSpacePoint vertice = vertices[index];
                DepthSpacePoint point = _sensor.CoordinateMapper.MapCameraPointToDepthSpace(vertice);

                if (float.IsInfinity(point.X) || float.IsInfinity(point.Y)) return;

                GameObject ellipse = _points[index];

               // Canvas.SetLeft(ellipse, point.X);
               // Canvas.SetTop(ellipse, point.Y);
            }
        }
    }

// Update is called once per frame
void Update()
    {
        //UpdateFacePoints();
        //UpdateFacePoints();
        if (_sensor != null)
        {
            //HighDefinitionFaceFrameArrivedEventArgs e
            //_faceReader.HighDefinitionFaceFrameSource.PropertyChanged +=
         /////////////////////////////////////////////////////////////////////// //  FaceReader_FrameArrived(_sensor,_faceModel.); //framerefrence
            Debug.Log(_sensor.UniqueKinectId + "id : " + _faceReader.HighDefinitionFaceFrameSource.IsTrackingIdValid);
            //_sensor.PropertyChanged() += FaceReader_FrameArrived();
        }
    }

/*
     using System;
    using System.ComponentModel;
    using System.Linq;
    using System.Windows;
    using System.Windows.Media;
    using System.Windows.Media.Media3D;
    using Microsoft.Kinect;
    using Microsoft.Kinect.Face;
    using System.Windows.Shapes;
    using System.Collections.Generic;
    using System.Windows.Controls;

namespace Kinect2FaceHD_NET
{
    public partial class MainWindow : Window
    {
        private KinectSensor _sensor = null;

        private BodyFrameSource _bodySource = null;

        private BodyFrameReader _bodyReader = null;

        private HighDefinitionFaceFrameSource _faceSource = null;

        private HighDefinitionFaceFrameReader _faceReader = null;

        private FaceAlignment _faceAlignment = null;

        private FaceModel _faceModel = null;

        private List<Ellipse> _points = new List<Ellipse>();

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            _sensor = KinectSensor.GetDefault();

            if (_sensor != null)
            {
                _bodySource = _sensor.BodyFrameSource;
                _bodyReader = _bodySource.OpenReader();
                _bodyReader.FrameArrived += BodyReader_FrameArrived;

                _faceSource = new HighDefinitionFaceFrameSource(_sensor);

                _faceReader = _faceSource.OpenReader();
                _faceReader.FrameArrived += FaceReader_FrameArrived;

                _faceModel = new FaceModel();
                _faceAlignment = new FaceAlignment();

                _sensor.Open();
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (_faceModel != null)
            {
                _faceModel.Dispose();
                _faceModel = null;
            }

            GC.SuppressFinalize(this);
        }

        private void BodyReader_FrameArrived(object sender, BodyFrameArrivedEventArgs e)
        {
            using (var frame = e.FrameReference.AcquireFrame())
            {
                if (frame != null)
                {
                    Body[] bodies = new Body[frame.BodyCount];
                    frame.GetAndRefreshBodyData(bodies);

                    Body body = bodies.Where(b => b.IsTracked).FirstOrDefault();

                    if (!_faceSource.IsTrackingIdValid)
                    {
                        if (body != null)
                        {
                            _faceSource.TrackingId = body.TrackingId;
                        }
                    }
                }
            }
        }

        private void FaceReader_FrameArrived(object sender, HighDefinitionFaceFrameArrivedEventArgs e)
        {
            using (var frame = e.FrameReference.AcquireFrame())
            {
                if (frame != null && frame.IsFaceTracked)
                {
                    frame.GetAndRefreshFaceAlignmentResult(_faceAlignment);
                    UpdateFacePoints();
                }
            }
        }

        private void UpdateFacePoints()
        {
            if (_faceModel == null) return;

            var vertices = _faceModel.CalculateVerticesForAlignment(_faceAlignment);

            if (vertices.Count > 0)
            {
                if (_points.Count == 0)
                {
                    for (int index = 0; index < vertices.Count; index++)
                    {
                        Ellipse ellipse = new Ellipse
                        {
                            Width = 2.0,
                            Height = 2.0,
                            Fill = new SolidColorBrush(Colors.Blue)
                        };

                        _points.Add(ellipse);
                    }

                    foreach (Ellipse ellipse in _points)
                    {
                        canvas.Children.Add(ellipse);
                    }
                }

                for (int index = 0; index < vertices.Count; index++)
                {
                    CameraSpacePoint vertice = vertices[index];
                    DepthSpacePoint point = _sensor.CoordinateMapper.MapCameraPointToDepthSpace(vertice);

                    if (float.IsInfinity(point.X) || float.IsInfinity(point.Y)) return;

                    Ellipse ellipse = _points[index];

                    Canvas.SetLeft(ellipse, point.X);
                    Canvas.SetTop(ellipse, point.Y);
                }
            }
        }
    }
}
*/
}

