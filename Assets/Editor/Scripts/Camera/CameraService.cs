using System;
using System.IO;
using UnityEditor;
using UnityEngine;
using System.Text;

namespace Mooji.Editor
{
    public class CameraService : ScriptableWizard
    {
        //[MenuItem( "MoojiTools/Camera/saveCameraPointInfo" , false , 3 )]
        public static void saveCameraPointInfo()
        {

            //GameObject[] gos = Selection.gameObjects;

            //foreach ( GameObject go in gos )
            //{
            //    CameraPointVo cpVo = go.GetComponent<CameraPointVo>();
            //    if ( cpVo == null )
            //        throw new Exception( "CameraPointVo == null" );



            //    Camera c = go.transform.GetChild( 0 ).GetComponent<Camera>();
            //    if ( c == null )
            //        throw new Exception( "go.transform.GetChild( 0 ) must be camera" );

            //    Matrix4x4 m =  c.gameObject.transform.localToWorldMatrix;

            //    cpVo.worldPosition      = getPosition( m );
            //    cpVo.worldQuaternion    = go.transform.GetChild( 0 ).rotation;

            //    cpVo.cp_forward = c.transform.forward;
            //    cpVo.cp_up = c.transform.up;

            //}

            GameTools.updatePrjs();

        }


        private static Vector3 getPosition( Matrix4x4 m )
        {
            return new Vector3( m[0 , 3] , m[1 , 3] , m[2 , 3] );
        }
    }


}
