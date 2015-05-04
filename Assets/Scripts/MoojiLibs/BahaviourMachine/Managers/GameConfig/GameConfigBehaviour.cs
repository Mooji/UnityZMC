using System;
using System.Collections.Generic;
using System.Reflection;
using System.Xml;
using UnityEngine;
using Mooji;

namespace Mooji
{
    public class GameConfigBehaviour : MonoBehaviour , IMoojiBehaviourManager
    {

        //  ================================    [vars]  ================================
        private EncryptionManager _encryptionManager;
        private GameConfigVo _gameConfigVo;
        private I18NConfigVo _i18nConfigVo;
        private TextAsset[] _gameConfigTxtAssetsArr;

        //  ================================    [vars getter and setter]  ================================

        public GameConfigVo gameConfigVo
        {
            set { this._gameConfigVo = value; }
            get { return _gameConfigVo; }
        }

        public I18NConfigVo i18nConfigVo
        {
            set { this._i18nConfigVo = value; }
            get { return _i18nConfigVo; }
        }

        //  ================================    [MoojiStateBehaviour lc]  ================================
        public void Awake()
        {
            _gameConfigVo = new GameConfigVo();
            _i18nConfigVo = new I18NConfigVo();
            _encryptionManager = this.gameObject.AddComponent<EncryptionManager>();
        }

        public void Start()
        {
            _gameConfigTxtAssetsArr = Resources.LoadAll<TextAsset>( "Configs" );

            for ( int i = 0 ; i < _gameConfigTxtAssetsArr.Length ; i++ )
            {
                TextAsset ta =  _gameConfigTxtAssetsArr[i];

                string txtAssetsName = ta.name;

                if ( txtAssetsName == "GameConfig" )
                {
                    this.parseGameConfigXML( ta );
                }

                _gameConfigTxtAssetsArr[i] = null;
            }

            BMC.registerInitCompleteBehaviourManager( BehaviourManagerType.GAME_CONFIG , this );
            BMC.getObserverBehaviour().publisheMsg( ObserverMsgTypeEnum.BEHAVIOUR_MACHINE_GLOBALBLACKBOARD_FINISHED , this);

        }

        //  ================================    [private methods]  ================================
        /// <summary>
        /// 装载游戏的配置文件
        /// </summary>
        private void parseGameConfigXML( TextAsset ta )
        {
            //byte[] result = QuickLZ.decompress( ta.bytes );
            byte[] result = _encryptionManager.dEncryption( ta.bytes );

            ByteArray myByteArr = new ByteArray();

            try
            {
                myByteArr.WriteByteArr( result , result.Length , 0 );

                int packLen = myByteArr.ReadInt();
                string packHeadStr = myByteArr.ReadString( packLen );

                int xmlStrLen = myByteArr.ReadInt();
                string xmlStr = myByteArr.ReadString( xmlStrLen );

                string[] headArr = packHeadStr.Split( new char[] { ';' } );

                int index = 0;
                foreach ( string item in headArr )
                {
                    if ( string.IsNullOrEmpty( item ) )
                        continue;

                    string[] tempArr = item.Split( new char[] { '|' } );
                    string fileName = tempArr[0];
                    int xmlFileLen = int.Parse( tempArr[1] );
                    string xml = xmlStr.Substring( index , xmlFileLen );
                    index += xmlFileLen;

                    if ( fileName == "GameConfig" )
                    {
                        buildingGameConfigVo( xml );
                    }
                    else if ( _gameConfigVo.isLanguageXmlFile( fileName ) )
                    {
                        parseI18NXml( xml );
                    }
                }
            }
            catch ( Exception e )
            {
                throw e;
            }
            finally
            {
                myByteArr.Close();
            }

        }


        /// <summary>
        /// 解析i18n
        /// </summary>
        private void parseI18NXml( string xmlFile )
        {

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml( xmlFile );

            XmlNodeList refXmlLst = xmlDoc.SelectNodes( "i18n/refs" );
            XmlNode refsXmlList = refXmlLst[0];
            foreach ( XmlNode refNode in refsXmlList.ChildNodes )
            {
                if ( refNode.GetType() == typeof( XmlComment ) )
                    continue;

                RefsVo rv =  new RefsVo();
                rv.id = refNode.Attributes["id"].Value;
                rv.val = refNode.Attributes["val"].Value;
                _i18nConfigVo.buildRefsVo( rv );
            }


            XmlNodeList itemsXmlLst = xmlDoc.SelectNodes( "i18n/items" );
            XmlNode itemsXmlList = itemsXmlLst[0];
            foreach ( XmlNode itemNode in itemsXmlList.ChildNodes )
            {

                //  如果是注释 continue
                if ( itemNode.GetType() == typeof( XmlComment ) )
                    continue;

                I18NItemVo item =  new I18NItemVo();

                item.key = itemNode.Attributes["key"].Value;

                if ( itemNode.Attributes["val"] != null )
                    item.val = itemNode.Attributes["val"].Value;
                else
                    item.val = itemNode.InnerText.Trim();

                //if ( string.IsNullOrEmpty( item.val ) )
                //    throw new Exception( "I18N中 " + item.key + "对应的Val没有填写");

                string refs = itemNode.Attributes["refs"] != null ? itemNode.Attributes["refs"].Value : null;
                if ( !string.IsNullOrEmpty( refs ) )
                    item.refArr = refs.Split( new char[] { ',' } );

                _i18nConfigVo.buildItemVo( item );
            }

        }

        private void buildingGameConfigVo( string xml )
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml( xml );
            XmlNodeList itemList = xmlDoc.SelectNodes( "GameConfigs/GameInfo" );
            this.BuildingGameConfigVo( itemList );
        }

        private void BuildingGameConfigVo( XmlNodeList itemList )
        {
            XmlNode node = itemList[0];
            Type t = typeof( GameConfigVo );
            FieldInfo[] fs = t.GetFields( BindingFlags.NonPublic | BindingFlags.Instance );

            for ( int i = 0 ; i < fs.Length ; i++ )
            {
                FieldInfo field = fs[i] as FieldInfo;
                string resultFieldName = field.Name.Substring( 1 , field.Name.Length - 1 ); // private int _id -- > id ;
                if ( node.Attributes[resultFieldName] != null )
                    field.SetValue( _gameConfigVo , node.Attributes[resultFieldName].Value );
            }
        }
    }
}
