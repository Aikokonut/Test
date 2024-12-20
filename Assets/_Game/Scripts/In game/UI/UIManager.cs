using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;
    //[SerializeField] private Button googleSignInButton;

    private void Start()
    {
        //googleSignInButton.onClick.AddListener(() =>
        //{
        //    // Gọi phương thức đăng nhập với Google
        //    FirebaseManager.Instance.SignInWithGoogle("YOUR_ID_TOKEN");
        //});
    }

    public void Init()
    {
        Instance = this;

        this.UIMoveDisc.Init();
    }

    public UIPreThrow UIPreThrow;
    public UIAfterThrow UIAfterThrow;
    public UISessionComplete UISessionComplete;
    public UIMainMenu UIMainMenu;
    public UIDirectionAdjuster UIDirectionAdjuster;
    public UIDiscSelector UIDiscSelector;
    public UIThrowSelector UIThrowSelector;
    public UIMoveDisc UIMoveDisc;
    public UILevelComplete UILevelComplete;
    public UIMiniMap UIMiniMap;
    public UILevelInfo UILevelInfo;
}
