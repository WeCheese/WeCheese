using UnityEngine;
using UnityEngine.UI;

public class CameraView : MonoBehaviour
{
    RawImage rawImage;
    AspectRatioFitter fitter;
    WebCamTexture webcamTexture;
    bool ratioSet;
    // Start is called before the first frame update
    void Start()
    {
        rawImage = GetComponent<RawImage>();
        fitter = GetComponent<AspectRatioFitter>();
        InitWebCam();
    }

    // Update is called once per frame
    void Update()
    {
        if (webcamTexture.width > 100 && !ratioSet) {
            ratioSet = true;
            SetAspectRatio();
        }
    }

    // set the ratio of hight and width
    void SetAspectRatio() {
        float ratio = (float) webcamTexture.width / (float) webcamTexture.height;
        fitter.aspectRatio = ratio;
    }

    // init the webcam texture
    void InitWebCam() {
        string camName = WebCamTexture.devices[0].name;
        webcamTexture = new WebCamTexture(camName, Screen.width, Screen.height, 30);
        rawImage.texture = webcamTexture;
        webcamTexture.Play();
    }

    // get image from the camera
    public WebCamTexture GetWebCamTexture() {
        return webcamTexture;
    }
}
