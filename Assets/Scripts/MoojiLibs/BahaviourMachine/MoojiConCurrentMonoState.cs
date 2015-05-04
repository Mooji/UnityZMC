//----------------------------------------------
//            Behaviour Machine
// Copyright © 2014 Anderson Campos Cardoso
//----------------------------------------------

using UnityEngine;
using System.Collections;
using BehaviourMachine;

namespace Mooji {

    /// <summary>
    /// Wrapper class for the InternalMonoState component.
    /// 继承这个类 && 添加到 StateMachine 后，具有获取游戏中所有publish出来的消息，从而指派给 behaviourMachine 的 action 等 。。。 
    /// <summary>
    [RequireComponent(typeof(StateMachine))]
    public class MoojiConCurrentMonoState : InternalMonoState 
    {
        public void Start()
        {
            BMC.getObserverBehaviour().publisheMsg( ObserverMsgTypeEnum.BEHAVIOUR_MACHINE_SET_AS_CONCURRENT , this );
        }
    }
}