using UnityEngine;
using System;
using System.Net.Sockets;
using System.Text;
using System.Collections;


enum brekel_faceAnimUnit
{
	brow_up_L,
	brow_up_R,
	brow_down_L,
	brow_down_R,
	eye_closed_L,
	eye_closed_R,
	cheek_puffed_L,
	cheek_puffed_R,
	lips_pucker,
	lip_stretch_L,
	lip_stretch_R,
	lip_lower_down_L,
	lip_lower_down_R,
	smile_L,
	smile_R,
	frown_L,
	frown_R,
	jaw_L,
	jaw_R,
	jaw_open,
	numFaceAnimUnits
};


enum brekel_facePoint
{
	brow_Inner_L,
	brow_Mid_L,
	brow_Outer_L,
	brow_Inner_R,
	brow_Mid_R,
	brow_Outer_R,
	eyelid_Up_L,
	eyelid_Down_L,
	eyelid_Up_R,
	eyelid_Down_R,
	nostril_L,
	nostril_R,
	cheek1_L,
	cheek2_L,
	cheek1_R,
	cheek2_R,
	lips_Up,
	lips_Up_Inner_L,
	lips_Up_Outer_L,
	lips_Corner_L,
	lips_Down_Outer_L,
	lips_Down_Inner_L,
	lips_Down,
	lips_Down_Inner_R,
	lips_Down_Outer_R,
	lips_Corner_R,
	lips_Up_Outer_R,
	lips_Up_Inner_R,
	jaw,
	numFacePoints
};


[Serializable]
public class BrekelFace
{
	public	Transform			neck;
	public	Transform			head;
	
	public	GameObject			face_mesh;
	public	int					blendshapeID_brow_up_L			= 0;
	public	int					blendshapeID_brow_up_R			= 1;
	public	int					blendshapeID_brow_down_L		= 2;
	public	int					blendshapeID_brow_down_R		= 3;
	public	int					blendshapeID_eye_closed_L		= 4;
	public	int					blendshapeID_eye_closed_R		= 5;
	public	int					blendshapeID_cheek_puffed_L		= 6;
	public	int					blendshapeID_cheek_puffed_R		= 7;
	public	int					blendshapeID_lips_pucker		= 8;
	public	int					blendshapeID_lip_stretch_L		= 9;
	public	int					blendshapeID_lip_stretch_R		= 10;
	public	int					blendshapeID_lip_lower_down_L	= 11;
	public	int					blendshapeID_lip_lower_down_R	= 12;
	public	int					blendshapeID_smile_L			= 13;
	public	int					blendshapeID_smile_R			= 14;
	public	int					blendshapeID_frown_L			= 15;
	public	int					blendshapeID_frown_R			= 16;
	public	int					blendshapeID_jaw_L				= 17;
	public	int					blendshapeID_jaw_R				= 18;
	public	int					blendshapeID_jaw_open			= 19;

	public	Transform			Brow_Inner_L;
	public	Transform			Brow_Mid_L;
	public	Transform			Brow_Outer_L;
	public	Transform			Brow_Inner_R;
	public	Transform			Brow_Mid_R;
	public	Transform			Brow_Outer_R;
	public	Transform			Eyelid_Up_L;
	public	Transform			Eyelid_Down_L;
	public	Transform			Eyelid_Up_R;
	public	Transform			Eyelid_Down_R;
	public	Transform			Nostril_L;
	public	Transform			Nostril_R;
	public	Transform			Cheek_1_L;
	public	Transform			Cheek_2_L;
	public	Transform			Cheek_1_R;
	public	Transform			Cheek_2_R;
	public	Transform			Lips_Up_Center;
	public	Transform			Lips_Up_Inner_L;
	public	Transform			Lips_Up_Outer_L;
	public	Transform			Lips_Corner_L;
	public	Transform			Lips_Down_Outer_L;
	public	Transform			Lips_Down_Inner_L;
	public	Transform			Lips_Down;
	public	Transform			Lips_Down_Inner_R;
	public	Transform			Lips_Down_Outer_R;
	public	Transform			Lips_Corner_R;
	public	Transform			Lips_Up_Outer_R;
	public	Transform			Lips_Up_Inner_R;
	public	Transform			Jaw;
	
	public	bool				isTracked;
	public	int					trackingID;
	public	float				timestamp;

	public	Vector3				neck_position_global;
	public	Quaternion			neck_rotation_global;

	public	Vector3				head_position_local;
	public	Quaternion			head_rotation_local;

	public	float[]				animUnitValues				= new float[(int)brekel_faceAnimUnit.numFaceAnimUnits];

	public	Vector3[]			facePoints_position_local	= new Vector3[(int)brekel_facePoint.numFacePoints];
	public	Quaternion[]		facePoints_rotation_local	= new Quaternion[(int)brekel_facePoint.numFacePoints];
}


public class Brekel_ProFace2_TCP_Streamer : MonoBehaviour
{
	// settings
	public	String					host						= "localhost";
	public	int						port						= 8884;
	public	float					scaleFactor					= 10f;

	// internal variables
	private	bool					isConnected					= false;
	private const char				expected_packetStart		= (char)2;
	private	const int				READ_BUFFER_SIZE			= 4880;
	private	TcpClient				client						= null;
	private	byte[]					readBuffer					= new byte[READ_BUFFER_SIZE];
	private	bool					readingFromNetwork			= false;
	private Vector3					vec;
	private	int						closest_ID;
	private	float					closest_distance;
	private SkinnedMeshRenderer		skinnedMeshRendererClosestFace;
	private SkinnedMeshRenderer[]	skinnedMeshRendererFaces		= new SkinnedMeshRenderer[6];

	// face data
	public	BrekelFace				closest_face;
	public	BrekelFace[]			faces						= new BrekelFace[6];



	//=============================
	// this is run once at startup
	//=============================
	void Start()
	{
		// get SkinnedMeshRenderers from face meshes
		for(int faceID=0; faceID<faces.GetLength(0); faceID++)
		{
			if(faces[faceID].face_mesh != null)
				skinnedMeshRendererFaces[faceID] = faces[faceID].face_mesh.GetComponent<SkinnedMeshRenderer>();
		}
		if(closest_face.face_mesh != null)
			skinnedMeshRendererClosestFace = closest_face.face_mesh.GetComponent<SkinnedMeshRenderer>();
		
		// connect to Brekel TCP network socket
		Connect();
	}


	//==========================
	// this is run once at exit
	//==========================
	void OnDisable()
	{
		// disconnect from Brekel TCP network socket
		Disconnect();
	}


	//============================
	// this is run once per frame
	//============================
	void Update()
	{
		// only update if connected and currently not updating the data
		if(isConnected && !readingFromNetwork)
		{
			// find body closest to the sensor
			closest_ID			= -1;
			closest_distance	= 9999999f;
			for(int faceID=0; faceID<faces.GetLength(0); faceID++)
			{
				if(!faces[faceID].isTracked)
					continue;
				if(faces[faceID].neck_position_global.z < closest_distance)
				{
					closest_ID			= faceID;
					closest_distance	= faces[faceID].neck_position_global.z;
				}
			}


			// apply to transforms (cannot be done in FetchFrame, only in Update thread)
			for(int faceID=0; faceID<faces.GetLength(0); faceID++)
			{
				Update_NeckHead(faces[faceID]);
				Update_BlendShapes(faces[faceID], skinnedMeshRendererFaces[faceID]);
				Update_FacePoints(faces[faceID]);
			}


			// copy data to closest skeleton
			if(closest_ID != -1)
			{
				closest_face.isTracked				= faces[closest_ID].isTracked;
				closest_face.trackingID				= faces[closest_ID].trackingID;
				closest_face.timestamp				= faces[closest_ID].timestamp;
				closest_face.neck_position_global	= faces[closest_ID].neck_position_global;
				closest_face.neck_rotation_global	= faces[closest_ID].neck_rotation_global;
				closest_face.head_position_local	= faces[closest_ID].head_position_local;
				closest_face.head_rotation_local	= faces[closest_ID].head_rotation_local;
				for(int animUnitID=0; animUnitID<(int)brekel_faceAnimUnit.numFaceAnimUnits; animUnitID++)
					closest_face.animUnitValues[animUnitID]	= faces[closest_ID].animUnitValues[animUnitID];
				for(int facePointID=0; facePointID<(int)brekel_facePoint.numFacePoints; facePointID++)
				{
					closest_face.facePoints_position_local[facePointID]	= faces[closest_ID].facePoints_position_local[facePointID];
					closest_face.facePoints_rotation_local[facePointID]	= faces[closest_ID].facePoints_rotation_local[facePointID];
				}

				Update_NeckHead(closest_face);
				Update_BlendShapes(closest_face, skinnedMeshRendererClosestFace);
				Update_FacePoints(closest_face);
			}
		}
	}


	//=============================
	// Update neck/head transforms
	//=============================
	void Update_NeckHead(BrekelFace face)
	{
		// apply position/rotation to neck transforms
		if(face.neck != null)
		{
			face.neck.position = face.neck_position_global;
			face.neck.rotation = face.neck_rotation_global;
		}

		// apply position/rotation to head transforms
		if(face.head != null)
		{
			face.head.localPosition = face.head_position_local;
			face.head.localRotation = face.head_rotation_local;
		}
	}


	//===========================
	// Update blendshapes values
	//===========================
	void Update_BlendShapes(BrekelFace face, SkinnedMeshRenderer skinnedMeshRenderer)
	{
		if(skinnedMeshRenderer != null)
		{
			skinnedMeshRenderer.SetBlendShapeWeight(face.blendshapeID_brow_up_L,		face.animUnitValues[(int)brekel_faceAnimUnit.brow_up_L]);
			skinnedMeshRenderer.SetBlendShapeWeight(face.blendshapeID_brow_up_R,		face.animUnitValues[(int)brekel_faceAnimUnit.brow_up_R]);
			skinnedMeshRenderer.SetBlendShapeWeight(face.blendshapeID_brow_down_L,		face.animUnitValues[(int)brekel_faceAnimUnit.brow_down_L]);
			skinnedMeshRenderer.SetBlendShapeWeight(face.blendshapeID_brow_down_R,		face.animUnitValues[(int)brekel_faceAnimUnit.brow_down_R]);
			skinnedMeshRenderer.SetBlendShapeWeight(face.blendshapeID_eye_closed_L,		face.animUnitValues[(int)brekel_faceAnimUnit.eye_closed_L]);
			skinnedMeshRenderer.SetBlendShapeWeight(face.blendshapeID_eye_closed_R,		face.animUnitValues[(int)brekel_faceAnimUnit.eye_closed_R]);
			skinnedMeshRenderer.SetBlendShapeWeight(face.blendshapeID_cheek_puffed_L,	face.animUnitValues[(int)brekel_faceAnimUnit.cheek_puffed_L]);
			skinnedMeshRenderer.SetBlendShapeWeight(face.blendshapeID_cheek_puffed_R,	face.animUnitValues[(int)brekel_faceAnimUnit.cheek_puffed_R]);
			skinnedMeshRenderer.SetBlendShapeWeight(face.blendshapeID_lips_pucker,		face.animUnitValues[(int)brekel_faceAnimUnit.lips_pucker]);
			skinnedMeshRenderer.SetBlendShapeWeight(face.blendshapeID_lip_stretch_L,	face.animUnitValues[(int)brekel_faceAnimUnit.lip_stretch_L]);
			skinnedMeshRenderer.SetBlendShapeWeight(face.blendshapeID_lip_stretch_R,	face.animUnitValues[(int)brekel_faceAnimUnit.lip_stretch_R]);
			skinnedMeshRenderer.SetBlendShapeWeight(face.blendshapeID_lip_lower_down_L,	face.animUnitValues[(int)brekel_faceAnimUnit.lip_lower_down_L]);
			skinnedMeshRenderer.SetBlendShapeWeight(face.blendshapeID_lip_lower_down_R,	face.animUnitValues[(int)brekel_faceAnimUnit.lip_lower_down_R]);
			skinnedMeshRenderer.SetBlendShapeWeight(face.blendshapeID_smile_L,			face.animUnitValues[(int)brekel_faceAnimUnit.smile_L]);
			skinnedMeshRenderer.SetBlendShapeWeight(face.blendshapeID_smile_R,			face.animUnitValues[(int)brekel_faceAnimUnit.smile_R]);
			skinnedMeshRenderer.SetBlendShapeWeight(face.blendshapeID_frown_L,			face.animUnitValues[(int)brekel_faceAnimUnit.frown_L]);
			skinnedMeshRenderer.SetBlendShapeWeight(face.blendshapeID_frown_R,			face.animUnitValues[(int)brekel_faceAnimUnit.frown_R]);
			skinnedMeshRenderer.SetBlendShapeWeight(face.blendshapeID_jaw_L,			face.animUnitValues[(int)brekel_faceAnimUnit.jaw_L]);
			skinnedMeshRenderer.SetBlendShapeWeight(face.blendshapeID_jaw_R,			face.animUnitValues[(int)brekel_faceAnimUnit.jaw_R]);
			skinnedMeshRenderer.SetBlendShapeWeight(face.blendshapeID_jaw_open,			face.animUnitValues[(int)brekel_faceAnimUnit.jaw_open]);
		}
	}


	//============================
	// Update facepoint positions
	//============================
	void Update_FacePoints(BrekelFace face)
	{
		if(face.Brow_Inner_L != null)		face.Brow_Inner_L.localPosition			= face.facePoints_position_local[(int)brekel_facePoint.brow_Inner_L];
		if(face.Brow_Mid_L != null)			face.Brow_Mid_L.localPosition			= face.facePoints_position_local[(int)brekel_facePoint.brow_Mid_L];
		if(face.Brow_Outer_L != null)		face.Brow_Outer_L.localPosition			= face.facePoints_position_local[(int)brekel_facePoint.brow_Outer_L];
		if(face.Brow_Inner_R != null)		face.Brow_Inner_R.localPosition			= face.facePoints_position_local[(int)brekel_facePoint.brow_Inner_R];
		if(face.Brow_Mid_R != null)			face.Brow_Mid_R.localPosition			= face.facePoints_position_local[(int)brekel_facePoint.brow_Mid_R];
		if(face.Brow_Outer_R != null)		face.Brow_Outer_R.localPosition			= face.facePoints_position_local[(int)brekel_facePoint.brow_Outer_R];
		if(face.Eyelid_Up_L != null)		face.Eyelid_Up_L.localPosition			= face.facePoints_position_local[(int)brekel_facePoint.eyelid_Up_L];
		if(face.Eyelid_Down_L != null)		face.Eyelid_Down_L.localPosition		= face.facePoints_position_local[(int)brekel_facePoint.eyelid_Down_L];
		if(face.Eyelid_Up_R != null)		face.Eyelid_Up_R.localPosition			= face.facePoints_position_local[(int)brekel_facePoint.eyelid_Up_R];
		if(face.Eyelid_Down_R != null)		face.Eyelid_Down_R.localPosition		= face.facePoints_position_local[(int)brekel_facePoint.eyelid_Down_R];
		if(face.Nostril_L != null)			face.Nostril_L.localPosition			= face.facePoints_position_local[(int)brekel_facePoint.nostril_L];
		if(face.Nostril_R != null)			face.Nostril_R.localPosition			= face.facePoints_position_local[(int)brekel_facePoint.nostril_R];
		if(face.Cheek_1_L != null)			face.Cheek_1_L.localPosition			= face.facePoints_position_local[(int)brekel_facePoint.cheek1_L];
		if(face.Cheek_2_L != null)			face.Cheek_2_L.localPosition			= face.facePoints_position_local[(int)brekel_facePoint.cheek2_L];
		if(face.Cheek_1_R != null)			face.Cheek_1_R.localPosition			= face.facePoints_position_local[(int)brekel_facePoint.cheek1_R];
		if(face.Cheek_2_R != null)			face.Cheek_2_R.localPosition			= face.facePoints_position_local[(int)brekel_facePoint.cheek2_R];
		if(face.Lips_Up_Center != null)		face.Lips_Up_Center.localPosition		= face.facePoints_position_local[(int)brekel_facePoint.lips_Up];
		if(face.Lips_Up_Inner_L != null)	face.Lips_Up_Inner_L.localPosition		= face.facePoints_position_local[(int)brekel_facePoint.lips_Up_Inner_L];
		if(face.Lips_Up_Outer_L != null)	face.Lips_Up_Outer_L.localPosition		= face.facePoints_position_local[(int)brekel_facePoint.lips_Up_Outer_L];
		if(face.Lips_Corner_L != null)		face.Lips_Corner_L.localPosition		= face.facePoints_position_local[(int)brekel_facePoint.lips_Corner_L];
		if(face.Lips_Down_Outer_L != null)	face.Lips_Down_Outer_L.localPosition	= face.facePoints_position_local[(int)brekel_facePoint.lips_Down_Outer_L];
		if(face.Lips_Down_Inner_L != null)	face.Lips_Down_Inner_L.localPosition	= face.facePoints_position_local[(int)brekel_facePoint.lips_Down_Inner_L];
		if(face.Lips_Down != null)			face.Lips_Down.localPosition			= face.facePoints_position_local[(int)brekel_facePoint.lips_Down];
		if(face.Lips_Down_Inner_R != null)	face.Lips_Down_Inner_R.localPosition	= face.facePoints_position_local[(int)brekel_facePoint.lips_Down_Inner_R];
		if(face.Lips_Down_Outer_R != null)	face.Lips_Down_Outer_R.localPosition	= face.facePoints_position_local[(int)brekel_facePoint.lips_Down_Outer_R];
		if(face.Lips_Corner_R != null)		face.Lips_Corner_R.localPosition		= face.facePoints_position_local[(int)brekel_facePoint.lips_Corner_R];
		if(face.Lips_Up_Outer_R != null)	face.Lips_Up_Outer_R.localPosition		= face.facePoints_position_local[(int)brekel_facePoint.lips_Up_Outer_R];
		if(face.Lips_Up_Inner_R != null)	face.Lips_Up_Inner_R.localPosition		= face.facePoints_position_local[(int)brekel_facePoint.lips_Up_Inner_R];
		if(face.Jaw != null)				face.Jaw.localPosition					= face.facePoints_position_local[(int)brekel_facePoint.jaw];
	}
	
	
	

	//======================================
	// Connect to Brekel TCP network socket
	//======================================
	private bool Connect()
	{
		// try to connect to the Brekel Kinect Pro Body TCP network streaming port
		try
		{
			// instantiate new TcpClient
			client = new TcpClient(host, port);

			// Start an asynchronous read invoking DoRead to avoid lagging the user interface.
			client.GetStream().BeginRead(readBuffer, 0, READ_BUFFER_SIZE, new AsyncCallback(FetchFrame), null);

			Debug.Log("Connected to Brekel Kinect Pro Face v2");
			return true;
		}
		catch (Exception ex)
		{
			Debug.Log("Error, can't connect to Brekel Kinect Pro Face v2!" + ex.ToString());
			return false;
		}
	}


	//===========================================
	// Disconnect from Brekel TCP network socket
	//===========================================
	private void Disconnect()
	{
		if(client != null)
			client.Close();
		Debug.Log("Disconnected from Brekel Kinect Pro Face v2");
	}

	
	//=====================================================================================
	// Fetch and parse data from the TCP network socket, asynchronous from the main thread
	//=====================================================================================
	private void FetchFrame(IAsyncResult ar)
	{
		int BytesRead = 0;
		//--------------------
		// try to read packet
		//--------------------
		try
		{
			// Finish asynchronous read into readBuffer and get number of bytes read.
			BytesRead = client.GetStream().EndRead(ar);

			// if no bytes were read server has closed
			if(BytesRead < 1)
			{
				Debug.Log("Brekel Kinect Pro Face v2 Disconnected");
				isConnected = false;
				return;
			}

			// Convert the byte array the message was saved into
			Encoding.ASCII.GetString(readBuffer, 0, BytesRead);

			// start reading packet
			int		index			= 0;
			char	packet_start	= (char)readBuffer[index];					index += 1;
			ushort	packet_size		= BitConverter.ToUInt16(readBuffer, index);	index += 2;
	
			// if we have a valid packet then parse it
			if(BytesRead>=READ_BUFFER_SIZE && packet_size>=READ_BUFFER_SIZE && packet_start==expected_packetStart)
			{
				// prevent Update from updating while we're parsing network data
				readingFromNetwork = true;

				int		max_num_faces		= BitConverter.ToInt32(readBuffer, index); index += 4;
				int		max_num_animUnits	= BitConverter.ToInt32(readBuffer, index); index += 4;

				// only continue parsing if number of faces and blendshapeIDs match our array sizes
				if(faces.GetLength(0)==max_num_faces && faces[0].animUnitValues.GetLength(0)==max_num_animUnits)
				{
					for(int faceID=0; faceID<max_num_faces; faceID++)	{ faces[faceID].isTracked		= BitConverter.ToBoolean(readBuffer, index);	index += 4; }
					for(int faceID=0; faceID<max_num_faces; faceID++)	{ faces[faceID].trackingID		= BitConverter.ToInt32(readBuffer, index);		index += 4;	}
					for(int faceID=0; faceID<max_num_faces; faceID++)	{ faces[faceID].timestamp		= BitConverter.ToSingle(readBuffer, index);		index += 4;	}
					for(int faceID=0; faceID<max_num_faces; faceID++)
					{
						vec.x = BitConverter.ToSingle(readBuffer, index);		index += 4;
						vec.y = BitConverter.ToSingle(readBuffer, index);		index += 4;
						vec.z = BitConverter.ToSingle(readBuffer, index);		index += 4;
						faces[faceID].neck_position_global = ConvertPosition(vec);
					}
					for(int faceID=0; faceID<max_num_faces; faceID++)
					{
						vec.x = BitConverter.ToSingle(readBuffer, index);		index += 4;
						vec.y = BitConverter.ToSingle(readBuffer, index);		index += 4;
						vec.z = BitConverter.ToSingle(readBuffer, index);		index += 4;
						faces[faceID].neck_rotation_global = ConvertRotation(vec);
					}
					for(int faceID=0; faceID<max_num_faces; faceID++)
					{
						vec.x = BitConverter.ToSingle(readBuffer, index);		index += 4;
						vec.y = BitConverter.ToSingle(readBuffer, index);		index += 4;
						vec.z = BitConverter.ToSingle(readBuffer, index);		index += 4;
						faces[faceID].head_position_local = ConvertPosition(vec);
					}
					for(int faceID=0; faceID<max_num_faces; faceID++)
					{
						vec.x = BitConverter.ToSingle(readBuffer, index);		index += 4;
						vec.y = BitConverter.ToSingle(readBuffer, index);		index += 4;
						vec.z = BitConverter.ToSingle(readBuffer, index);		index += 4;
						faces[faceID].head_rotation_local = ConvertRotation(vec);
					}
					for(int faceID=0; faceID<max_num_faces; faceID++)
					{
						for(int animUnitID=0; animUnitID<(int)brekel_faceAnimUnit.numFaceAnimUnits; animUnitID++)
						{
							faces[faceID].animUnitValues[animUnitID] = BitConverter.ToSingle(readBuffer, index) * 100f;
							index += 4;
						}
					}
					for(int faceID=0; faceID<max_num_faces; faceID++)
					{
						for(int facePointID=0; facePointID<(int)brekel_facePoint.numFacePoints; facePointID++)
						{
							vec.x = BitConverter.ToSingle(readBuffer, index);		index += 4;
							vec.y = BitConverter.ToSingle(readBuffer, index);		index += 4;
							vec.z = BitConverter.ToSingle(readBuffer, index);		index += 4;
							faces[faceID].facePoints_position_local[facePointID] = ConvertPosition(vec);
						}
					}
					for(int faceID=0; faceID<max_num_faces; faceID++)
					{
						for(int facePointID=0; facePointID<(int)brekel_facePoint.numFacePoints; facePointID++)
						{
							vec.x = BitConverter.ToSingle(readBuffer, index);		index += 4;
							vec.y = BitConverter.ToSingle(readBuffer, index);		index += 4;
							vec.z = BitConverter.ToSingle(readBuffer, index);		index += 4;
							faces[faceID].facePoints_rotation_local[facePointID] = ConvertRotation(vec);
						}
					}
				}
			}

			// done reading from network
			isConnected			= true;
			readingFromNetwork	= false;
		}


		//----------------------------------------------------------------
		// some non-critical error has occurred, possibly a broken packet
		//----------------------------------------------------------------
		catch
		{
			// do nothing
		}

		// Start a new asynchronous read into readBuffer.
		client.GetStream().BeginRead(readBuffer, 0, READ_BUFFER_SIZE, new AsyncCallback(FetchFrame), null);
	}


	//=================================================================================================================
	// Helper function to convert a position from a right-handed (Brekel) to left-handed coordinate system (both Y-up)
	//=================================================================================================================
	private Vector3 ConvertPosition(Vector3 position)
	{
		position.x *= -scaleFactor;
		position.y *=  scaleFactor;
		position.z *=  scaleFactor;
		return position;
	}


	//=================================================================================================================
	// Helper function to convert a rotation from a right-handed (Brekel) to left-handed coordinate system (both Y-up)
	//=================================================================================================================
	private Quaternion ConvertRotation(Vector3 rotation)
	{
		Quaternion qx = Quaternion.AngleAxis(rotation.x, Vector3.right);
		Quaternion qy = Quaternion.AngleAxis(rotation.y, Vector3.down);
		Quaternion qz = Quaternion.AngleAxis(rotation.z, Vector3.back);
		Quaternion qq = qz * qy * qx;
		return qq;
	}
}
