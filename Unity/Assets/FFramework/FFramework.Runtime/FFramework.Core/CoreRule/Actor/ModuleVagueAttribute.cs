using System;

namespace FFramework
{

    /// <summary>
    /// 模糊一个模块，使得这个模块派生路径上只能被派生一个，通过此抽象模块类也能访问派生模块
    /// </summary>
    [AttributeUsage(AttributeTargets.Class,AllowMultiple =false,Inherited = false)]
    public class ModuleVagueAttribute : System.Attribute
    {
        
    }
}
