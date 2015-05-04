using UnityEngine;
using System.Collections;
using Mooji;
using System.Collections.Generic;
using DG.Tweening;

public class AIPlayerMoveByAgent : MonoBehaviour , ISubscriber
{

    private NavMeshAgent _agent;

    // Use this for initialization
    void Start()
    {

        BMC.getObserverBehaviour().registerMsg( this );
    }

    // Update is called once per frame
    void Update()
    {

    }

    public System.Collections.Generic.List<ObserverMsgTypeEnum> subscriberMessages()
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
                RaycastHit hit = (RaycastHit) paramsObj;

                GetComponent<Animator>().applyRootMotion = true;
                _agent = GetComponent<NavMeshAgent>();
                _agent.updateRotation = false;

                this.transform.DOLookAt( hit.point , .2f , AxisConstraint.Y);


                _agent.SetDestination( hit.point );
                GetComponent<Animator>().SetFloat( "Forward" , 1 );

                break;
            }
        }
    }
}
