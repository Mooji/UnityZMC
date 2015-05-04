using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace Mooji
{
    /// <summary>
    /// 人物移动管理器
    /// </summary>
    public class PlayerMovementBehaviour : MonoBehaviour , ISubscriber , IMoojiBehaviourManager
    {
        private  RaycastHit     _raycastHit;
        private GameObject[]    _unitsGameObjArr;
        private GameObject _mainPlayer;
        public void Start()
        {

            List<GameObject> goLst = new List<GameObject>();
            List<PlayerConstant.PlayerControlerType> pctLst = new List<PlayerConstant.PlayerControlerType>();


            //      ===================== test ===============================
            _unitsGameObjArr = GameObject.FindGameObjectsWithTag( "tag_testPlayers" );
            _mainPlayer = GameObject.FindGameObjectWithTag( "Player" );

            foreach ( GameObject item in _unitsGameObjArr )
            {

                goLst.Add( item );
                pctLst.Add( PlayerConstant.PlayerControlerType.FOLLOWER );
            }


            goLst.Add( _mainPlayer );
            pctLst.Add( PlayerConstant.PlayerControlerType.PROTAGONIST );



            BMC.registerInitCompleteBehaviourManager( BehaviourManagerType.PLAYER_MOVEMENT , this );


            BMC.getObserverBehaviour().publisheMsg( ObserverMsgTypeEnum.PLAYER_CONTROLER_CHANGED , new object[] { goLst , pctLst } );
            BMC.getObserverBehaviour().publisheMsg( ObserverMsgTypeEnum.PLAYER_FOLLOW_TARGET_CHANGED , _mainPlayer );


            BMC.getObserverBehaviour().registerMsg( this );


            //      ===================== test ===============================
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

                    _mainPlayer = GameObject.FindGameObjectWithTag( "Player" );



                    return;


                    //_raycastHit = (RaycastHit) paramsObj;

                    //IAIBattleUnit battleUnit = _raycastHit.collider.gameObject.GetComponent<IAIBattleUnit>();

                    ////  是一个战斗单位
                    //if ( battleUnit != null )
                    //{
                    //    //  告诉作战单位 你将要被攻击
                    //    battleUnit.doReadyToAcceptAttack( _raycastHit );


                    //    //  移动到 可以攻击的位置
                    //    Vector3 targetPosition = battleUnit.doGetComponent<Rigidbody>().position + _raycastHit.point.normalized * -1f;
                    //    NavMeshPath navMeshPath = new NavMeshPath();
                    //    GameConfigVo gameConfigVo = BehaviourManagermentCenter.getGameConfigBehaviour().gameConfigVo;
                    //    int gameLayerByMovement = NavMesh.GetAreaFromName( gameConfigVo.gameLayer_unit_movement );

                    //    if ( NavMesh.CalculatePath( _mainPlayer.GetComponent<Rigidbody>().position , targetPosition , gameLayerByMovement , navMeshPath ) )
                    //    {
                    //        _mainPlayer.GetComponent<AIPlayerMovementByCalculatePath>().doMove( navMeshPath );
                    //    }
                    //}

                    //else
                    //{
                    //    NavMeshPath navMeshPath = new NavMeshPath();
                    //    GameConfigVo gameConfigVo = BehaviourManagermentCenter.getGameConfigBehaviour().gameConfigVo;
                    //    int gameLayerByMovement = NavMesh.GetAreaFromName( gameConfigVo.gameLayer_unit_movement );

                    //    if ( NavMesh.CalculatePath( _mainPlayer.GetComponent<Rigidbody>().position , _raycastHit.point , gameLayerByMovement , navMeshPath ) )
                    //    {
                    //        _mainPlayer.GetComponent<AIPlayerMovementByCalculatePath>().doMove( navMeshPath );
                    //    }
                    //}

                   


                    break;
                }
            }// end switch ...
        }


        public PlayerConstant.BearingClockType getBearingClockType( Transform origin , Vector3 targetVec )
        {
            Vector3 offsetVec3 = ( origin.InverseTransformDirection( origin.position ) - targetVec );
            float forwardAndBehindDot = Vector3.Dot( origin.forward , offsetVec3 );
            float leftAndRightDot = Vector3.Dot( origin.right , offsetVec3 );
            PlayerConstant.BearingClockType result  = PlayerConstant.BearingClockType.NONE;

            if ( forwardAndBehindDot > 0 )
            {
                if ( leftAndRightDot == 0 )
                {
                    result = PlayerConstant.BearingClockType.BEHIND;
                }
                else
                {
                    result = leftAndRightDot > 0 ? PlayerConstant.BearingClockType.BEHIND_LEFT : PlayerConstant.BearingClockType.BEHIND_RIGHT;
                }
            }
            else
            {

                if ( leftAndRightDot == 0 )
                {
                    result = PlayerConstant.BearingClockType.FORWARD;
                }
                else
                {
                    result = leftAndRightDot > 0 ? PlayerConstant.BearingClockType.FORWARD_LEFT : PlayerConstant.BearingClockType.FROWARD_RIGHT;
                }
            }

            return result;

        }

        public PlayerConstant.BearingClockType getBearingClockType( Transform origin , Transform target )
        {
            return getBearingClockType( origin , target.position );
        }



    }
}
