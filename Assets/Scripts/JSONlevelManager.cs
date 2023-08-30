using System.IO;
using System.Xml;
using UnityEngine;

public class JSONlevelManager : MonoBehaviour
{
    public Level level;
    public string SavePath { get; private set; }

    private void Start()
    {
        SavePath = PathCollection.LevelsPath;
    }

    [ContextMenu("SaveLevel")]
    public void SaveLevel()
    {
        File.WriteAllText(SavePath + "/" + level.Name + ".json", JsonUtility.ToJson(level, true));
        Debug.Log("LevelSaved " + SavePath);
    }

    public void LoadLevel(string levelname)
    {
        level = JsonUtility.FromJson<Level>(File.ReadAllText(SavePath + levelname));
    }

    [System.Serializable]
    public struct Level
    {
        [Header("LevelInfo")]
        public string Name;

        [Header("EconomicAttributes")]
        public int StartSun;

        [Header("ZombiesAttributes")]
        public ZombieTypes[] TypesOnLevel;

        public enum ZombieTypes
        {
            Default,
            ConeHead,
            BucketHead,
            GargantuaZombie,
            ImpZombie
        }

        [Header("WavesAttributes")]
        public int WavesCount;
        public int ZombieIncreasingQuantity;
        public bool StartWithDefaultZombie;

    }
}
