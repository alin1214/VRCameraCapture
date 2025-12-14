using UnityEngine;
using UnityEngine.InputSystem;

public class VRAlbumToggle : MonoBehaviour
{
    [SerializeField] private GameObject albumPanel;
    [SerializeField] private InputActionReference openAlbumAction;

    private void OnEnable()
    {
        if (openAlbumAction != null)
            openAlbumAction.action.Enable();
    }

    private void OnDisable()
    {
        if (openAlbumAction != null)
            openAlbumAction.action.Disable();
    }

    private void Start()
    {
        if (albumPanel != null)
            albumPanel.SetActive(false);
    }

    private void Update()
    {
        if (openAlbumAction == null) return;

        if (openAlbumAction.action.WasPressedThisFrame())
            ToggleAlbum();
    }

    private void ToggleAlbum()
    {
        if (albumPanel == null) return;
        albumPanel.SetActive(!albumPanel.activeSelf);
    }
}

