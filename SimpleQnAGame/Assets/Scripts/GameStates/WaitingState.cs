using UnityEngine;

namespace Assets.Scripts.GameStates
{
    
    public class WaitingState : GameState
    {
        private int waitingTime;
        private float lastDecrement;
        private bool cdSet;
        const int WAITING_TIME = 6;

        public WaitingState()
        {
            waitingTime = WAITING_TIME;
            cdSet = false;
        }
        public override STATES UpdateState()
        {
            if (!cdSet)
            {
                PlayerUIManager.instance.ShowWarningText();
                PlayerUIManager.instance.SetWarningText(waitingTime.ToString());
                cdSet = true;
            }

            if (waitingTime != 0)
            {
                if(Time.time - lastDecrement > 1.0f)
                {
                  
                    lastDecrement = Time.time;
                    DecrementWaitingTime();
                    //Set the CountDown time
                    PlayerUIManager.instance.SetWarningText(waitingTime.ToString());
                }
               return STATES.WAITING;
            }
            else
            {
                cdSet = false;
                ResetWaitingTime();

                //Hide cd till start text
                PlayerUIManager.instance.HideWarningText();

                //Hide the blockwalls
                QuestionComponentsManager.instance.HideBlockWalls();
                return STATES.SHOW_QUESTION;
            }

            
        }

        private void ResetWaitingTime()
        {
            waitingTime = WAITING_TIME;
        }
        private void DecrementWaitingTime()
        {
            waitingTime--;
        }
    }
}
