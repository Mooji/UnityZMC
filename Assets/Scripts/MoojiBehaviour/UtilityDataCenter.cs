
using UnityEngine;
using BehaviourMachine;
using Mooji;
using System.Collections.Generic;


namespace Mooji
{

    public class UtilityDataCenter : BMC
    {
        /// <summary>
        /// ��û������MainScene����£����¼���MainScene
        /// </summary>
        public string ReloadMainSceneBehaviour  = BMC.moojiNameSpaceName + typeof( ReloadMainSceneBehaviour ).Name;
        /// <summary>
        /// �����ƶ������� �ű�
        /// </summary>
        public string PlayerMovementManager = BMC.moojiNameSpaceName + typeof( PlayerMovementBehaviour ).Name;
        /// <summary>
        /// �������ذ�� ʮ�ֱ�ǿ��ƽű�
        /// </summary>
        public string PlayerClickedGroundMask = BMC.moojiNameSpaceName + typeof( ClickedGroundMaskBehaviour ).Name;
        /// <summary>
        /// �����ڰ壬��������ģ�� ��Ҳ����Ĺ��Ķ��� 
        /// </summary>
        public string SceneBlackBoard = BMC.moojiNameSpaceName + typeof( SceneBlackBoard ).Name;
        /// <summary>
        /// 
        /// </summary>
        public string GameBootstrapper = BMC.moojiNameSpaceName + typeof( GameBootstrapper ).Name;

    }
}

