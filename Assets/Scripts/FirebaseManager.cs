using Firebase;
using Firebase.Database;
using Firebase.Extensions;
using UnityEngine;

public class FirebaseManager : MonoBehaviour
{
    public static FirebaseManager Instance;

    private DatabaseReference reference;

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

        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
        {
            FirebaseApp app = FirebaseApp.DefaultInstance;
            reference = FirebaseDatabase.DefaultInstance.RootReference;
        });
    }

    public void SavePlayerData(string userId, string userName)
    {
        if (reference != null)
        {
            string path = "players/" + userId;
            string json = JsonUtility.ToJson(new PlayerData { userId = userId, userName = userName });
            reference.SetRawJsonValueAsync(path, json);
        }
        else
        {
            Debug.LogError("Firebase reference is not initialized.");
        }
    }
}

[System.Serializable]
public class PlayerData
{
    public string userId;
    public string userName;
}
