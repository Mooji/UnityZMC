using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace Mooji
{
    [BehaviourMachine.NodeInfo( category = "MoojiAction/" , icon = "Axis" )]
    public class ActionDoNext : MoojiBehaviourMachineAction
    {

        protected override void actionAwake()
        {
        }

        protected override void actionStart()
        {
            doNextActionState();
        }
    }
}
