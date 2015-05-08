using UnityEngine;

namespace Mooji
{

    public class CamRig : MonoBehaviour
    {

        public float fov = 60f;


        void OnDrawGizmos()
        {
            if ( this.gameObject.name == "GhostRig" )
            {
                UnityEditor.Handles.color = Color.red;
            }
            else
                UnityEditor.Handles.color = Color.black;

            UnityEditor.Handles.ArrowCap( 0 , transform.position , Quaternion.LookRotation( transform.forward ) , 3f );
        }

        void OnDrawGizmosSelected()
        {
            //UnityEditor.Handles.color = new Color( 1f , 1f , 1f , 0.1f );
            //Vector3[] vert = new Vector3[4];
            //vert[0] = transform.position + transform.rotation * Quaternion.Euler( 0.5f * fov , 0.8f * fov , 0 ) * ( transform.forward * 100f );
            //vert[1] = transform.position + transform.rotation * Quaternion.Euler( 0.5f * fov , -0.8f * fov , 0 ) * ( transform.forward * 100f );
            //vert[2] = transform.position + transform.rotation * Quaternion.Euler( -0.5f * fov , -0.8f * fov , 0 ) * ( transform.forward * 100f );
            //vert[3] = transform.position + transform.rotation * Quaternion.Euler( -0.5f * fov , 0.8f * fov , 0 ) * ( transform.forward * 100f );
            //UnityEditor.Handles.DrawLine( transform.position , vert[0] );
            //UnityEditor.Handles.DrawLine( transform.position , vert[1] );
            //UnityEditor.Handles.DrawLine( transform.position , vert[2] );
            //UnityEditor.Handles.DrawLine( transform.position , vert[3] );
            //for ( int i = 1 ; i <= 10 ; i++ )
            //{
            //    vert[0] = transform.position + transform.rotation * Quaternion.Euler( 0.5f * fov , 0.8f * fov , 0 ) * ( transform.forward * 10f * i );
            //    vert[1] = transform.position + transform.rotation * Quaternion.Euler( 0.5f * fov , -0.8f * fov , 0 ) * ( transform.forward * 10f * i );
            //    vert[2] = transform.position + transform.rotation * Quaternion.Euler( -0.5f * fov , -0.8f * fov , 0 ) * ( transform.forward * 10f * i );
            //    vert[3] = transform.position + transform.rotation * Quaternion.Euler( -0.5f * fov , 0.8f * fov , 0 ) * ( transform.forward * 10f * i );
            //    UnityEditor.Handles.DrawSolidRectangleWithOutline( vert , UnityEditor.Handles.color , Color.black );
            //}
        }
    }
}

