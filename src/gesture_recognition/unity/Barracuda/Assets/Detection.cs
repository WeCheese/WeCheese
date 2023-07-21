using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Barracuda;
using UnityEngine.UI;

public class Detection : MonoBehaviour
{
    const int IMAGE_SIZE = 224;
    const int IMAGE_SIZE_PT = 256;
    const string INPUT_NAME = "input_1";
    const string LANDMARK = "Identity";
    const string SCORE = "Identity_1";
    const string PTOUTPUT = "output";
    string model_type = "pytorch";

    [Header("Model")]
    public NNModel modelFile;

    [Header("Scene")]
    public CameraView CameraView;
    public Preprocess preprocess;
    public Text uiText;

    IWorker worker;
    // Start is called before the first frame update
    void Start()
    {
        var model = ModelLoader.Load(modelFile);
        worker = WorkerFactory.CreateWorker(WorkerFactory.Type.ComputePrecompiled, model);
    }

    // Update is called once per frame
    void Update()
    {
        WebCamTexture webCamTexture = CameraView.GetWebCamTexture();
        if (webCamTexture.didUpdateThisFrame && webCamTexture.width > 100) {
            if (model_type == "pytorch") {
                preprocess.ScaleAndCropImage(webCamTexture, IMAGE_SIZE_PT, RunModel);
            } else
            if (model_type == "mediapipe") {
                preprocess.ScaleAndCropImage(webCamTexture, IMAGE_SIZE, RunModel);
            }
        }
    }

    void RunModel(byte[] pixels) {
        StartCoroutine(RunModelRoutine(pixels));
    }

    IEnumerator RunModelRoutine(byte[] pixels) {
        Tensor tensor = new Tensor();
        if (model_type == "pytorch") {
            tensor = TransformInputPytorch(pixels);
        } else
        if (model_type == "mediapipe") {
            tensor = TransformInput(pixels);
        }

        worker.Execute(tensor);
        float score = 100;
        float wrist_x = 0;
        float wrist_y = 0;
        
        // get ouptut
        if (model_type == "pytorch") {
            Tensor outputTensor = worker.PeekOutput(PTOUTPUT);
            wrist_x = outputTensor[0];
            wrist_y = outputTensor[1];
        } else
        if (model_type == "mediapipe") {
            Tensor outputTensor = worker.PeekOutput(LANDMARK);
            score = worker.PeekOutput(SCORE)[0];
            float[] temp = outputTensor.ToReadOnlyArray();
            wrist_x = temp[0] / IMAGE_SIZE;
            wrist_y = 1 - temp[1] / IMAGE_SIZE;
            outputTensor.Dispose();
        }

        // set UI
        if (score > 0.005) {
            uiText.text = score.ToString() + ", " +  wrist_x.ToString() + ", " +  wrist_y.ToString();
        } else {
            uiText.text = "no hand detected";
        }

        // dispose tensors
        tensor.Dispose();
        yield return null;
    }

    Tensor TransformInput(byte[] pixels) {
        float[] transformedPixels = new float[pixels.Length];

        for (int i = 0; i < pixels.Length; i++) {
            transformedPixels[i] = pixels[i] / 255f;
        }

        return new Tensor(1, IMAGE_SIZE, IMAGE_SIZE, 3, transformedPixels);
    }

    Tensor TransformInputPytorch(byte[] pixels) {
        float[] transformedPixels = new float[pixels.Length];

        for (int i = 0; i < pixels.Length; i++) {
            transformedPixels[i] = (pixels[i] - 128f) / 256f;
        }

        return new Tensor(1, IMAGE_SIZE_PT, IMAGE_SIZE_PT, 3, transformedPixels);
    }
}
