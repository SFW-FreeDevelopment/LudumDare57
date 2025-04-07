using UnityEngine;
using UnityEngine.UI;

public class DarknessAspectSync : MonoBehaviour
{
    public RawImage rawImage;

    void Start()
    {
        UpdateAspect();
    }

    void Update()
    {
        UpdateAspect();
    }

    void UpdateAspect()
    {
        if (rawImage == null) return;

        Material mat = rawImage.material;
        if (mat != null)
        {
            float aspect = (float)Screen.width / Screen.height;
            mat.SetFloat("_AspectRatio", aspect);
        }
    }
}
