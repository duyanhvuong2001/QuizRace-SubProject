using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.GameStates
{
    public class EndGameState : GameState
    {
        private bool leaderboardShown;
        public EndGameState()
        {
            leaderboardShown = false;
        }
        public override STATES UpdateState()
        {
            if(!leaderboardShown)
            {
                //Write to the leaderboard
                PlayerUIManager.instance.leaderboardController.WriteLeaderboard();

                //Show the leaderboard
                PlayerUIManager.instance.leaderboardController.ShowLeaderboard();
                leaderboardShown=true;
            }
            


            return STATES.END_GAME;
        }
    }
}

