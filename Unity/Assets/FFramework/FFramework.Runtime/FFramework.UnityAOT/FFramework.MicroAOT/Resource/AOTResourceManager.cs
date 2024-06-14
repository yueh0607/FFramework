using System.Collections;
using System.Collections.Generic;
using YooAsset;

namespace FFramework.MicroAOT
{
    /// <summary>
    /// 资源模块初始化器
    /// </summary>
    public class AOTResourceManager : AOTSingleton<AOTResourceManager>
    {
        private AOTResourceInitParameters m_InitParams;
        private bool m_LoadStarted = false;

        private ResourcePackage m_GameLogicPackage;
        private ResourcePackage m_GameResourcePackage;

        public ResourcePackage GameLogicPackage => m_GameLogicPackage;
        public ResourcePackage GameResourcePackage => m_GameLogicPackage;

        private PackageVersionProxy m_Versions;

        private const int DOWNLOAD_TRY_AGAGIN_COUNT = 3;     //下载失败尝试次数
        private const int DOWNLOAD_MAX_MEANWHILE_NUM = 10;   //最大同时下载N个文件


        public AOTResourceInitParameters InitHandle => m_InitParams;

        public bool Initialized =>
            m_GameLogicPackage.InitializeStatus == EOperationStatus.Succeed &&
            m_GameResourcePackage.InitializeStatus == EOperationStatus.Succeed;

        public struct PackageVersionProxy
        {
            public string GameResourcePackageVersion { get; internal set; }
            public string GameLogicPackageVersion { get; internal set; }
        }



        class RemoteService : IRemoteServices
        {
            string m_DefaultCDN;
            string m_FallbackCDN;

            public RemoteService(string defaultCDN, string fallbackCDN)
            {
                this.m_DefaultCDN = defaultCDN;
                this.m_FallbackCDN = fallbackCDN;
            }

            public string GetRemoteFallbackURL(string fileName)
            {
                return m_DefaultCDN;
            }

            public string GetRemoteMainURL(string fileName)
            {
                return m_FallbackCDN;
            }
        }


        public class GameQueryServices : IBuildinQueryServices
        {
            public bool Query(string packageName, string fileName, string fileCRC)
            {
                // 注意：fileName包含文件格式
                return StreamingAssetQueryHelper.FileExists(packageName, fileName, fileCRC);
            }
        }


        /// <summary>
        /// (All)第一步：初始化资源模块
        /// </summary>
        /// <param name="initParam"></param>
        /// <returns></returns>
        public IEnumerator Initialize(AOTResourceInitParameters initParams)
        {
            if (m_LoadStarted) yield break;
            m_LoadStarted = true;

            this.m_InitParams = initParams;


            // 初始化资源系统
            if (!YooAssets.Initialized)
                YooAssets.Initialize();

            string gameLogicPackageName = AOTStartInfoManager.GameLogicPackageName;
            string gameResourcePackageName = AOTStartInfoManager.GameResourcePackageName;

            m_GameLogicPackage = YooAssets.TryGetPackage(gameLogicPackageName) ?? YooAssets.CreatePackage(gameLogicPackageName);
            m_GameResourcePackage = YooAssets.TryGetPackage(gameResourcePackageName) ?? YooAssets.CreatePackage(gameResourcePackageName);


            YooAssets.SetDefaultPackage(m_GameResourcePackage);

            IEnumerator InitPackage(ResourcePackage package, AOTResourceInitParameters moduleParameter, EDefaultBuildPipeline pipeline)
            {
                //初始化成功，不需要等待
                if (package.InitializeStatus == EOperationStatus.Succeed) yield break;


                InitializeParameters yooParams = null;
                if (m_InitParams is AOTResourceInitParameters.Simulated simulateParam)
                {
                    EditorSimulateModeParameters simulateModeParameters = new EditorSimulateModeParameters();
                    simulateModeParameters.SimulateManifestFilePath = EditorSimulateModeHelper.SimulateBuild(pipeline, package.PackageName);
                    simulateModeParameters.DecryptionServices = simulateParam.DecryptionServices;
                    yooParams = simulateModeParameters;
                }
                else if (m_InitParams is AOTResourceInitParameters.Offline offlineParam)
                {
                    OfflinePlayModeParameters offlinePlayModeParameters = new OfflinePlayModeParameters();
                    offlinePlayModeParameters.DecryptionServices = offlineParam.DecryptionServices;
                    yooParams = offlinePlayModeParameters;
                }
                else if (m_InitParams is AOTResourceInitParameters.Host hostParam)
                {
                    HostPlayModeParameters hostPlayModeParameters = new HostPlayModeParameters();
                    hostPlayModeParameters.BuildinQueryServices = hostParam.BuildinQueryServices;
                    hostPlayModeParameters.DecryptionServices = hostParam.DecryptionServices;
                    hostPlayModeParameters.BuildinQueryServices = hostParam.BuildinQueryServices;
                    hostPlayModeParameters.RemoteServices = new RemoteService(hostParam.DefaultHostServer, hostParam.FallbackHostServer);
                    hostPlayModeParameters.BreakpointResumeFileSize = 1024 * 1024 * 10;
                    yooParams = hostPlayModeParameters;
                }
                else if (m_InitParams is AOTResourceInitParameters.WebGL webglParam)
                {
                    WebPlayModeParameters webPlayModeParameters = new WebPlayModeParameters();
                    webPlayModeParameters.DecryptionServices = null;
                    webPlayModeParameters.BuildinQueryServices = webglParam.BuildinQueryServices;
                    webPlayModeParameters.RemoteServices = new RemoteService(webglParam.DefaultHostServer, webglParam.FallbackHostServer);
                    webPlayModeParameters.BuildinQueryServices = webglParam.BuildinQueryServices;
                    webPlayModeParameters.BreakpointResumeFileSize = 1024 * 1024 * 10;
                    yooParams = webPlayModeParameters;
                }



                InitializationOperation operation = package.InitializeAsync(yooParams);
                while (!operation.IsDone)
                {
                    moduleParameter.InitCallback?.Invoke(package, operation);
                    yield return null;
                }
            }

            //初始化资源包
            yield return InitPackage(m_GameLogicPackage, m_InitParams, EDefaultBuildPipeline.RawFileBuildPipeline);

            yield return InitPackage(m_GameResourcePackage, m_InitParams, EDefaultBuildPipeline.BuiltinBuildPipeline);

        }

        /// <summary>
        /// （Host/WebGL）第二步：更新包版本
        /// </summary>
        /// <returns></returns>
        public IEnumerator UpdatePackageVersionAsync()
        {
            IEnumerator UpdatePackageVersion(ResourcePackage package, AOTResourceInitParameters moduleParam, string[] outVersionBuffer, int index)
            {
                UpdatePackageVersionOperation operation = package.UpdatePackageVersionAsync();
                while (!operation.IsDone)
                {
                    moduleParam.InitCallback?.Invoke(package, operation);
                    yield return null;
                }
                outVersionBuffer[index] = operation.PackageVersion;
            }

            string[] versions = new string[2];
            yield return UpdatePackageVersion(m_GameLogicPackage, m_InitParams, versions, 0);


            yield return UpdatePackageVersion(m_GameResourcePackage, m_InitParams, versions, 1);


            m_Versions = new PackageVersionProxy()
            {
                GameLogicPackageVersion = versions[0],
                GameResourcePackageVersion = versions[1]
            };
        }

        /// <summary>
        /// (Host/WebGL)第三步：更新资源清单
        /// </summary>
        /// <returns></returns>
        public IEnumerator UpdatePackageManifestAsync()
        {
            IEnumerator UpdatePackageManifest(ResourcePackage package, AOTResourceInitParameters moduleParam, string version)
            {
                UpdatePackageManifestOperation operation = package.UpdatePackageManifestAsync(version);
                while (!operation.IsDone)
                {
                    moduleParam.InitCallback?.Invoke(package, operation);
                    yield return null;
                };
            }

            yield return UpdatePackageManifest(m_GameLogicPackage, m_InitParams, m_Versions.GameLogicPackageVersion);
            yield return UpdatePackageManifest(m_GameResourcePackage, m_InitParams, m_Versions.GameResourcePackageVersion);
        }

        /// <summary>
        /// (Host/WebGL)第四步：下载资源
        /// </summary>
        /// <param name="progress"></param>
        /// <returns></returns>
        public IEnumerator Download()
        {
            IEnumerator DownloadPackageResource(ResourcePackage package, AOTResourceInitParameters moduleParam)
            {
                DownloaderOperation operation = package.CreateResourceDownloader(DOWNLOAD_MAX_MEANWHILE_NUM, DOWNLOAD_TRY_AGAGIN_COUNT);
                if (operation.TotalDownloadCount == 0)
                {
                    moduleParam.InitCallback?.Invoke(package, operation);
                    yield break;
                }
                else
                {
                    operation.BeginDownload();
                    while (!operation.IsDone)
                    {
                        moduleParam.InitCallback?.Invoke(package, operation);
                        yield return null;
                    }
                }
            }

            yield return DownloadPackageResource(m_GameLogicPackage, m_InitParams);
            yield return DownloadPackageResource(m_GameResourcePackage, m_InitParams);
        }

    }


}
