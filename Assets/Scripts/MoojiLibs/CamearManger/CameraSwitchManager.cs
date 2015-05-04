using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
namespace Mooji
{
    public class CameraSwitchManager : MonoBehaviour
    {
        public GameObject defaultCameraPointGo;

        public GameObject switchTargetGo;
        public List<GameObject> cameraPointLst;



        private float _currAllWeightSum = 0f;
        private GameObject _mainCamera;
        private CameraPointVo _defaultCameraPointVo;

        public void Start()
        {
            _mainCamera = GameObject.FindGameObjectWithTag( "MainCamera" );
            _defaultCameraPointVo = defaultCameraPointGo.GetComponent<CameraPointVo>();
           
        }

        public void Update()
        {
            //  本轮重新计算 权重和
            _currAllWeightSum = 0f;

            //  如果list中一个point点都没了，那么就加入默认的point
            if ( cameraPointLst.Count == 0 )
            {
                addDefaultCameraPointVo();
            }


            Vector3 tempPositionVec3    = Vector3.zero;
            Vector3 resultQVec3         = Vector3.zero;

            Vector3 tempFrowardVec3 = Vector3.zero;
            Vector3 tempUpVec3 = Vector3.zero;

            foreach ( GameObject item in cameraPointLst )
            {
                if ( item == null )
                    continue;


                CameraPointVo tempCameraPointVo =  item.GetComponent<CameraPointVo>();
                float weight = tempCameraPointVo.weight;
                _currAllWeightSum += weight;
                tempPositionVec3 += tempCameraPointVo.worldPosition * weight;

                tempFrowardVec3 += tempCameraPointVo.cp_forward * weight;
                tempUpVec3 += tempCameraPointVo.cp_up * weight;

            }

            if ( _currAllWeightSum > _defaultCameraPointVo.weight )
            {
                tempPositionVec3 = tempPositionVec3 / _currAllWeightSum;



                //resultQVec3 = (Camera.main.transform.position.normalized - tempPositionVec3.normalized );
                //  rotation
                //Camera.main.transform.rotation = Quaternion.Lerp( Camera.main.transform.rotation , Quaternion.LookRotation( resultQVec3 ) , Time.deltaTime );

                tempFrowardVec3 = tempFrowardVec3 / _currAllWeightSum;
                tempUpVec3 = tempUpVec3 / _currAllWeightSum;

                // Vector3 aa =  ( Camera.main.transform.forward + Camera.main.transform.up ) - (  );
                Camera.main.transform.rotation = Quaternion.Lerp( Camera.main.transform.rotation , Quaternion.LookRotation( tempFrowardVec3 , tempUpVec3 ) , Time.deltaTime );

                _mainCamera.transform.position = Vector3.Lerp( _mainCamera.transform.position , tempPositionVec3 , Time.deltaTime );
            }

            //  清除权重比为0的cameraPointVo
            if ( _currAllWeightSum > 0 && cameraPointLst.Count > 1 )
            {

                for ( int i = 0 ; i < cameraPointLst.Count ; i++ )
                {
                    GameObject item = cameraPointLst[i];

                    if ( item == null )
                        continue;

                    CameraPointVo cpVo = item.GetComponent<CameraPointVo>();
                    int weightRatio = cpVo.getWeightedRatio( _currAllWeightSum );

                    if ( weightRatio == 0 )
                    {
                        cpVo.reSet();
                        cameraPointLst.Remove( item );
                    }
                }
            }

        }

        public void someGoTriggerStateChanged( Collider other , SphereCollider cameraPointCollider , int state )
        {
            switch ( state )
            {
                //  OnTriggerEnter
                case 1:
                {

                    break;
                }
                //  OnTriggerStay
                case 2:
                {
                    if ( !cameraPointLst.Contains( cameraPointCollider.gameObject ) )
                        cameraPointLst.Add( cameraPointCollider.gameObject );

                    break;
                }
                //  OnTriggerExit
                case 3:
                {

                    if ( cameraPointCollider.GetComponent<CameraPointVo>().isOnTriggerExitAddDefaultPoint )
                    {
                        addDefaultCameraPointVo();
                    }

                    cameraPointLst.Remove( cameraPointCollider.gameObject );

                    break;
                }
            }
        }


        private void addDefaultCameraPointVo()
        {
            if ( cameraPointLst == null )
                return;

            if ( !cameraPointLst.Contains( defaultCameraPointGo ) )
            {
                cameraPointLst.Add( defaultCameraPointGo );
                _defaultCameraPointVo.cameraFollowTarget();
            }

        }




    }
}
