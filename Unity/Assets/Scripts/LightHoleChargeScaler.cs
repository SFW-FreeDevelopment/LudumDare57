using UnityEngine;

public class LightHoleChargeScaler : MonoBehaviour
{
    public PlayerLightController lightController;
    public float minScale = 0.4f;
    public float maxScale = 1.4f;

    private Vector3 chargeBaseScale = Vector3.one;

    void Start()
    {
        chargeBaseScale = transform.localScale;
    }

    void Update()
    {
        if (lightController == null) return;

        if (GameManager.Instance != null && GameManager.Instance.GameOver)
        {
            transform.localScale = new Vector3(0, 0, 0);
            return;
        }
        
        float charge = lightController.GetLightCharge(); // 0–1
        float targetScale = Mathf.Lerp(minScale, maxScale, charge);
        chargeBaseScale = new Vector3(targetScale, targetScale, 1f);
        transform.localScale = chargeBaseScale;
    }
}
