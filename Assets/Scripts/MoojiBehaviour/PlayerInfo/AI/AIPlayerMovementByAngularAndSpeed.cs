using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Mooji
{
    public class AIPlayerMovementByAngularAndSpeed : AIMovement , ISubscriber
    {

        public float speedDampTime = 0.1f;              // Damping time for the Speed parameter.
        public float angularSpeedDampTime = 0.7f;       // Damping time for the AngularSpeed parameter
        public float angleResponseTime = 0.6f;          // Response time for turning an angle into angularSpeed

        public float deadZone = 5f;             // The number of degrees for which the rotation isn't controlled by Mecanim.

        private NavMeshAgent nav;               // Reference to the nav mesh agent.
        private Animator anim;                  // Reference to the Animator.
        private  RaycastHit hit;
        public void Awake()
        {
            nav = GetComponent<NavMeshAgent>();
            nav.updateRotation = false;
            nav.speed = 5f;
            anim = GetComponent<Animator>();

            deadZone *= Mathf.Deg2Rad;
        }

        protected override void absStart()
        {
            BMC.getObserverBehaviour().registerMsg( this );
        }

        void Update()
        {
            // Calculate the parameters that need to be passed to the animator component.
            NavAnimSetup();
        }


        void OnAnimatorMove()
        {
            // Set the NavMeshAgent's velocity to the change in position since the last frame, by the time it took for the last frame.
            nav.velocity = anim.deltaPosition / Time.deltaTime;

            // The gameobject's rotation is driven by the animation's rotation.
            transform.rotation = anim.rootRotation;
        }


        void NavAnimSetup()
        {
            // Create the parameters to pass to the helper function.
            float speed;
            float angle;



            //speed = 0f;
            //angle = FindAngle( transform.forward , hit.point - transform.position , transform.up );



            speed = Vector3.Project( nav.desiredVelocity , transform.forward ).magnitude * 1.2f;

            angle = FindAngle( transform.forward , nav.desiredVelocity , transform.up );

            // If the angle is within the deadZone...
            if ( Mathf.Abs( angle ) < deadZone )
            {
                // ... set the direction to be along the desired direction and set the angle to be zero.
                transform.LookAt( transform.position + nav.desiredVelocity );
                angle = 0f;
            }

            // Call the Setup function of the helper class with the given parameters.
            this.Setup( speed , angle );
        }


        float FindAngle( Vector3 fromVector , Vector3 toVector , Vector3 upVector )
        {
            if ( toVector == Vector3.zero )
                return 0f;

            float angle = Vector3.Angle( fromVector , toVector );
            Vector3 normal = Vector3.Cross( fromVector , toVector );
            angle *= Mathf.Sign( Vector3.Dot( normal , upVector ) );
            angle *= Mathf.Deg2Rad;

            return angle;
        }


        private void Setup( float speed , float angle )
        {
            // Angular speed is the number of degrees per second.
            float angularSpeed = angle / angleResponseTime;

            // Set the mecanim parameters and apply the appropriate damping to them.
            anim.SetFloat( "Forward" , speed , speedDampTime , Time.deltaTime );
            anim.SetFloat( "Turn" , angularSpeed , angularSpeedDampTime , Time.deltaTime );
        }

        public List<ObserverMsgTypeEnum> subscriberMessages()
        {
            List<ObserverMsgTypeEnum> lst = new List<ObserverMsgTypeEnum>();
            lst.Add( ObserverMsgTypeEnum.INPUT_CLICKED_HIT_3D_GAME_OBJECT );
            return lst;
        }

        public void receivedMessage( ObserverMsgTypeEnum messageType , object paramsObj , PublisheOptionVo options )
        {
            switch ( messageType )
            {
                case ObserverMsgTypeEnum.INPUT_CLICKED_HIT_3D_GAME_OBJECT:
                {

                    hit = (RaycastHit) paramsObj;
                    nav.SetDestination( hit.point );
                    break;
                }
            }
        }
    }


}




