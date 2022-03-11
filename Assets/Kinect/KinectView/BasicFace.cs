using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Microsoft.Kinect.Face;
using Microsoft.Kinect;
using Windows.Kinect;
public class BasicFace : MonoBehaviour
{
    // The sensor objects.
    KinectSensor _sensor = null;

    // The color frame reader is used to display the RGB stream
    ColorFrameReader _colorReader = null;

    // The body frame reader is used to identify the bodies
    BodyFrameReader _bodyReader = null;

    // The list of bodies identified by the sensor
    IList<Body> _bodies = null;

    // The face frame source
    FaceFrameSource _faceSource = null;

    // The face frame reader
    FaceFrameReader _faceReader = null;


    // Required to access the face vertices.
    private FaceAlignment _faceAlignment = null;

    // Required to access the face model points.
    private FaceModel _faceModel = null;

    // Used to display 1,000 points on screen.
    private List<GameObject> _points = new List<GameObject>();
    // Start is called before the first frame update
    // Color frame display.
    private Texture2D texture;
    private byte[] pixels;
    private int width;
    private int height;

    public GameObject quad;


    void Start()
    {

        _sensor = KinectSensor.GetDefault();

        if (_sensor != null)
        {
            width = _sensor.ColorFrameSource.FrameDescription.Width;
            height = _sensor.ColorFrameSource.FrameDescription.Height;
            pixels = new byte[width * height * 4];
            texture = new Texture2D(width, height, TextureFormat.RGBA32, false);
            quad.GetComponent<Renderer>().sharedMaterial.mainTexture = texture;
            quad.GetComponent<Renderer>().sharedMaterial.SetTextureScale("_MainTex", new Vector2(-1, 1));

            _sensor.Open();

            _bodies = new Body[_sensor.BodyFrameSource.BodyCount];

            _colorReader = _sensor.ColorFrameSource.OpenReader();
            _colorReader.FrameArrived += ColorReader_FrameArrived;
            _bodyReader = _sensor.BodyFrameSource.OpenReader();
           // _bodyReader.FrameArrived += BodyReader_FrameArrived;

            // Initialize the face source with the desired features
            _faceSource = FaceFrameSource.Create(_sensor, 0, FaceFrameFeatures.BoundingBoxInColorSpace |
                                                          FaceFrameFeatures.FaceEngagement |
                                                          FaceFrameFeatures.Glasses |
                                                          FaceFrameFeatures.Happy |
                                                          FaceFrameFeatures.LeftEyeClosed |
                                                          FaceFrameFeatures.MouthOpen |
                                                          FaceFrameFeatures.PointsInColorSpace |
                                                          FaceFrameFeatures.RightEyeClosed);
            _faceReader = _faceSource.OpenReader();
            _faceReader.FrameArrived += FaceReader_FrameArrived;


            //_faceSource = HighDefinitionFaceFrameSource.Create(_sensor);
            //   _faceSource = new HighDefinitionFaceFrameSource(_sensor);
            _faceReader = _faceSource.OpenReader();
            _faceReader.FrameArrived += FaceReader_FrameArrived;

        //    _faceModel = _faceSource.KinectSensor //_faceSource. FaceFrameFeatures; // FaceModel;
            FaceAlignment aligment = Microsoft.Kinect.Face.FaceAlignment.Create();
            _faceAlignment = aligment;//_faceModel.CalculateVerticesForAlignment(aligment) ;
        }
    }
    //void BodyReader_FrameArrived(object sender, BodyFrameArrivedEventArgs e)
    //{
    //    using (var frame = e.FrameReference.AcquireFrame())
    //    {
    //        if (frame != null)
    //        {
    //            frame.GetAndRefreshBodyData(_bodies);

    //            Body body = _bodies.Where(b => b.IsTracked).FirstOrDefault();

    //            if (!_faceSource.IsTrackingIdValid)
    //            {
    //                if (body != null)
    //                {
    //                    // Assign a tracking ID to the face source
    //                    _faceSource.TrackingId = body.TrackingId;
    //                }
    //            }
    //        }
    //    }
    //}
    void ColorReader_FrameArrived(object sender, ColorFrameArrivedEventArgs e)
    {
        using (var frame = e.FrameReference.AcquireFrame())
        {
            if (frame != null)
            {

               // camera.Source = frame.ColorFrameSource();// ToBitmap();
            }
        }
    }
    void FaceReader_FrameArrived(object sender, FaceFrameArrivedEventArgs e)
    {
        using (var frame = e.FrameReference.AcquireFrame())
        {
            if (frame != null)
            {
                // Get the face frame result
                FaceFrameResult result = frame.FaceFrameResult;

                if (result != null)
                {
                    // Get the face points, mapped in the color space
                    var eyeLeft = result.FacePointsInColorSpace[FacePointType.EyeLeft];
                    var eyeRight = result.FacePointsInColorSpace[FacePointType.EyeRight];
                    var nose = result.FacePointsInColorSpace[FacePointType.Nose];
                    var mouthLeft = result.FacePointsInColorSpace[FacePointType.MouthCornerLeft];
                    var mouthRight = result.FacePointsInColorSpace[FacePointType.MouthCornerRight];

                    // Get the face characteristics
                    var eyeLeftClosed = result.FaceProperties[FaceProperty.LeftEyeClosed];
                    var eyeRightClosed = result.FaceProperties[FaceProperty.RightEyeClosed];
                    var mouthOpen = result.FaceProperties[FaceProperty.MouthOpen];
                }
            }
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (_colorReader != null)
        {
            using (var frame = _colorReader.AcquireLatestFrame())
            {
                if (frame != null)
                {
                    frame.CopyConvertedFrameDataToArray(pixels, ColorImageFormat.Rgba);
                    texture.LoadRawTextureData(pixels);
                    texture.Apply();
                }
            }
        }


    }
}
