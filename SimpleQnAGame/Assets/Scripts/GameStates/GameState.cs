using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GameState
{
    public abstract STATES UpdateState();
}

public enum STATES
{
    IN_LOBBY,
    PREPARE,
    LOAD_QUESTIONS,
    WAITING,
    SHOW_QUESTION,
    COUNTDOWN,
    ADD_SCORE,
    END_QUESTION,
    END_GAME
}
