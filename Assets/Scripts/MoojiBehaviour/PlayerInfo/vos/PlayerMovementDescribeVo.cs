using UnityEngine;

namespace Mooji
{
    public class PlayerMovementDescribeVo 
    {
        /// <summary>
        /// 是否要添加 点击后在地板上出现的十字标记
        /// </summary>
        public bool isAddClickGroundMask = false;
        /// <summary>
        /// 该角色控制类型 
        /// </summary>
        public PlayerConstant.PlayerControlerType playerControlerType;
        /// <summary>
        /// 跟随的相对偏移量 
        /// </summary>
        public Vector3 floowOffsetVec3;

    }
}
