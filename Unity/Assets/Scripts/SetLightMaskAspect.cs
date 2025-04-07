using UnityEngine;

public class SetLightMaskAspect : MonoBehaviour
{
    public Material maskedDarknessMaterial;

    void Start()
    {
        float screenAspect = (float)Screen.width / Screen.height;
        maskedDarknessMaterial.SetFloat("_AspectRatio", screenAspect);
    }
}
