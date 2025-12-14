using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class PhotoStripManager : MonoBehaviour
{
    public XRSocketInteractor slot1;
    public XRSocketInteractor slot2;
    public XRSocketInteractor slot3;
    public XRGrabInteractable grabInteractable;

    bool madeGrabbable = false;

    void Update()
    {
        if (madeGrabbable) return;

        bool filled1 = slot1 != null && slot1.selectTarget != null;
        bool filled2 = slot2 != null && slot2.selectTarget != null;
        bool filled3 = slot3 != null && slot3.selectTarget != null;

        if (filled1 && filled2 && filled3)
        {
            if (grabInteractable != null)
            {
                grabInteractable.enabled = true;
            }
            madeGrabbable = true;
        }
    }
}

