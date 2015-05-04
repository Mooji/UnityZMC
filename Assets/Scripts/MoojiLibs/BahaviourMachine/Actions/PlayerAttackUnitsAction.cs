using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using BehaviourMachine;


namespace Mooji
{
    [NodeInfo( category = "MoojiAction/Battle/" ,
                icon = "Axis" ,
                description = "" ,
                url = "" )]
    public class PlayerAttackUnitsAction : MoojiBehaviourMachineAction
    {

        private PlayerUnitManager _pum;
        private PlayerInfoBlackBoard _playerInfoBB;

        protected override void actionAwake()
        {
            _playerInfoBB = SceneBlackBoard.getInstance().getBlackBoard<PlayerInfoBlackBoard>();
            _pum = _playerInfoBB.getCurrleadGo().GetComponent<PlayerUnitManager>();
        }

        protected override void actionStart()
        {
            AIBattleUnit target     = _playerInfoBB.currSelectBattleUnit;
            _pum.doAttack( target );
        }
    }
}
