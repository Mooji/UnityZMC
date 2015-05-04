using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


namespace Mooji
{
    [BehaviourMachine.NodeInfo( category = "MoojiAction/DefaultSettings/" , icon = "Axis" )]
    public class DefaultSettingsAction : MoojiBehaviourMachineAction
    {
        /// <summary>
        /// 默认选中的 玩家可以控制的单位
        /// </summary>
        public GameObject defaultSelectLeadUnit;
        /// <summary>
        /// 默认的摄像机GO
        /// </summary>
        public GameObject defaultMainCameraPoint;
       
        protected override void actionAwake()
        {
        }

        protected override void actionStart()
        {
            //  设置默认可以控制的 unit
            playerInfoBB.setCurrLeadGo( defaultSelectLeadUnit );

            //  设置主摄像机 跟随的 unit
            CameraPointVo mainCameraPointVo =  defaultMainCameraPoint.GetComponent<CameraPointVo>();
            mainCameraPointVo.setFollowerTarget( defaultSelectLeadUnit );
            mainCameraPointVo.initDefaultCameraPoint();
            mainCameraPointVo.cameraFollowTarget();


            //  [保存]点击地板后的特效
            GameObject clickedFloorAnimGo = BMC.getPrefabsBehaviour().instantiatePrefab( PrefbasAssets.PrefabAssetsConstant.CLICKED_GROUND_MASK , true );
            playerInfoBB.setClickedFloorAnimGo( clickedFloorAnimGo );

            //  [保存]选择一个可控制单位的特效 或 高亮什么的
            GameObject currSelectLeadAnimGo = BMC.getPrefabsBehaviour().instantiatePrefab( PrefbasAssets.PrefabAssetsConstant.CLICKED_GROUND_MASK , true );
            playerInfoBB.setCurrSelectLeadAnimGo( currSelectLeadAnimGo );

            //  [保存]住摄像机 机位对象
            playerInfoBB.setMainCameraPointVo( mainCameraPointVo  );

        }
    }
}
