using UnityEngine;
using System.Collections;
using UnityEditor;
using Mooji;


//[CustomEditor( typeof( CameraPointVo ) )]
public class EditorCameraPointVo : Editor
{
    public override void OnInspectorGUI()
    {

        EditorGUILayout.HelpBox( "123" , MessageType.Info );

        DrawDefaultInspector();


       // CameraPointVo cpVo = (CameraPointVo) target;


        GUILayout.BeginHorizontal();

            if ( GUILayout.Button( "Create This Pointinfo" , new GUILayoutOption[] { GUILayout.Width( 200 ) } ) )
            {

            }

            if ( GUILayout.Button( "Create All PointInfo By Tag is 'CameraPointInfo'" , new GUILayoutOption[] { GUILayout.Width( 300 ) } ) )
            {

            }



        GUILayout.EndHorizontal();


        

    }

}
