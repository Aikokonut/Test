using UnityEngine;
using UnityEngine.UI;

public class GameUI : MonoBehaviour
{
    [SerializeField] private Button throwButton;
    [SerializeField] private Slider angleSlider;
    [SerializeField] private Slider powerSlider;

    private PlayerController playerController;

    private void Start()
    {
        playerController = FindObjectOfType<PlayerController>();
        throwButton.onClick.AddListener(OnThrowButtonClicked);
    }

    private void OnThrowButtonClicked()
    {
        float angle = angleSlider.value;
        float power = powerSlider.value;
        playerController.ThrowDisc(angle, power);
    }
}
