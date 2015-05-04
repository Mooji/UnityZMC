using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEditor;
using System.IO;
using Mooji;

namespace Mooji.Editor
{
    public class OpenFileScriptable : ScriptableObject
    {
        [MenuItem( "MoojiTools/Open GameConfig.xml" , false , 2 )]
        public static void openGameConfig()
        {
            GameConfigVo vo = GameTools.createGameConfigVoByXml();
            System.Diagnostics.Process p = new System.Diagnostics.Process();
            p.StartInfo.FileName = vo.comment_textEditorEXE;
            p.StartInfo.Arguments = vo.comment_gameConfigXmlFilePath;
            p.Start();
        }
        [MenuItem( "MoojiTools/Open ZH_CN.xml" , false , 2 )]
        public static void openZH_CN()
        {
            openFile( Application.dataPath + "/Editor/Doc/I18N/ZH_CN.xml" );
        }
        [MenuItem( "MoojiTools/Open EN_US.xml" , false , 2 )]
        public static void openEN_US()
        {
            openFile( Application.dataPath + "/Editor/Doc/I18N/EN_US.xml" );
        }
        [MenuItem( "MoojiTools/Open ZH_HANT.xml" , false , 2 )]
        public static void openZH_HANT()
        {
            openFile( Application.dataPath + "/Editor/Doc/I18N/ZH_HANT.xml" );
        }

        public static void openFile( string path )
        {
            GameConfigVo vo = GameTools.createGameConfigVoByXml();
            string editFile = vo.comment_textEditorEXE;

            if ( File.Exists( editFile ) )
            {
                System.Diagnostics.Process p = new System.Diagnostics.Process();
                p.StartInfo.FileName = vo.comment_textEditorEXE;
                p.StartInfo.Arguments = path;
                p.Start();
            }
            else
            {
                throw new Exception( editFile + " not exists ! input comment_textEditorEXE val --> gameConfig.xml" );
            }

        }



    }
}
