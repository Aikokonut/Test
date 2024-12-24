using Firebase;
using Firebase.Auth;
using Firebase.Database;
using Firebase.Extensions;
using UnityEngine;

public class AuthManager : MonoBehaviour
{
    private FirebaseAuth auth;
    private FirebaseUser currentUser;

    public static AuthManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }

        auth = FirebaseAuth.DefaultInstance;
    }

    public void SavePlayerData(string playerName, int score)
    {
        if (currentUser != null)
        {
            var userId = currentUser.UserId;
            var databaseRef = FirebaseDatabase.DefaultInstance.RootReference;

            // Create a new player data object
            var playerData = new PlayerData
            {
                Name = playerName,
                Score = score
            };

            // Save the data to Firebase under the "players" node
            databaseRef.Child("players").Child(userId).SetRawJsonValueAsync(JsonUtility.ToJson(playerData)).ContinueWithOnMainThread(task =>
            {
                if (task.IsCompleted)
                {
                    Debug.Log("Player data saved successfully!");
                }
                else
                {
                    Debug.LogError("Failed to save player data: " + task.Exception);
                }
            });
        }
        else
        {
            Debug.LogError("No user is signed in. Cannot save player data.");
        }
    }
}

[System.Serializable]
public class PlayerData
{
    public string Name;
    public int Score;
}
