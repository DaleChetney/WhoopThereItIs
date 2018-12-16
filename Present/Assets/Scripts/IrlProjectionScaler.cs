using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class IrlProjectionScaler : MonoBehaviour
{
    // Don't touch this value unless we move where the IRL area is on the screen
    private const float ScreenRatio = 0.65f;

    [SerializeField]
    private Camera _targetCamera;
    [SerializeField]
    private RawImage _targetImage;

    private RenderTexture _dynamicTexture;

    void Start()
    {
        if (_targetCamera == null)
            _targetCamera = GetComponent<Camera>();

        if(_targetCamera == null)
        {
            Debug.LogError($"No Target Camera provided or available on the current GameObject ({gameObject.name})!!");
            return;
        }

        var mainCamera = Camera.main;

        if (mainCamera == null)
        {
            Debug.LogError("No Main Camera found!! Ensure one camera in the scene has the MainCamera tag!!");
            return;
        }
        if(mainCamera == _targetCamera)
        {
            Debug.LogError($"Main Camera and Target Camera cannot be the same ({_targetCamera.name})!!");
            return;
        }
        if (MultipleMainCamerasInScene())
        {
            Debug.LogError("More than one MainCamera in the scene! Ensure only one camera has the MainCamera tag!!");
        }

        int screenWidth = mainCamera.pixelWidth;
        int screenHeight = mainCamera.pixelHeight;

        int stretch = (int)(2 * (ScreenRatio * screenWidth));

        string log = "IrlProjectionScaler Start() Details";
        log += $"\nMain Camera: {mainCamera.name}, Target Camera: {_targetCamera.name}, Target Image: {_targetImage}";
        log += "Current Resolution Values:";
        log += $"\n- [From Camera] Scaled: {mainCamera.scaledPixelWidth}, {mainCamera.scaledPixelHeight}, Unscaled: {mainCamera.pixelWidth}, {mainCamera.pixelHeight}";
        log += $"\n- [From Screen] Dimensions: {Screen.width}, {Screen.height}, Resolution: {screenWidth}, {screenHeight}";
        log += $"\nCalculated Stretch: {stretch}";

        _dynamicTexture = new RenderTexture(stretch, screenHeight, 24);

        if (_targetCamera.targetTexture != null)
            _targetCamera.targetTexture.Release();

        _targetCamera.fieldOfView = 60;
        _targetCamera.targetTexture = _dynamicTexture;

        _targetImage.rectTransform.pivot = new Vector2(0.5f, 0.5f);
        _targetImage.rectTransform.anchorMin = new Vector2(ScreenRatio, 0);
        _targetImage.rectTransform.anchorMax = new Vector2(1, 1);
        _targetImage.rectTransform.sizeDelta = new Vector2(stretch, 0);

        _targetImage.texture = _dynamicTexture;

        _targetImage.gameObject.SetActive(true);

        Debug.Log(log);
    }

    private bool MultipleMainCamerasInScene()
    {
        var allCameras = Camera.allCameras;
        int numMainCameras = allCameras.Count(c => c.tag == "MainCamera");
        return numMainCameras > 1;
    }

    void OnDestroy()
    {
        if(_dynamicTexture != null)
        {
            _dynamicTexture.Release();
            _dynamicTexture.DiscardContents();
            _dynamicTexture = null;
        }
    }
}
