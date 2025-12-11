using System;
using System.Collections;
using System.IO;
using Localization;
using Newtonsoft.Json.Linq;
using ProSheep;
using Scenes.Loading;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Scenes.ChapterMenu
{
    public class LevelObject : MonoBehaviour
    {
        /// <summary>
        /// From database
        /// </summary>
        public class LevelData
        {
            public LevelData(string uid, string cover, string json, string label, string describe, int category_id,
                int default_unlock)
            {
                max_stars = 3;
                this.uid = uid;
                this.cover = cover;
                this.json = json;
                this.label = label;
                this.describe = describe;
                this.category_id = category_id;
                this.default_unlock = default_unlock;
            }

            public string uid;
            public string cover;
            public string json;
            public string label;
            public string describe;
            public int category_id;
            public int default_unlock;
            public int max_stars;
        }

        public class LevelUserData
        {
            public LevelUserData(int stars, float process, int unlock)
            {
                this.stars = stars;
                this.process = process;
                unlocked = unlock == 1 ? true : false;
            }

            public int stars = 0;
            public float process = 0;
            public bool unlocked = false;
        }

        public bool IsInitialized => _levelDataCached != null;
        private LevelData _levelDataCached;
        private LevelUserData _levelUserDataCached;

        private Texture2D _coverT2DCache; // Release by hand! 
        private Sprite _coverSpriteCache; // Release by hand! 
        public Image coverImage;
        public Image processImage;
        public TMPro.TMP_Text labelText;
        public TMPro.TMP_Text buttonText;
        public Button buttonButton;
        public GameObject starGroup;
        //public TMPro.TMP_Text descriptionText;

        public Sprite okProcessImage;
        public Sprite doingProcessImage;
        public Sprite noProcessImage;

        public Sprite validStarImage;
        public Sprite nonvalidStarImage;

        public Sprite enableButtonImage;
        public Sprite disableButtonImage;

        public UnityEvent<LevelObject> onButtonClick;

        private void Start()
        {
            buttonButton.onClick.AddListener(() =>
            {
                if (IsInitialized)
                {
                    onButtonClick?.Invoke(this);
                }
                else
                {
                    Debug.LogError($"This level isn't initialized! ");
                }
            });
        }

        /// <summary>
        /// Initialize this level by LevelData. 
        /// </summary>
        /// <param name="levelData"></param>
        public void UpdateLevelObjectUsingLevelData(LevelData levelData, LevelUserData levelUserData)
        {
            if (IsInitialized)
            {
                Debug.LogWarning($"This level has already been initialized, but still trying to initialize! ");
                return;
            }

            _levelDataCached = levelData;
            _levelUserDataCached = levelUserData;

            Event_SetButtonEnabled(_levelDataCached.default_unlock == 1);
            Event_SetLabel(_levelDataCached.label);
            Event_SetStar(_levelUserDataCached.stars);

            // Set Process
            var processValue = _levelUserDataCached.process;
            var newProcess = ProcessType.NO;
            
            if (_levelUserDataCached.unlocked && processValue < 1)
                newProcess = ProcessType.DOING;
            
            if (_levelUserDataCached.unlocked && processValue >= 1)
                newProcess = ProcessType.OK;
            
            if (_levelUserDataCached.unlocked == false)
                newProcess = ProcessType.NO;
            
            Event_SetProcess(newProcess);
            
            // Bind Start Game button event
            try
            {
                var _addressableBundleName = (string)JObject.Parse(_levelDataCached.json)["AddressableBundle"];
                if (_addressableBundleName != "")
                {
                    onButtonClick.AddListener(clickingLevel =>
                    {
                        clickingLevel.buttonButton.interactable = false;
                        LoadingManager.LoadScene(_addressableBundleName);
                    });
                }
                else
                {
                    Debug.LogWarning($"Empty level detected. \nUID: {_levelDataCached.uid}");
                }
            }
            catch (Exception e)
            {
                Debug.LogWarning($"Can't update level object! \nUID: {_levelDataCached.uid}\n{e}");
            }

            // Update cover
            FreeDownloadManager.CheckValidate().DownloadFile(Path.GetFileName(_levelDataCached.cover), false, (fileName, isSuccess) =>
            {
                try
                {
                    if (isSuccess)
                    {
                        var levelCoverName = Path.GetFileName(_levelDataCached.cover);
                        
                        if (_coverSpriteCache != null) Destroy(_coverSpriteCache);
                        if (_coverT2DCache != null) Destroy(_coverT2DCache);
                        if (BinaryFileConverter.LoadImage(levelCoverName, ref _coverSpriteCache, ref _coverT2DCache))
                        {
                            coverImage.sprite = _coverSpriteCache;
                        }
                    }
                    else
                    {
                        throw new NullReferenceException($"Download failed: {fileName}! ");
                    }
                }
                catch (Exception e)
                {
                    Debug.LogWarning($"Cover set failed: {fileName}! \n{e}");
                }
            });

            // Download and Refresh localization
            Brightend.DictServiceOnline.UpdateLocalizationSystemFromOnline();
        }

        public void Event_SetStar(int starNum)
        {
            // 3 stars limitation
            starNum = Mathf.Clamp(starNum, 0, 3);

            for (var i = 0; i < 3; i++)
            {
                var status = i < starNum;
                Event_SetStar(i, status);
            }
        }

        public void Event_SetButtonEnabled(bool enabled)
        {
            buttonButton.interactable = enabled;
            buttonButton.image.sprite = enabled ? enableButtonImage : disableButtonImage;
        }

        private void Event_SetStar(int starIndex, bool value)
        {
            // 3 stars limitation
            starIndex = Mathf.Clamp(starIndex, 0, 2);

            var starObject = starGroup.transform.GetChild(starIndex).gameObject.GetComponent<Image>();
            starObject.sprite = value ? validStarImage : nonvalidStarImage;
        }

        public void Event_SetCover(Sprite image)
        {
            coverImage.sprite = image;
        }

        public void Event_SetLabel(string newLabel)
        {
            labelText.text = newLabel;
            
            // 变动Text后，这里也要重新应用一下
            labelText.GetComponent<RuntimeStringLocalization>().UpdateStringReferenceValue();
        }

        public void Event_SetButtonText(string newText)
        {
            buttonText.text = newText;
        }

        public void Event_SetProcess(ProcessType processType)
        {
            Sprite _image = null;
            switch (processType)
            {
                case ProcessType.OK:
                    _image = okProcessImage;
                    break;
                case ProcessType.DOING:
                    _image = doingProcessImage;
                    break;
                case ProcessType.NO:
                    _image = noProcessImage;
                    break;
            }

            processImage.sprite = _image;
        }

        private void OnDestroy()
        {
            if (_coverSpriteCache != null) Destroy(_coverSpriteCache);
            if (_coverT2DCache != null) Destroy(_coverT2DCache);
        }

        public enum ProcessType
        {
            OK,
            DOING,
            NO
        }
    }
}