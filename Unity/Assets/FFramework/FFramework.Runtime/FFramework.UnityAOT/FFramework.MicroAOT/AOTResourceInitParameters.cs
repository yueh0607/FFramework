using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YooAsset;

namespace FFramework.MicroAOT
{
    public abstract class AOTResourceInitParameters
    {

        //包初始化回调
        public Action<ResourcePackage,AsyncOperationBase> InitCallback = null;
        
   

        public AOTResourceInitParameters()
        {

        }

        public class Simulated : AOTResourceInitParameters
        {
            //加密服务
            public IDecryptionServices DecryptionServices = null;
            public string SimulateBuild()
            {
                return EditorSimulateModeHelper.SimulateBuild(EDefaultBuildPipeline.BuiltinBuildPipeline, AOTStartInfoManager.GameResourcePackageName);
            }
        }

        public class Offline : AOTResourceInitParameters
        {
            //加密服务
            public IDecryptionServices DecryptionServices = null;
        }

        public class Host : AOTResourceInitParameters
        {
            public string DefaultHostServer = "http://127.0.0.1/CDN/WebGL/v1.0";
            public string FallbackHostServer = "http://127.0.0.1/CDN/WebGL/v1.0";
            public IBuildinQueryServices BuildinQueryServices = null;
            //加密服务
            public IDecryptionServices DecryptionServices = null;

            public IRemoteServices RemoteServices = null;

     
        }
        public class WebGL : AOTResourceInitParameters
        {
            public string DefaultHostServer = "http://127.0.0.1/CDN/WebGL/v1.0";
            public string FallbackHostServer = "http://127.0.0.1/CDN/WebGL/v1.0";
            public IBuildinQueryServices BuildinQueryServices = null;
            public IRemoteServices RemoteServices = null;

        }


        
    }



}
