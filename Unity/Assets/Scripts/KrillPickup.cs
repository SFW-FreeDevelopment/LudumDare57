using UnityEngine;

public class KrillPickup : MonoBehaviour
{
    public float chargeAmount = 0.1f;
    public GameObject pickupEffect;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        var playSound1 = Random.Range(0, 10) < 5;
        OneShotAudioPlayer.PlayClip(playSound1 ? OneShotAudioPlayer.SoundEffect.Collect1 : OneShotAudioPlayer.SoundEffect.Collect2);
        
        // Handle light charge increase
        PlayerLightController lightController = other.GetComponent<PlayerLightController>();
        if (lightController != null)
        {
            lightController.AddCharge(chargeAmount);
        }

        // Trigger glow pulse if available
        JellyGlowPulse pulse = other.GetComponent<JellyGlowPulse>();
        if (pulse != null)
        {
            pulse.TriggerPulse();
        }

        // Optional sparkle FX
        if (pickupEffect != null)
        {
            Instantiate(pickupEffect, transform.position, Quaternion.identity);
        }

        Destroy(gameObject);
    }
}
