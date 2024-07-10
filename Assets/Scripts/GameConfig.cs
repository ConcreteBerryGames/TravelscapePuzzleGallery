using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameConfig : MonoBehaviour
{
    public static Dictionary<int, PuzzleConfig> config;
    public static string mode;
    public static int activePuzzle;
    private static List<int> donePictures = new List<int>();
    static GameConfig()
    {
        config = new Dictionary<int, PuzzleConfig>();
        config.Add(1, new PuzzleConfig { easyColumns = 3, easyRows = 4, hardColumns = 4, hardRows = 6, materialName = "Desert", done = false });
        config.Add(2, new PuzzleConfig { easyColumns = 3, easyRows = 2, hardColumns = 6, hardRows = 4, materialName = "DeadSea", done = false });
        config.Add(3, new PuzzleConfig { easyColumns = 3, easyRows = 3, hardColumns = 4, hardRows = 4, materialName = "AbuDhabi", done = false });
        config.Add(4, new PuzzleConfig { easyColumns = 3, easyRows = 3, hardColumns = 4, hardRows = 4, materialName = "Alps", done = false });
        config.Add(5, new PuzzleConfig { easyColumns = 4, easyRows = 3, hardColumns = 5, hardRows = 4, materialName = "Cappadocia", done = false });
        config.Add(6, new PuzzleConfig { easyColumns = 3, easyRows = 2, hardColumns = 6, hardRows = 4, materialName = "Masada", done = false });
        config.Add(7, new PuzzleConfig { easyColumns = 3, easyRows = 2, hardColumns = 6, hardRows = 4, materialName = "Madeira", done = false });
        config.Add(8, new PuzzleConfig { easyColumns = 2, easyRows = 3, hardColumns = 3, hardRows = 4, materialName = "Iceland", done = false });
        config.Add(9, new PuzzleConfig { easyColumns = 2, easyRows = 3, hardColumns = 3, hardRows = 4, materialName = "Piza", done = false });
        config.Add(10, new PuzzleConfig { easyColumns = 3, easyRows = 2, hardColumns = 4, hardRows = 3, materialName = "Trail", done = false });
        config.Add(11, new PuzzleConfig { easyColumns = 3, easyRows = 2, hardColumns = 4, hardRows = 3, materialName = "Giorgia", done = false });
        config.Add(12, new PuzzleConfig { easyColumns = 3, easyRows = 3, hardColumns = 4, hardRows = 4, materialName = "AbuDhabi2", done = false });
    }

    public static PuzzleConfig GetPuzzleConfig(int id)
    {
        return config[id];
    }

    public static void SetAcitivePuzzle(GameObject puzzle)
    {
        activePuzzle = puzzle.GetComponent<PuzzleManager>().id;
    }

    public static void SetAsDone(int id)
    {
        config[id].done = true;
        if (!donePictures.Contains(id)) donePictures.Add(id);
        
        string toSave = String.Join(",", donePictures.ToArray());
        PlayerPrefs.SetString("donePictures", toSave);
    }

    public static void SetMode(string targetMode)
    {
        mode = targetMode;
    }

    private void UndoPuzzles() {
        foreach (KeyValuePair<int, PuzzleConfig> puzzle in config)
        {
            puzzle.Value.done = false;
        }
    }

    void Awake() {
        GameConfig.mode = "easy";
        string[] done = PlayerPrefs.GetString("donePictures").Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
        for (int i = 0; i < done.Length; i++)
        {
            SetAsDone(Convert.ToInt32(done[i]));
        }
    }

    void Start()
    {
        GameEvents.current.onResetButtonClick += UndoPuzzles;
        GameEvents.current.onPictureClick += SetAcitivePuzzle;
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