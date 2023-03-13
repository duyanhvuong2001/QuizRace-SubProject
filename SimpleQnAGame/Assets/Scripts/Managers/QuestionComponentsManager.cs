using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestionComponentsManager : MonoBehaviour
{
    public static QuestionComponentsManager instance;

    private void Awake()
    {
        if(instance)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;

    }
    //Components
    private Animator animator;

    //Panel
    [SerializeField]
    private GameObject questionPanel;
  

    //Question fields
    [SerializeField] private Text questionText;
    public Text countdown;

    //Answer fields
    [SerializeField] private Text answerA;
    [SerializeField] private Text answerB;
    [SerializeField] private Text answerC;
    [SerializeField] private Text answerD;

    //Answer boxes
    [SerializeField] private AnswerBox answerABox;
    [SerializeField] private AnswerBox answerBBox;
    [SerializeField] private AnswerBox answerCBox;
    [SerializeField] private AnswerBox answerDBox;

    //Barrier
    [SerializeField] private GameObject blockWalls;

    //Playzone
    [SerializeField] private Playzone playzone;
    

    //Functions
    private void Start()
    {
        animator = GetComponent<Animator>();
        //countdownTillStart.gameObject.SetActive(false);
    }

    

    public void SetUpQuestion(Question q)
    {
        //Question index to display
        int questionIdx = GameManager.instance.currentQuestionIndex;
        questionIdx++;

        //Set up qs description
        questionText.text = questionIdx + ". " + q.Description;

        //Set up cdtime
        SetCountDownText(q.Time);

        //Set up answer 
        answerA.text = "A. " + q.Answers[0].toString();
        answerB.text = "B. " + q.Answers[1].toString();
        answerC.text = "C. " + q.Answers[2].toString();
        answerD.text = "D. " + q.Answers[3].toString();

        //Set up answer boxes
        answerABox.SetAnswer(q.Answers[0]);
        answerBBox.SetAnswer(q.Answers[1]);
        answerCBox.SetAnswer(q.Answers[2]);
        answerDBox.SetAnswer(q.Answers[3]);

        //Set them active

    }

    public void ShowBlockWalls()
    {
        blockWalls.SetActive(true);
    }

    public void HideBlockWalls()
    {
        blockWalls.SetActive(false);
    }
    public void ShowQuestion()
    {
        //Show UI
        animator.SetTrigger("show");

     
    }

    public void ShowAnswerBoxes()
    {
        //Set zones active
        answerABox.gameObject.SetActive(true);
        answerBBox.gameObject.SetActive(true);
        answerCBox.gameObject.SetActive(true);
        answerDBox.gameObject.SetActive(true);
    }
    public void HideQuestion()
    {
        //Hide UI
        animator.SetTrigger("hide");
    }

    public void HideAnswerBoxes()
    {
        //Set zones inactive
        answerABox.gameObject.SetActive(false);
        answerBBox.gameObject.SetActive(false);
        answerCBox.gameObject.SetActive(false);
        answerDBox.gameObject.SetActive(false);
    }

    public void SetCountDownText(int number)
    {
        countdown.text = number.ToString();
    }

    public void SetAnswerColor(Answer answer, Color color)
    {
        //Check what is the chosen answer to change color
        if (answer.Description == answerA.text.Substring(3))
        {
            answerA.color = color;
          
            return;
        }

        if (answer.Description == answerB.text.Substring(3))
        {
            answerB.color = color;
            return;
        }

        if (answer.Description == answerC.text.Substring(3))
        {
            answerC.color = color;
            return;
        }

        if (answer.Description == answerD.text.Substring(3))
        {
            answerD.color = color;
            return;
        }
    }

    public void ResetAnswerColors()
    {
        //Reset colors of all answer boxes
        answerA.color = Color.black;
        answerB.color = Color.black;
        answerC.color = Color.black;
        answerD.color = Color.black;
    }

    public Playzone Playzone
    {
        get
        {
            return playzone;
        }
    }
   
}
