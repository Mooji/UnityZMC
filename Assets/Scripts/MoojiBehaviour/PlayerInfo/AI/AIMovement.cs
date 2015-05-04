using UnityEngine;

namespace Mooji
{
    [RequireComponent( typeof( Animator ) )]
    [RequireComponent( typeof( Rigidbody ) )]
    public abstract class AIMovement : AI
    {
        /// <summary>
        /// 人物刚体
        /// </summary>
        protected Rigidbody playerRigidbody;
        /// <summary>
        /// 游戏的配置文件
        /// </summary>
        protected GameConfigVo gameConfigVo;
        /// <summary>
        /// 人物移动的层
        /// </summary>
        protected int gameLayerByMovement;
        /// <summary>
        /// 子类的 start 写这里
        /// </summary>
        protected abstract void absStart();


        public void Start()
        {
            gameConfigVo        = BMC.getGameConfigBehaviour().gameConfigVo;
            gameLayerByMovement = NavMesh.GetAreaFromName( gameConfigVo.gameLayer_unit_movement );
            playerRigidbody     = GetComponent<Rigidbody>();
            absStart();

        }

        //protected NavMeshPath getPath( Vector3 targetPosition )
        //{
        //    if ( navMeshPath == null )
        //        navMeshPath = new NavMeshPath();
        //    else
        //        navMeshPath.ClearCorners();

        //    if ( NavMesh.CalculatePath( playerRigidbody.position , targetPosition , this.gameLayerByMovement , navMeshPath ) )
        //    {

        //        return navMeshPath;
        //    }

        //    return null;
        //}

        //protected NavMeshPath getPath( RaycastHit hit )
        //{
        //    if ( this.isHitOnPlayerMovementFloor( hit ) )
        //    {
        //        return getPath( hit.point );
        //    }

        //    return null;
        //}

        /// <summary>
        /// 是否点击了地板
        /// </summary>
        /// <param name="hit"></param>
        /// <returns></returns>
        public bool isHitOnPlayerMovementFloor( RaycastHit hit )
        {
            return hit.collider.gameObject.name == gameConfigVo.playerMovementFloor;
        }


    }
}
