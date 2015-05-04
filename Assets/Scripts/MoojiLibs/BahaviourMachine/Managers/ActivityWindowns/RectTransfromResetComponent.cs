using UnityEngine;


namespace Mooji
{
    public class RectTransfromResetComponent : MonoBehaviour
    {
        private char[] _splitArrItem = new char[] { '|' };
        private char[] _splitInfoItem = new char[] { ',' };
        private string[] _infoArr = null;

        public void Start()
        {
        }


        public void reSetRectTransformInfo( string prefabPath )
        {
            if ( _infoArr == null )
            {
                string savedInfo = Resources.Load<TextAsset>( prefabPath ).ToString();
                _infoArr = savedInfo.Split( _splitArrItem );
            }
            doReset();
        }

        //public void reSetRectTransformInfo()
        //{
        //    if ( _infoArr == null )
        //    {
        //        string prefabPath = GameService.getInstance().prefabService.getPrefabPath( this.gameObject.name.Replace( "(Clone)" , "" ) );
        //        string savedInfo = Resources.Load<TextAsset>( prefabPath ).ToString();
        //        _infoArr = savedInfo.Split( _splitArrItem );
        //    }

        //    doReset();
        //}

        private void doReset()
        {
            RectTransform currTransform = this.GetComponent<RectTransform>();

            string[] positionArr = _infoArr[0].Split( _splitInfoItem );
            Vector3 localPosition = new Vector3( float.Parse( positionArr[0] ) , float.Parse( positionArr[1] ) , float.Parse( positionArr[2] ) );

            string[] offsetMinArr = _infoArr[1].Split( _splitInfoItem );
            Vector2 offsetMin = new Vector2( float.Parse( offsetMinArr[0] ) , float.Parse( offsetMinArr[1] ) );

            string[] offsetMaxArr = _infoArr[2].Split( _splitInfoItem );
            Vector2 offsetMax = new Vector2( float.Parse( offsetMaxArr[0] ) , float.Parse( offsetMaxArr[1] ) );

            string[] pivotArr = _infoArr[3].Split( _splitInfoItem );
            Vector2 pivot = new Vector2( float.Parse( pivotArr[0] ) , float.Parse( pivotArr[1] ) );

            string[] anchorMinArr = _infoArr[4].Split( _splitInfoItem );
            Vector2 anchorMin = new Vector2( float.Parse( anchorMinArr[0] ) , float.Parse( anchorMinArr[1] ) );

            string[] anchorMaxArr = _infoArr[5].Split( _splitInfoItem );
            Vector2 anchorMax = new Vector2( float.Parse( anchorMaxArr[0] ) , float.Parse( anchorMaxArr[1] ) );

            string[] localRotationArr = _infoArr[6].Split( _splitInfoItem );
            Quaternion q =  new Quaternion( float.Parse( localRotationArr[0] ) , float.Parse( localRotationArr[1] ) , float.Parse( localRotationArr[2] ) , float.Parse( localRotationArr[3] ) );

            string[] localScaleArr = _infoArr[7].Split( _splitInfoItem );
            Vector2 localScale = new Vector3( float.Parse( localScaleArr[0] ) , float.Parse( localScaleArr[1] ) , float.Parse( localScaleArr[2] ) );


            currTransform.localPosition = localPosition;
            currTransform.offsetMin = offsetMin;
            currTransform.offsetMax = offsetMax;
            currTransform.pivot = pivot;
            currTransform.anchorMin = anchorMin;
            currTransform.anchorMax = anchorMax;
            currTransform.localRotation = q;
            currTransform.localScale = localScale;
        }

    }
}

