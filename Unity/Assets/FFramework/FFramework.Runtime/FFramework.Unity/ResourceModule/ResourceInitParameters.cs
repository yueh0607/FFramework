using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YooAsset;

namespace FFramework
{
    public class ResourceInitParameters
    {
        //游戏默认包名
        public string DefaultPackageName = "GameResource";

        //游戏逻辑包名
        public string GameLogicPackageName = "GameLogic";

        //包初始化回调
        public Action<ResourcePackage,AsyncOperationBase> InitCallback = null;
        
   

        public ResourceInitParameters()
        {

        }

        public class Simulate : ResourceInitParameters
        {
            //加密服务
            public IDecryptionServices DecryptionServices = null;
            public string SimulateBuild()
            {
                return EditorSimulateModeHelper.SimulateBuild(EDefaultBuildPipeline.BuiltinBuildPipeline, DefaultPackageName);
            }
        }

        public class Offline : ResourceInitParameters
        {
            //加密服务
            public IDecryptionServices DecryptionServices = null;
        }

        public class Host : ResourceInitParameters
        {
            public string DefaultHostServer = "http://127.0.0.1/CDN/WebGL/v1.0";
            public string FallbackHostServer = "http://127.0.0.1/CDN/WebGL/v1.0";
            public IBuildinQueryServices BuildinQueryServices = null;
            //加密服务
            public IDecryptionServices DecryptionServices = null;

            public IRemoteServices RemoteServices = null;

     
        }
        public class WebGL : ResourceInitParameters
        {
            public string DefaultHostServer = "http://127.0.0.1/CDN/WebGL/v1.0";
            public string FallbackHostServer = "http://127.0.0.1/CDN/WebGL/v1.0";
            public IBuildinQueryServices BuildinQueryServices = null;
            public IRemoteServices RemoteServices = null;

        }


        
    }



}
