using Assets.PlayId.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Firebase;
using Firebase.Database;
using Firebase.Extensions;
public class UILevelComplete : UIComponent
{
    [SerializeField] private Button mainMenuBtn;

    private void Awake()
    {
        this.mainMenuBtn.onClick.AddListener(this.MainMenuBtnClick);
    }

    private void MainMenuBtnClick()
    {
        SaveScoreToFirebase(LevelManager.Instance.CurrentThrow);
        var gameManager = Singleton<GameManager>.Instance;
        gameManager.LoadMainMenu();
    }

    private void SaveScoreToFirebase(int score)
    {
        var userId = PlayIdServices.Instance.Auth.SavedUser.Id.ToString(); 
        var databaseRef = FirebaseDatabase.DefaultInstance.RootReference;

        databaseRef.Child("scores").Child(userId).SetValueAsync(score).ContinueWith(task =>
        {
            if (task.IsCompleted)
            {
                Debug.Log("Score saved successfully!");
            }
            else
            {
                Debug.LogError($"Failed to save score: {task.Exception}");
            }
        });
    }
}
