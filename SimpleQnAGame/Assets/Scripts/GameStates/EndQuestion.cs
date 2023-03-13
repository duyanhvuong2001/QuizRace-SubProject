using UnityEngine;

namespace Assets.Scripts.GameStates
{
    public class EndQuestion : GameState
    {
        private int waitingTime;
        private float lastDecrease;
        private bool textSet;
        const int WAITING_TIME = 4;

        public EndQuestion()
        {
            waitingTime = WAITING_TIME;
            textSet = false;
        }

        private void UpdatePlayerView()
        {
            PlayerUIManager.instance.ShowWarningText();
            //Change the correct answer text on the display canvas to green

            Answer correctAnswer = GameManager.instance.CurrentQuestion.CorrectAnswer;

            QuestionComponentsManager.instance.SetAnswerColor(correctAnswer, Color.green);

            Answer myAnswer = GameManager.instance.MyAnswer;
            if(myAnswer==null)
            {
                PlayerUIManager.instance.SetWarningText("INCORRECT :(");
                return;
            }
            //If my answer is correct:
            if (!myAnswer.IsCorrect || myAnswer == null)
            {
                //Change the Warning Text to INCORRECT :(
                PlayerUIManager.instance.SetWarningText("INCORRECT :(");
                //Change my answer text to red
                QuestionComponentsManager.instance.SetAnswerColor(myAnswer, Color.red);

            }
            else
            {
                //else:
             
                //Change the Warning Text to CORRECT!!! xD

                PlayerUIManager.instance.SetWarningText("CORRECT!! xD");
            }
        }
        public override STATES UpdateState()
        {
            if (!textSet)
            {
                if ((string)PhotonNetwork.player.CustomProperties["role"] == "player")
                {
                    UpdatePlayerView();
                }
                textSet = true;
            }
            

            if (waitingTime != 0)
            {
                if (Time.time - lastDecrease > 1.0f)
                {
                    lastDecrease = Time.time;

                    //Decrease waiting time
                    waitingTime--;
                }
                return STATES.END_QUESTION;
            }
            else
            {
                //Next question
                //Reset waiting time
                waitingTime = WAITING_TIME;

                //Update leaderboards
                PlayerUIManager.instance.OnUpdateLeaderboard();

                //Hide the question UI
                QuestionComponentsManager.instance.HideQuestion();

                //Hide the answer zones
                QuestionComponentsManager.instance.HideAnswerBoxes();

                //Hide cd till start
                PlayerUIManager.instance.HideWarningText();

                //Reset the bool variable
                textSet = false;

                return STATES.PREPARE;
            }
        }
    }
}
