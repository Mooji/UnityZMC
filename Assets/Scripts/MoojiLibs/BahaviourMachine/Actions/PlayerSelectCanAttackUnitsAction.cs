using BehaviourMachine;
using UnityEngine;
namespace Mooji
{
    [NodeInfo( category = "MoojiAction/Battle/" ,
                icon = "Axis" ,
                description = "" ,
                url = "" )]

    /// <summary>
    /// 选择了一个可以攻击的 战斗单位 
    /// </summary>
    /// <returns></returns>
    public class PlayerSelectCanAttackUnitsAction : MoojiBehaviourMachineAction
    {

        protected override void actionAwake()
        {
        }

        protected override void actionStart()
        {
            GameObject playerGo             = playerInfoBB.getCurrleadGo();
            AIBattleUnit playerBattleUnit   = playerGo.GetComponent<AIBattleUnit>();

            if ( playerBattleUnit == playerInfoBB.currSelectBattleUnit )
            {
                playerInfoBB.getSelectBattleUnitLocaltionMaskGo().SetActive( false );
                sendFSMEvent( FSMEventsBlackBoard.FSM_SELECT_SELF );
            }
            else
            {
                GameObject selectBattleUnitLocaltionMaskGo = playerInfoBB.getSelectBattleUnitLocaltionMaskGo();
                selectBattleUnitLocaltionMaskGo.transform.SetParent( playerInfoBB.currSelectBattleUnit.transform );
                selectBattleUnitLocaltionMaskGo.transform.localPosition = Vector3.zero;
                selectBattleUnitLocaltionMaskGo.SetActive( true );
                doNextActionState();
            }
        }
    }
}
