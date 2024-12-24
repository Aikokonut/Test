using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoomSelection : MonoBehaviour
{
    [SerializeField] private Button singlePlayerBtn;
    [SerializeField] private Button multiplayerBtn;
    [SerializeField] private Dropdown roomListDropdown;

    private void Start()
    {
        singlePlayerBtn.onClick.AddListener(OnSinglePlayerSelected);
        multiplayerBtn.onClick.AddListener(OnMultiplayerSelected);
    }

    private void OnSinglePlayerSelected()
    {
        // Start single player game
        Debug.Log("Single player mode selected");
        // Add logic to start a single player game
    }

    private void OnMultiplayerSelected()
    {
        // Show list of rooms and allow player to choose
        Debug.Log("Multiplayer mode selected");
        // Add logic to fetch rooms from server or Firebase and display them
    }
}