using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    
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
    public GameObject gameOverUI;
    public Button restartButton;
    public Button mainMenuButton;
    public Button playGameButton;
    public bool isMainMenu = false;
    
    [Header("Sound FX")]
    public AudioClip gameOverSound;
    public AudioClip gameWonSound;
    public AudioClip clickSound;
    public AudioClip backSound;
    public AudioClip collect1Sound;
    public AudioClip collect2Sound;

    private bool gameEnded = false;
    private float elapsedTime = 0f;
    public float ElapsedTime => elapsedTime;

    void Awake()
    {
        Instance = this;
        
        if (restartButton != null)
            restartButton.onClick.AddListener(() => {
                Time.timeScale = 1f;
                OneShotAudioPlayer.PlayClip(OneShotAudioPlayer.SoundEffect.Click);
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            });
        
        
        if (playGameButton != null)
            playGameButton.onClick.AddListener(() => {
                Time.timeScale = 1f;
                OneShotAudioPlayer.PlayClip(OneShotAudioPlayer.SoundEffect.Click);
                SceneManager.LoadScene("Game");
            });
        
        if (mainMenuButton != null)
            mainMenuButton.onClick.AddListener(() => {
                Time.timeScale = 1f;
                OneShotAudioPlayer.PlayClip(OneShotAudioPlayer.SoundEffect.Click);
                SceneManager.LoadScene("Main");
            });
    }
    
    void Update()
    {
        if (isMainMenu) return;
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
        gameOverUI.SetActive(true);
        gameOverText.gameObject.SetActive(true);

        if (won)
        {
            gameOverText.text = "Hooray, you made it the bottom!";
            OneShotAudioPlayer.PlayClip(gameWonSound);
            onWin?.Invoke();
        }
        else
        {
            gameOverText.text = "Uh oh, you're all out of light!";
            OneShotAudioPlayer.PlayClip(gameOverSound);
            onLose?.Invoke();
        }
        
        // Optional: Freeze time or show UI
        Time.timeScale = 0f;
    }
}
