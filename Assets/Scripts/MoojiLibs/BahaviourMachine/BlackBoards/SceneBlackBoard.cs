using System;
using System.Collections.Generic;

namespace Mooji
{
    public class SceneBlackBoard : ISceneBlackBoard
    {

        //  ========================================================================================================

        private static SceneBlackBoard _instance;

        public static SceneBlackBoard getInstance()
        {
            if ( _instance == null )
                _instance = new SceneBlackBoard();

            return _instance;
        }

        //  ========================================================================================================


        private Dictionary<Type ,ISceneBlackBoard> _blackBoardMapping = new Dictionary<Type , ISceneBlackBoard>();


        /// <summary>
        /// 在 GameBootstrapper 中添加进去
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T getBlackBoard<T>() where T : SceneBlackBoard , new()
        {
            return addBlackBoard<T>();
        }

        public T addBlackBoard<T>() where T : SceneBlackBoard , new()
        {
            Type t = typeof( T );

            if ( _blackBoardMapping.ContainsKey( t ) )
            {
                return _blackBoardMapping[t] as T;
            }
            else
            {
                _blackBoardMapping[t] = new T() ;
                return _blackBoardMapping[t] as T ;
            }
        }
    }
}
