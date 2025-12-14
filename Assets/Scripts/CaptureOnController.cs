using UnityEngine;
using UnityEngine.XR;

public class CaptureOnController : MonoBehaviour
{
    public VRCameraCapture cameraCapture;
    public PhotoDisplayManager photoDisplayManager;

    private bool wasPressedLastFrame = false;

    void Update()
    {
        if (cameraCapture == null || photoDisplayManager == null)
            return;

        InputDevice device = InputDevices.GetDeviceAtXRNode(XRNode.RightHand);
        if (!device.isValid)
            return;

        if (device.TryGetFeatureValue(CommonUsages.primaryButton, out bool isPressed))
        {
            if (isPressed && !wasPressedLastFrame)
            {
                Debug.Log("VR controller button pressed → CAPTURE");

                cameraCapture.Capture();
                cameraCapture.OnScreenshotTaken += OnPhotoCaptured;
            }

            wasPressedLastFrame = isPressed;
        }
    }

    private void OnPhotoCaptured(Texture2D photo)
    {
        cameraCapture.OnScreenshotTaken -= OnPhotoCaptured;

        photoDisplayManager.ShowPhoto(photo);
    }
}
