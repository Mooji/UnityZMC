using System;
using System.Collections.Generic;
using UnityEngine;

namespace Mooji
{
    [RequireComponent( typeof( ICamFluencePointsFilter ) )] //  必须包含一个对于碰撞的过滤器
    public class CameraInFluenceController : MonoBehaviour
    {

        //  ============================ SerializeField ============================
        /// <summary>
        /// 那个层的碰撞进行过滤
        /// </summary>
        public LayerMask layerMask;
        /// <summary>
        /// 默认的一个主摄像机数据(就是跟着主角走的)
        /// </summary>
        public Transform mainCamera;
        /// <summary>
        /// 主要碰撞体 由每个触发点 携带的驱动（如果有的话）控制 position 或 rotation
        /// </summary>
        public Transform ghostTarget;
        /// <summary>
        /// 是否自动跟着ghostRig移动 
        /// </summary>
        public bool ghostAttachMainCamera;

        //  ============================ Private ============================

        /// <summary>
        /// 过滤碰撞后权重的 service 接口
        /// </summary>
        private ICamFluencePointsFilter _fluencePointFilter;
        /// <summary>
        /// 过滤意图
        /// </summary>
        private CamFilterIntent _defCamFilterIntent;


        //  ============================ Functions ============================

        void Awake()
        {
            //  碰撞 权重计算等 过滤器
            _fluencePointFilter = GetComponent<ICamFluencePointsFilter>();
            //  在实现ICamFluencePointsFilter接口的类中 创建一个过滤意图
            _defCamFilterIntent = _fluencePointFilter.createFilterIntent();

        }

        void Update()
        {
            CamFilterResultInfo camFilterResultInfo;

            if ( _fluencePointFilter.filter( _defCamFilterIntent , out camFilterResultInfo ) )
            {

                List<ICamFluenceTriggerPoint> hitTriggerPointLst = camFilterResultInfo.hitTriggerPointLst;

                for ( int i = 0 ; i < hitTriggerPointLst.Count ; i++ )
                {
                    IGhostCamRigDriver rigDriver = ( hitTriggerPointLst[i] as Component ).GetComponent<IGhostCamRigDriver>();

                    if ( rigDriver != null )
                    {
                        rigDriver.run( camFilterResultInfo );
                    }
                }


            }
            //  使用默认的数据
            else
            {
                throw new Exception( "最大的Collider没有设置 或 层不对 或 你Y的走出了最大的collider~" );
            }


            //  主摄像机跟着Ghost CamRig 移动 和旋转
            //if ( ghostAttachMainCamera && mainCamera != null )
            //{
            //    mainCamera.position = ghostRig.position;
            //    mainCamera.rotation = ghostRig.rotation;
            //}

        }

    }
}
