using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


namespace Mooji
{
    [BehaviourMachine.NodeInfo( category = "MoojiAction/Player/" , icon = "Axis" )]
    public class PlayerChangedCurrLeadAction : MoojiBehaviourMachineAction
    {

        protected override void actionAwake()
        {

        }

        protected override void actionStart()
        {
            //  切换主摄像机跟对的目标
            ////playerInfoBB.getMainCameraPointVo().setFollowerTarget( playerInfoBB.getCurrleadGo() );

            //////  选中的单位有一个 选中的标志
            ////GameObject selectAnim = playerInfoBB.getsetCurrSelectLeadAnimGo();
            ////selectAnim.transform.SetParent( playerInfoBB.getCurrleadGo().transform , false );
            ////selectAnim.transform.localPosition = Vector3.zero;

        }
    }
}
