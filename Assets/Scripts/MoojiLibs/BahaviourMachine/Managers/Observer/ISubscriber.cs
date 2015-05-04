using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mooji
{
    public interface ISubscriber
    {


        /// <summary>
        /// 订阅那些消息
        /// </summary>
        List<ObserverMsgTypeEnum> subscriberMessages();

        /// <summary>
        /// 接受到的消息
        /// </summary>
        /// <param name="messageType"></param>
        /// <param name="paramsObj"></param>
        /// <param name="options"></param>
        void receivedMessage( ObserverMsgTypeEnum messageType , object paramsObj , PublisheOptionVo options );
       
    }
}
