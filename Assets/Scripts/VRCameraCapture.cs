using System;
using System.Collections;
using UnityEngine;

public class VRCameraCapture : MonoBehaviour
{
    [Header("Camera to Capture")]
    public Camera captureCamera;

    [Header("Resolution")]
    public int captureWidth = 1024;
    public int captureHeight = 1024;

    [HideInInspector] public Texture2D lastCapturedTexture;

    public Action<Texture2D> OnScreenshotTaken;

    public void Capture()
    {
        StartCoroutine(CaptureRoutine());
    }

    private IEnumerator CaptureRoutine()
    {
        // wait until frame rendered
        yield return new WaitForEndOfFrame();

        RenderTexture rt = new RenderTexture(captureWidth, captureHeight, 24);
        captureCamera.targetTexture = rt;
        captureCamera.Render();

        RenderTexture.active = rt;
        Texture2D tex = new Texture2D(captureWidth, captureHeight, TextureFormat.RGB24, false);
        tex.ReadPixels(new Rect(0, 0, captureWidth, captureHeight), 0, 0);
        tex.Apply();

        captureCamera.targetTexture = null;
        RenderTexture.active = null;
        Destroy(rt);

        lastCapturedTexture = tex;
        Debug.Log("Screenshot captured");
        OnScreenshotTaken?.Invoke(tex);
    }
    public Texture2D GetLastScreenshot()
    {
        return lastCapturedTexture;
    }

}
