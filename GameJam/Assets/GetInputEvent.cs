using System.Collections;
using System.Collections.Generic;
using Managers;
using UnityEngine;
using UnityEngine.InputSystem;

public class GetInputEvent : MonoBehaviour
{
    // Start is called before the first frame update
    public void OnPlayerJoined(PlayerInput playerInput) {
        Debug.Log("Joined: " + playerInput.playerIndex);
        playerInput.GetComponent<PlayerController.PlayerController>().playerID = playerInput.playerIndex;
        GameManager.Instance.AddPlayerToScoreboard(playerInput.playerIndex);
    }

    public void OnPlayerRemoved(PlayerInput playerInput) {
        Debug.Log("removed : " + playerInput.playerIndex);
    }
}
