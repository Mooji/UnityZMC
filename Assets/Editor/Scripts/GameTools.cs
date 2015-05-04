using System;
using UnityEditor;
using Mooji;
using System.Reflection;
using System.Xml;
using UnityEditor;
using UnityEngine;
using System.IO;
using System.Collections.Generic;
using System.Text;

namespace Mooji.Editor
{
    public class GameTools : AssetPostprocessor
    {

        /**
         * 构建客户端数据库 parmas[0] orderID params[1] xlsxFilePath  params[2] binFile outPutFilePath--
         */
        public static int ORDER_BUILD_CLIENT_DATEBASE = 100;
        /**
         * 构建客户端数据库 parmas[0] orderID params[1] xlsxFilePath  params[2] C# FILES outPutFilePath--
         */
        public static int ORDER_BUILD_CLIENT_DATEBASE_CSHARP_FILES = 101;

        private static string PREFABS_PREFIX = "Assets\\Resources\\Prefabs";


        public static void OnPostprocessAllAssets( String[] importedAssets , String[] deletedAssets , String[] movedAssets , String[] movedFromAssetPaths )
        {
           

            foreach ( var item in importedAssets )
            {

                if ( item.IndexOf( ".prefab" ) >= 0 )
                {
                    bool genPrefabs = CheckGeneratePrefabs( importedAssets , deletedAssets , movedAssets , movedFromAssetPaths );
                    generatePrefabsFile();
                    break;
                }

                //Debug.Log( "=====================Reimported Asset: ==============================" + item );

                switch ( item )
                {
                    //  主配置文件被修改后
                    case "Assets/Editor/Doc/Configs/GameConfig.xml":
                    {
                        GameConfigScriptable.configChanged();
                        updatePrjs();
                        break;
                    }
                    //case "Assets/Editor/Doc/I18N/ZH_CN.xml":
                    //{
                    //    CompressionConfigs.buildGameConfigsXml();
                    //    break;
                    //}
                    //case "Assets/Editor/Doc/I18N/EN_US.xml":
                    //{
                    //    CompressionConfigs.buildGameConfigsXml();
                    //    break;
                    //}
                    //case "Assets/Editor/Doc/I18N/ZH_HANT.xml":
                    //{
                    //    CompressionConfigs.buildGameConfigsXml();
                    //    break;
                    //}

                    ////  数据库配置文件 jar 生成bin文件
                    //case "Assets/Editor/Doc/DataBase/DataBase.xlsx":
                    //{
                    //    PackageDataBaseService.buildDataBaseByDeveloper();
                    //    break;
                    //}

                    ////  Java构建的bin文件生成成功后。c#重新构建
                    //case "Assets/Editor/Doc/DataBase/DataBase.bytes":
                    //{
                    //    PackageDataBaseService.buildDataBaseByRelease();
                    //    break;
                    //}
                    ////  c#构建的 release DataBase.bytes 生成后（与之前的有改变情况下触发）
                    //case "Assets/Resources/DateBase/DataBase.bytes":
                    //{
                    //    Debug.Log( "Auto building DataBase POJO Class And TableMappingSerivce ..." );
                    //    PackageDataBaseService.buildDataBaseCSharpFiles();
                    //    break;
                    //}
                    ////   PackageDataBaseService.buildDataBaseCSharpFiles(); complete
                    //case "Assets/Editor/Doc/DataBase/allCSharpFilesComplete.txt":
                    //{

                    //    Debug.Log( "Auto building DataBase POJO Structure " );
                    //    GameTools.updatePrjs();

                    //    Debug.Log( "Auto package Database c# files by ==Release== complete !" );

                    //    if ( PackageAll.isPackageAllFlag )
                    //        GameService.getInstance().issueMessageService.issMsg( PackageAll.ORDER_PACKALL_DATABASE_CSHARP_FILES , null );

                    //    break;
                    //}

                }

            }
            //foreach ( var str in deletedAssets )
            //{
            //    Debug.Log( "Deleted Asset: " + str );
            //}

            //for ( var i=0 ; i < movedAssets.Length ; i++ )
            //    Debug.Log( "Moved Asset: " + movedAssets[i] + " from: " + movedFromAssetPaths[i] );
        }

        private static void generatePrefabsFile()
        {
            StringBuilder enums = new StringBuilder();
            StringBuilder enumMaps = new StringBuilder();
            String enter = "\n";

            List<string> prefabNames = new List<string>();
            List<string> allPrefabs = findAllPrefabs(PREFABS_PREFIX);
            string prefix = PREFABS_PREFIX.Substring(PREFABS_PREFIX.LastIndexOf("\\") + 1);
            foreach (string pf in allPrefabs)
            {
                string subPath = pf.Substring(pf.IndexOf(prefix));
                subPath = subPath.Substring(0, subPath.LastIndexOf("."));
                int index = subPath.LastIndexOf("\\");
                string prefabName = subPath.Substring(index + 1).Trim();
                if (prefabName.Contains(" "))
                {
                    throw new Exception("Prefab名称包含了空格, " + subPath);
                }
                if (prefabNames.Contains(prefabName))
                {
                    throw new Exception("Prefab名称重复, " + subPath);
                }
                prefabNames.Add(prefabName);
                string upperName = prefabName.ToUpper();

                enums.Append(upperName).Append(",").Append(enter).Append("\t\t\t");

                //_prefabMapping[PrefabAssetsConstant.UI_EMPTY_CONTAINER] = "Prefabs/UI/Container/EmptyContainer";
                string enumMap = "_prefabMapping[PrefabAssetsConstant." + upperName + "] = \"" + subPath + "\";";
                enumMaps.Append("\t\t\t").Append(enumMap.Replace("\\", "/")).Append(enter);

            }
            string filePath = Application.dataPath + "\\Editor\\Doc\\Template\\PrefabsTemplate.txt";
            FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            StreamReader sr = new StreamReader(fs);
            string con = sr.ReadToEnd();
            con = con.Replace("#enums#", enums.ToString());
            con = con.Replace("#enumMaps#", enumMaps.ToString());
            sr.Close();
            fs.Close();

            filePath = Application.dataPath + "\\Scripts\\MoojiBehaviour\\Constants\\PrefabAssetsConstant.cs";
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
            File.WriteAllText(filePath, con);
        }

        private static List<string> findAllPrefabs(string path)
        {
            List<string> files = new List<string>();
            DirectoryInfo dir = new DirectoryInfo(path);
            foreach (FileInfo file in dir.GetFiles())
            {
                if (file.FullName.EndsWith(".prefab"))
                {
                    files.Add(file.FullName);
                }
            }
            foreach (DirectoryInfo info in dir.GetDirectories())
            {
                List<string> tmps = findAllPrefabs(info.FullName);
                if (tmps.Count > 0)
                {
                    files.AddRange(tmps);
                }
            }

            return files;
        }

        private static bool CheckGeneratePrefabs(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
        {
            foreach (var item in importedAssets)
            {
                if (item.StartsWith(PREFABS_PREFIX))
                {
                    return true;
                }
            }

            foreach (var item in deletedAssets)
            {
                if (item.StartsWith(PREFABS_PREFIX))
                {
                    return true;
                }
            }
            foreach (var item in movedAssets)
            {
                if (item.StartsWith(PREFABS_PREFIX))
                {
                    return true;
                }
            }
            foreach (var item in movedFromAssetPaths)
            {
                if (item.StartsWith(PREFABS_PREFIX))
                {
                    return true;
                }
            }
            return false;
        }

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <returns></returns>
        //public static XmlNode processGameConfigXml()
        //{
        //    configRootPath = Application.dataPath + "/Editor/Doc/";
        //    XmlDocument xmlDoc = new XmlDocument();
        //    xmlDoc.Load( configRootPath + "Configs/GameConfig.xml" );
        //    XmlNodeList itemList = xmlDoc.SelectNodes( "GameConfigs/GameInfo" );
        //    new XmlService().BuildingGameConfigVo( itemList );

        //    //string assetsPath = GameService.getInstance().gameConfig.comment_assetsPath;
        //    //string dataBasePath = GameService.getInstance().gameConfig.comment_dataBasePath;
        //    //string dateBaseOutPutPath = GameService.getInstance().gameConfig.comment_dateBaseOutPutPath;
        //    //string dataBaseOutPutPath_developer = GameService.getInstance().gameConfig.comment_dateBaseOutPutPath_developer;
        //    //string javaPluginPath = GameService.getInstance().gameConfig.comment_javaPluginPath;
        //    //string cSharpFileRootPath = GameService.getInstance().gameConfig.comment_dataBaseCSharpFileRoot;

        //    //GameService.getInstance().gameConfig.comment_dataBasePath = dataBasePath.Replace( "{assetsPath}" , assetsPath );
        //    //GameService.getInstance().gameConfig.comment_dateBaseOutPutPath = dateBaseOutPutPath.Replace( "{assetsPath}" , assetsPath );
        //    //GameService.getInstance().gameConfig.comment_dateBaseOutPutPath_developer = dataBaseOutPutPath_developer.Replace( "{assetsPath}" , assetsPath );
        //    //GameService.getInstance().gameConfig.comment_javaPluginPath = javaPluginPath.Replace( "{assetsPath}" , assetsPath );
        //    //GameService.getInstance().gameConfig.comment_dataBaseCSharpFileRoot = cSharpFileRootPath.Replace( "{assetsPath}" , assetsPath );

        //    return itemList[0];
        //}

        //public static void osOpenFile( string filePath )
        //{
        //    System.Diagnostics.Process p = new System.Diagnostics.Process();
        //    p.StartInfo.FileName = "Explorer";
        //    p.StartInfo.Arguments = filePath;
        //    p.Start();
        //}

        public static void updatePrjs()
        {
            System.Diagnostics.Process p = new System.Diagnostics.Process();
            p.StartInfo.FileName = "cmd.exe";
            p.StartInfo.UseShellExecute = false;    //是否使用操作系统shell启动
            p.StartInfo.RedirectStandardInput = true;//接受来自调用程序的输入信息
            p.StartInfo.RedirectStandardOutput = true;//由调用程序获取输出信息
            p.StartInfo.RedirectStandardError = true;//重定向标准错误输出
            p.StartInfo.CreateNoWindow = false;//不显示程序窗口
            p.Start();//启动程序
            p.StandardInput.WriteLine( "&exit" );
            System.Threading.Thread.Sleep( 500 );
            p.Close();
        }

        public static GameConfigVo createGameConfigVoByXml()
        {
            GameConfigVo vo         = new GameConfigVo();
            string configRootPath   = Application.dataPath + "/Editor/Doc/";
            XmlDocument xmlDoc      = new XmlDocument();

            xmlDoc.Load( configRootPath + "Configs/GameConfig.xml" );
            XmlNodeList itemList = xmlDoc.SelectNodes( "GameConfigs/GameInfo" );

            XmlNode node = itemList[0];
            Type t = typeof( GameConfigVo );

            FieldInfo[] fs = t.GetFields( BindingFlags.NonPublic | BindingFlags.Instance );

            for ( int i = 0 ; i < fs.Length ; i++ )
            {
                FieldInfo field = fs[i] as FieldInfo;
                string resultFieldName = field.Name.Substring( 1 , field.Name.Length - 1 ); // private int _id -- > id ;
                if ( node.Attributes[resultFieldName] != null )
                    field.SetValue( vo , node.Attributes[resultFieldName].Value );
            }

            return vo;
        }

    }
}
