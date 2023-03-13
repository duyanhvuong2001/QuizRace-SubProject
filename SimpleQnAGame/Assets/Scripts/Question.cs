using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Question
{
    private List<Answer> _answerList;
    private int _time;
    private string _description;

    public Question(string description, int time, List<Answer> answerList)
    {
        _description = description;
        _time = time;
        _answerList = answerList;
    }

    public void CountDown()
    {
        _time -= 1;
    }

    public List<Answer> Answers
    {
        get
        {
            return _answerList;
        }
    }

    public string Description
    {
        get { return _description; }
    }

    public int Time
    {
        get { return _time; }
    }

    public void DebugInfo()
    {
        Debug.Log(_description);
        foreach(Answer answer in _answerList)
        {
            answer.DebugInfo();
        }
    }

    public Answer CorrectAnswer
    {
        get
        {
            foreach (Answer answer in _answerList)
            {
                if (answer.IsCorrect)
                {
                    return answer;
                }
            }

            return null;
        }
    }

    public bool IsTheSameQuestion(string questionDesc)
    {
        return _description == questionDesc;
    }
}
