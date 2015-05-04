using UnityEngine;
namespace Mooji
{
    [RequireComponent( typeof( Animator ) )]
    [RequireComponent( typeof( Rigidbody ) )]
    public class AIAnimatorLookAt : MonoBehaviour
    {

        /// <summary>
        /// animator中 控制转向的 变量
        /// </summary>
        public string aniKeyTrun        = "Turn";
        /// <summary>
        /// 旋转系数
        /// </summary>
        public float turnDampTime = 0.2f;
        /// <summary>
        /// 帮助转身最大角度
        /// </summary>
        public float fastTrunMaxAngle = 360f;
        /// <summary>
        /// 帮助转身最小角度
        /// </summary>
        public float fastTrunMinAngel = 180f;
        /// <summary>
        /// 动画组件
        /// </summary>
        private Animator    _ani;
        /// <summary>
        /// 
        /// </summary>
        private Rigidbody   _playerRigidbody;
        /// <summary>
        /// 
        /// </summary>
        private float       _turnAmount;


        public void Awake()
        {
            _ani = GetComponent<Animator>();
            _playerRigidbody = GetComponent<Rigidbody>();
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="targetVec"></param>
        /// <returns></returns>
        public Vector3 lookAtTargetVec( Vector3 targetVec )
        {
            targetVec = targetVec - this._playerRigidbody.position;
            targetVec = this.transform.InverseTransformDirection( targetVec );
            targetVec = Vector3.ProjectOnPlane( targetVec , Vector3.up );
            _turnAmount = Mathf.Atan2( targetVec.x , targetVec.z );
            _ani.SetFloat( aniKeyTrun , _turnAmount , turnDampTime , Time.fixedDeltaTime );
            return targetVec;
        }

        /// <summary>
        /// 某些动画转身很慢，这里帮助快速转身
        /// </summary>
        /// <param name="forwardAmount">根据动画移动的参数决定</param>
        public void helpTrunFast( float t)
        {
            float turnSpeed = Mathf.Lerp( fastTrunMaxAngle , fastTrunMinAngel , t );
            transform.Rotate( 0 , _turnAmount * turnSpeed * Time.deltaTime , 0 );
        }

        public void stopLookAt()
        {
            _turnAmount = 0f;
            _ani.SetFloat( aniKeyTrun , _turnAmount );
        }

        public float getCurrTrunAmount()
        {
            return _turnAmount;
        }

    }
}
