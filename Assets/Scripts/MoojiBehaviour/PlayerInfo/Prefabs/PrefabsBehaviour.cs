using System.Collections.Generic;
using UnityEngine;

namespace Mooji
{
    /// <summary>
    /// 连接数据库
    /// </summary>
    public class PrefabsBehaviour : MonoBehaviour , IMoojiBehaviourManager
    {
        /// <summary>
        /// prefad 自动生成类
        /// </summary>
        private PrefbasAssets _prefabAssets;
        /// <summary>
        /// enum 和 prefab path 的 mapping
        /// </summary>
        private Dictionary<PrefbasAssets.PrefabAssetsConstant , string> _prefabMapping = null;
        /// <summary>
        /// prefabClazz go 缓存mapping
        /// </summary>
        private Dictionary<PrefbasAssets.PrefabAssetsConstant,GameObject> _cachedGameObjByPrefabClazzMapping;


        public void Awake()
        {
            _prefabAssets = new PrefbasAssets();
            _prefabMapping = _prefabAssets.getPrefabMapping();

            _cachedGameObjByPrefabClazzMapping = new Dictionary<PrefbasAssets.PrefabAssetsConstant , GameObject>();
        }

        public void Start()
        {
            BMC.registerInitCompleteBehaviourManager( BehaviourManagerType.ASSETS_PREFAB , this );
            BMC.getObserverBehaviour().publisheMsg( ObserverMsgTypeEnum.BEHAVIOUR_MACHINE_GLOBALBLACKBOARD_FINISHED , this );

        }

        //  ==============================================================
        public GameObject instantiatePrefab( PrefbasAssets.PrefabAssetsConstant prefabAssetsConstant , bool isCachePrefabObj )
        {
            GameObject tempGo = null;

            //  先从缓存池中拿 一般窗体的 prefabGameobj 都是不缓存的，获取后，内存直接管理 instane 不需要重复实例化
            tempGo = getGoFromCachePool( prefabAssetsConstant );

            //  没有缓存过
            if ( tempGo == null )
            {
                //  配置文件中是否有这个路径
                if ( _prefabMapping.ContainsKey( prefabAssetsConstant ) )
                {
                    string prefabPath = _prefabMapping[prefabAssetsConstant];

                    //  加载prefabGameobj
                    tempGo = Resources.Load<GameObject>( prefabPath );

                    if ( tempGo == null )
                    {
                        throw new System.Exception( "prefabPath = " + prefabPath + ", is null!" );
                    }

                    //  缓存
                    if ( isCachePrefabObj )
                    {
                        _cachedGameObjByPrefabClazzMapping[prefabAssetsConstant] = tempGo;
                    }
                }
            }

            return tempGo == null ? null : GameObject.Instantiate( tempGo ) as GameObject;
        }


        public string getPrefabPath( PrefbasAssets.PrefabAssetsConstant prefabAssetsConstant )
        {
            return _prefabMapping[prefabAssetsConstant];
        }

        private GameObject getGoFromCachePool( PrefbasAssets.PrefabAssetsConstant prefabAssetsConstant )
        {
            if ( _cachedGameObjByPrefabClazzMapping.ContainsKey( prefabAssetsConstant ) )
            {
                return _cachedGameObjByPrefabClazzMapping[prefabAssetsConstant];
            }
            return null;
        }



    }
}