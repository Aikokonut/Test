using Firebase;
using Firebase.Database;
using Firebase.Extensions;
using UnityEngine;

public class FirebaseManager : MonoBehaviour
{
    public static FirebaseManager Instance { get; private set; }

    private DatabaseReference reference;
    private bool isInitialized = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        InitializeFirebase();
    }

    private void InitializeFirebase()
    {
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
        {
            if (task.Exception != null)
            {
                Debug.LogError("Firebase dependencies are not available: " + task.Exception);
                return;
            }

            FirebaseApp app = FirebaseApp.DefaultInstance;
            reference = FirebaseDatabase.DefaultInstance.RootReference;
            isInitialized = true;
            Debug.Log("Firebase Initialized");
        });
    }

    public void SavePlayerData(string userId, string userName)
    {
        if (!isInitialized)
        {
            Debug.LogError("FirebaseManager is not initialized.");
            return;
        }

        string path = "players/" + userId;
        string json = JsonUtility.ToJson(new PlayerData { userId = userId, userName = userName });
        reference.SetRawJsonValueAsync(path, json);
    }

    public bool IsInitialized()
    {
        return isInitialized;
    }
}

[System.Serializable]
public class PlayerData
{
    public string userId;
    public string userName;
}
