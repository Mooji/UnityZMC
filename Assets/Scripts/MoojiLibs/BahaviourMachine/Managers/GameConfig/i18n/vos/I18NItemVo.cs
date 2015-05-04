using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mooji
{
    public class I18NItemVo
    {
        public string key;
        public string val;
        public string[] refArr;
        /// <summary>
        /// 是否已经解释过了
        /// </summary>
        public bool isTranslate = false;
    }
}
