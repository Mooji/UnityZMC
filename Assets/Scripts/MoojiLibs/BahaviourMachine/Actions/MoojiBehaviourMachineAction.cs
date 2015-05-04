using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BehaviourMachine;

namespace Mooji
{
    public abstract class MoojiBehaviourMachineAction : ActionNode
    {

        protected bool isAwake = false;

        protected PlayerInfoBlackBoard playerInfoBB;


        protected abstract void actionAwake();

        protected abstract void actionStart();

        protected virtual void destory()
        {

        }

        protected void doNextActionState()
        {
            owner.SendEvent( GlobalBlackboard.FINISHED );
        }

        protected void sendFSMEvent( int id )
        {
            owner.SendEvent( id );
        }
        public override Status Update()
        {
            if ( !isAwake )
            {
                playerInfoBB = SceneBlackBoard.getInstance().getBlackBoard<PlayerInfoBlackBoard>();

                actionAwake();
                actionStart();
                isAwake = true;
            }
            else
                actionStart();

            return Status.Success;
        }

        protected void publisheMsgByCoroutine( ObserverMsgTypeEnum msg , object paramsObj )
        {
            BMC.getObserverBehaviour().publisheMsgByCoroutine( msg , paramsObj );
        }
    }
}
