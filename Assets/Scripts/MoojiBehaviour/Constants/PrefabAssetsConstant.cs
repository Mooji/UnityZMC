using System.Collections.Generic;

namespace Mooji
{

    public class PrefbasAssets
    {

        public enum PrefabAssetsConstant
        {
CLICKED_GROUND_MASK,
			PLANE,
			EMPTY_PANEL,
			UI_EMPTY_CONTAINER,
			
        }


        private Dictionary<PrefabAssetsConstant,string> _prefabMapping;
        public PrefbasAssets()
        {
            _prefabMapping = new Dictionary<PrefabAssetsConstant , string>();
			_prefabMapping[PrefabAssetsConstant.CLICKED_GROUND_MASK] = "Prefabs/Clicked_Ground_Mask";
			_prefabMapping[PrefabAssetsConstant.PLANE] = "Prefabs/Plane";
			_prefabMapping[PrefabAssetsConstant.EMPTY_PANEL] = "Prefabs/Demo/empty_panel";
			_prefabMapping[PrefabAssetsConstant.UI_EMPTY_CONTAINER] = "Prefabs/UI/Container/ui_empty_container";

        }

        public Dictionary<PrefabAssetsConstant,string> getPrefabMapping()
        {
            return this._prefabMapping;
        }
    }

}

