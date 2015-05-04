
using UnityEngine;
using BehaviourMachine;
using Mooji;
using System.Collections.Generic;


namespace Mooji
{

    public class UtilityDataCenter : BMC
    {
        /// <summary>
        /// 在没有启动MainScene情况下，重新加载MainScene
        /// </summary>
        public string ReloadMainSceneBehaviour  = BMC.moojiNameSpaceName + typeof( ReloadMainSceneBehaviour ).Name;
        /// <summary>
        /// 人物移动管理器 脚本
        /// </summary>
        public string PlayerMovementManager = BMC.moojiNameSpaceName + typeof( PlayerMovementBehaviour ).Name;
        /// <summary>
        /// 人物点击地板后 十字标记控制脚本
        /// </summary>
        public string PlayerClickedGroundMask = BMC.moojiNameSpaceName + typeof( ClickedGroundMaskBehaviour ).Name;
        /// <summary>
        /// 场景黑板，保存所有模块 玩家操作的过的对象 
        /// </summary>
        public string SceneBlackBoard = BMC.moojiNameSpaceName + typeof( SceneBlackBoard ).Name;
        /// <summary>
        /// 
        /// </summary>
        public string GameBootstrapper = BMC.moojiNameSpaceName + typeof( GameBootstrapper ).Name;

    }
}

