using System.Net;

namespace FFramework
{
    public abstract class FServer
    {
        private IPEndPoint m_ServerEndPoint;

        protected abstract FTask OnStart();

        protected abstract FTask OnStop();


        public FServer(IPEndPoint serverEndPoint)
        {
            this.m_ServerEndPoint = serverEndPoint;
        }



        public async FTask Run()
        {
            await OnStart();
        }

        public async FTask Stop()
        {
            await OnStop();
        }


    }
}
