using System;
using System.Text;
using System.Collections.Generic;
using UnityEngine;

namespace Mooji.Editor
{
    /// <summary>
    /// 自动生成不要修改
    /// </summary>
    public class GameConfigsTemplete
    {
        public  string usingStr          = "using System;\nusing UnityEngine;\n";
        public  string nameSpaceStr      = "namespace Mooji{\n\t{classInfo}\n}";
        public  string clazzName         = "public class GameConfigVo{\n\t\t{contrentInfo}\n}";
        public  string fieldsTemplete    = "private String {privateFieldName};\npublic String {publicFieldName}{ get { return this.getVal({privateFieldName}); }set { {privateFieldName} = value; }}\n\n";
        public  string method_1 = "public bool isDeveloperMode(){return this.developerMode == \"true\";}\n";
        public  string method_2 = "public bool isLanguageXmlFile( string fileName ){return this.supportLanguages.IndexOf( fileName ) >= 0;}\n";
        public  string method_3 = "private string getVal( string val ){ string resultVal = val; resultVal = resultVal.Replace( \"{assetsPath}\" , Application.dataPath ); resultVal = resultVal.Replace( \"\\r\" , \"\" ).Replace( \"\\n\" , \"\" ).Replace( \"\\t\" , \"\" ).Trim();return resultVal;}\n";
        private string[] getMehtodsArr()
        {
            return new string[] { method_1 , method_2 , method_3 };
        }

        public StringBuilder build( List<string> filedsArr )
        {
            StringBuilder content = new StringBuilder();
            StringBuilder clazzSB = new StringBuilder();
            StringBuilder sb = new StringBuilder();

            sb.Append( usingStr );

            //////////////////////////// content ////////////////////////////
            string preFieldName = null;
            //  字段
            foreach ( string item in filedsArr )
            {
                if(preFieldName != null)
                {
                    fieldsTemplete = fieldsTemplete.Replace( preFieldName , item ).Replace( preFieldName , "_" + item );
                }
                else
                {
                    fieldsTemplete = fieldsTemplete.Replace( "{publicFieldName}" , item ).Replace( "{privateFieldName}" , "_" + item );
                }
                content.Append( fieldsTemplete ).Append( "\n" );
                preFieldName = item;
            }
            //  methods
            string[] mArr =  getMehtodsArr();
            for ( int i = 0 ; i < mArr.Length ; i++ )
            {
                content.Append( mArr[i] ).Append( "\n" );
            }
            //////////////////////////// content end ////////////////////////////

            clazzSB.Append( clazzName.Replace( "{contrentInfo}" , content.ToString() ) );

            sb.Append( nameSpaceStr.Replace( "{classInfo}" , clazzSB.ToString() ) );

            return sb;
        }

        //public bool isDeveloperMode() { return this.developerMode == "true"; }

        //public bool isLanguageXmlFile( string fileName ) { return this.supportLanguages.IndexOf( fileName ) >= 0; }

        //private string getVal( string val )
        //{
        //    string resultVal = val;
        //    resultVal = resultVal.Replace( "{assetsPath}" , Application.dataPath );
        //    resultVal = resultVal.Replace( "\r" , "" ).Replace( "\n" , "" ).Replace( "\t" , "" ).Trim(); return resultVal;
        //}

    }
}
