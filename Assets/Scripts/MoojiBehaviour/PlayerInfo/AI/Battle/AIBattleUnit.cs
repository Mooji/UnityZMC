using System.Collections.Generic;
using UnityEngine;

namespace Mooji
{
    public class AIBattleUnit : AI , ISubscriber
    {
        public void OnTriggerEnter( Collider other )
        {
            Debug.Log( other.name );
        }

        public void Start()
        {
            BMC.getObserverBehaviour().registerMsg( this );
        }

        public void OnDestroy()
        {
        }

        public List<ObserverMsgTypeEnum> subscriberMessages()
        {
            List<ObserverMsgTypeEnum> lst = new List<ObserverMsgTypeEnum>();
            return lst;
        }

        public void receivedMessage( ObserverMsgTypeEnum messageType , object paramsObj , PublisheOptionVo options )
        {
        }

        //  ===========================================================================

        public void attack( AIBattleUnit targetBattleUnit )
        {

        }



    }


    class CollisionRefereeVo
    {
        public AIBattleUnit from;
        public AIBattleUnit target;


        public CollisionRefereeVo( AIBattleUnit from , AIBattleUnit target )
        {
            this.from = from;
            this.target = target;
        }
    }


}
