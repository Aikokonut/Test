using Firebase;
using Firebase.Database;
using Firebase.Auth;
using UnityEngine;

public class FirebaseManager : MonoBehaviour
{
    private FirebaseAuth firebaseAuth;
    private FirebaseDatabase firebaseDatabase;

    public static FirebaseManager Instance { get; private set; }  // Singleton instance

    private bool isInitialized = false;

    private void Awake()
    {
        // Ensure that only one instance exists
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        // Initialize Firebase
        InitializeFirebase();
    }

    private void InitializeFirebase()
    {
        firebaseAuth = FirebaseAuth.DefaultInstance;
        firebaseDatabase = FirebaseDatabase.DefaultInstance;

        // Wait until Firebase is initialized
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
        {
            FirebaseApp app = FirebaseApp.DefaultInstance;
            isInitialized = app != null;
            Debug.Log("Firebase initialized: " + isInitialized);
        });
    }

    public bool IsInitialized()
    {
        return isInitialized;
    }

    // Example method to save player data to Firebase
    public void SavePlayerData(string userId, string userName)
    {
        if (isInitialized)
        {
            var reference = firebaseDatabase.GetReference("players").Child(userId);
            reference.Child("name").SetValueAsync(userName).ContinueWith(task =>
            {
                if (task.IsCompleted)
                {
                    Debug.Log("Player data saved successfully.");
                }
                else
                {
                    Debug.LogError("Failed to save player data.");
                }
            });
        }
        else
        {
            Debug.LogError("Firebase is not initialized.");
        }
    }
}
