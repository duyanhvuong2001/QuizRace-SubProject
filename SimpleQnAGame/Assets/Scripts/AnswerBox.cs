using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnswerBox : MonoBehaviour
{
    [SerializeField]
    private Answer _answer;

    public void SetAnswer(Answer answer)
    {
        _answer = answer;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            collision.gameObject.SendMessage("OnEnterAnswerBox", _answer);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            collision.gameObject.SendMessage("OnExitAnswerBox");
        }
    }
}
