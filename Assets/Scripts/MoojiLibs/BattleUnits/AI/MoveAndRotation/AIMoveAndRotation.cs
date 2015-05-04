using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Mooji
{
    public abstract class AIMoveAndRotation : AI
    {


        //  ====================== abs funs ======================
        protected abstract void absAwake();
        protected abstract void absStart();

        protected abstract IAnimatorHashID createAnimatorHashID();

        protected abstract int createMovementLayer();

        //  ====================== protected vars ======================

        protected IAnimatorHashID animatorHashID;
        protected int movementLayer;
        protected NavMeshPath currNavMeshPath;
        protected int navMeshPathCornersLen;
        protected int currPathIndex;
        protected Vector3 currMoveToPosition;


        void Awake()
        {
            animatorHashID = createAnimatorHashID();

            absAwake();
        }

        void Start()
        {
            currNavMeshPath = new NavMeshPath();
            movementLayer = createMovementLayer();

            absStart();
        }

        protected bool checkMoveToPosition( Vector3 targetPosition )
        {
            NavMeshHit nmhit;

            if ( NavMesh.SamplePosition( targetPosition , out nmhit , Mathf.Infinity , -1 ) )
            {
                if ( NavMesh.CalculatePath( this.transform.position , nmhit.position , -1 , currNavMeshPath ) )
                {

                    navMeshPathCornersLen = currNavMeshPath.corners.Length;
                    currPathIndex = 1;
                    currMoveToPosition = currNavMeshPath.corners[currPathIndex];

                    return true;
                }
            }

            currNavMeshPath = null;
            return false;
        }

        protected bool doNextMovePosition()
        {
            if ( currPathIndex == navMeshPathCornersLen - 1 )
            {
                return false;
            }
            else
            {
                currPathIndex += 1;
                currMoveToPosition = currNavMeshPath.corners[currPathIndex];
                return true;
            }
        }


        public IAnimatorHashID getHasIDVo()
        {
            return animatorHashID;
        }

        public int getMovementLayer()
        {
            return movementLayer;
        }



    }
}
