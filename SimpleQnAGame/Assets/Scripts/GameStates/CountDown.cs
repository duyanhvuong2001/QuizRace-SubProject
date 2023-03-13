using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.GameStates
{
    public class CountDown : GameState
    {
        private float lastDecrease;
        public CountDown()
        {

        }

        public override STATES UpdateState()
        {
            //get the cd time
            int cd = int.Parse(QuestionComponentsManager.instance.countdown.text);

            if (cd != 0)
            {
                if (Time.time - lastDecrease > 1.0f)
                {
                    lastDecrease = Time.time;
                    cd--;
                    QuestionComponentsManager.instance.SetCountDownText(cd);
                }
                return STATES.COUNTDOWN;
            }
            else
            {
                return STATES.ADD_SCORE;
            }
        }
    }
}
