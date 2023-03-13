using UnityEngine;

namespace Assets.Scripts.GameStates
{
    public class Prepare : GameState
    {
        private bool canProceed;
        private bool playerRelocated;

        public Prepare()
        {
            canProceed = false;
            playerRelocated = false;
        }
        public override STATES UpdateState()
        {
            if(!playerRelocated)
            {
                //Display attention message
                DisplayReadyMessage();

                //Move the player to spawnPoint
                GameManager.instance.MovePlayerToSpawnPoint();

                 //Set the block walls active
                QuestionComponentsManager.instance.ShowBlockWalls();
                playerRelocated =true;
            }
            
            //Check if next question is ready
            if (GameManager.instance.NextQuestionReady())
            {
                if(canProceed)
                {
                    GameManager.instance.NextQuestion();

                    ResetProperties();

                    return STATES.WAITING;
                }
            }
            else
            {//else, that would just be game end
                return STATES.END_GAME;
            }

            
            return STATES.PREPARE;
        }

        public void ProceedNextQuestion()
        {
            canProceed = true;
        }

        private void ResetProperties()
        {
            canProceed = false;
            playerRelocated = false;
        }

        private void DisplayReadyMessage()
        {
            if ((string)PhotonNetwork.player.CustomProperties["role"]=="host")
            {
                PlayerUIManager.instance.DisplayMessage("Press START to go to next question", Color.green);
            }
            else
            {
                PlayerUIManager.instance.DisplayMessage("Question is coming!", Color.green);
            }
        }
    }
}
