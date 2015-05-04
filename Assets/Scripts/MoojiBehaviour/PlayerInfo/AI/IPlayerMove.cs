using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Mooji
{
    public interface IPlayerMove
    {
        void doMove( Vector3 targetPosition );
    }
}
