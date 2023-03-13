using System.IO;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndGameLeaderboardController : MonoBehaviour
{
    private Animator _animator;
    private string _leaderboardText;
    [SerializeField] private GameObject _exportButton;
    [SerializeField] private Text _leaderboardDisplayZone;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
       
    }

    private void PopulateLeaderboard()
    {
        _leaderboardText = PlayerUIManager.instance.GetLeaderboardText();
    }

    public void WriteLeaderboard()
    {
        PopulateLeaderboard();

        _leaderboardDisplayZone.text = _leaderboardText;
    }

    public void ShowLeaderboard()
    {
        if ((string)PhotonNetwork.player.CustomProperties["role"] == "player")
        {
            _exportButton.SetActive(false);
        }
        _animator.SetTrigger("show");
    }

    public void HideLeaderboard()
    {
        _animator.SetTrigger("hide");
    }

    public void WriteToFile()
    {
        StreamWriter writer = new StreamWriter(Application.dataPath + "/record.csv");

        Dictionary<string, int> leaderboardDict = PlayerUIManager.instance.GetLeaderboardDict();
        string columns = "player" + "," + "score\n";
        writer.Write(columns);

        foreach (KeyValuePair<string, int> kvp in leaderboardDict)
        {
            string row = kvp.Key + "," + kvp.Value +"\n";
            writer.WriteLine(row);
        }
        writer.Close();

        PlayerUIManager.instance.DisplayMessage("Player records have been written!", Color.green);

    }

}
