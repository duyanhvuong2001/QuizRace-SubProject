using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.GameStates
{
    public class AddScore : GameState
    {
        public override STATES UpdateState()
        {
            //Calculate player's score
            GameManager.instance.OnScoreCalculation();

            //

            //
            return STATES.END_QUESTION;
        }
    }
}
