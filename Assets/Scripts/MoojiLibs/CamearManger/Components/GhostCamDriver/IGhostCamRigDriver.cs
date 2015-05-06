using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Mooji
{
    public interface IGhostCamRigDriver
    {

        void init(Transform targetGhostPlayer , Transform targetGhostRig);

        void run( CamFilterResultInfo resultInfo );

    }
}
