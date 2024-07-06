using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameConfig : MonoBehaviour
{
    public static Dictionary<int, PuzzleConfig> config;
    public static string mode;
    static GameConfig()
    {
        config = new Dictionary<int, PuzzleConfig>();
        config.Add(1, new PuzzleConfig { easyColumns = 3, easyRows = 4, hardColumns = 4, hardRows = 6, materialName = "DeadSea", done = false });
        config.Add(2, new PuzzleConfig { easyColumns = 4, easyRows = 3, hardColumns = 6, hardRows = 4, materialName = "dupa", done = false });
        config.Add(3, new PuzzleConfig { easyColumns = 4, easyRows = 3, hardColumns = 6, hardRows = 4, materialName = "dupa", done = false });
        config.Add(4, new PuzzleConfig { easyColumns = 4, easyRows = 3, hardColumns = 6, hardRows = 4, materialName = "dupa", done = false });
        config.Add(5, new PuzzleConfig { easyColumns = 4, easyRows = 3, hardColumns = 6, hardRows = 4, materialName = "dupa", done = false });
        config.Add(6, new PuzzleConfig { easyColumns = 4, easyRows = 3, hardColumns = 6, hardRows = 4, materialName = "dupa", done = false });
        config.Add(7, new PuzzleConfig { easyColumns = 4, easyRows = 3, hardColumns = 6, hardRows = 4, materialName = "dupa", done = false });
        config.Add(8, new PuzzleConfig { easyColumns = 4, easyRows = 3, hardColumns = 6, hardRows = 4, materialName = "dupa", done = false });
        config.Add(9, new PuzzleConfig { easyColumns = 4, easyRows = 3, hardColumns = 6, hardRows = 4, materialName = "dupa", done = false });
        config.Add(10, new PuzzleConfig { easyColumns = 4, easyRows = 3, hardColumns = 6, hardRows = 4, materialName = "dupa", done = false });
        config.Add(11, new PuzzleConfig { easyColumns = 4, easyRows = 3, hardColumns = 6, hardRows = 4, materialName = "dupa", done = false });
    }

    public static PuzzleConfig GetPuzzleConfig(int id)
    {
        return config[id];
    }

    public static void SetAsDone(int id)
    {
        config[id].done = true;
    }

    public static void SetMode(string targetMode)
    {
        mode = targetMode;
    }
}

public class PuzzleConfig
{
    public int easyColumns;
    public int easyRows;
    public int hardColumns;
    public int hardRows;
    public string materialName;
    public bool done;
}