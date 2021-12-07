using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartGameConfig : MonoBehaviour
{
    // this game object will stay between menu and main game mode

    // get list of all saved games 
    public List<string> GetSavedGames()
    {
        // get the list of saved files 
        return SaveController.GetSavedFiles();
    }

    // delete save game
    public bool RemoveSavedGames(string filename)
    {
        return SaveController.DeleteSavedGame(filename);
    }
 
    // set filename to be loaded when scene start
    public void LoadGameOnSceneStart(string filename)
    {
        current_game_filename = filename;
    }

    // save with new filename 
    public bool SaveAsNew()
    {
        SaveController save = FindObjectOfType<SaveController>();
        if (save)
        {
            save.Save();
            return true;
        }
        return false;
    }

    // save over current filename
    public bool QuickSave()
    {
        SaveController save = FindObjectOfType<SaveController>();
        if (save)
        {
            save.Save(current_game_filename);
            return true;
        }
        return false;
    }

    // vars 

    [SerializeField] protected string current_game_filename = "";
    private static StartGameConfig _instance;
    public static StartGameConfig Instance
    {
        get { return _instance; }
    }
    void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        _instance = this;

        DontDestroyOnLoad(this.gameObject);
    }
}
