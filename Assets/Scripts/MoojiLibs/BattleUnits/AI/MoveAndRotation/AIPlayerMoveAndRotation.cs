using DG.Tweening;
using UnityEngine;
namespace Mooji
{
    public class AIPlayerMoveAndRotation : AIMoveAndRotation
    {
        /// <summary>
        /// 实际移动速度，和 ani的速度无关
        /// </summary>
        public float moveSpeed;
        /// <summary>
        /// 转身速度
        /// </summary>
        public float rotationSpeed;
        /// <summary>
        /// 移动的最大速度 5.66米/s
        /// </summary>
        public float aniMoveMaxSpeed       = 5.66f;
        /// <summary>
        /// 快到达目的地后，从跑 -》 走 -》 停 Ani的 间隔
        /// </summary>
        public float aniRunToWalkSpeed     = 0.2f;
        /// <summary>
        /// 刚体
        /// </summary>
        private Rigidbody rbody;
        /// <summary>
        /// 动画组件
        /// </summary>
        private Animator _ani;
        /// <summary>
        /// 移动完成后，有个从跑到停止的小动作
        /// </summary>
        private Tweener _moveCompleteTweener;
        /// <summary>
        /// 动画组件的 HASHID
        /// </summary>
        private SimpleAnimatorHashID _hashId;

        public void OnDestroy()
        {

            destoryTweener();

            _hashId = null;
        }

        private void destoryTweener()
        {
            if ( _moveCompleteTweener != null )
                _moveCompleteTweener.Kill();

            _moveCompleteTweener = null;
        }


        protected override void absAwake()
        {
            // nothing to do ...
        }

        protected override void absStart()
        {
            rbody = GetComponent<Rigidbody>();

            _ani = GetComponent<Animator>();
            _ani.applyRootMotion = false;

            _hashId = getHasIDVo() as SimpleAnimatorHashID;
        }

        protected override IAnimatorHashID createAnimatorHashID()
        {
            return this.gameObject.GetComponent<IAnimatorHashID>();
        }

        protected override int createMovementLayer()
        {
            return -1 ;
        }

        /// <summary>
        /// 上一层判断，已经是 距离 100米，在movement层上的有效点击点
        /// </summary>
        /// <param name="targetPosition"></param>
        public void move( Vector3 targetPosition )
        {
            if ( checkMoveToPosition( targetPosition ) )
            {
                joinUpdateCallBack( this.doMove );
            }
        }

        public void stopMove()
        {
            _ani.SetFloat( _hashId.runFloat , 0f );
            _ani.SetFloat( _hashId.turnFloat , 0f );
            destoryTweener();
            removeUpdateCallBack( this.doMove );
        }


        private void doMove()
        {

            Vector3 toPosition = Vector3.MoveTowards( this.transform.position , currMoveToPosition , moveSpeed * Time.deltaTime );
            this.transform.position = toPosition;
            _ani.SetFloat( _hashId.runFloat , aniMoveMaxSpeed , .1f , Time.deltaTime );


            Vector3 targetDir = currMoveToPosition - transform.position;
            targetDir.y = 0;

            Vector3 newDir = Vector3.RotateTowards( transform.forward , targetDir , rotationSpeed * Time.deltaTime , 0F );
            transform.rotation = Quaternion.LookRotation( newDir );

            float d = Vector3.Distance( rbody.position , currMoveToPosition );
            if ( d == 0f )
            {
                bool hasNextPath =  doNextMovePosition();

                if ( !hasNextPath )
                {
                    destoryTweener();

                    _moveCompleteTweener = DOVirtual.Float( aniMoveMaxSpeed , 0 , aniRunToWalkSpeed , ( float val ) =>
                    {
                        _ani.SetFloat( _hashId.runFloat , val );
                    } );

                    removeUpdateCallBack( this.doMove );
                }
            }
        }





    }
}
