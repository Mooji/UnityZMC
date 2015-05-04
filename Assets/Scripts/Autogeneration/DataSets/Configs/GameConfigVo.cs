using System;
using UnityEngine;
namespace Mooji{
	public class GameConfigVo{
		private String _developerMode;
public String developerMode{ get { return this.getVal(_developerMode); }set { _developerMode = value; }}


private String _traceLogMode;
public String traceLogMode{ get { return this.getVal(_traceLogMode); }set { _traceLogMode = value; }}


private String _supportLanguages;
public String supportLanguages{ get { return this.getVal(_supportLanguages); }set { _supportLanguages = value; }}


private String _serverNum;
public String serverNum{ get { return this.getVal(_serverNum); }set { _serverNum = value; }}


private String _serverIp;
public String serverIp{ get { return this.getVal(_serverIp); }set { _serverIp = value; }}


private String _serverProt;
public String serverProt{ get { return this.getVal(_serverProt); }set { _serverProt = value; }}


private String _serverPlatfrom;
public String serverPlatfrom{ get { return this.getVal(_serverPlatfrom); }set { _serverPlatfrom = value; }}


private String _mainSceneName;
public String mainSceneName{ get { return this.getVal(_mainSceneName); }set { _mainSceneName = value; }}


private String _tags;
public String tags{ get { return this.getVal(_tags); }set { _tags = value; }}


private String _packageArrRef;
public String packageArrRef{ get { return this.getVal(_packageArrRef); }set { _packageArrRef = value; }}


private String _playerMovementFloor;
public String playerMovementFloor{ get { return this.getVal(_playerMovementFloor); }set { _playerMovementFloor = value; }}


private String _goName_levelExtent;
public String goName_levelExtent{ get { return this.getVal(_goName_levelExtent); }set { _goName_levelExtent = value; }}


private String _ag_uiWindowPrefabMapping;
public String ag_uiWindowPrefabMapping{ get { return this.getVal(_ag_uiWindowPrefabMapping); }set { _ag_uiWindowPrefabMapping = value; }}


private String _maxActivedWindwosCount;
public String maxActivedWindwosCount{ get { return this.getVal(_maxActivedWindwosCount); }set { _maxActivedWindwosCount = value; }}


private String _gameLayer_unit_movement;
public String gameLayer_unit_movement{ get { return this.getVal(_gameLayer_unit_movement); }set { _gameLayer_unit_movement = value; }}


private String _gameLayer_unit_attack;
public String gameLayer_unit_attack{ get { return this.getVal(_gameLayer_unit_attack); }set { _gameLayer_unit_attack = value; }}


private String _gameLayer_camera;
public String gameLayer_camera{ get { return this.getVal(_gameLayer_camera); }set { _gameLayer_camera = value; }}


private String _gameLayer_clickInput;
public String gameLayer_clickInput{ get { return this.getVal(_gameLayer_clickInput); }set { _gameLayer_clickInput = value; }}


private String _comment_gametags_filePath;
public String comment_gametags_filePath{ get { return this.getVal(_comment_gametags_filePath); }set { _comment_gametags_filePath = value; }}


private String _unitySceneArr;
public String unitySceneArr{ get { return this.getVal(_unitySceneArr); }set { _unitySceneArr = value; }}


private String _publishedFileName;
public String publishedFileName{ get { return this.getVal(_publishedFileName); }set { _publishedFileName = value; }}


private String _comment_gameConfigXmlFilePath;
public String comment_gameConfigXmlFilePath{ get { return this.getVal(_comment_gameConfigXmlFilePath); }set { _comment_gameConfigXmlFilePath = value; }}


private String _comment_gameConfigBytesFilePath;
public String comment_gameConfigBytesFilePath{ get { return this.getVal(_comment_gameConfigBytesFilePath); }set { _comment_gameConfigBytesFilePath = value; }}


private String _comment_gameConfigCSharpFilePath;
public String comment_gameConfigCSharpFilePath{ get { return this.getVal(_comment_gameConfigCSharpFilePath); }set { _comment_gameConfigCSharpFilePath = value; }}


private String _comment_dataBasePath;
public String comment_dataBasePath{ get { return this.getVal(_comment_dataBasePath); }set { _comment_dataBasePath = value; }}


private String _comment_dateBaseOutPutPath_developer;
public String comment_dateBaseOutPutPath_developer{ get { return this.getVal(_comment_dateBaseOutPutPath_developer); }set { _comment_dateBaseOutPutPath_developer = value; }}


private String _comment_allCSharpFilesComplete;
public String comment_allCSharpFilesComplete{ get { return this.getVal(_comment_allCSharpFilesComplete); }set { _comment_allCSharpFilesComplete = value; }}


private String _comment_dataBaseCSharpFileRoot;
public String comment_dataBaseCSharpFileRoot{ get { return this.getVal(_comment_dataBaseCSharpFileRoot); }set { _comment_dataBaseCSharpFileRoot = value; }}


private String _comment_dataBaseTableMappingPath;
public String comment_dataBaseTableMappingPath{ get { return this.getVal(_comment_dataBaseTableMappingPath); }set { _comment_dataBaseTableMappingPath = value; }}


private String _comment_dateBaseOutPutPath;
public String comment_dateBaseOutPutPath{ get { return this.getVal(_comment_dateBaseOutPutPath); }set { _comment_dateBaseOutPutPath = value; }}


private String _comment_javaPluginPath;
public String comment_javaPluginPath{ get { return this.getVal(_comment_javaPluginPath); }set { _comment_javaPluginPath = value; }}


private String _comment_publishedRoot;
public String comment_publishedRoot{ get { return this.getVal(_comment_publishedRoot); }set { _comment_publishedRoot = value; }}


private String _comment_textEditorEXE;
public String comment_textEditorEXE{ get { return this.getVal(_comment_textEditorEXE); }set { _comment_textEditorEXE = value; }}


private String _comment_showJavaPluginOutputMsg;
public String comment_showJavaPluginOutputMsg{ get { return this.getVal(_comment_showJavaPluginOutputMsg); }set { _comment_showJavaPluginOutputMsg = value; }}


private String _comment_gameServicePath;
public String comment_gameServicePath{ get { return this.getVal(_comment_gameServicePath); }set { _comment_gameServicePath = value; }}


private String _comment_services;
public String comment_services{ get { return this.getVal(_comment_services); }set { _comment_services = value; }}


private String _comment_prefabConstant;
public String comment_prefabConstant{ get { return this.getVal(_comment_prefabConstant); }set { _comment_prefabConstant = value; }}


public bool isDeveloperMode(){return this.developerMode == "true";}

public bool isLanguageXmlFile( string fileName ){return this.supportLanguages.IndexOf( fileName ) >= 0;}

private string getVal( string val ){ string resultVal = val; resultVal = resultVal.Replace( "{assetsPath}" , Application.dataPath ); resultVal = resultVal.Replace( "\r" , "" ).Replace( "\n" , "" ).Replace( "\t" , "" ).Trim();return resultVal;}


}
}