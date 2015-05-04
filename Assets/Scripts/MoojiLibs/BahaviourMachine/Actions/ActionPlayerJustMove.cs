

namespace Mooji
{
    [BehaviourMachine.NodeInfo( category = "MoojiAction/Units/" , icon = "Axis" )]
    public class ActionPlayerJustMove : MoojiBehaviourMachineAction
    {


        protected override void actionAwake()
        {
        }

        protected override void actionStart()
        {
            PlayerUnitManager _pum = playerInfoBB.getCurrleadGo().GetComponent<PlayerUnitManager>();
            _pum.doJustMoveToPosition( playerInfoBB.currClickedRaycastHit.point );
           
        }
    }
}
