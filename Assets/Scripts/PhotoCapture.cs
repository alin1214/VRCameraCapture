using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class PhotoCapture : MonoBehaviour
{
    [Header("Input")]
    [SerializeField] private InputActionReference takePhotoAction;

    [Header("Photo Display")]
    [SerializeField] private Image photoDisplayArea;
    [SerializeField] private GameObject photoFrame;
    [SerializeField] private GameObject cameraUI;

    [Header("Timing")]
    [SerializeField] private float triggerCooldown = 0.35f;

    [Header("Audio")]
    [SerializeField] private AudioSource cameraAudio;

    private bool viewingPhoto = false;
    private float lastTriggerTime = -10f;

    void OnEnable()
    {
        if (takePhotoAction != null)
            takePhotoAction.action.Enable();
    }

    void OnDisable()
    {
        if (takePhotoAction != null)
            takePhotoAction.action.Disable();
    }

    void Start()
    {
        if (photoFrame != null) photoFrame.SetActive(false);
        if (cameraUI != null) cameraUI.SetActive(true);
    }

    void Update()
    {
        if (takePhotoAction == null) return;

        if (takePhotoAction.action.WasPressedThisFrame())
        {
            if (Time.time - lastTriggerTime < triggerCooldown)
                return;

            lastTriggerTime = Time.time;

            if (!viewingPhoto)
                StartCoroutine(CapturePhoto());
            else
                RemovePhoto();
        }
    }

    IEnumerator CapturePhoto()
    {
        if (cameraAudio != null) cameraAudio.Play();
        if (cameraUI != null) cameraUI.SetActive(false);

        viewingPhoto = true;

        yield return new WaitForEndOfFrame();

        Texture2D raw = ScreenCapture.CaptureScreenshotAsTexture();

        Texture2D saved = new Texture2D(raw.width, raw.height, TextureFormat.RGBA32, false);
        saved.SetPixels32(raw.GetPixels32());
        saved.Apply();

        Destroy(raw);

        Sprite savedSprite = Sprite.Create(
            saved,
            new Rect(0, 0, saved.width, saved.height),
            new Vector2(0.5f, 0.5f),
            100f
        );

        ShowPhoto(savedSprite);

        if (PhotoAlbumManager.Instance != null)
            PhotoAlbumManager.Instance.AddPhoto(savedSprite);
    }

    void ShowPhoto(Sprite s)
    {
        if (photoFrame != null)
            photoFrame.SetActive(true);

        if (photoDisplayArea != null)
            photoDisplayArea.sprite = s;
    }

    void RemovePhoto()
    {
        viewingPhoto = false;

        if (photoFrame != null)
            photoFrame.SetActive(false);

        if (photoDisplayArea != null)
            photoDisplayArea.sprite = null;

        if (cameraUI != null)
            cameraUI.SetActive(true);
    }
}
