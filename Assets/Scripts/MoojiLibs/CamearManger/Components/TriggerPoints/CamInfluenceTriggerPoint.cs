using System;
using UnityEngine;

namespace Mooji
{
    /// <summary>
    /// 影响主摄像机的移动的触发点
    /// </summary>
    public class CamInfluenceTriggerPoint : MonoBehaviour
    {

        //  ============================ SerializeField ============================
        /// <summary>
        /// 位移权重
        /// </summary>
        public float positionWeight = 5f;
        /// <summary>
        /// 旋转权重
        /// </summary>
        public float quaternionWeight = 5f;
        //  ============================ Private ============================
        /// <summary>
        /// 数据读取器 自己 或 孩子必须包含这个组件
        /// </summary>
        private CamRig _cRig;
        /// <summary>
        /// 该碰撞点的 collider
        /// </summary>
        private Collider _collider;


        void Awake()
        {
            _cRig = GetComponentInChildren<CamRig>();

            if ( _cRig == null )
                throw new Exception( "自己 或 子物体必须包含一个 CamRig 组件, @see CamRig" );
        }


        virtual public float[] getPositionAndQuaternionInfluenceWeight( Transform target )
        {

            float tempPosW = 0f;
            float tempQuaternionW = quaternionWeight;

            if ( _collider == null )
                _collider = GetComponent<Collider>();

            if ( _collider is SphereCollider )
            {
                SphereCollider col = _collider as SphereCollider;
                tempPosW = positionWeight;
                tempPosW = ( col.radius > 0 ) && ( ( target.position - transform.position ).sqrMagnitude < ( col.radius * col.radius ) ) ? tempPosW : 0f;
            }
            else if ( _collider is BoxCollider )
            {
                Vector3 pos = transform.InverseTransformPoint( target.position );
                BoxCollider col = _collider as BoxCollider;
                Bounds bound = new Bounds( col.center , col.size );
                tempPosW = bound.Contains( pos ) ? positionWeight : 0f;
            }

            return new float[] { tempPosW , tempQuaternionW };
        }

        virtual public Vector3 getCamRigPosition()
        {
            return _cRig.transform.position;
        }

        virtual public Vector3 getCamRigfacing()
        {
            //return _cRig.position+ _cRig.transform.forward;
            return _cRig.transform.forward;
        }

        virtual public Vector3 getCamRigUp()
        {
            //return _cRig.position + _cRig.transform.up;
            return _cRig.transform.up;
        }



    }
}
