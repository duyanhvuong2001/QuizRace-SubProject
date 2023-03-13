using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class JoinGameManager : MonoBehaviour
{
    //References
    public InputField sessionInput;
    public GameObject joinGameButton;
    public string version;
    public Text errMsg;

    //default startup
    private void Awake()
    {
        if(!PhotonNetwork.connected)
        {
            if (!PhotonNetwork.ConnectUsingSettings(version))
            {
                errMsg.text = "Failed to connect to server";
            }
        }
        
    }

    public void OnJoinGame()
    {
        PhotonNetwork.JoinRoom(sessionInput.text);
        PhotonNetwork.playerName = "player";
    }

    private void OnPhotonJoinRoomFailed()
    {
        errMsg.text = "Failed to join room ID: " + sessionInput.text;
    }
    public void CheckValidSessionInput()
    {
        if (sessionInput.text.Length == 0)
        {
            errMsg.text = "Room ID cannot be blank";
            joinGameButton.SetActive(false);
        }
        else
        {
            errMsg.text = "";
            joinGameButton.SetActive(true);
        }
    }

    private void OnJoinedRoom()
    {
        PhotonNetwork.LoadLevel("MainGameScene");
    }
}
