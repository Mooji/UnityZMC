using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Dest.Math;

namespace Mooji
{

    [ExecuteInEditMode]
    public class DrawGizmos : MonoBehaviour
    {
        public GameObject[] drawSphereGoArr;

        private void OnDrawGizmos()
        {
            if ( null != drawSphereGoArr )
            {
                foreach ( GameObject item in drawSphereGoArr )
                {
                    if ( item == null ) continue;

                    Sphere3 sphere  = new Sphere3( item.transform.position , item.transform.localScale.x );
                    Gizmos.DrawWireSphere( sphere.Center , sphere.Radius );
                }
            }



        }

    }


}
