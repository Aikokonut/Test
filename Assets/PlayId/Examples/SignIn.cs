using Assets.PlayId.Scripts;
using Assets.PlayId.Scripts.Data;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Assets.PlayId.Examples
{
    public class SignIn : MonoBehaviour
    {
        public Text Log;
        public Text Output;
        [SerializeField] GameObject background;
        [SerializeField] Text playerName;

        public void Start()
        {
            PlayIdServices.Instance.Auth.TryResume(OnSignIn);
        }

        public void DoSignIn()
        {
            PlayIdServices.Instance.Auth.SignIn(OnSignIn, caching: false);
        }

        void OnSignIn(bool success, string error, User user)
        {
            if (success)
            {
                Output.text = $"Hello, {user.Name}!";
                Debug.Log(user.TokenResponse.IdToken);

                try
                {
                    var jwt = new JWT(user.TokenResponse.IdToken);
                    jwt.ValidateSignature(PlayIdServices.Instance.Auth.SavedUser.ClientId);
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
                    }
                }

                if (FirebaseManager.Instance != null)
                {
                    SavePlayerDataToFirebase(user.Id.ToString(), user.Name);
                }
                else
                {
                    Debug.LogError("FirebaseManager is not initialized.");
                }

                playerName.text = user.Name;
                LoadGameScene();
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
            if (FirebaseManager.Instance != null)
            {
                FirebaseManager.Instance.SavePlayerData(userId, userName);
            }
            else
            {
                Debug.LogError("FirebaseManager.Instance is null");
            }
        }

        private void LoadGameScene()
        {
            SceneManager.LoadScene("Game_LakeSide");
        }
    }
}
