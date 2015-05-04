using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Mooji;

namespace Mooji
{
    /// <summary>
    ///  UI 层 窗体 等 管理类 
    /// </summary>
    public class ActivityUIBehaviour : MonoBehaviour , ISubscriber
    {

        private GameObject uiCanvas;

        private Dictionary< UIConstant.UILayerType , GameObject> _layerContainerMapping;

        public void Awake()
        {
            _layerContainerMapping = new Dictionary<UIConstant.UILayerType , GameObject>();
            uiCanvas = GameObject.FindGameObjectWithTag( GameTagsConstent.tag_mainCanvas );
        }

        public void OnValidate()
        {
            if ( null == uiCanvas )
                throw new Exception( "ActivityService:initService uiCanvas is null!" );

        }

        public void Start()
        {

            BMC.getObserverBehaviour().registerMsg( this );

            RectTransform canvasRt = uiCanvas.GetComponent<RectTransform>();

            Array uiLayerEnumArr = Enum.GetValues( typeof( UIConstant.UILayerType ) ); // ? 这个类 需要自动生成
            PrefbasAssets.PrefabAssetsConstant uiEmptyContainer = PrefbasAssets.PrefabAssetsConstant.UI_EMPTY_CONTAINER;

            foreach ( UIConstant.UILayerType item in uiLayerEnumArr )
            {
                GameObject windowContainer = BMC.getPrefabsBehaviour().instantiatePrefab( uiEmptyContainer , true );

                RectTransform windowRectTransfrom = windowContainer.GetComponent<RectTransform>();
                windowRectTransfrom.name = item.ToString();
                windowRectTransfrom.SetParent( canvasRt );

                string emptyContainerPrefabPath = BMC.getPrefabsBehaviour().getPrefabPath( uiEmptyContainer );
                windowContainer.AddComponent<RectTransfromResetComponent>().reSetRectTransformInfo( emptyContainerPrefabPath );

                _layerContainerMapping[item] = windowContainer;

            }

        }


        public List<ObserverMsgTypeEnum> subscriberMessages()
        {
            return null;
        }

        public void receivedMessage( ObserverMsgTypeEnum messageType , object paramsObj , PublisheOptionVo options )
        {
        }
    }
}
