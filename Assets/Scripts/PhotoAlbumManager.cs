using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PhotoAlbumManager : MonoBehaviour
{
    public static PhotoAlbumManager Instance;

    [SerializeField] private Transform albumGridParent;
    [SerializeField] private GameObject photoThumbnailPrefab;

    private List<Sprite> photos = new List<Sprite>();

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public void AddPhoto(Sprite photoSprite)
    {
        if (photoSprite == null) return;

        photos.Add(photoSprite);

        if (albumGridParent == null || photoThumbnailPrefab == null) return;

        GameObject thumb = Instantiate(photoThumbnailPrefab, albumGridParent);

        Image img = thumb.GetComponentInChildren<Image>(true);
        if (img != null)
        {
            img.sprite = photoSprite;
            img.preserveAspect = true;
            return;
        }

        RawImage raw = thumb.GetComponentInChildren<RawImage>(true);
        if (raw != null)
        {
            raw.texture = photoSprite.texture;
        }
    }
}
