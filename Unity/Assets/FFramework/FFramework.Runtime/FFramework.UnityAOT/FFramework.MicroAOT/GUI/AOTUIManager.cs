using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YooAsset;

namespace FFramework
{
    public class AOTUIManager : AOTSingleton<AOTUIManager>
    {
        private Dictionary<ValueTuple<string, int>, AOTPanel> m_Panels = new Dictionary<(string, int), AOTPanel>();
        public IEnumerator Open(string location,int key=0) 
        {
            var panelPrefab = YooAssets.LoadAssetAsync<GameObject>(location);
            yield return panelPrefab;
            var ins = panelPrefab.InstantiateAsync();
            yield return ins;

            if (ins.Result.TryGetComponent(out AOTPanel panel))
            {
                panel.Location = location;
                panel.Key = key;

      
                panel.OnShow();
            }
            else throw new System.ArgumentNullException($"{location}:{key} has not a {nameof(AOTPanel)} Component");

        }

        public void Focus(string location) 
        {

        }

        public void Close()
        {

        }
    }
}
