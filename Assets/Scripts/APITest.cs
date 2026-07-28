using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft.Json.Linq;
using static GameManager;

public class APITest : MonoBehaviour
{
    public const string apiUrl = "https://flappybird-clone-7c5ba-default-rtdb.firebaseio.com/";
    private string scoresApi => apiUrl + "scores.json";
    private string userScoreApi => apiUrl + "scores/" + userName + ".json";

    public string userName;
    public int highestScore;

    public List<UserScoreEntry> allDownloadedScores = new List<UserScoreEntry>();

    void Start()
    {
        StartCoroutine(LoadHighestScore());
        DownloadLeaderboard();
    }

    IEnumerator LoadHighestScore()
    {
        UnityWebRequest request = UnityWebRequest.Get(userScoreApi);
        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError(request.error);
        }
        else
        {
            string json = request.downloadHandler.text;
            Debug.Log("Raw response: " + json);

            if (json != "null")
            {
                ScoreData data = JsonUtility.FromJson<ScoreData>(json);
                if (data != null)
                    highestScore = data.score;
            }
        }
    }

    [System.Serializable]
    public class ScoreData
    {
        public string name;
        public int score;
    }

    public void UploadScoreToDateBase(int scoreToUpload)
    {
        if (scoreToUpload > highestScore)
        {
            StartCoroutine(UploadScoreCoroutine(scoreToUpload));
        }
    }

    IEnumerator UploadScoreCoroutine(int scoreToUpload)
    {
        string json = JsonUtility.ToJson(new ScoreData { name = userName, score = scoreToUpload });
        UnityWebRequest request = new UnityWebRequest(userScoreApi, "PUT");
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(json);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            highestScore = scoreToUpload;
            Debug.Log("Score uploaded: " + scoreToUpload);
        }
        else
        {
            Debug.LogError(request.error);
        }
    }

    public void DownloadLeaderboard()
    {
        StartCoroutine(DownloadLeaderboardCoroutine());
    }

    IEnumerator DownloadLeaderboardCoroutine()
    {
        UnityWebRequest request = UnityWebRequest.Get(scoresApi);
        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError(request.error);
            yield break;
        }

        string json = request.downloadHandler.text;
        allDownloadedScores.Clear();

        if (json == "null") yield break;

        JObject root = JObject.Parse(json);

        foreach (var property in root.Properties())
        {
            JObject entry = (JObject)property.Value;

            UserScoreEntry newEntry = new UserScoreEntry
            {
                userName = entry["name"]?.ToString() ?? property.Name,
                score = entry["score"]?.ToObject<int>() ?? 0
            };

            allDownloadedScores.Add(newEntry);
        }

        allDownloadedScores.Sort((a, b) => b.score.CompareTo(a.score));

        foreach (var entry in allDownloadedScores)
            Debug.Log($"{entry.userName}: {entry.score}");
    }
}