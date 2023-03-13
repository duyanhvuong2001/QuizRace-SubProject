using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Hashtable = ExitGames.Client.Photon.Hashtable;
using System;

public class HostMenuManager : MonoBehaviour
{
    //References
    public InputField sessionInput;
    public GameObject hostGameButton;
    public string version;
    public Text errMsg;

    //default startup
    private void Awake()
    {
        if (!PhotonNetwork.ConnectUsingSettings(version))
        {
            errMsg.text = "Failed to connect to server";
        }

       
    }

    private void Start()
    {
        
    }

    //function allowing players to create a room for game
    public void HostGame()

    {

        if (PhotonNetwork.CreateRoom(sessionInput.text, new RoomOptions() { MaxPlayers = 20 }, null))
        {
            //Set new properties for the host
            Hashtable hashtable = new Hashtable();
            hashtable.Add("role", "host");
            PhotonNetwork.player.SetCustomProperties(hashtable);
        }
        else
        {
            errMsg.text = "Failed to create room";
        };
        


    }


    public void CheckValidSessionInput()
    {
        if(sessionInput.text.Length == 0)
        {
            errMsg.text = "Room ID cannot be blank";
            hostGameButton.SetActive(false);
        }
        else
        {
            errMsg.text = "";
            hostGameButton.SetActive(true);
        }
    }

   

    private void OnJoinedRoom()
    {
        PhotonNetwork.LoadLevel("MainGameScene");
    }

    public void ReturnToMain()
    {
        PhotonNetwork.Disconnect();
        PhotonNetwork.LoadLevel("MainMenu");
    }

    private void OnConnectedToMaster()
    {
        Console.WriteLine("Connected to Master");
        if (PhotonNetwork.JoinLobby(TypedLobby.Default))
        {
            Console.WriteLine("Joined Lobby");
        }
        else
        {
            Console.WriteLine("Failed in joining lobby");
        }
    }

    private void OnDisconnectedFromPhoton()
    {
        string notification = "Disconnected from photon services";


        Console.WriteLine(notification);
    }
}
