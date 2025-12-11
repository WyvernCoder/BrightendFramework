using Newtonsoft.Json.Linq;
using ProSheep;
using UnityEngine;

namespace Scenes.ChapterMenu
{
    public class ChapterMenu_S : MonoBehaviour
    {
        void Start()
        {
            // Try fetch levels
            Brightend.LevelServiceCache.Download((isSucc, levelArray) =>
            {
                if (isSucc)
                {
                    //TODO ???
                }
                else
                {
                    Debug.LogError($"Can't fetch level list from server! ");
                }
            });
        }

        public void LoadCachedLevelsToScene(bool withUserStatus = true)
        {
            var _levelArray = Brightend.LevelServiceCache.CachedLevel;
            foreach (JObject _level in _levelArray)
            {
                var _uid = (string)_level["uid"];
                var _coverPath = (string)_level["cover"];
                var _jsonData = (string)_level["json"];
                var _label = (string)_level["label"];
                var _describe = (string)_level["describe"];
                var _category = (int)_level["category_id"];
                var _defaultUnlock = (int)_level["default_unlock"];
                var _maxStars = (int)_level["max_stars"];
                
                //TODO Spawn Level Instants
                
                //TODO Load Level Unlock Status
                if (withUserStatus)
                {
                    var __isUnlocked = Brightend.UserServiceCache.GetLevelUnlock(_uid) || (_defaultUnlock == 1);
                    var __stars = Brightend.UserServiceCache.GetLevelStar(_uid);
                    var __process = Brightend.UserServiceCache.GetLevelProcess(_uid);
                }
            }
        }
    }
}
