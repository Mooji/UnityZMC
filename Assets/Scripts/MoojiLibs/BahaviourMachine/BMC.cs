
using UnityEngine;
using BehaviourMachine;
using Mooji;
using System.Collections.Generic;
/// <summary>
/// BMC == BehaviourManagermentCenter
/// </summary>
public class BMC : MonoBehaviour
{

    public static string moojiNameSpaceName             = "Mooji.";
    public string ObserverBehaviour                     = moojiNameSpaceName + typeof( ObserverBehaviour ).Name;
    public string MouseInputBehaviour                   = moojiNameSpaceName + typeof( MouseInputBehaviour ).Name;
    public string PrefabsBehaviour                      = moojiNameSpaceName + typeof( PrefabsBehaviour ).Name;
    public string GameServerBehaviour                   = moojiNameSpaceName + typeof( GameServerBehaviour ).Name;
    public string GameConfigBehaviour                   = moojiNameSpaceName + typeof( GameConfigBehaviour ).Name;
    public string ActivityUIBehaviour                   = moojiNameSpaceName + typeof( ActivityUIBehaviour ).Name;
    public string UtilityDataCenter                     = moojiNameSpaceName + typeof( UtilityDataCenter ).Name;
    public string PlayerMovementBehaviour               = moojiNameSpaceName + typeof( PlayerMovementBehaviour ).Name;
    public string ConvertFsmEventToObserverBehaviour    = moojiNameSpaceName + typeof( ConvertFsmEventToObserverBehaviour ).Name;

    private static Dictionary<BehaviourManagerType , IMoojiBehaviourManager> _initCompleteBehaviourMapping = new Dictionary<BehaviourManagerType , IMoojiBehaviourManager>();


    public static void registerInitCompleteBehaviourManager( BehaviourManagerType type , IMoojiBehaviourManager instance )
    {
        _initCompleteBehaviourMapping[type] = instance;
    }
    public static ObserverBehaviour getObserverBehaviour()
    {
        return getBehaviourManager( BehaviourManagerType.OB_SERVER ) as ObserverBehaviour;
    }
    public static GameConfigBehaviour getGameConfigBehaviour()
    {
        return getBehaviourManager( BehaviourManagerType.GAME_CONFIG ) as GameConfigBehaviour;
    }

    public static MouseInputBehaviour getMouseInputBehaviour()
    {
        return getBehaviourManager( BehaviourManagerType.INPUT_MOUSE ) as MouseInputBehaviour;
    }

    public static PrefabsBehaviour getPrefabsBehaviour()
    {
        return getBehaviourManager( BehaviourManagerType.ASSETS_PREFAB ) as PrefabsBehaviour;
    }
    public static GameServerBehaviour getGameServerBehaviour()
    {
        return getBehaviourManager( BehaviourManagerType.GAME_SERVER ) as GameServerBehaviour;
    }
    public static ActivityUIBehaviour getActivityUIBehaviour()
    {
        return getBehaviourManager( BehaviourManagerType.UI ) as ActivityUIBehaviour;
    }

    public static PlayerMovementBehaviour getPlayerMovementBehaviour()
    {
        return getBehaviourManager( BehaviourManagerType.PLAYER_MOVEMENT ) as PlayerMovementBehaviour;
    }

    private static IMoojiBehaviourManager getBehaviourManager( BehaviourManagerType type )
    {

        if ( _initCompleteBehaviourMapping.ContainsKey( type ) )
            return _initCompleteBehaviourMapping[type];
        else
            return null;
    }


    public void OnLevelWasLoaded( int level )
    {
        this.gameObject.transform.SetSiblingIndex( 0 );
    }





}
