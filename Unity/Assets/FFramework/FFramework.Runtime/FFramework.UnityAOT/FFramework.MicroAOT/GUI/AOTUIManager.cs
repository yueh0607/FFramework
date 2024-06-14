using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using YooAsset;

namespace FFramework
{
    public class AOTUIManager : AOTSingleton<AOTUIManager>
    {
        private Dictionary<ValueTuple<string, int>, AOTPanel> m_Panels = new Dictionary<(string, int), AOTPanel>();

        private Camera m_UICamera;
        private EventSystem m_EventSystem;
        private Transform m_PanelRoot;

        public void Initialize(Camera uiCamera,EventSystem eventSystem,Transform PanelRoot)
        {
            this.m_PanelRoot = PanelRoot;
            this.m_UICamera = uiCamera;
            this.m_EventSystem = eventSystem;
        }


        public IEnumerator Open(string location, int key = 0)
        {
            if (m_Panels.ContainsKey(new ValueTuple<string, int>(location, key)))
                throw new InvalidOperationException($"Panel {location}:{key} has opened");

            var panelPrefab = YooAssets.LoadAssetAsync<GameObject>(location);
            yield return panelPrefab;
            var ins = panelPrefab.InstantiateAsync();
            yield return ins;

            panelPrefab.Release();

            if (ins.Result.TryGetComponent(out AOTPanel panel))
            {
                m_Panels.Add((location, key), panel);
                panel.Location = location;
                panel.Key = key;
                yield return panel.OnShow();
            }
            else throw new System.ArgumentNullException($"{location}:{key} has not a {nameof(AOTPanel)} Component");
        }


        public IEnumerator Close(string location, int key)
        {
            if (!m_Panels.ContainsKey(new ValueTuple<string, int>(location, key)))
                throw new InvalidOperationException($"Panel {location}:{key} has not opened");

            var panel = m_Panels[(location, key)];
            yield return panel.OnClose();
            GameObject.Destroy(panel.gameObject);

        }
    }
}
