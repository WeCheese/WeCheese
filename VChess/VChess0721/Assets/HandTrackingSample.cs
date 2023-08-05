using System;
using Unity.Collections.LowLevel.Unsafe;
using System.Collections.Generic;
using TensorFlowLite;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

[RequireComponent(typeof(WebCamInput))]
public class HandTrackingSample : MonoBehaviour
{
    [SerializeField, FilePopup("*.tflite")]
    private string palmModelFile = "";
    [SerializeField, FilePopup("*.tflite")]
    private string landmarkModelFile = "";

    [SerializeField]
    private RawImage cameraView = null;
    [SerializeField]
    private RawImage debugPalmView = null;

    private PalmDetect palmDetect;
    private HandLandmarkDetect landmarkDetect;

    // just cache for GetWorldCorners
    private readonly Vector3[] rtCorners = new Vector3[4];
    private readonly Vector3[] worldJoints = new Vector3[HandLandmarkDetect.JOINT_COUNT];
    private PrimitiveDraw draw;
    private List<PalmDetect.Result> palmResults;
    private HandLandmarkDetect.Result landmarkResult;

    public GameObject new_cursor;
    private float prev_x = 0;
    private float prev_y = 0;

    public ARCameraManager manager;
    private Texture2D m_CameraTexture;
    private bool first_frame = true;
    public TextMeshProUGUI Display2;

    private bool hand_recog_mode_on = false;
    public MoveChess move_chess;

    private void Start()
    {
        palmDetect = new PalmDetect(palmModelFile);
        landmarkDetect = new HandLandmarkDetect(landmarkModelFile);
        Debug.Log($"landmark dimension: {landmarkDetect.Dim}");

        //draw = new PrimitiveDraw();

        manager.frameReceived += OnCameraFrameReceived;

        //var webCamInput = GetComponent<WebCamInput>();
        //Debug.Log($"webcam name: {webCamInput.name}");
        //webCamInput.OnTextureUpdate.AddListener(OnTextureUpdate);
    }

    public void ToggleMode()
    {
        hand_recog_mode_on = !hand_recog_mode_on;
    }

    void OnCameraFrameReceived(ARCameraFrameEventArgs eventArgs)
    {
        cameraUpdate();
    }

    private void OnDestroy()
    {
        //var webCamInput = GetComponent<WebCamInput>();
        //webCamInput.OnTextureUpdate.RemoveListener(OnTextureUpdate);

        palmDetect?.Dispose();
        landmarkDetect?.Dispose();
    }

    void Update()
    {
        if (hand_recog_mode_on && landmarkResult != null && landmarkResult.score > 0.2f)
        {
            // 1: Hand move recog
            float curr_x = landmarkResult.joints[0].x;
            float curr_y = landmarkResult.joints[0].y;
            Debug.Log($"hand move: {curr_x - prev_x} {curr_y - prev_y}");
            Debug.Log($"hand move raw pos: {curr_x} {curr_y}");

            if (!first_frame && PlaceBoard.board_placed)
            {
                //new_cursor.transform.Translate((curr_y - prev_y) * 0.2f, 0f, (curr_x - prev_x) * 0.2f, Space.Self);
                new_cursor.transform.Translate((curr_y - prev_y) * (-0.9f), 0f, (curr_x - prev_x) * (-0.9f), Space.Self);

                // 2: Gesture recog
                float x1 = landmarkResult.joints[4].x;
                float y1 = landmarkResult.joints[4].y;
                float x2 = landmarkResult.joints[8].x;
                float y2 = landmarkResult.joints[8].y;
                float disSquare = (x1 - x2) * (x1 - x2) + (y1 - y2) * (y1 - y2);
                float tipThreshold = 0.002f;

                Display2.text = string.Format("pos:{0},{1} dis:{2}", x1, y1, disSquare);
                if (disSquare < tipThreshold)
                {
                    Display2.text = string.Format("Good!!! dis:{0}", disSquare);
                    move_chess.SelectByHand();
                }
            }
            first_frame = false;

            prev_x = curr_x;
            prev_y = curr_y;
        }
    }


    // Responsible for generating: palmResults, landmarkResult
    unsafe void cameraUpdate()
    {
        if (!manager.TryAcquireLatestCpuImage(out XRCpuImage image))
            return;

        const TextureFormat format = TextureFormat.RGBA32;

        if (m_CameraTexture == null || m_CameraTexture.width != image.width || m_CameraTexture.height != image.height)
            m_CameraTexture = new Texture2D(image.width, image.height, format, false);

        // Convert the image to format, flipping the image across the Y axis.
        // We can also get a sub rectangle, but we'll get the full image here.
        var conversionParams = new XRCpuImage.ConversionParams(image, format, XRCpuImage.Transformation.None);

        // Texture2D allows us write directly to the raw texture data
        // This allows us to do the conversion in-place without making any copies.
        var rawTextureData = m_CameraTexture.GetRawTextureData<byte>();
        try
        {
            image.Convert(conversionParams, new IntPtr(rawTextureData.GetUnsafePtr()), rawTextureData.Length);
        }
        finally
        {
            // We must dispose of the XRCpuImage after we're finished
            // with it to avoid leaking native resources.
            image.Dispose();
        }

        // Apply the updated texture data to our texture
        m_CameraTexture.Apply();

        //// Set the RawImage's texture so we can visualize it.
        //m_RawCameraImage.texture = m_CameraTexture;

        palmDetect.Invoke(m_CameraTexture);

        palmResults = palmDetect.GetResults(0.7f, 0.3f);

        if (palmResults.Count <= 0) return;

        // Detect only first palm
        landmarkDetect.Invoke(m_CameraTexture, palmResults[0]);
        // debugPalmView.texture = landmarkDetect.inputTex;

        landmarkResult = landmarkDetect.GetResult();
    }

    private void OnTextureUpdate(Texture texture)
    {
        palmDetect.Invoke(texture);
        //cameraView.material = palmDetect.transformMat;
        //cameraView.rectTransform.GetWorldCorners(rtCorners);

        palmResults = palmDetect.GetResults(0.7f, 0.3f);


        if (palmResults.Count <= 0) return;

        // Detect only first palm
        landmarkDetect.Invoke(texture, palmResults[0]);
        // debugPalmView.texture = landmarkDetect.inputTex;

        landmarkResult = landmarkDetect.GetResult();


        float curr_x = landmarkDetect.output0[0];
        float curr_y = landmarkDetect.output0[1];
        Debug.Log($"hand move: {curr_x - prev_x} {curr_y - prev_y}");

        new_cursor.transform.Translate((curr_x - prev_x) * 0.0005f, 0f, (curr_y - prev_y) * 0.0005f, Space.Self);

        prev_x = landmarkDetect.output0[0];
        prev_y = landmarkDetect.output0[1];
    }

    private void DrawFrames(List<PalmDetect.Result> palms)
    {
        Vector3 min = rtCorners[0];
        Vector3 max = rtCorners[2];

        draw.color = Color.green;
        foreach (var palm in palms)
        {
            draw.Rect(MathTF.Lerp(min, max, palm.rect, true), 0.02f, min.z);

            foreach (var kp in palm.keypoints)
            {
                draw.Point(MathTF.Lerp(min, max, (Vector3)kp, true), 0.05f);
            }
        }
        draw.Apply();
    }

    void DrawCropMatrix(in Matrix4x4 matrix)
    {
        draw.color = Color.red;

        Vector3 min = rtCorners[0];
        Vector3 max = rtCorners[2];

        var mtx = matrix.inverse;
        Vector3 a = MathTF.LerpUnclamped(min, max, mtx.MultiplyPoint3x4(new Vector3(0, 0, 0)));
        Vector3 b = MathTF.LerpUnclamped(min, max, mtx.MultiplyPoint3x4(new Vector3(1, 0, 0)));
        Vector3 c = MathTF.LerpUnclamped(min, max, mtx.MultiplyPoint3x4(new Vector3(1, 1, 0)));
        Vector3 d = MathTF.LerpUnclamped(min, max, mtx.MultiplyPoint3x4(new Vector3(0, 1, 0)));

        draw.Quad(a, b, c, d, 0.02f);
        draw.Apply();
    }

    private void DrawJoints(Vector3[] joints)
    {
        draw.color = Color.blue;

        // Get World Corners
        Vector3 min = rtCorners[0];
        Vector3 max = rtCorners[2];

        Matrix4x4 mtx = Matrix4x4.identity;

        // Get joint locations in the world space
        float zScale = max.x - min.x;
        for (int i = 0; i < HandLandmarkDetect.JOINT_COUNT; i++)
        {
            Vector3 p0 = mtx.MultiplyPoint3x4(joints[i]);
            Vector3 p1 = MathTF.Lerp(min, max, p0);
            p1.z += (p0.z - 0.5f) * zScale;
            worldJoints[i] = p1;
        }

        // Cube
        for (int i = 0; i < HandLandmarkDetect.JOINT_COUNT; i++)
        {
            draw.Cube(worldJoints[i], 0.3f);
        }

        // Connection Lines
        var connections = HandLandmarkDetect.CONNECTIONS;
        for (int i = 0; i < connections.Length; i += 2)
        {
            draw.Line3D(
                worldJoints[connections[i]],
                worldJoints[connections[i + 1]],
                0.05f);
        }

        draw.Apply();
    }

}
