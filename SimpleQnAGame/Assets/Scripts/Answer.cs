using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Answer 
{
    private string _description;
    private bool _correct;

    public Answer(string description, bool correct)
    {
        _description = description;
        _correct = correct;
    }

    public string toString()
    {
        return _description;
    }

    public bool IsCorrect
    {
        get { return _correct; }
    }

    public string Description
    {
        get
        {
            return _description;
        }
    }
    public void DebugInfo()
    {
        Debug.Log(_description + " " + _correct);
    }
}
