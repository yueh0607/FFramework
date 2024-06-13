using System.Collections;
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

            InitializeParameters yooParams = null;
            if (m_InitParams is AOTResourceInitParameters.Simulated simulateParam)
            {
                EditorSimulateModeParameters simulateModeParameters = new EditorSimulateModeParameters();
                simulateModeParameters.SimulateManifestFilePath = simulateParam.SimulateBuild();
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
                yooParams = hostPlayModeParameters;
            }
            else if (m_InitParams is AOTResourceInitParameters.WebGL webglParam)
            {
                WebPlayModeParameters webPlayModeParameters = new WebPlayModeParameters();
                webPlayModeParameters.DecryptionServices = null;
                webPlayModeParameters.BuildinQueryServices = webglParam.BuildinQueryServices;
                webPlayModeParameters.RemoteServices = webglParam.RemoteServices;
                yooParams = webPlayModeParameters;
            }

            // 初始化资源系统
            if (!YooAssets.Initialized)
                YooAssets.Initialize();

            string gameLogicPackageName = AOTStartInfoManager.GameLogicPackageName;
            string gameResourcePackageName = AOTStartInfoManager.GameResourcePackageName;

            m_GameLogicPackage = YooAssets.TryGetPackage(gameLogicPackageName) ?? YooAssets.CreatePackage(gameLogicPackageName);
            m_GameResourcePackage = YooAssets.TryGetPackage(gameResourcePackageName) ?? YooAssets.CreatePackage(gameResourcePackageName);


            YooAssets.SetDefaultPackage(m_GameResourcePackage);

            IEnumerator InitPackage(ResourcePackage package, InitializeParameters param, AOTResourceInitParameters moduleParameter)
            {
                //初始化成功，不需要等待
                if (package.InitializeStatus == EOperationStatus.Succeed) yield break;

                InitializationOperation operation = package.InitializeAsync(param);
                while (!operation.IsDone)
                {
                    moduleParameter.InitCallback?.Invoke(package, operation);
                    yield return null;
                }
            }

            //初始化资源包
            yield return InitPackage(m_GameLogicPackage, yooParams, m_InitParams);

            yield return InitPackage(m_GameResourcePackage, yooParams, m_InitParams);

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


        public AssetHandle LoadAssetAsync<T>(string location) where T : UnityEngine.Object
        {
            return YooAssets.LoadAssetAsync<T>(location);
        }
        public AssetHandle LoadAssetAsync<T>(AssetInfo location) where T : UnityEngine.Object
        {
            return YooAssets.LoadAssetAsync(location);
        }

        public RawFileHandle LoadRawFileAsync(string location)
        {
            return YooAssets.LoadRawFileAsync(location);
        }

        public RawFileHandle LoadRawFileAsync(AssetInfo location)
        {
            return YooAssets.LoadRawFileAsync(location);
        }

        public SubAssetsHandle LoadSubAssetAsync(string location)
        {
            return YooAssets.LoadSubAssetsAsync(location);
        }

        public SubAssetsHandle LoadSubAssetAsync(AssetInfo location)
        {
            return YooAssets.LoadSubAssetsAsync(location);
        }


        public SceneHandle LoadSceneAsync(string location)
        {
            return YooAssets.LoadSceneAsync(location);
        }
        public SceneHandle LoadSceneAsync(AssetInfo location)
        {
            return YooAssets.LoadSceneAsync(location);
        }

        public AssetInfo[] LoadAllAssetByTag(string tag)
        {
            return YooAssets.GetAssetInfos(tag);
        }

    }
}
