using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Mooji
{
    public interface ICamFluenceTriggerPoint
    {
        float[] getPositionAndQuaternionInfluenceWeight( Transform target );
        Vector3 getCamRigPosition();
        Quaternion getCamRigRotation();
        Vector3 getCamRigfacing();
        Vector3 getCamRigUp();
        CamRig getCamRig();
        void setCamRigPosition( Vector3 pos );
        void setCamRigRotation( Quaternion q );
    }
}
