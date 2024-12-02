using System.Collections;
using System.Collections.Generic;
using SQLite4Unity3d;
using UnityEngine;

public class DatabaseManager : MonoBehaviour
{
    public static DatabaseManager Instance { get; private set;}
    [SerializeField] private string databasePath;
    private SQLiteConnection connection;
    private void Awake() {
        if(Instance == null){
            Instance = this;
            EstablishConnection();
        } else    
            Destroy(this);
    }
    private void OnDestroy() {
        if(Instance == this){
            if(connection != null)
                connection.Close();
            Instance = null;
        }
    }

    private void Start(){
        connection.Query<GameEvent>(
            $"INSERT INTO events VALUES(1, 'Pancakes', 1, 1);"
        );
    }

    private void EstablishConnection(){
        connection = new SQLiteConnection(databasePath);
        if(connection == null){
            Debug.Log("Connection Null!");
            return;
        } 

        Debug.Log("Connection Established!");
        
        // CREATE TABLES BELOW
        connection.CreateTable<GameEvent>();
        connection.CreateTable<Item>();
        connection.CreateTable<DefaultNextEvent>();
        connection.CreateTable<ChoiceKeyword>();
        connection.CreateTable<EventChoice>();
        connection.CreateTable<ChoiceReward>();
        connection.CreateTable<ChoiceRequirement>();
    }

    public SQLiteConnection Connection(){
        return connection;
    }
}