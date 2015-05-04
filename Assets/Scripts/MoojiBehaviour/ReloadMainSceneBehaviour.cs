
using UnityEngine;
using BehaviourMachine;
using Mooji;
using System.Collections.Generic;


namespace Mooji
{
    public class ReloadMainSceneBehaviour : MonoBehaviour
    {

        public void Awake()
        {
            if ( !isLoadMainScene() )
            {
                OnLevelWasLoaded( 0 );
            }
        }

        public void OnLevelWasLoaded( int level )
        {
            if ( !isLoadMainScene() )
            {
                Application.LoadLevel( "MainScene" );
            }
        }

        /// <summary>
        ///  «∑Òº”‘ÿπ˝MainScene
        /// </summary>
        /// <returns>true Loaded !</returns>
        private bool isLoadMainScene()
        {
            return BMC.getObserverBehaviour() != null;
        }
    }
}

