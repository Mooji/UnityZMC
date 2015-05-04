using System.Collections.Generic;
using UnityEngine;

namespace Mooji
{
    /// <summary>
    /// 连接数据库
    /// </summary>
    public class GameServerBehaviour : MonoBehaviour , IMoojiBehaviourManager
    {
        public void Start()
        {
            //Invoke( "connectionServer" , 1 );
            connectionServer();
        }

        private void connectionServer()
        {
            BMC.registerInitCompleteBehaviourManager( BehaviourManagerType.GAME_SERVER , this );
            BMC.getObserverBehaviour().publisheMsg( ObserverMsgTypeEnum.BEHAVIOUR_MACHINE_GLOBALBLACKBOARD_FINISHED , this );
        }

    }
}