using UnityEngine;
using System;
namespace Mooji
{
    public struct CamFilterIntent
    {
        public Transform followTargetGhost;
        public LayerMask layerMask; 
        public Type[] requireComponentArr;
        public float colliderRadius ;
    }
}
