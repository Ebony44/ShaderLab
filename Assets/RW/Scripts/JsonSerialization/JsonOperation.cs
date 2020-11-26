using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class JsonOperation : MonoBehaviour
{
    public PlayerData data;
    string jsonData;

    private void Awake()
    {
        data = LoadPlayerDataFromFile();
    }
    void Start()
    {
        //Player player = new Player
        //{
        //    name = "Unity",
        //    level = 2,
        //    selected = true,
        //    highscore = 12.2f,
        //};


        ////Serialziation of player class to JSON
        //string json = JsonUtility.ToJson(player);
        //Debug.Log(json);


        //Player player1 = new Player();
        //player1 = JsonUtility.FromJson<Player>(json);
        //Debug.Log("Name: " + player1.name);

        PlayerList playerList = new PlayerList();

        playerList.players.Add(new Player
        {
            name = "Unity",
            level = 2,
            selected = true,
            highscore = 12.2f,
        });
        playerList.players.Add(new Player
        {
            name = "John",
            level = 3,
            selected = true,
            highscore = 15f,
        });


        //Serialziation of player class to JSON
        // string json = JsonUtility.ToJson(playerList.players[0]);
        // Debug.Log(json);

        string json = JsonUtility.ToJson(playerList);
        Debug.Log(json);

        Debug.Log(Application.persistentDataPath);
        Debug.Log(Path.DirectorySeparatorChar);

        SaveDataToJson(playerList);

        var playerData = LoadPlayerData(json);


        // Player player1 = new Player();
        // player1 = JsonUtility.FromJson<Player>(json);
        // Debug.Log("Name: " + player1.name);


    }

    public void SaveDataToJson<T>(T objectData)
    {
        string json = JsonUtility.ToJson(objectData);
        File.WriteAllText(Application.persistentDataPath + Path.DirectorySeparatorChar + "PlayerData.txt", json);
        Debug.Log(Application.persistentDataPath + Path.DirectorySeparatorChar + "PlayerData.txt");
    }

    public T LoadDataFromJson<T>(string jsonData)
    {

        T temp = JsonUtility.FromJson<T>(jsonData);
        return temp;
    }
    public Player LoadPlayerData(string jsonData)
    {
        // var tempPlayer = JsonUtility.FromJson<Player>(jsonData);
        return LoadDataFromJson<Player>(jsonData);
    }
    public PlayerData LoadPlayerDataFromFile()
    {
        PlayerData playerData = null;
        if (File.Exists(Application.persistentDataPath + Path.DirectorySeparatorChar + "PlayerData.txt"))
        {
            playerData = ScriptableObject.CreateInstance<PlayerData>();
            string json = File.ReadAllText((Application.persistentDataPath + Path.DirectorySeparatorChar + "PlayerData.txt"));
            JsonUtility.FromJsonOverwrite(json, playerData);
        }
        else
        {
            playerData = Resources.Load<PlayerData>("Player Data");
        }
        return playerData;
    }

}


[System.Serializable]
public class Player
{
    public string name;
    public int level;
    public bool selected;
    public float highscore;
}

[System.Serializable] 
public class PlayerList
{
    public List<Player> players = new List<Player>();
}