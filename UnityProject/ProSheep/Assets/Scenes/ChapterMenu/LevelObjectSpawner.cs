using System;
using Newtonsoft.Json.Linq;
using ProSheep;
using UnityEngine;

namespace Scenes.ChapterMenu
{
    public class LevelObjectSpawner : MonoBehaviour
    {
        public static LevelObjectSpawner Instance;

        public Transform SpawnRoot;
        public GameObject LevelObjectPrefab;

        private void Start()
        {
            // Spawn levels once load 
            if (Brightend.LevelServiceCache.IsCached())
            {
                SpawnLevels(Brightend.LevelServiceCache.CachedLevel);
            }
            else
            {
                Brightend.LevelServiceCache.Download((isSucceed, levelArray) =>
                {
                    if (isSucceed)
                    {
                        SpawnLevels(levelArray);
                    }
                });
            }
        }

        public void SpawnLevels(JArray levelObjects)
        {
            if (CanSpawnLevels() == false)
            {
                return;
            }

            try
            {
                foreach (JObject levelObject in levelObjects)
                {
                    var newLevelEntry = Instantiate(LevelObjectPrefab, SpawnRoot);
                    
                    // (string uid,string cover,JObject json,string label,string describe,int category_id,int default_unlock)
                    var levelData = new LevelObject.LevelData(
                        (string)levelObject["uid"],
                        (string)levelObject["cover"],
                        (string)levelObject["json"],
                        (string)levelObject["label"],
                        (string)levelObject["describe"],
                        (int)levelObject["category_id"],
                        (int)levelObject["default_unlock"]
                    );

                    var levelProcess = Brightend.UserServiceCache.GetLevelProcess(levelData.uid);
                    var levelStars = Brightend.UserServiceCache.GetLevelStar(levelData.uid);
                    var levelUnlock = Brightend.UserServiceCache.GetLevelUnlock(levelData.uid);

                    var levelUserData = new LevelObject.LevelUserData(
                        levelStars,
                        levelProcess,
                        levelUnlock ? 1 : 0
                    );

                    newLevelEntry.name = levelData.uid;

                    var levelObj = newLevelEntry.GetComponent<LevelObject>();
                    levelObj.UpdateLevelObjectUsingLevelData(levelData, levelUserData);
                }
            }
            catch (Exception e)
            {
                Debug.LogError($"Can't spawn level objects: \n{e.ToString()}");
            }

            // Do arrangement if it exists 
            if (TryGetComponent<SineArrangement>(out var sineArrangement))
            {
                sineArrangement.DoArrangement();
            }

            Debug.Log($"Level objects has been spawned. ");
        }

        public bool CanSpawnLevels()
        {
            if (LevelObjectPrefab == null)
            {
                Debug.LogError($"LevelObjectPrefab isn't set");
                return false;
            }

            if (SpawnRoot == null)
            {
                SpawnRoot = new GameObject("Level Object Root").transform;
                return false;
            }

            if (Brightend.UserServiceCache.IsCached() == false)
            {
                // UserService should be cached on user login. 
                // Check `isEditor` && `IsCached == false`, `true` means editor environment. 
                if (Application.isEditor)
                {
                    // Try fetch Test User data
                    Debug.Log(
                        $"Editor develop mode detected. \nUserServiceCache isn't cached, trying to login with test_user pass...");
                    Brightend.UserServiceCache.Download("test_user", "pass",
                        (isSuccess, cachedUser, cachedUserData) =>
                        {
                            if (isSuccess)
                            {
                                // Re-Try spawning
                                SpawnLevels(Brightend.LevelServiceCache.CachedLevel);
                            }
                            else
                            {
                                Debug.Log($"Can't login with username test_user! ");
                            }
                        });

                    return false;
                }
                else
                {
                    Debug.LogError($"FORBIDDEN ACCESS");
                    Application.Quit(-1);
                    return false;
                }
            }

            return true;
        }
    }
}