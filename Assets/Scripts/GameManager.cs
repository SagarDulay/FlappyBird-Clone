using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static int currentScore;

    [SerializeField] private GameObject pipeObstaclePrefab;
    [SerializeField] private float timeBetweenPipes;

    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private GameObject gameOverScreen;

    [SerializeField] private APITest api;
    [SerializeField] private Rigidbody2D birdRigidbody;

    public void StartGame()
    {
        scoreText.gameObject.SetActive(true);
        birdRigidbody.constraints = RigidbodyConstraints2D.None;
        InvokeRepeating("SpawnPipes", 2f, timeBetweenPipes);
    }

    void SpawnPipes()
    {
        Instantiate(pipeObstaclePrefab);
    }

    private void Update()
    {
        scoreText.text = "Score: " + currentScore.ToString();
    }

    public void ShowGameOverScreen()
    {
        api.UploadScoreToDateBase(currentScore);
        gameOverScreen.SetActive(true);
    }

    public void RestartGame()
    {
        api.UploadScoreToDateBase(currentScore);
        currentScore = 0;

        SceneManager.LoadScene(0);
    }

    
}
