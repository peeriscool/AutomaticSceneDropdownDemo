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
            _bodySource = _sensor.BodyFrameSource;
            _bodyReader = _bodySource.OpenReader();
            _bodyReader.FrameArrived += BodyReader_FrameArrived;

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
                    GameObject ellipse = GameObject.CreatePrimitive(PrimitiveType.Sphere);
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
                    Debug.Log("Face points Visualized");
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
        
        if (_sensor != null)
        {
           // FaceReader_FrameArrived(_sensor,_faceMode); //framerefrence
            Debug.Log(_sensor.UniqueKinectId + "id : " + _faceReader.HighDefinitionFaceFrameSource.IsTrackingIdValid);
            //_sensor.PropertyChanged() += FaceReader_FrameArrived();
            UpdateFacePointsprefab();

        }
    }
 }


