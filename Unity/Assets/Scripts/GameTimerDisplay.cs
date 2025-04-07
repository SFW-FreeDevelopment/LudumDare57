using UnityEngine;
using TMPro;

public class GameTimerDisplay : MonoBehaviour
{
    public TextMeshProUGUI timeText;
    public GameManager gameManager; // Reference to GameManager

    void FixedUpdate()
    {
        if (gameManager == null) return;

        float time = gameManager.ElapsedTime;
        int minutes = Mathf.FloorToInt(time / 60f);
        int seconds = Mathf.FloorToInt(time % 60f);
        timeText.text = $"TIME: {minutes}m {seconds}s";
    }
}
