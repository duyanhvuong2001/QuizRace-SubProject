using Assets.Scripts.GameStates;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateController : Photon.MonoBehaviour
{
    //Reference to current state
    GameState currentState;

    //Other State;
    InLobby inLobbyState;
    LoadQuestions loadQuestionsState;
    ShowQuestion showQuestionState;
    CountDown countDownState;
    AddScore addScoreState;
    EndQuestion endQuestionState;
    WaitingState waitingState;
    Prepare prepareState;
    EndGameState endGameState;


    // Start is called before the first frame update
    void Awake()
    {
        inLobbyState = new InLobby();
        loadQuestionsState = new LoadQuestions();
        prepareState = new Prepare();
        showQuestionState = new ShowQuestion();
        countDownState = new CountDown();
        endQuestionState = new EndQuestion();
        waitingState = new WaitingState();
        addScoreState = new AddScore();
        endGameState = new EndGameState();
        

        //set initial state
        currentState = inLobbyState;
    }

    // Update is called once per frame
    void Update()
    {
        STATES state = currentState.UpdateState();
        switch (state)
        {
            case STATES.IN_LOBBY:
                currentState = inLobbyState;
                break;
            case STATES.LOAD_QUESTIONS:
                currentState = loadQuestionsState;
                break;
            case STATES.PREPARE:
                currentState = prepareState;
                break;
            case STATES.WAITING:
                currentState = waitingState;
                break;
            case STATES.SHOW_QUESTION:
                currentState = showQuestionState;
                break;
            case STATES.COUNTDOWN:
                currentState = countDownState;
                break;
            case STATES.ADD_SCORE:
                currentState = addScoreState;
                break;
            case STATES.END_QUESTION:
                currentState = endQuestionState;
                break;
            case STATES.END_GAME:
                currentState = endGameState;
                break;
        }

        Debug.Log(state.ToString());
    }

    public void OnStartButtonClicked()
    {
        if(currentState == inLobbyState)
        {
            photonView.RPC("StartGame", PhotonTargets.All);

            //Close the room to prevent other players joining
            PhotonNetwork.room.IsOpen = false;
        }

        if(currentState == prepareState)
        {
            if(!QuestionComponentsManager.instance.Playzone.PlayerInsideZone())
            {
                photonView.RPC("NextQuestion", PhotonTargets.All);
            }
        }
    }


    [PunRPC]
    public void StartGame()
    {
        inLobbyState.StartGame();
    }

    [PunRPC]
    public void NextQuestion()
    {
        prepareState.ProceedNextQuestion();
    }
}
