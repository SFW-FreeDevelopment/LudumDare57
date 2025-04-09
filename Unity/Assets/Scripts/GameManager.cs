using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections.Generic;

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
    public AudioSource gameMusicAudioSource;

    [Header("Sound FX")]
    public AudioClip gameOverSound;
    public AudioClip gameWonSound;
    public AudioClip clickSound;
    public AudioClip backSound;
    public AudioClip collect1Sound;
    public AudioClip collect2Sound;

    public bool GameOver => gameEnded;

    private bool gameEnded = false;
    private float elapsedTime = 0f;
    public float ElapsedTime => elapsedTime;
    
    [Header("Krill Spawn Settings")]
    public GameObject krillPrefab;
    public int krillPerZone = 10;
    public int zoneCount = 10;
    public float zoneWidth = 10f;
    public float zoneHeight = 10f;
    public float krillMinSpacing = 1f;
    public float yOffset = -1f; // Shift all krill zones downward by 1 unit

    void Awake()
    {
        Instance = this;

        if (restartButton != null)
            restartButton.onClick.AddListener(() => {
                OneShotAudioPlayer.PlayClip(OneShotAudioPlayer.SoundEffect.Click);
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            });

        if (playGameButton != null)
            playGameButton.onClick.AddListener(() => {
                OneShotAudioPlayer.PlayClip(OneShotAudioPlayer.SoundEffect.Click);
                SceneManager.LoadScene("Game");
            });

        if (mainMenuButton != null)
            mainMenuButton.onClick.AddListener(() => {
                OneShotAudioPlayer.PlayClip(OneShotAudioPlayer.SoundEffect.Click);
                SceneManager.LoadScene("Main");
            });
    }

    void Start()
    {
        if (!isMainMenu)
        {
            SpawnKrillAcrossZones();
        }
    }

    void Update()
    {
        if (isMainMenu || gameEnded) return;

        elapsedTime += Time.deltaTime;

        float currentDepthUnits = Mathf.Max(0f, -playerTransform.position.y);
        float currentDepthMeters = currentDepthUnits * (maxDepth / maxDepthUnits);

        if (currentDepthMeters >= maxDepth)
        {
            EndGame(true);
        }

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
        gameMusicAudioSource.enabled = false;

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
    }
    
    void SpawnKrillAcrossZones()
    {
        for (int i = 0; i < zoneCount; i++)
        {
            float zoneMinX = -zoneWidth / 2f;
            float zoneMaxX = zoneWidth / 2f;
            
            float zoneMinY = -i * zoneHeight + yOffset;
            float zoneMaxY = zoneMinY + zoneHeight;

            List<Vector2> spawnedThisZone = new List<Vector2>();
            int attempts = 0;
            int maxAttempts = 1000;

            while (spawnedThisZone.Count < krillPerZone && attempts < maxAttempts)
            {
                attempts++;
                float x = Random.Range(zoneMinX, zoneMaxX);
                float y = Random.Range(zoneMinY, zoneMaxY);
                Vector2 candidate = new Vector2(x, y);

                bool tooClose = false;
                foreach (Vector2 pos in spawnedThisZone)
                {
                    if (Vector2.Distance(pos, candidate) < krillMinSpacing)
                    {
                        tooClose = true;
                        break;
                    }
                }

                if (!tooClose)
                {
                    Instantiate(krillPrefab, candidate, Quaternion.identity);
                    spawnedThisZone.Add(candidate);
                }
            }
        }
    }
}
