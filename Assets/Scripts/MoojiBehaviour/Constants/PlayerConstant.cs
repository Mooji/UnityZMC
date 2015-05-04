using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mooji
{
    public class PlayerConstant
    {
        public enum PlayerControlerType
        {
            /// <summary>
            /// 主角（主要控制对象）
            /// </summary>
            PROTAGONIST,
            /// <summary>
            /// 跟随者(跟随主角)
            /// </summary>
            FOLLOWER,
            /// <summary>
            /// 不能控制的
            /// </summary>
            INCONTROLLABLE
        }

        /// <summary>
        /// 方位时钟
        /// val > x right ,
        /// val > z forward
        /// </summary>
        public enum BearingClockType
        {
            NONE,
            FORWARD , 
            FORWARD_LEFT ,
            FROWARD_RIGHT ,
            BEHIND,
            BEHIND_LEFT ,
            BEHIND_RIGHT 
        }

        public enum PlayerMovementState
        {
            NONE,
            /// <summary>
            /// 准备开始移动（有可以移动的路径）
            /// </summary>
            READY_TO_MOVE , 
            /// <summary>
            /// 移动中
            /// </summary>
            MOVING ,  
            /// <summary>
            /// 准备停止移动（寻路后到达终点）
            /// </summary>
            READY_TO_STOP ,
            /// <summary>
            /// 停止移动（寻路后到达终点）
            /// </summary>
            MOVE_COMPLETE
        }

    }
}
