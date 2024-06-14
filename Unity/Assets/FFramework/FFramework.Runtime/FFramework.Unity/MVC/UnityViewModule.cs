using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FFramework
{
    public class UnityViewModule : ViewModuleBase, IModule
    {

        UnityResourceModule m_UnityResourceModule;
        protected override async FTask<T> OnLoadViewAsset<T>(string viewPath)
        {
            var handle =  m_UnityResourceModule.LoadAssetAsync<GameObject>(viewPath);
            await handle.EnsureDone();
            return handle.GetAssetObject<GameObject>().AddComponent(typeof(T)) as T;
        }

        protected override void OnUnloadViewAsset(IView view)
        {
            GameObject.Destroy(((View)view).gameObject);
        }

        void IModule.OnCreate(object moduleParameter)
        {
            m_UnityResourceModule = Envirment.Current.GetModule<UnityResourceModule>();
        }

        void IModule.OnDestroy()
        {

        }
    }
}
