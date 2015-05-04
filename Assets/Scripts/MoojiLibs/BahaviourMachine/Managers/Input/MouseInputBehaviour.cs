using UnityEngine;
using UnityEngine.EventSystems;

namespace Mooji
{
    public class MouseInputBehaviour : MonoBehaviour , IMoojiBehaviourManager
    {

        private int             _playerMovementLayerMask;
        private EventSystem     _uiEventSystem = null;


        public void Awake()
        {
            _uiEventSystem = GameObject.FindGameObjectWithTag( GameTagsConstent.tag_eventSystem ).GetComponent<EventSystem>();
        }

        public void Start()
        {
            BMC.registerInitCompleteBehaviourManager( BehaviourManagerType.INPUT_MOUSE , this );
            BMC.getObserverBehaviour().publisheMsg( ObserverMsgTypeEnum.BEHAVIOUR_MACHINE_GLOBALBLACKBOARD_FINISHED , this );

        }

        public void Update()
        {

            //  点击左键 
            if ( UnityEngine.Input.GetButtonDown( "Fire1" ) )
            {
                if ( _uiEventSystem.currentSelectedGameObject != null && _uiEventSystem.IsPointerOverGameObject() )
                {
                    string selectGoName = _uiEventSystem.currentSelectedGameObject.name;
                    _uiEventSystem.SetSelectedGameObject( null );
                    BMC.getObserverBehaviour().publisheMsgByCoroutine( ObserverMsgTypeEnum.INPUT_CLICKED_HIT_2D_GAME_OBJECT , null );
                }
                else
                {

                    BMC.getObserverBehaviour().publisheMsgByCoroutine( ObserverMsgTypeEnum.INPUT_CLICKED_HIT_3D_GAME_OBJECT , null );
                }
            }
        }
    }
}
