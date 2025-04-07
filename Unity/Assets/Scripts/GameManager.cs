using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    [Header("Tracking")]
    public Transform playerTransform;
    public PlayerLightController lightController;
    public float maxDepth = 100f; // in meters
    public float maxDepthUnits = 1f; // depth in Unity units that maps to maxDepth
    
    [Header("Game State")]
    public UnityEvent onWin;
    public UnityEvent onLose;
    public TextMeshProUGUI gameOverText;
    public GameObject mainUI;

    private bool gameEnded = false;
    private float elapsedTime = 0f;
    public float ElapsedTime => elapsedTime;

    void Update()
    {
        if (gameEnded) return;

        // Track time
        elapsedTime += Time.deltaTime;

        // Check win condition (hit max depth)
        float currentDepthUnits = Mathf.Max(0f, -playerTransform.position.y);
        float currentDepthMeters = currentDepthUnits * (maxDepth / maxDepthUnits);

        if (currentDepthMeters >= maxDepth)
        {
            EndGame(true);
        }

        // Check lose condition (glow power zero or less)
        if (lightController.GetLightCharge() <= 0f)
        {
            EndGame(false);
        }
    }

    public void EndGame(bool won)
    {
        if (gameEnded) return;

        gameEnded = true;
        mainUI.SetActive(false);
        gameOverText.gameObject.SetActive(true);

        if (won)
        {
            gameOverText.text = "Hooray, you made it the bottom!";
            onWin?.Invoke();
        }
        else
        {
            gameOverText.text = "Uh oh, you're all out of light!";
            onLose?.Invoke();
        }

        // Optional: Freeze time or show UI
        Time.timeScale = 0f;
    }
}
