using System;
using System.Collections.Generic;

[Serializable]
public class PlayerDataModel
{
    public string Name = "";
    public int ID = 0;
    public string CreateTimeStr = "";
    public int CreateTime = 0;
    public string RectentQuitTimeStr = "";
    public int RectentQuitTime = 0;
    public string RectentPlayTimeStr = "";
    public int RectentPlayTime = 0;
    public int GameTime = 0;
    public string RectentSaveGameTimeStr = "";
    public int RectentSaveGameTime = 0;
    public int RectentPlaySaveID = 0;
    public int PlayerIcon = 0;
}

public enum CurLanguage
{
    Cn = 1,
    En = 2,
}

public enum RenderQuality
{
    VeryLow = 1,
    Low = 2,
    Medium = 3,
    High = 4,
    VeryHigh = 5,
    Ultra = 6,
}

[Serializable]
public class SettingModel
{
    public int MusicPriority;
    public int MusicQuality;
    public CurLanguage CurrLanguage;
    public RenderQuality RenderQuality;
}

public enum GameMode
{
}

public enum GameDifficulty
{
}

[Serializable]
public class SaveModel
{
    public List<SaveData> SaveDataList = new List<SaveData>();
}

[Serializable]
public class SaveData
{
    public string SaveName = "";
    public int SaveID = 0;
    public int CreatSaveTime = 0;
    public int RectentSaveTime = 0;
    public int Viewer = 0;
    public int ViewerIcon = 0;
    public int SaveAllGameTime = 0;
    public int SaveVersion = 0;
}