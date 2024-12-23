using Assets.PlayId.Scripts;
using Assets.PlayId.Scripts.Data;
using UnityEngine;
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

                // Kiểm tra nếu Firebase đã được khởi tạo
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
