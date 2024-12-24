using Firebase;
using Firebase.Database;
using Firebase.Extensions;
using UnityEngine;

public class FirebaseManager : MonoBehaviour
{
    private static FirebaseManager _instance;
    private DatabaseReference databaseReference;
    private bool isInitialized;

    public static FirebaseManager Instance
    {
        get
        {
            if (_instance == null)
            {
                GameObject obj = new GameObject("FirebaseManager");
                _instance = obj.AddComponent<FirebaseManager>();
                DontDestroyOnLoad(obj);
            }
            return _instance;
        }
    }

    public bool IsInitialized()
    {
        return isInitialized;
    }

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }
        _instance = this;
        DontDestroyOnLoad(gameObject);
        InitializeFirebase();
    }

    public void InitializeFirebase()
    {
        Debug.Log("Initializing Firebase...");
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsCompleted && task.Result == DependencyStatus.Available)
            {
                FirebaseApp app = FirebaseApp.DefaultInstance;
                databaseReference = FirebaseDatabase.GetInstance(app)
                    .GetReferenceFromUrl("https://disc-7b4c7-default-rtdb.asia-southeast1.firebasedatabase.app/");
                isInitialized = true;
                Debug.Log("Firebase Initialized Successfully.");
            }
            else
            {
                Debug.LogError("Firebase Initialization Failed: " + task.Exception);
            }
        });
    }

    public void SavePlayerData(string userId, string userName)
    {
        if (!isInitialized || databaseReference == null)
        {
            Debug.LogError("Firebase is not initialized.");
            return;
        }

        databaseReference.Child("players").Child(userId).Child("name").SetValueAsync(userName).ContinueWithOnMainThread(task =>
        {
            if (task.IsCompleted && !task.IsFaulted && !task.IsCanceled)
            {
                Debug.Log("Player data saved successfully.");
            }
            else
            {
                Debug.LogError("Failed to save player data: " + task.Exception);
            }
        });
    }

    public void GetPlayerData(string userId)
    {
        if (!isInitialized || databaseReference == null)
        {
            Debug.LogError("Firebase is not initialized.");
            return;
        }

        databaseReference.Child("players").Child(userId).Child("name").GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsCompleted && task.Result != null)
            {
                DataSnapshot snapshot = task.Result;
                Debug.Log("Player name: " + snapshot.Value);
            }
            else
            {
                Debug.LogError("Failed to retrieve player data: " + task.Exception);
            }
        });
    }
}
