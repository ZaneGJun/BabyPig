using System;
using System.Collections.Generic;

namespace Pld
{
    abstract class PLDRefObject
    {
        /// <summary>
        /// 引用计数
        /// </summary>
        public int RefCount { get; private set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        protected PLDRefObject()
        {
            RefCount = 0;
        }

        /// <summary>
        /// 减少计数
        /// </summary>
        public void Release()
        {
            RefCount = --RefCount > 0 ? RefCount : 0;
        }

        /// <summary>
        /// 增加计数
        /// </summary>
        public void Retain()
        {
            ++RefCount;
        }
    }
}
