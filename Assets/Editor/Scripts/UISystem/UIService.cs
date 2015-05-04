using System;
using System.IO;
using UnityEditor;
using UnityEngine;
using System.Text;

namespace Mooji.Editor
{
    public class UIService : ScriptableWizard
    {
        [MenuItem( "MoojiTools/Save RectTransform info" , false , 3 )]
        public static void saveRectTransforminfo()
        {
           
            GameObject[] gos = Selection.gameObjects;

            foreach ( GameObject go in gos )
            {
                string path =  AssetDatabase.GetAssetPath( PrefabUtility.GetPrefabObject( go ) );
                path = path.Replace( ".prefab" , ".txt" ).Replace( "Assets" , "" );
                StringBuilder sb = new StringBuilder();
                RectTransform prefabGameObjRT =  Selection.activeGameObject.GetComponent<RectTransform>();
                sb.Append( prefabGameObjRT.localPosition.ToString() ).Append( "|" );
                sb.Append( prefabGameObjRT.offsetMin.ToString() ).Append( "|" );// left,bottom
                sb.Append( prefabGameObjRT.offsetMax.ToString() ).Append( "|" ); // right,top
                sb.Append( prefabGameObjRT.pivot.ToString() ).Append( "|" );
                sb.Append( prefabGameObjRT.anchorMin.ToString() ).Append( "|" );
                sb.Append( prefabGameObjRT.anchorMax.ToString() ).Append( "|" );
                sb.Append( prefabGameObjRT.localRotation.ToString() ).Append( "|" );
                sb.Append( prefabGameObjRT.localScale.ToString() );

                sb = sb.Replace( "(" , "" ).Replace( ")" , "" );

                //  写入Resources中
                File.WriteAllText( Application.dataPath + "/" + path , sb.ToString() );
            }

            GameTools.updatePrjs();

        }
    }


}
