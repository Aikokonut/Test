using Assets.PlayId.Scripts;
using Assets.PlayId.Scripts.Data;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace Assets.PlayId.Examples
{
    public class SignIn : MonoBehaviour
    {
        public Text Log;
        public Text Output;
        [SerializeField] GameObject background;
        [SerializeField] Text playerName;

        private void Awake()
        {
            Debug.Log("Initializing Firebase in Awake.");
            FirebaseManager.Instance.InitializeFirebase();
        }

        private void Start()
        {
            Debug.Log($"Is Firebase Initialized? {FirebaseManager.Instance.IsInitialized()}");

            StartCoroutine(WaitForFirebaseInitialization());
        }

        private IEnumerator WaitForFirebaseInitialization()
        {
            while (!FirebaseManager.Instance.IsInitialized())
            {
                yield return null;
            }

            Debug.Log("Firebase Initialized Successfully.");
            PlayIdServices.Instance.Auth.TryResume(OnSignIn);
        }

        public void DoSignIn()
        {
            PlayIdServices.Instance.Auth.SignIn(OnSignIn, caching: false);
        }

        private void OnSignIn(bool success, string error, User user)
        {
            if (user == null)
            {
                Output.text = "Error: User data is null.";
                Debug.LogError("User is null. Sign-in failed.");
                return;
            }

            if (success)
            {
                Output.text = $"Hello, {user.Name}!";
                Debug.Log(user.TokenResponse.IdToken);

                try
                {
                    var jwt = new JWT(user.TokenResponse.IdToken);
                    jwt.ValidateSignature(PlayIdServices.Instance.Auth.SavedUser.ClientId);
                    Output.text += "\nId Token (JWT) validated.";
                }
                catch (System.Exception ex)
                {
                    if (ex.Message.Contains("expired"))
                    {
                        Output.text = "Token expired. Please sign in again.";
                        PlayIdServices.Instance.Auth.SignIn(OnSignIn, caching: false);
                        return;
                    }
                    else
                    {
                        Output.text = $"Error: {ex.Message}";
                        return;
                    }
                }

                if (FirebaseManager.Instance != null && FirebaseManager.Instance.IsInitialized())
                {
                    SavePlayerDataToFirebase(user.Id.ToString(), user.Name);
                }
                else
                {
                    Debug.LogError("FirebaseManager is not initialized.");
                }

                playerName.text = user.Name;

                background.SetActive(false);
                this.gameObject.SetActive(false);
                Camera.main.gameObject.SetActive(false);

                var gameManager = Singleton<GameManager>.Instance;
                gameManager.LoadGameMap("Game_LakeSide");

            }
            else
            {
                Output.text = error;
            }
        }

        public void SignOut()
        {
            PlayIdServices.Instance.Auth.SignOut(revokeAccessToken: false);
            Output.text = "Not signed in";
        }

        private void SavePlayerDataToFirebase(string userId, string userName)
        {
            if (FirebaseManager.Instance != null && FirebaseManager.Instance.IsInitialized())
            {
                FirebaseManager.Instance.SavePlayerData(userId, userName);
            }
            else
            {
                Debug.LogError("FirebaseManager.Instance is not initialized or Firebase is not ready.");
            }
        }
    }
}
