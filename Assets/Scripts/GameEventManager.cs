using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEventManager : MonoBehaviour
{
    private GameEvent currentEvent;
    [SerializeField] private GameObject AdminModeLabel;
    [SerializeField] private List<ExpandedEventChoice> currentEventChoices = new List<ExpandedEventChoice>();
    private AdminModeEditModes currAdminMode;
    private int defaultNextEventID = -1;
    private bool isInAdminMode = false;

    // Start is called before the first frame update
    void Start(){
        AdminModeLabel.SetActive(false);
        loadEvent(1);
    }

    private void Update() {
        Debug.Log("Event: " + currentEvent.id + " | " + defaultNextEventID );
    }
    
    private void loadEvent(int eventID){
        if(eventID == -1) eventID = 1;

        // Finds the event with correct Event ID
        GameEvent gameEvent = DatabaseManager.Instance.Connection().Query<GameEvent>(
            $"SELECT * FROM events WHERE id = {eventID};"
        )[0];

        if (gameEvent.awaitsChoice){
            defaultNextEventID = -1;
            currentEventChoices = loadEventChoices(eventID);
        } else {
            currentEventChoices.Clear();
            // Load Default next event 
            Debug.Log(DatabaseManager.Instance.Connection().Query<DefaultNextEvent>(
                $"SELECT * FROM defaultNextEvents WHERE sourceEventID = {gameEvent.id};"
            ).Count);
            DefaultNextEvent eventLink = DatabaseManager.Instance.Connection().Query<DefaultNextEvent>(
                $"SELECT * FROM defaultNextEvents WHERE sourceEventID = {gameEvent.id};"
            )[0];
            if(eventLink == null)
                defaultNextEventID = -1;
            else
                defaultNextEventID = eventLink.nextEventID;
        }

        this.currentEvent = gameEvent;
        displayCurrentEvent();
    }

    private List<ExpandedEventChoice> loadEventChoices(int eventID){
        List<ExpandedEventChoice> eventChoices = new List<ExpandedEventChoice>();

        
        // Loads Event Choices from the database, using their IDs for loading data
        Debug.Log(DatabaseManager.Instance.Connection().Query<EventChoice>(
            $"SELECT * FROM eventChoices WHERE eventID = {eventID};"
        ).Count);
        List<EventChoice> retEventChoices = DatabaseManager.Instance.Connection().Query<EventChoice>(
            $"SELECT * FROM eventChoices WHERE eventID = {eventID};"
        );

        foreach(EventChoice eventChoice in retEventChoices){
            List<string> choiceKeywords = loadKeywords(eventChoice.choiceID);
            List<int> requiredList = loadRequiredItems(eventChoice.choiceID);
            List<int> rewardList = loadRewardedItems(eventChoice.choiceID);

            ExpandedEventChoice expEventChoice = new ExpandedEventChoice{
                text = eventChoice.text,
                targetEventID = eventChoice.targetEventID,
                keywords = choiceKeywords,
                requiredItemIDs = requiredList,
                rewardedItemIDs = rewardList
            };

            eventChoices.Add(expEventChoice);
        }

        Debug.Log(eventChoices.Count);
        return eventChoices;
    }

    private List<string> loadKeywords(int choiceID){
        List<string> keywords = new List<string>();

        // Loads keywords from the database based on choiceID
        List<ChoiceKeyword> choiceKeys = DatabaseManager.Instance.Connection().Query<ChoiceKeyword>(
            $"SELECT * FROM choiceKeywords WHERE choiceID = {choiceID};"
        );

        foreach(ChoiceKeyword choiceKey in choiceKeys){
            keywords.Add(choiceKey.keyword);
        }


        Debug.Log(keywords.Count);
        return keywords;
    }

    private List<int> loadRequiredItems(int choiceID){
        List<int> requiredList = new List<int>();
        
        // Loads required item ids from database
        List<ChoiceRequirement> retRequiredList = DatabaseManager.Instance.Connection().Query<ChoiceRequirement>(
            $"SELECT * FROM choiceRequirements WHERE choiceID = {choiceID};"
        );
        
        foreach(ChoiceRequirement choiceReq in retRequiredList){
            requiredList.Add(choiceReq.requirementItemID);
        }

        return requiredList;
    }   

    private List<int> loadRewardedItems(int choiceID){
        List<int> rewardList = new List<int>();
        
        // Loads rewarded item ids from database
        List<ChoiceReward> retRewardList = DatabaseManager.Instance.Connection().Query<ChoiceReward>(
            $"SELECT * FROM choiceRewards WHERE choiceID = {choiceID};"
        );
        
        foreach(ChoiceReward choiceReward in retRewardList){
            rewardList.Add(choiceReward.rewardItemID);
        }

        return rewardList;
    }   

    private void displayCurrentEvent(){
        MonitorTextManager monitor = MonitorTextManager.Instance;
        string displayText = currentEvent.text;
        
        if(currentEvent.displayChoices){
            foreach(ExpandedEventChoice choice in currentEventChoices){
                if(playerHasChoiceRequirements(choice)){
                    displayText += choice.text + "\n";
                }
            }
        }

        if(currentEvent.awaitsChoice){
            displayText += ">";
        }

        monitor.SetMonitorText(displayText);
        StartCoroutine(delayedAwaitInput());
    }

    private IEnumerator delayedAwaitInput(){
        yield return new WaitForEndOfFrame();
        MonitorTextManager.Instance.AwaitUserInput(processEventInput);
    }

    public void processEventInput(string input){

        if(currentEvent.id == 1){
            if(!isInAdminMode && string.Compare(input, "admin mode", true) == 0){
                EnterAdminMode();
                return;
            }
            // if(isInAdminMode && string.Compare(input, "admin mode", true) != 0)
            //     ExitAdminMode();
        }

        if(isInAdminMode){
            ProcessAdminModeInput(input);
            return;
        }

        if(!currentEvent.awaitsChoice || currentEventChoices.Count == 0){
            Debug.Log("There are no choices!");
            loadEvent(defaultNextEventID);
            return;
        }

        processNormalInput(input);
    }

    private void processNormalInput(string input){
        Debug.Log("Processing Normal Input");
        if(input.Length == 0){
            Debug.Log("Invalid User Input");
            displayCurrentEvent();
            return;
        }

        int chosenEventIndex = validateInputToChoiceIndex(input);
        if(chosenEventIndex == -1){
            Debug.Log("Invalid User Input");
            displayCurrentEvent();
            return;
        }

        ExpandedEventChoice chosenEvent = currentEventChoices[chosenEventIndex];
        foreach(int itemID in chosenEvent.rewardedItemIDs){
            PlayerItemManager.Instance.giveItem(itemID);
        }
        
        Debug.Log("Loading: " + chosenEvent.targetEventID);
        loadEvent(chosenEvent.targetEventID);
    }

    private void EnterAdminMode(){
        isInAdminMode = true;
        MonitorTextManager.Instance.SetAdminMode(true);
        this.AdminModeLabel.SetActive(true);
        currAdminMode = AdminModeEditModes.ActionSelection;
        loadEvent(9999);
    }

    private void ExitAdminMode(){
        MonitorTextManager.Instance.SetAdminMode(false);
        this.AdminModeLabel.SetActive(false);
        isInAdminMode = false;
    }

    private void ProcessAdminModeInput(string input){
        Debug.Log("Process Admin");
        switch(currAdminMode){
            case AdminModeEditModes.ActionSelection:
                processNormalInput(input);
                return;
        }

    }

    private bool playerHasChoiceRequirements(ExpandedEventChoice choice){
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
            ExpandedEventChoice eventChoice = currentEventChoices[i];
            if(!playerHasChoiceRequirements(eventChoice)) continue;
            foreach(string keyword in eventChoice.keywords){
                foreach(string word in inputWords){
                    if(String.Compare(word, keyword, true) == 0) return i;
                }
            }
        }
        return -1;
    }

    private struct ExpandedEventChoice {
        public string text;
        public int targetEventID;
        public List<string> keywords;
        public List<int> requiredItemIDs;
        public List<int> rewardedItemIDs;
    }

    private enum AdminModeEditModes {
        ActionSelection, TestingEvent, TestingEventOffset,
        CreatingNewEvent, WritingNewEventText, AskingIfEventHasChoices, AskingForDefaultNextEvent, AskingIfEventChoicesDisplay, DoesEventHaveOtherChoice,
        NewChoiceIDEnter, WritingNewChoiceText, WritingChoiceKeyword, DoesChoiceHaveAnotherKeyword, DoesChoiceHaveReq, DoesChoiceHaveReward,
        WritingReqID, CreatingItemName
    }
    /*
        9999 main admin menu
        9998 create new eventChoices event id entry
        9997 edit eventChoices
        9996 delete eventChoices
        9995 edit item
        9994 blank
        9993 has choices?
        9992 default next eventChoices
        9991 choice id enter
        9990 displaychoices 
        9989 choice more keywords
        9988 choice have req
        9987 insert reqID
        9986 more choices
        9985 item name
        9984 another reqID
        9983 reward
        9982 rewardID
        9981 another reward
    */
}
