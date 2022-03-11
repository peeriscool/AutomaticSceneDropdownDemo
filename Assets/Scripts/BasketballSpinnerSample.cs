using UnityEngine;
using System.Linq;
using Windows.Kinect;
using Microsoft.Kinect.Face;
using System.Collections.Generic;
public class BasketballSpinnerSample : MonoBehaviour
{
    // Kinect members.
    private KinectSensor sensor;
    private ColorFrameReader colorReader;
    private BodyFrameReader bodyReader;
    private Body[] bodies;

    // Color frame display.
    private Texture2D texture;
    private byte[] pixels;
    private int width;
    private int height;

    // Visual elements.
    public GameObject quad;
    public GameObject ball;

    // Parameters
    public float scale = 2f;
    public float speed = 10f;

    //------------------Rude insertion-----------------------------
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
        sensor = KinectSensor.GetDefault();

        if (sensor != null)
        {
            // Initialize readers.
            bodyReader = sensor.BodyFrameSource.OpenReader();
            colorReader = sensor.ColorFrameSource.OpenReader();

            // Body frame data.
            bodies = new Body[sensor.BodyFrameSource.BodyCount];

            // Color frame data.
            width = sensor.ColorFrameSource.FrameDescription.Width;
            height = sensor.ColorFrameSource.FrameDescription.Height;
            pixels = new byte[width * height * 4];
            texture = new Texture2D(width, height, TextureFormat.RGBA32, false);

            // Assign the texture to the proper game object. Also, flip the texture vertically (Kinect bug).
            quad.GetComponent<Renderer>().sharedMaterial.mainTexture = texture;
            quad.GetComponent<Renderer>().sharedMaterial.SetTextureScale("_MainTex", new Vector2(-1, 1));

            //-------------rude insertion--------------------------
            _faceSource = HighDefinitionFaceFrameSource.Create(sensor);
            //   _faceSource = new HighDefinitionFaceFrameSource(_sensor);
            _faceReader = _faceSource.OpenReader();
            _faceReader.FrameArrived += FaceReader_FrameArrived;

            _faceModel = _faceSource.FaceModel;
            FaceAlignment aligment = Microsoft.Kinect.Face.FaceAlignment.Create();
            _faceAlignment = aligment;//_faceModel.CalculateVerticesForAlignment(aligment) ;
            sensor.Open();
            GetFaceTracking();
        }
    }

    /// <summary>
    /// rude insertion
    /// </summary>
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

    void Update()
    {
        if (colorReader != null)
        {
            using (var frame = colorReader.AcquireLatestFrame())
            {
                if (frame != null)
                {
                    frame.CopyConvertedFrameDataToArray(pixels, ColorImageFormat.Rgba);
                    texture.LoadRawTextureData(pixels);
                    texture.Apply();
                }
            }
        }
        if (bodyReader != null)
        {
            using (var frame = bodyReader.AcquireLatestFrame())
            {
                if (frame != null)
                {
                    frame.GetAndRefreshBodyData(bodies);

                    var body = bodies.Where(b => b.IsTracked).FirstOrDefault();

                    if (body != null)
                    {
                        // Detect the hand (left or right) that is closest to the sensor.
                        var handTipRight = body.Joints[JointType.HandTipRight].Position;
                        var handTipLeft = body.Joints[JointType.HandTipLeft].Position;
                        var closer = handTipRight.Z < handTipLeft.Z ? handTipRight : handTipLeft;

                        // Map the 3D position of the hand to the 2D color frame (1920x1080).
                        var point = sensor.CoordinateMapper.MapCameraPointToColorSpace(closer);
                        var position = new Vector2(0f, 0f);

                        if (!float.IsInfinity(point.X) && !float.IsInfinity(point.Y))
                        {
                            position.x = point.X;
                            position.y = point.Y;
                        }

                        // Map the 2D position to the Unity space.
                        var world = Camera.main.ViewportToWorldPoint(new Vector3(position.x / width, position.y / height, 0f));
                        var center = quad.GetComponent<Renderer>().bounds.center;

                        // Move and rotate the ball.
                        ball.transform.localScale = new Vector3(scale, scale, scale) / closer.Z;
                        ball.transform.position = new Vector3(world.x - 0.5f - center.x, -world.y + 0.5f, -1f);
                        ball.transform.Rotate(0f, speed, 0f);
                    }
                }
            }
        }
    }
    void GetFaceTracking()
        {
        //-------Rude intrution---------------------//
        if (_faceModel == null) return;
        else
        {
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
                    DepthSpacePoint point = sensor.CoordinateMapper.MapCameraPointToDepthSpace(vertice);

                    if (float.IsInfinity(point.X) || float.IsInfinity(point.Y)) return;

                    GameObject ellipse = _points[index];

                    // Canvas.SetLeft(ellipse, point.X);
                    // Canvas.SetTop(ellipse, point.Y);
                }
            }
        }
    }
    void OnApplicationQuit()
    {
        if (bodyReader != null)
        {
            bodyReader.Dispose();
        }

        if (colorReader != null)
        {
            colorReader.Dispose();
        }

        if (sensor != null && sensor.IsOpen)
        {
            sensor.Close();
        }
    }
}