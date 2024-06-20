using FFramework;
namespace UnitTest
{
    [ModuleStatic("FTest")]
    internal class TestGenModuleStatic : IModule
    {
        public int a { get; set; }
        public void Test()
        {
     
        } 
        void IModule.OnCreate(object moduleParameter)
        {
            throw new NotImplementedException();
        }

        void IModule.OnDestroy()
        {
            throw new NotImplementedException();
        }
    }
}
