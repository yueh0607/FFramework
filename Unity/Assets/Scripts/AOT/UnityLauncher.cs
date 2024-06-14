
using System.Collections;
using UnityEngine;
using System;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace FFramework.MicroAOT
{
    public class UnityLauncher : MonoBehaviour
    {
        private void Awake()
        {

            this.StartCoroutine(LoadGameFlow());

        }


        IEnumerator LoadGameFlow()
        {

            yield return InitResource();
            yield return InitHotUpdate();
        }




        #region 资源加载初始化

        private AOTResourceInitParameters m_ResParams;
        IEnumerator InitResource()
        {
            var mode = ModeHelper.GetRunMode();

            switch (mode)
            {
                case ERunMode.Simulated:
                    {
                        m_ResParams = new AOTResourceInitParameters.Simulated();
                        
                        break;
                    }
                case ERunMode.Real:
                    {
                        m_ResParams = new AOTResourceInitParameters.Offline()
                        {

                        };
                         
                        break;
                    }
            }
            yield return AOTResourceManager.Instance.Initialize(m_ResParams);
        }
        #endregion

        #region 热更新

        IEnumerator InitHotUpdate()
        {

            var mode = ModeHelper.GetRunMode();
            bool isSimulated = false;
            switch (mode)
            {
                case ERunMode.Simulated:
                    {
                        isSimulated = true;
                        break;
                    }
                case ERunMode.Real:
                    {
                        isSimulated = false;
                        break;
                    }
            }


            yield return AOTHotUpdateManager.Instance.PatchMetaData(isSimulated);
            yield return AOTHotUpdateManager.Instance.LoadHotUpdateAssemblies(isSimulated);
        }
        #endregion

    }
}
