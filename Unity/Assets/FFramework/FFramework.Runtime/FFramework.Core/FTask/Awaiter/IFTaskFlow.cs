using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FFramework
{
    public interface IFTaskFlow
    {

        /// <summary>
        /// 挂起时
        /// </summary>
        void OnSuspend();

        /// <summary>
        /// 恢复时
        /// </summary>
        void OnRestore();

        /// <summary>
        /// 取消时
        /// </summary>
        void OnCancel();

        /// <summary>
        /// 开始时
        /// </summary>
        void OnStart();

        /// <summary>
        /// 失败时
        /// </summary>
        void OnFailed();

        /// <summary>
        /// 成功时
        /// </summary>
        void OnSucceed();
    }
}
