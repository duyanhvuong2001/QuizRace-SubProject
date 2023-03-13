using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    //References
    //Functions
    public void LoadHostGameScene()
    {
        SceneManager.LoadScene("HostGameScene");
    }

    public void LoadJoinGameScene()
    {
        SceneManager.LoadScene("JoinGameScene");
    }

}
