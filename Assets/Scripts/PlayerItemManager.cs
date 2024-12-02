using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerItemManager : MonoBehaviour
{
    public static PlayerItemManager Instance { get; private set;}
    [SerializeField] private Dictionary<int, int> itemCounts = new Dictionary<int, int>();

    private void Awake() {
        if (Instance == null)
            Instance = this;
        else 
            Destroy(this);
    }
    private void OnDestroy() {
        if(Instance == this)
            Instance = null;
    }

    public int getItemCount(int itemID){
        if(!itemCounts.ContainsKey(itemID)) return 0;
        return itemCounts[itemID];
    }

    public void giveItem(int itemID){
        if(!itemCounts.ContainsKey(itemID)){
            itemCounts.Add(itemID,1);
            return;
        }

        itemCounts[itemID] ++;
    }

    public void takeItem(int itemID){
        if(!itemCounts.ContainsKey(itemID)) return;
        itemCounts[itemID] --;

        if(itemCounts[itemID] <= 0) 
            itemCounts.Remove(itemID);
    }

    public void queryItemInfo(){
        // TODO : Ask the database for the item's info and do something about it ig?
        
    }
}
