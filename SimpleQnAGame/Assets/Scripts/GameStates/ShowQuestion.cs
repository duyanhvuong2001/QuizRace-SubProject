using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.GameStates
{
    public class ShowQuestion : GameState
    {
        public override STATES UpdateState()
        {
            //First set up the question frame
            QuestionComponentsManager.instance.SetUpQuestion(GameManager.instance.myQuestionList[GameManager.instance.currentQuestionIndex]);

            //Then show the question
            QuestionComponentsManager.instance.ShowQuestion();
            QuestionComponentsManager.instance.ShowAnswerBoxes();

            return STATES.COUNTDOWN;
        }
    }
}
