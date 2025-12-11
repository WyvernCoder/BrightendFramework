using System;
using ProSheep;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Components;

namespace Localization
{
    /// <summary>
    /// Attach this component to be with the TMP_Text component, then this comp will add the TMP_Text.text to Localization Table. 
    /// </summary>
    public class RuntimeStringLocalization : MonoBehaviour
    {
        private LocalizeStringEvent localizeStringEvent => GetComponent<LocalizeStringEvent>();
        private TMPro.TMP_Text targetText => GetComponent<TMPro.TMP_Text>();
        
        private void OnEnable()
        {
            var targetComponent = localizeStringEvent;
            
            if(localizeStringEvent == null)
                targetComponent = gameObject.AddComponent<LocalizeStringEvent>();
            
            targetComponent.OnUpdateString.AddListener(incomingText =>
            {
                // 只在有对应值的时候才去修改，即显示为“#ABC”格式，而不是被清空
                if(incomingText != "")
                    targetText.text = incomingText;
            });
            
            UpdateStringReferenceValue();
        }

        /// <summary>
        /// 更新 Entry 为当前Text显示的文字
        /// </summary>
        public void UpdateStringReferenceValue()
        {
            localizeStringEvent.StringReference =
                new LocalizedString(Brightend.DictServiceOnline.StringDatabaseTableName, targetText.text);
        }
    }
}
