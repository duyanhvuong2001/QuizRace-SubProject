using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class PlayerUIManager : Photon.MonoBehaviour
{
    public static PlayerUIManager instance;
    //References
    public GameObject spawnButton;
    public InputField playerNameField;
    public GameObject menuCanvas;
    public Text pingText;
    public GameObject playerFeed;
    public GameObject feedGrid;
    public GameObject startButton;
    public Text leaderboard;

    //leaderboard reference
    public EndGameLeaderboardController leaderboardController;

    [SerializeField] private GameObject mainCamera;

    [SerializeField]
    private Text countdownTillStart;


    private void Awake()
    {
        if (instance)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;

        if (photonView.isMine)
        {
            if ((string)PhotonNetwork.player.CustomProperties["role"] == "host")
            {
                InitiateHostView();
            }
            else if ((string)PhotonNetwork.player.CustomProperties["role"] == "player")
            {
                InitiatePlayerView();
            }
        }
    }
    private void InitiateHostView()
    {
        menuCanvas.SetActive(false);

        //activate the start button
        startButton.SetActive(true);
    }

    private void InitiatePlayerView()
    {
        //Set the player menu active
        menuCanvas.SetActive(true);

        //Deactivate the start button
        startButton.SetActive(false);
    }
    private void Update()
    {
        //Update the ping shown on the HUD
        UpdatePing();
    }

    public void OnUpdateLeaderboard()
    {
        photonView.RPC("UpdateLeaderboard", PhotonTargets.All);
    }

    [PunRPC]
    public void UpdateLeaderboard()
    {
        ResetLeaderboard();
        leaderboard.text = GetLeaderboardText();
    }

    public string GetLeaderboardText()
    {
        string leaderboardText = "";

        Dictionary<string, int> leaderboardDict = GetLeaderboardDict();

        int ranking = 1;
        foreach (KeyValuePair<string, int> kvp in leaderboardDict.OrderByDescending(kp => kp.Value))
        {
            leaderboardText += ranking.ToString() + ". " + kvp.Key + ": " + kvp.Value + "\n";
            ranking++;
        }

        return leaderboardText;
    }

    public Dictionary<string,int> GetLeaderboardDict()
    {
        Dictionary<string, int> leaderboardDict = new Dictionary<string, int>();
        foreach (PhotonPlayer player in PhotonNetwork.playerList)
        {
            if ((string)player.CustomProperties["role"] == "player")
            {
                //get the player's name
                string playerName = player.NickName;

                //get the player's score
                int playerScore = (int)player.CustomProperties["score"];

                //Add it to the leaderboard
                leaderboardDict.Add(playerName, playerScore);
            }
        }

        return leaderboardDict;

    }

    public void ResetLeaderboard()
    {
        leaderboard.text = "";
    }

    public void OnSpawnButtonClicked()
    {
        menuCanvas.SetActive(false);
        mainCamera.SetActive(false);
        string playerName = playerNameField.text;

        PhotonPlayer player = PhotonNetwork.player;

        //Set new properties for the new player
        Hashtable scoreKeyVal = new Hashtable();
        scoreKeyVal.Add("score", 0);
        player.SetCustomProperties(scoreKeyVal);

        //Set new properties for the host
        Hashtable roleKeyVal = new Hashtable();
        roleKeyVal.Add("role", "player");
        PhotonNetwork.player.SetCustomProperties(roleKeyVal);
        GameManager.instance.SpawnPlayer(playerName);
    }
    //Menu functions
    public void CheckValidPlayerName()
    {
        if (playerNameField.text.Length == 0)
        {
            spawnButton.SetActive(false);
        }
        else
        {
            spawnButton.SetActive(true);
        }
    }

   

    //Network/RPC functions
    private void UpdatePing()
    {
        int ping = PhotonNetwork.GetPing();
        pingText.text = "Ping: " + ping;
        if (ping < 100)
        {
            pingText.color = Color.green;
        }
        else if (ping >= 100 && ping <= 500)
        {
            pingText.color = Color.yellow;
        }
        else
        {
            pingText.color = Color.red;
        }
    }

    public void DisplayMessage(string msg, Color color)
    {
        GameObject feed = Instantiate(playerFeed, new Vector2(0, 0), Quaternion.identity);
        feed.transform.SetParent(feedGrid.transform);
        feed.GetComponent<Text>().text = msg;
        feed.GetComponent<Text>().color = color;
    }

    public void ShowWarningText()
    {
        countdownTillStart.gameObject.SetActive(true);
    }

    public void HideWarningText()
    {
        countdownTillStart.gameObject.SetActive(false);
    }
    public void SetWarningText(string text)
    {
        countdownTillStart.text = text;
    }

    public Text CountDownTillStart
    {
        get
        {
            return countdownTillStart;
        }
    }
}
