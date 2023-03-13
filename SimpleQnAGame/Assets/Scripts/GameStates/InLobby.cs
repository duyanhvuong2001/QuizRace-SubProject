using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.GameStates
{
    public class InLobby : GameState
    {
        private bool canStart;
        public InLobby()
        {
            canStart = false;
        }

        public override STATES UpdateState()
        {
            if(canStart)
            {
                canStart=false;
                
                return STATES.LOAD_QUESTIONS;
            }
            else
            {
                return STATES.IN_LOBBY;
            }
        }

        public void StartGame()
        {
            canStart=true;
        }
    }
}
