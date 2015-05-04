using System;
using System.Collections.Generic;
using UnityEngine;

namespace Mooji
{
    public class ClickedGroundMaskBehaviour : MonoBehaviour , ISubscriber , IMoojiBehaviourManager
    {
        private GameObject _maskGo = null;

        public void Start()
        {
            if ( _maskGo == null )
                _maskGo = BMC.getPrefabsBehaviour().instantiatePrefab( PrefbasAssets.PrefabAssetsConstant.CLICKED_GROUND_MASK , false );

            BMC.getObserverBehaviour().registerMsg( this );

            BMC.getObserverBehaviour().publisheMsg( ObserverMsgTypeEnum.BEHAVIOUR_MACHINE_GLOBALBLACKBOARD_FINISHED , this );
        }

        public List<ObserverMsgTypeEnum> subscriberMessages()
        {
            List<ObserverMsgTypeEnum> lst = new List<ObserverMsgTypeEnum>();
            lst.Add( ObserverMsgTypeEnum.INPUT_CLICKED_HIT_3D_GAME_OBJECT );
            return lst;
        }


        public void receivedMessage( ObserverMsgTypeEnum messageType , object paramsObj , PublisheOptionVo options )
        {
            switch ( messageType )
            {
                case ObserverMsgTypeEnum.INPUT_CLICKED_HIT_3D_GAME_OBJECT:
                {
                    //  点击地板的 十字图片 移动到点击的地方
                    RaycastHit raycastHit = ( (RaycastHit) paramsObj );
                    _maskGo.transform.position = new Vector3( raycastHit.point.x , 0f , raycastHit.point.z );
                    break;
                }

            }// end switch ...
        }

    }
}
