using System;
using System.Collections.Generic;
using UnityEngine;

namespace Mooji
{
    [RequireComponent( typeof( ICamFluencePointsFilter ) )]
    public class CameraInFluenceController : MonoBehaviour
    {

        //  ============================ SerializeField ============================
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
        /// 
        /// </summary>
        public Transform ghostRig;


        //public ICameraFluenceZone zfz;
        //  ============================ Private ============================

        /// <summary>
        /// 过滤碰撞后权重的 service 接口
        /// </summary>
        private ICamFluencePointsFilter _fluencePointFilter;
        private CamFilterIntent _defCamFilterIntent;




        void Awake()
        {

            _defCamFilterIntent = new CamFilterIntent();
            _defCamFilterIntent.followTargetGhost = ghostTarget;
            _defCamFilterIntent.layerMask = layerMask;
            _defCamFilterIntent.requireComponentArr = null;
            _defCamFilterIntent.colliderRadius = 1f;

            //  碰撞 权重计算等 过滤器
            _fluencePointFilter = GetComponent<ICamFluencePointsFilter>();

            //if ( _defCamRig == null )
            //    throw new Exception( "没有设置主 （默认）摄像机的 数据::" + this.gameObject.name );

            if ( _fluencePointFilter == null )
                throw new Exception( "没有碰撞检测的过滤器！" );
        }


        void Start()
        {
           

        }



        void Update()
        {
            CamFilterResultInfo camFilterResultInfo;

            if ( _fluencePointFilter.filter( _defCamFilterIntent , out camFilterResultInfo ) )
            {

                List<CamInfluenceTriggerPoint> hitTriggerPointLst = camFilterResultInfo.hitTriggerPointLst;

                for ( int i = 0 ; i < hitTriggerPointLst.Count ; i++ )
                {
                    IGhostCamRigDriver rigDriver = hitTriggerPointLst[i].GetComponent<IGhostCamRigDriver>();

                    if ( rigDriver != null )
                    {
                        rigDriver.init( ghostTarget , ghostRig );
                        rigDriver.run( camFilterResultInfo );

                    }
                }


            }
            //  使用默认的数据
            else
            {
                throw new Exception( "最大的Collider没有设置 或 层不对 或 你Y的走出了最大的collider~" );
            }

        }

    }
}
