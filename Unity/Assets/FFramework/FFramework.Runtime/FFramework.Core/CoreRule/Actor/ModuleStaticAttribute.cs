namespace FFramework
{
    public class ModuleStaticAttribute : System.Attribute
    {
        private string m_StaticClassName;

        public string Name=> m_StaticClassName;

        public ModuleStaticAttribute(string staticClassName)
        {
            m_StaticClassName = staticClassName;
        }
    }
}
