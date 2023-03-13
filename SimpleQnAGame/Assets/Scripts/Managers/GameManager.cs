using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public struct QuestionListPair
{
    public int QuestionListIndex;
    public int QuestionIndex;
}
public class GameManager : Photon.MonoBehaviour
{
    //instance of GameManager
    public static GameManager instance;

    private void Awake()
    {
        if (GameManager.instance)
        {
            //Destroy GOs if there has been an instance
            Destroy(gameObject);
            return;
        }
        instance = this;
    }

    //References
    public GameObject playerPrefab;
    public GameObject spawnButton;
    public GameObject spawnPoint;
    public PlayerUIManager playerUIManager;
    [SerializeField] private GameObject mainCamera;
    public GameObject myPlayer;

    //Logic
    public int currentQuestionIndex;
    private Answer myAnswer;
    const int MAX_NUM_OF_QUESTION = 10;
    private List<QuestionListPair> questionListFormedLogic;

    //Resources
    [SerializeField]
    private List<Sprite> playerSprites;
    public List<Question> myQuestionList;
    private string questionFileDirectory;
    private TextAsset questionTextAsset;
    private List<List<Question>> questionListChoice;

    //Question List
    private List<Question> mathQuestionList;
    private List<Question> historyQuestionList;
    private List<Question> geographyQuestionList;

    //Properties
    public string QuestionFileDirectory
    {
        get { return questionFileDirectory; }
    }

    public Question CurrentQuestion
    {
        get
        {
            return myQuestionList[currentQuestionIndex];
        }
    }

    //Setup Functions

    public Answer MyAnswer
    {
        get
        {
            return myAnswer;
        }
    }
    private void Start()
    {
        //Initiate all question list
        mathQuestionList = LoadQuestions("TextResources/math_q_list");
        historyQuestionList = LoadQuestions("TextResources/history_q_list");
        geographyQuestionList = LoadQuestions("TextResources/geography_q_list");

        questionListChoice = new List<List<Question>>();

        questionListChoice.Add(mathQuestionList);
        questionListChoice.Add(historyQuestionList);
        questionListChoice.Add(geographyQuestionList);

        questionListFormedLogic = new List<QuestionListPair>();

        //Initiate currQuestion index
        currentQuestionIndex = -1;

        //If this is a host, randomize the data structure of the question list
        if ((string)PhotonNetwork.player.CustomProperties["role"]=="host")
        {
            List<QuestionListPair> questionListLogic = HostSetUpQuestionList();
            for(int i=0;i<questionListLogic.Count;i++)
            {
                photonView.RPC("AddQuestionListLogic", PhotonTargets.AllBuffered, questionListLogic[i].QuestionListIndex, questionListLogic[i].QuestionIndex);
            }
        }

        
    }

    [PunRPC]
    private void AddQuestionListLogic(int questionListIdx, int questionIdx)
    {
        questionListFormedLogic.Add(new QuestionListPair
        {
            QuestionListIndex = questionListIdx,
            QuestionIndex = questionIdx
        });
    }

    public void InitiateQuestionList()
    {
        myQuestionList = new List<Question>();

        foreach(QuestionListPair questionListPair in questionListFormedLogic)
        {
            myQuestionList.Add(questionListChoice[questionListPair.QuestionListIndex][questionListPair.QuestionIndex]);
        }

        foreach(Question question in myQuestionList)
        {
            question.DebugInfo();
        }
    }

    private List<QuestionListPair> HostSetUpQuestionList()
    {
        List<QuestionListPair> questionListLogic = new List<QuestionListPair>();

        while (questionListLogic.Count < MAX_NUM_OF_QUESTION)
        {
            int questionListIdx = Random.Range(0, questionListChoice.Count);

            int questionIdx = Random.Range(0, questionListChoice[questionListIdx].Count);

            //If the key pair is not added before
            bool duplicatedQuestion = false;

            foreach(QuestionListPair questionListPair in questionListLogic)
            {
                if(questionListIdx == questionListPair.QuestionListIndex && questionIdx==questionListPair.QuestionIndex)
                {
                    duplicatedQuestion = true;
                    break;
                }
            }
            
            if(!duplicatedQuestion)
            {
                QuestionListPair qlPair = new QuestionListPair
                {
                    QuestionListIndex = questionListIdx,
                    QuestionIndex = questionIdx
                };

                questionListLogic.Add(qlPair);
            }
        }

        return questionListLogic;
    }
    private List<Question> LoadQuestions(string fileDirectory)
    {
        //New question List
        List<Question> questionList = new List<Question>();


        //Load the resource from asset
        questionTextAsset = (TextAsset)Resources.Load(fileDirectory);

        //Convert to list of strings
        List<string> splittedQuestionFileText = questionTextAsset.text.Split("\n").ToList();


        //Construct question list
        while(splittedQuestionFileText.Count>0)
        {
            Question question = ConstructQuestion(splittedQuestionFileText);
            questionList.Add(question);
        }

        return questionList;
    }

    private Question ConstructQuestion(List<string> splittedQuestionFileText)
    {

        string[] questionParts = PopStringList(splittedQuestionFileText).Split(";");

        //question description
        string description = questionParts[0];

        //question countdown time
        int cdTime = int.Parse(questionParts[1]);

        //number of answers
        const int NUM_OF_ANSWER = 4;

        //Answer list
        List<Answer> answers = new List<Answer>();

        //Add answers to list via streamreader
        for(int i=0;i<NUM_OF_ANSWER;i++)
        {
            Answer answer = ConstructAnswer(splittedQuestionFileText);
            answers.Add(answer);
        }

        //Finally construct question
        Question question = new Question(description, cdTime, answers);

        return question;
    }

    private Answer ConstructAnswer(List<string> splittedQuestionFileText)
    {
        string[] answerParts = PopStringList(splittedQuestionFileText).Split(';');

        //Construct answer using read data
        Answer answer = new Answer(answerParts[0], bool.Parse(answerParts[1]));

        return answer;
    }

    private string PopStringList(List<string> stringList)
    {
        string popped = stringList[0];
        stringList.RemoveAt(0);

        return popped;
    }

    //Player functions
    public void SpawnPlayer(string playerName)
    {
        PhotonNetwork.playerName = playerName;
        GameObject qPlayer = PhotonNetwork.Instantiate(playerPrefab.name, spawnPoint.transform.position, Quaternion.identity, 0);

        myPlayer = qPlayer;


        photonView.RPC("OnPlayerJoined", PhotonTargets.Others, PhotonNetwork.player);
    }

    public void MovePlayerToSpawnPoint()
    {
        if ((string)PhotonNetwork.player.CustomProperties["role"]=="player")
        {
            myPlayer.transform.position = spawnPoint.transform.position;
        }
    }

    public void SetAnswer(Answer answer)
    {
        //Set my answer to this answer
        myAnswer = answer;

        //Set the chosen answer's color to orange
        QuestionComponentsManager.instance.SetAnswerColor(answer, new Color32(255, 127, 80, 255));

    }

    public void UnsetAnswer()
    {
        //unset answer
        myAnswer = null;

        //change color to black
        QuestionComponentsManager.instance.ResetAnswerColors();
    }

    public void OnScoreCalculation()
    {
        //Check if the player answered
        if(myAnswer != null)
        {
            //If so, are they correct?
            if (myAnswer.IsCorrect)
            {
                AddPlayerScore();
            }
        }
           
    }
    private void AddPlayerScore()
    {
        //Get current score
        int myScore = (int)PhotonNetwork.player.CustomProperties["score"];

        //Increase score
        myScore++;
        //Create new hashtable
        Hashtable hashtable = new Hashtable();

        //Set new score
        hashtable.Add("score", myScore);

        //Set new custom properties
        PhotonNetwork.player.SetCustomProperties(hashtable);
    }

    //Network/RPC functions
    [PunRPC]
    private void OnPlayerJoined(PhotonPlayer player)
    {
        //Warn other players about new player's presence
        playerUIManager.DisplayMessage(player.NickName + " has joined the game", Color.green);

       

        //Add the newly joined player's reference to the playerlist
        Debug.Log(PhotonNetwork.playerList.Length);
    }

    private void OnPhotonPlayerDisconnected(PhotonPlayer player)
    {
        //Warn other players about this player's departure
        playerUIManager.DisplayMessage(player.NickName + " has left the game", Color.red);
    }



    //Game State related functions
    public bool NextQuestionReady()
    {
        if(currentQuestionIndex<myQuestionList.Count-1)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void NextQuestion()
    {
        currentQuestionIndex++;
    }
}
