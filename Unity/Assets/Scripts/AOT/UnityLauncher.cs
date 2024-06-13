using System.Collections;
using UnityEngine;
namespace FFramework.MicroAOT
{
    public class UnityLauncher : MonoBehaviour
    {
        private void Awake() => this.StartCoroutine(LoadGameFlow());

        
        IEnumerator LoadGameFlow()
        {
            yield return InitResource();
            yield return InitHotUpdate();
        }


        #region 资源加载初始化

        private AOTResourceInitParameters m_ResParams;
        IEnumerator InitResource()
        {
            m_ResParams = new AOTResourceInitParameters.Simulated();
            yield return AOTResourceManager.Instance.Initialize(m_ResParams);
        }
        #endregion

        #region 热更新

        IEnumerator InitHotUpdate()
        {
            yield return AOTHotUpdateManager.Instance.PatchMetaData();
            yield return AOTHotUpdateManager.Instance.LoadHotUpdateAssemblies();
        }
        #endregion

    }
}
