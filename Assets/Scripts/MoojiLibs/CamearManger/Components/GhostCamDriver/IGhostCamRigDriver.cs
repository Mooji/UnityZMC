using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Mooji
{
    public interface IGhostCamRigDriver
    {
        void run( CamFilterResultInfo resultInfo );

        void init( CamDriverIntent camDriverIntent);

    }
}
