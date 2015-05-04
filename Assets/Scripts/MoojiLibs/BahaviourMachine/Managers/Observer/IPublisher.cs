using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mooji
{
    /// <summary>
    /// 发布者接口
    /// </summary>
    public interface IPublisher
    {
        /// <summary>
        /// 发布一条消息
        /// </summary>
        /// <param name="messageType">消息类型</param>
        /// <param name="parmasObj">消息的参数</param>
        /// <param name="options">消息的设置 暂时无用 占坑</param>
        void publishMsg( ObserverMsgTypeEnum messageType , object parmasObj , PublisheOptionVo options = null );
    }
}

