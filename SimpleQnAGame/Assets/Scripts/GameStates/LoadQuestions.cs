using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class LoadQuestions : GameState
{
    public override STATES UpdateState()
    {
        //Initialize 
        GameManager.instance.InitiateQuestionList();

        return STATES.PREPARE;
    }
}
