using BehaviourMachine;
using UnityEngine;
using System.Collections.Generic;

namespace Mooji
{
    [NodeInfo( category = "MoojiAction/Battle/" ,
                icon = "Axis" ,
                description = "" ,
                url = "" )]
    public class PlayerCloseToCanAttackUnitsAction : MoojiBehaviourMachineAction
    {

        private PlayerUnitManager _playerUnitManagerComponent;
        private PlayerInfoBlackBoard _playerInfoBlackBoard;

        protected override void actionAwake()
        {
            _playerInfoBlackBoard = SceneBlackBoard.getInstance().getBlackBoard<PlayerInfoBlackBoard>();
            _playerUnitManagerComponent = _playerInfoBlackBoard.getCurrleadGo().GetComponent<PlayerUnitManager>();
        }

        protected override void actionStart()
        {
            if ( isInAttackDistance() )
            {
                doNextActionState();
            }
            else
            {
                _playerUnitManagerComponent.joinUpdateCallBack( this.checkDistanceByCanAttack );
                _playerUnitManagerComponent.doMoveToByPosition( _playerInfoBlackBoard.currSelectBattleUnit.transform.position );
            }
        }

        private void checkDistanceByCanAttack()
        {
            if( this.isInAttackDistance())
            {
                _playerUnitManagerComponent.removeUpdateCallBack( this.checkDistanceByCanAttack );
                _playerUnitManagerComponent.doStopMoveToPosition();
                doNextActionState();
            }
        }


        private bool isInAttackDistance()
        {
            return Vector3.Distance( _playerUnitManagerComponent.transform.position , _playerInfoBlackBoard.currSelectBattleUnit.transform.position ) <= _playerUnitManagerComponent.canAttackDistance; 
        }

        
    }
}
