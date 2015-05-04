using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Xml;
using UnityEditor;
using UnityEngine;
using System.Security.Cryptography;
using Mooji;



namespace Mooji.Editor
{
    public class GameConfigScriptable : ScriptableWizard
    {
        private static Dictionary<string,StringBuilder> xmlMapping;
        private static int i18nFileCount = 0;
        private static List<XmlNode> removeXmlNodeLst;
        private static XmlNodeList itemList;

        internal static void configChanged()
        {

            xmlMapping = new Dictionary<string , StringBuilder>();
            i18nFileCount = 0;
            removeXmlNodeLst = new List<XmlNode>();

            buildGameConfigCSharpFiles();

        }

        [MenuItem( "MoojiTools/GameConfig Test/Build GameConfig.cs (Don't try)" )]
        public static void buildGameConfigCSharpFiles()
        {
            //  1. 将Gameconfig.xml 转换成 GameConfigVo.cs
            doBuildGameConfigCSharpFiles();

            //  2. 将GameConfig.xml 压缩成Bytes
            convertionXmlToBytes();

            //  3. 自动生成Prefabs的地址映射c#文件

            GameConfigVo vo = GameTools.createGameConfigVoByXml();

            //  4. 添加Tags
            addTags( vo );

            //  5. 添加Layers
            addLayers( vo );
        }

        private static void addLayers( GameConfigVo vo )
        {
            //SerializedObject tagManager = new SerializedObject( AssetDatabase.LoadAllAssetsAtPath( "ProjectSettings/TagManager.asset" )[0] );
            //SerializedProperty it = tagManager.GetIterator();
            //SerializedProperty tagit = null;

            //List<string> layersLst = new List<string>();
            //layersLst.Add( GameService.getInstance().gameConfig.layer_unit_movement );
            //layersLst.Add( GameService.getInstance().gameConfig.layer_unit_attack );



            //int startLayerIndex = 8 ;
            //int i = 0;
            //while ( it.NextVisible( true ) )
            //{
            //    if ( i >= layersLst.Count || startLayerIndex >= 31 )
            //        break;

            //    if ( it.name == "User Layer " + startLayerIndex )
            //    {
            //        if ( it.type == "string" )
            //            it.stringValue = layersLst[i];

            //        startLayerIndex += 1;
            //        i += 1;
            //    }
            //}
            ////Builtin Layer 1
            ////return;

            //////  unity3d 5.0 version 
            ////while ( it.NextVisible( true ) )
            ////{
            ////    if ( it.name == "layers" )
            ////    {
            ////        tagit = it;
            ////        break;
            ////    }
            ////}

            ////int i = 0;
            ////int startIndex = 0;
            ////while ( tagit.NextVisible( true ) )
            ////{
            ////    if ( startIndex >= 9 ) // layers + size = 8 + 1
            ////    {
            ////        if ( i < layersLst.Count )
            ////        {
            ////            if ( tagit.type == "string" )
            ////                tagit.stringValue = layersLst[i];
            ////        }
            ////        else
            ////        {
            ////            if ( tagit.type == "string" )
            ////                tagit.stringValue = "";
            ////        }

            ////        i += 1;
            ////    }

            ////    startIndex += 1;

            ////}

            //tagManager.ApplyModifiedProperties();

        }

        private static void addTags( GameConfigVo vo )
        {

            SerializedObject tagManager = new SerializedObject( AssetDatabase.LoadAllAssetsAtPath( "ProjectSettings/TagManager.asset" )[0] );
            SerializedProperty it = tagManager.GetIterator();
            SerializedProperty tagit = null;

            string[] tagsLst = vo.tags.Split( new char[] { ',' } );

            //  删除所有Tag
            while ( it.NextVisible( true ) )
            {
                if ( it.name == "tags" )
                {
                    tagit = it;
                    break;
                }
            }

            tagit.ClearArray();
            tagit.arraySize = tagsLst.Length + 1;

            for ( int i = 0 ; i < tagit.arraySize - 1 ; i++ )
            {
                SerializedProperty dataPoint = tagit.GetArrayElementAtIndex( i );
                if ( string.IsNullOrEmpty( dataPoint.stringValue ) )
                {
                    string tagVal =  tagsLst[i];
                    tagVal = tagVal.Replace( "\t" , "" ).Trim() ;
                    dataPoint.stringValue = tagVal;
                }
            }
            tagManager.ApplyModifiedProperties();


            //  更新cs文件
            String enter = "\n";
            StringBuilder sb = new StringBuilder();
            sb.Append( "\t/// <summary>" ).Append( enter );
            sb.Append( "\t/// 自动生成不要修改" ).Append( enter );
            sb.Append( "\t/// </summary>" ).Append( enter );
            sb.Append( "public class GameTagsConstent{" ).Append( enter );

            foreach ( string tags in tagsLst )
            {
                string tagsVal = tags.Replace( "\t" , "" ).Trim();
                sb.Append( " public const string  " + tags + " = \"" + tagsVal + "\";" ).Append( enter );
            }
            sb.Append( "}" ).Append( enter );

            File.WriteAllText( vo.comment_gametags_filePath.Replace( "{assetsPath}" , Application.dataPath ) , sb.ToString() );

        }

        private static void doBuildGameConfigCSharpFiles()
        {
            String configRootPath = Application.dataPath + "/Editor/Doc/";
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load( configRootPath + "Configs/GameConfig.xml" );
            itemList = xmlDoc.SelectNodes( "GameConfigs/GameInfo" );

            String gameConfigCSharpFilePath = null;

            List<String> attrLst = new List<String>();
            XmlAttributeCollection attrCollention = itemList[0].Attributes;
            foreach ( XmlAttribute attr in attrCollention )
            {
                if ( attr.Name == "comment_gameConfigCSharpFilePath" )
                {
                    gameConfigCSharpFilePath = attr.Value;
                }

                attrLst.Add( attr.Name );
            }

            StringBuilder sb =  new GameConfigsTemplete().build( attrLst );
            File.WriteAllText( gameConfigCSharpFilePath.Replace( "{assetsPath}" , Application.dataPath ) , sb.ToString() );
        }

        private static void convertionXmlToBytes()
        {
            GameConfigVo vo = GameTools.createGameConfigVoByXml();

            string[] resultInfoArr = vo.packageArrRef.Split( new char[] { ':' } );
            CompreResultVo cr = new CompreResultVo();
            cr.filePath = resultInfoArr[0];
            cr.childFileArr = resultInfoArr[1].Split( new char[] { ',' } );
            processCompreXmlVo( cr );
            processCompreResultVo( cr );

            removeXmlNodeLst.RemoveAll( delegate( XmlNode node ) { return true; } );
        }
        private static void processCompreResultVo( CompreResultVo cr )
        {
            StringBuilder packHead = new StringBuilder();
            StringBuilder packConstant = new StringBuilder();
            foreach ( var item in xmlMapping.Keys )
            {
                StringBuilder sb = xmlMapping[item];
                packHead.Append( item ).Append( "|" ).Append( sb.ToString().Length ).Append( ";" );
                packConstant.Append( sb.ToString() );
            }

            ByteArray myByteArr = new ByteArray();
            myByteArr.Seek( 0 , SeekOrigin.Begin );
            myByteArr.WriteString( packHead.ToString() );
            myByteArr.WriteString( packConstant.ToString() );


            myByteArr.Seek( 0 , SeekOrigin.Begin );
            byte[] resultArr = new byte[myByteArr.Length];
            myByteArr.Read( resultArr , 0 , (int) myByteArr.Length );
            myByteArr.Close();


            //  压缩 后 加密

            EncryptionManager encryptionManager = new EncryptionManager();
            encryptionManager.Awake();
            byte[] encryptionAndComressByteArr =  encryptionManager.encryption( resultArr );

            //  写入Resources中
            File.WriteAllBytes( Application.dataPath + "/" + cr.filePath.Replace( "\r\n" , "" ).Trim() , encryptionAndComressByteArr );

        }

        private static void processCompreXmlVo( CompreResultVo cr )
        {
            foreach ( string path in cr.childFileArr )
            {

                string currFilePath = path.Replace( "\r\n" , "" ).Trim();

                //  将文件读取出来了
                FileStream fs = File.OpenRead( Application.dataPath + currFilePath );
                //  写入到byte[]中
                byte[] resultByteArr = new byte[fs.Length];
                fs.Read( resultByteArr , 0 , (int) fs.Length );
                fs.Close();

                //  文件中的字符串保存在sb中,然后将文件名作为key ， 文件内容作为 val
                string[] s = currFilePath.Split( new char[] { '/' } );
                string fileName = s[s.Length - 1].Split( new char[] { '.' } )[0];

                if ( xmlMapping.ContainsKey( fileName ) )
                    throw new Exception( fileName + " 重复打包了！" );

                if ( currFilePath.IndexOf( "I18N" ) >= 0 )
                    i18nFileCount += 1;

                if ( i18nFileCount > 1 )
                    throw new Exception( "I18N配置只能有一种！" );

                string xmlStr = Encoding.UTF8.GetString( resultByteArr );
                xmlStr = clearXml( fileName , xmlStr );
                xmlMapping[fileName] = new StringBuilder( xmlStr );
            }
        }
        private static string clearXml( string fileName , string xml )
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml( xml );
            XmlNode tempXmlNode = xmlDoc.FirstChild;

            doClearXml( tempXmlNode );

            foreach ( var item in removeXmlNodeLst )
            {
                item.ParentNode.RemoveChild( item );
            }
            removeXmlNodeLst.RemoveAll( delegate( XmlNode node ) { return true; } );

            string resultXmlStr = xmlDoc.InnerXml;
            return resultXmlStr;
        }
        private static void doClearXml( XmlNode tempXmlNode )
        {
            if ( tempXmlNode == null )
                return;

            //  如果是注释添加到删除列表后，往下找
            if ( tempXmlNode.GetType() == typeof( XmlComment ) )
            {
                //  添加到删除列表
                if ( removeXmlNodeLst.IndexOf( tempXmlNode ) == -1 )
                {
                    removeXmlNodeLst.Add( tempXmlNode );
                    doClearXml( tempXmlNode.NextSibling );
                }

            }
            //  comment_ 开头字段不打不包进去
            else if ( tempXmlNode.Name.IndexOf( "comment_" ) >= 0 )
            {
                if ( removeXmlNodeLst.IndexOf( tempXmlNode ) == -1 )
                {
                    removeXmlNodeLst.Add( tempXmlNode );
                    doClearXml( tempXmlNode.NextSibling );
                }
            }
            //  test 节点不打包
            else if ( tempXmlNode.Name == "test" )
            {
                if ( removeXmlNodeLst.IndexOf( tempXmlNode ) == -1 )
                {
                    removeXmlNodeLst.Add( tempXmlNode );
                    doClearXml( tempXmlNode.NextSibling );
                }

            }
            else
            {
                //comment attribute 移除
                if ( tempXmlNode.Attributes != null && tempXmlNode.Attributes.Count > 0 )
                {
                    XmlAttribute xa =  tempXmlNode.Attributes["comment"];
                    if ( xa != null )
                    {
                        tempXmlNode.Attributes.Remove( xa );
                    }
                }

                //  遍历子节点
                XmlNodeList tempXmlLst = tempXmlNode.ChildNodes;
                foreach ( XmlNode node in tempXmlLst )
                {
                    doClearXml( node );
                }

                doClearXml( tempXmlNode.NextSibling );
            }

        }


    }
}

