using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Mooji
{
    public class PlayerInfoBlackBoard : SceneBlackBoard
    {
        /// <summary>
        /// 当前选中的作战单位
        /// </summary>
        private AIBattleUnit _currSelectBattleUnit;

        /// <summary>
        /// 当前点击RayCastHit
        /// </summary>
        public RaycastHit currClickedRaycastHit;


        private GameObject _selectBattleUnitLocaltionMaskGo;


        private GameObject _currSelectedLeadGo;


        private GameObject _clickedFloorAnimGo;
        private GameObject _currSelectLeadAnimGo;

        /// <summary>
        /// 得到当前玩家可以控制的主角
        /// </summary>
        /// <returns></returns>
        public GameObject getCurrleadGo()
        {
            return _currSelectedLeadGo;
        }
        /// <summary>
        /// 设置当前可以操控的玩家
        /// </summary>
        /// <param name="go"></param>
        /// <returns></returns>
        public void setCurrLeadGo( GameObject go )
        {
            _currSelectedLeadGo = go;
        }


        public AIBattleUnit currSelectBattleUnit
        {
            get { return _currSelectBattleUnit; }
            set { this._currSelectBattleUnit = value; }
        }

        public GameObject getSelectBattleUnitLocaltionMaskGo()
        {
            if ( _selectBattleUnitLocaltionMaskGo == null )
            {
                _selectBattleUnitLocaltionMaskGo = BMC.getPrefabsBehaviour().instantiatePrefab( PrefbasAssets.PrefabAssetsConstant.CLICKED_GROUND_MASK , true );
            }

            return _selectBattleUnitLocaltionMaskGo;
        }

        public void setClickedFloorAnimGo( GameObject clickedFloorAnimGo )
        {
            this._clickedFloorAnimGo = clickedFloorAnimGo;
        }
        public GameObject getClickedFloorAnimGo()
        {
            return this._clickedFloorAnimGo;
        }

        public void setCurrSelectLeadAnimGo( GameObject currSelectLeadAnimGo )
        {
            this._currSelectLeadAnimGo = currSelectLeadAnimGo;
        }
        public GameObject getsetCurrSelectLeadAnimGo()
        {
            return this._currSelectLeadAnimGo;
        }
        
    }
}
