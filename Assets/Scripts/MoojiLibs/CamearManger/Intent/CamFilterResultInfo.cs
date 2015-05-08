using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine ;

namespace Mooji
{
    public struct CamFilterResultInfo
    {
        public Vector3 resultMixedPosition;
        public Vector3 resultMixedForward;
        public Vector3 resultMixedUp;
        public List<ICamFluenceTriggerPoint> hitTriggerPointLst;
    }
}
