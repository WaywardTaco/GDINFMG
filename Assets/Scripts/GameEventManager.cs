using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEventManager : MonoBehaviour
{
    private GameEvent currentEvent;
    private List<EventChoice> currentEventChoices;
    private int defaultNextEventID = -1;

    // Start is called before the first frame update
    void Start(){
        loadEvent(1);
    }

    private void Update() {
        Debug.Log("Event: " + currentEvent.text + " | " + currentEventChoices[0].targetEventID + " | " + currentEventChoices[0].keywords );
    }
    
    private void loadEvent(int eventID){
        if(eventID == -1) eventID = 1;

        // TODO : Load basic event info from the database given the eventID
        GameEvent tempEvent = new GameEvent{
            text = "Pancakes",
            displayChoices = true,
            awaitsChoice = true
        };







        if (tempEvent.awaitsChoice){
            defaultNextEventID = -1;
            currentEventChoices = loadEventChoices();
        } else {
            currentEventChoices.Clear();
            // Load Default next event
            defaultNextEventID = 5; // TODO : Get the defaultNextEvent from the database





        }

        this.currentEvent = tempEvent;
        displayCurrentEvent();
    }

    private List<EventChoice> loadEventChoices(){
        List<EventChoice> eventChoices = new List<EventChoice>();

        // TODO : Load Event Choices from the database, use their IDs for loading data





        for(int tempChoiceID = 0; tempChoiceID < 2; tempChoiceID++){
            List<string> choiceKeywords = loadKeywords(tempChoiceID);
            List<int> requiredList = loadRequiredItems(tempChoiceID);
            List<int> rewardList = loadRewardedItems(tempChoiceID);

            EventChoice tempEvent = new EventChoice{
                text = "words",
                targetEventID = -1,
                keywords = choiceKeywords,
                requiredItemIDs = requiredList,
                rewardedItemIDs = rewardList
            };

            eventChoices.Add(tempEvent);
        }

        return eventChoices;
    }

    private List<string> loadKeywords(int choiceID){
        List<string> keywords = new List<string>();

        // TODO : Load keywords from the database based on choiceID
        keywords.Add("Pancakes");

        




        return keywords;
    }

    private List<int> loadRequiredItems(int ChoiceID){
        List<int> requiredList = new List<int>();
        
        // TODO : Load required item ids from database
        // requiredList.Add(1);



        return requiredList;
    }   

    private List<int> loadRewardedItems(int ChoiceID){
        List<int> rewardList = new List<int>();
        
        // TODO : Load required item ids from database
        rewardList.Add(1);



        return rewardList;
    }   

    private void displayCurrentEvent(){
        MonitorTextManager monitor = MonitorTextManager.Instance;
        string displayText = currentEvent.text;
        
        if(currentEvent.displayChoices){
            foreach(EventChoice choice in currentEventChoices){
                if(playerHasChoiceRequirements(choice)){
                    displayText += choice.text + "\n";
                }
            }
        }

        if(currentEvent.awaitsChoice){
            displayText += ">";
            monitor.SetMonitorText(displayText);
            StartCoroutine(delayedAwaitInput());
        } else 
            monitor.SetMonitorText(displayText);
    }

    private IEnumerator delayedAwaitInput(){
        yield return new WaitForEndOfFrame();
        MonitorTextManager.Instance.AwaitUserInput(processEventInput);
    }

    public void processEventInput(string input){
        if(!currentEvent.awaitsChoice || currentEventChoices.Count == 0){
            loadEvent(defaultNextEventID);
            return;
        }

        int chosenEventIndex = validateInputToChoiceIndex(input);
        if(chosenEventIndex == -1){
            Debug.Log("Invalid User Input");
            displayCurrentEvent();
            return;
        }

        EventChoice chosenEvent = currentEventChoices[chosenEventIndex];
        foreach(int itemID in chosenEvent.rewardedItemIDs){
            PlayerItemManager.Instance.giveItem(itemID);
        }
        
        Debug.Log("Loading: " + chosenEvent.targetEventID);
        loadEvent(chosenEvent.targetEventID);
    }

    private bool playerHasChoiceRequirements(EventChoice choice){
        foreach(int itemID in choice.requiredItemIDs){
            if(PlayerItemManager.Instance.getItemCount(itemID) <= 0) return false;
        }
        return true;
    }

    private int validateInputToChoiceIndex(string input){
        string[] inputWords;
        string[] separators = {(" ")};
        inputWords = input.Split(separators, System.StringSplitOptions.None);

        int length = currentEventChoices.Count;
        for(int i = 0; i < length; i++){
            EventChoice eventChoice = currentEventChoices[i];
            if(!playerHasChoiceRequirements(eventChoice)) continue;
            foreach(string keyword in eventChoice.keywords){
                foreach(string word in inputWords){
                    if(String.Compare(word, keyword, true) == 0) return i;
                }
            }
        }
        return -1;
    }

    private struct GameEvent {
        public string text;
        public bool displayChoices;
        public bool awaitsChoice;
    }

    private struct EventChoice {
        public string text;
        public int targetEventID;
        public List<string> keywords;
        public List<int> requiredItemIDs;
        public List<int> rewardedItemIDs;
    }
}
