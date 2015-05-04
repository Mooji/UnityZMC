using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace Mooji
{
    public class I18NConfigVo
    {
        private Dictionary<string , RefsVo> _refMapping = new Dictionary<string , RefsVo>();

        private Dictionary<string,I18NItemVo> _i18nItemMapping = new Dictionary<string , I18NItemVo>();


        public void reSet()
        {
            _refMapping = new Dictionary<string , RefsVo>();
            _i18nItemMapping = new Dictionary<string , I18NItemVo>();
        }


        public void buildRefsVo(RefsVo rv)
        {
            if( _refMapping.ContainsKey(rv.id) )
            {
                throw new Exception( " _refMapping.ContainsKey " + rv.id );
            }
            _refMapping[rv.id] = rv;
        }

        public void buildItemVo( I18NItemVo item )
        {
            if ( _i18nItemMapping.ContainsKey( item.key ) )
            {
                throw new Exception( " _i18nItemMapping.ContainsKey " + item.key );
            }
            _i18nItemMapping[item.key] = item;
        }


        public string getVal(string key)
        {
            if(_i18nItemMapping.ContainsKey(key))
            {
                I18NItemVo item = _i18nItemMapping[key];

                if ( item.isTranslate || item.refArr == null )
                {
                    return item.val;
                }
                else
                {
                    int index = 1;
                    foreach ( string refId in item.refArr )
                    {
                        item.val = item.val.Replace( "{" + index + "}" , _refMapping[refId].val );
                        index++;
                    }

                    item.isTranslate = true;

                    return item.val;
                }
            }

            return "";
        }

    }
}
