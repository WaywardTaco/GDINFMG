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
            connection.Close();
            Instance = null;
        }
    }

    private void EstablishConnection(){
        connection = new SQLiteConnection(databasePath);
        if(connection == null) return;
        
        // CREAT TABLES BELOW
    }
}
