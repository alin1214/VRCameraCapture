using UnityEngine;
using System;

public class PhotoDisplayManager : MonoBehaviour
{
    public Renderer photoRenderer;
    public float freezeDuration = 2f;
    public Texture2D lastCapturedPhoto;
    public int lastPhotoWidth;
    public int lastPhotoHeight;
    public Action OnPhotoFreezeComplete;
    public GameObject photoCardPrefab;
    public GameObject lastPhotoCard;

    void Awake()
    {
        gameObject.SetActive(false);
    }

    public void ShowPhoto(Texture2D photo)
    {
        lastCapturedPhoto = photo;
        lastPhotoWidth = photo.width;
        lastPhotoHeight = photo.height;

        photoRenderer.material.mainTexture = photo;

        gameObject.SetActive(true);
        Invoke(nameof(FreezeDone), freezeDuration);
    }

    void FreezeDone()
    {
        gameObject.SetActive(false);
        OnPhotoFreezeComplete?.Invoke();
    }

    public void SpawnFinalPhotoCard()
    {
        if (photoCardPrefab == null || lastCapturedPhoto == null) return;

        if (lastPhotoCard != null)
        {
            Destroy(lastPhotoCard);
            lastPhotoCard = null;
        }

        GameObject card = Instantiate(photoCardPrefab, transform.position, transform.rotation);

        Renderer rend = card.GetComponentInChildren<Renderer>();
        if (rend != null)
        {
            rend.material.mainTexture = lastCapturedPhoto;
        }

        lastPhotoCard = card;
    }
}


