using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mooji
{
    public enum ObserverMsgTypeEnum
    {
        LOGIN ,
        LOGIN_COMPLETE ,
        LOGIN_ERROR ,

        /// <summary>
        /// INPUT模块，点击后射线命中了一个3d Game obj
        /// [0] RaycastHit
        /// </summary>
        INPUT_CLICKED_HIT_3D_GAME_OBJECT ,

        /// <summary>
        /// INPUT模块，点击后射线命中了一个2d Game obj
        /// [0] ????
        /// </summary>
        INPUT_CLICKED_HIT_2D_GAME_OBJECT ,

        /// <summary>
        /// BehaviourMachine 中 fsmEvent = -10 的一个事件s
        /// 管理器处事完毕（在start回调后）
        /// [0] Monobehaviour
        /// </summary>
        BEHAVIOUR_MACHINE_GLOBALBLACKBOARD_FINISHED ,
        /// <summary>
        /// BehaviourMachine 中 自定义的 事件
        /// [0] ID:int
        /// </summary>
        BEHAVIOUR_MACHINE_FSM_EVENT ,
        /// <summary>
        /// [0] MoojiMonoState
        /// </summary>
        BEHAVIOUR_MACHINE_SET_AS_CONCURRENT ,

        /// <summary>
        /// 点击地板后主控玩家开始移动
        /// [0] RaycastHit
        /// </summary>
        PLAYER_START_TO_MOVE_BY_CLICK_GROUND,
        /// <summary>
        /// Player 的控制方式改变
        /// [0] List[GameObject]
        /// [1] List[PlayerConstant.playerControlerType]
        /// </summary>
        PLAYER_CONTROLER_CHANGED ,
        /// <summary>
        /// 角色（移动单位） 跟随的目标单位改变
        /// [0] GameObject    //  要跟随的对象 
        /// </summary>
        PLAYER_FOLLOW_TARGET_CHANGED ,
        /// <summary>
        /// 角色（移动单位状态改变）
        /// [0] GameObject
        /// [1] PlayerMovementState
        /// </summary>
        PLAYER_MOVEMENT_STATE_CHANGED ,
        /// <summary>
        /// 角色选择了一个可以攻击的单位 
        /// [0]AIBattleUnit 如果是 null 取SceneBlackBoard中的 选中的GO
        /// </summary>
        PLAYER_SELECT_CAN_ATTACK_UNITS ,
        /// <summary>
        /// 角色开始攻击
        /// [0] 攻击的gos 如果null 就是 SceneBlackBoard中的 选中的GO
        /// </summary>
        PLAYER_START_ATTACK ,
        /// <summary>
        /// 玩家移动时候 碰到了 定位球
        /// [0]定位球GO
        /// </summary>
        PLAYER_TRIGGER_MOVEMENT_POSITION_DEVICE



    }

}
