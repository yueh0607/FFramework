using System;
using System.Collections.Generic;
using YooAsset;

namespace FFramework
{
    [ModuleStatic("FResource")]
    public class UnityResourceModule : IModule
    {
        private Dictionary<string, ResourcePackage> m_Packages = new Dictionary<string, ResourcePackage>();

        private ResourcePackage m_GameLogicPackage;
        private ResourcePackage m_GameResourcePackage;

        private bool m_LoadStarted = false;

        private ResourceInitParameters m_InitParameters;

        private PackageVersionProxy m_PackageVersion;


        private const int DOWNLOAD_TRY_AGAGIN_COUNT = 3;     //下载失败尝试次数
        private const int DOWNLOAD_MAX_MEANWHILE_NUM = 10;   //最大同时下载N个文件

        public struct PackageVersionProxy
        {
            public string GameResourcePackageVersion { get; internal set; }
            public string GameLogicPackageVersion { get; internal set; }
        }

        public UnityAssetHandle LoadAssetAsync<T>(string assetPath) where T : UnityEngine.Object
        {
            UnityAssetHandle handle =
                Envirment.Current.GetModule<PoolModule>().Get<UnityAssetHandle, UnityAssetHandle.Poolable>();

            AssetHandle yooHandle = YooAssets.LoadAssetAsync<T>(assetPath);

            handle.SetHandle(yooHandle);

            return handle;
        }

        /// <summary>
        /// 加载原生资源（bytes、text）
        /// </summary>
        /// <typeparam name="AssetType"></typeparam>
        /// <param name="assetPath"></param>
        /// <returns></returns>
        public UnityRawFileHandle LoadRawFileAsync(string assetPath)
        {
            UnityRawFileHandle handle =
                Envirment.Current.GetModule<PoolModule>().Get<UnityRawFileHandle, UnityRawFileHandle.Poolable>();

            RawFileHandle yooHandle = YooAssets.LoadRawFileAsync(assetPath);
            handle.SetHandle(yooHandle);
            return handle;
        }

        /// <summary>
        /// 加载场景
        /// </summary>
        /// <typeparam name="AssetType"></typeparam>
        /// <param name="assetPath"></param>
        /// <returns></returns>
        public UnitySceneHandle LoadSceneAsync(string assetPath)
        {
            UnitySceneHandle handle =
               Envirment.Current.GetModule<PoolModule>().Get<UnitySceneHandle, UnitySceneHandle.Poolable>();

            SceneHandle yooHandle = YooAssets.LoadSceneAsync(assetPath);
            handle.SetHandle(yooHandle);
            return handle;
        }


        /// <summary>
        /// 加载子资源(SpriteAltas)
        /// </summary>
        /// <typeparam name="AssetType"></typeparam>
        /// <param name="assetPath"></param>
        /// <returns></returns>
        public UnitySubAssetHandle LoadSubAssetsAsync<AssetType>(string assetPath) where AssetType : UnityEngine.Object
        {
            UnitySubAssetHandle handle =
              Envirment.Current.GetModule<PoolModule>().Get<UnitySubAssetHandle, UnitySubAssetHandle.Poolable>();

            SubAssetsHandle yooHandle = YooAssets.LoadSubAssetsAsync<AssetType>(assetPath);
            handle.SetHandle(yooHandle);
            return handle;
        }


        /// <summary>
        /// (All)第一步：初始化资源模块
        /// </summary>
        /// <param name="initParam"></param>
        /// <returns></returns>
        public async FTask Initialize(object initParam)
        {
            if (initParam is ResourceInitParameters resIntParam)
            {
                m_LoadStarted = true;
                m_InitParameters = resIntParam;

                InitializeParameters initParameters = null;

                if (initParam is ResourceInitParameters.Simulate simulateParam)
                {
                    EditorSimulateModeParameters simulateModeParameters = new EditorSimulateModeParameters();
                    simulateModeParameters.SimulateManifestFilePath = simulateParam.SimulateBuild();
                    simulateModeParameters.DecryptionServices = simulateParam.DecryptionServices;
                    initParameters = simulateModeParameters;
                }
                else if (initParam is ResourceInitParameters.Offline offlineParam)
                {
                    OfflinePlayModeParameters offlinePlayModeParameters = new OfflinePlayModeParameters();
                    offlinePlayModeParameters.DecryptionServices = offlineParam.DecryptionServices;
                    initParameters = offlinePlayModeParameters;
                }
                else if (initParam is ResourceInitParameters.Host hostParam)
                {
                    HostPlayModeParameters hostPlayModeParameters = new HostPlayModeParameters();
                    hostPlayModeParameters.BuildinQueryServices = hostParam.BuildinQueryServices;
                    hostPlayModeParameters.DecryptionServices = hostParam.DecryptionServices;
                    initParameters = hostPlayModeParameters;
                }
                else if (initParam is ResourceInitParameters.WebGL webglParam)
                {
                    WebPlayModeParameters webPlayModeParameters = new WebPlayModeParameters();
                    webPlayModeParameters.DecryptionServices = null;
                    webPlayModeParameters.BuildinQueryServices = webglParam.BuildinQueryServices;
                    webPlayModeParameters.RemoteServices = webglParam.RemoteServices;
                    initParameters = webPlayModeParameters;
                }

                // 初始化资源系统
                YooAssets.Initialize();
                m_GameLogicPackage = YooAssets.CreatePackage(resIntParam.GameLogicPackageName);
                m_GameResourcePackage = YooAssets.CreatePackage(resIntParam.DefaultPackageName);

                YooAssets.SetDefaultPackage(m_GameResourcePackage);

                async FTask InitPackage(ResourcePackage package, InitializeParameters param, ResourceInitParameters moduleParameter)
                {
                    InitializationOperation operation = package.InitializeAsync(param);
                    await FTask.WaitUntil(
                        () =>
                        {
                            moduleParameter.InitCallback?.Invoke(package, operation);
                            return operation.IsDone;
                        });
                }

                //初始化资源包
                await InitPackage(m_GameLogicPackage, initParameters, resIntParam);
                await InitPackage(m_GameResourcePackage, initParameters, resIntParam);


                var token = await FTask.CatchToken();
                if (token != null && token.IsCancellationRequested)
                    throw new InvalidOperationException("Initialize cannot be cancel");

            }
            else throw new ArgumentException($"{nameof(initParam)} is not {typeof(ResourceInitParameters).Name}");
        }


        /// <summary>
        /// （Host/WebGL）第二步：更新包版本
        /// </summary>
        /// <returns></returns>
        public async FTask UpdatePackageVersionAsync()
        {
            async FTask<string> UpdatePackageVersion(ResourcePackage package, ResourceInitParameters moduleParam)
            {
                UpdatePackageVersionOperation operation = package.UpdatePackageVersionAsync();
                await FTask.WaitUntil(() =>
                {
                    moduleParam.InitCallback?.Invoke(package, operation);
                    return operation.IsDone;
                });
                return operation.PackageVersion;
            }

            string gameLogicVersion = await UpdatePackageVersion(m_GameLogicPackage, m_InitParameters);
            string gameResourceVersion = await UpdatePackageVersion(m_GameResourcePackage, m_InitParameters);

            m_PackageVersion = new PackageVersionProxy()
            {
                GameLogicPackageVersion = gameLogicVersion,
                GameResourcePackageVersion = gameResourceVersion
            };
        }


        /// <summary>
        /// (Host/WebGL)第三步：更新资源清单
        /// </summary>
        /// <returns></returns>
        public async FTask UpdatePackageManifestAsync()
        {
            async FTask UpdatePackageManifest(ResourcePackage package, ResourceInitParameters moduleParam, string version)
            {
                UpdatePackageManifestOperation operation = package.UpdatePackageManifestAsync(version);
                await FTask.WaitUntil(() =>
                {
                    moduleParam.InitCallback?.Invoke(package, operation);
                    return operation.IsDone;
                });
            }

            await UpdatePackageManifest(m_GameLogicPackage, m_InitParameters, m_PackageVersion.GameLogicPackageVersion);
            await UpdatePackageManifest(m_GameResourcePackage, m_InitParameters, m_PackageVersion.GameLogicPackageVersion);
        }


        /// <summary>
        /// (Host/WebGL)第四步：下载资源
        /// </summary>
        /// <param name="progress"></param>
        /// <returns></returns>
        public async FTask Download()
        {
            async FTask DownloadPackageResource(ResourcePackage package, ResourceInitParameters moduleParam)
            {
                DownloaderOperation operation = package.CreateResourceDownloader(DOWNLOAD_MAX_MEANWHILE_NUM, DOWNLOAD_TRY_AGAGIN_COUNT);
                if (operation.TotalDownloadCount == 0)
                {
                    moduleParam.InitCallback?.Invoke(package, operation);
                    return;
                }
                else
                {
                    operation.BeginDownload();
                    await FTask.WaitUntil(() =>
                    {
                        moduleParam.InitCallback?.Invoke(package, operation);
                        return operation.IsDone;
                    });
                }
                await FTask.CompletedTask;
            }

            await DownloadPackageResource(m_GameLogicPackage, m_InitParameters);
            await DownloadPackageResource(m_GameResourcePackage, m_InitParameters);
        }
         

        void IModule.OnCreate(object moduleParameter)
        {

        }

        void IModule.OnDestroy()
        {

        }
    }
}
