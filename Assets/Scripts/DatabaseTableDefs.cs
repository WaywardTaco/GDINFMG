using System;
using SQLite4Unity3d;

[Table("events")]
public class GameEvent {
    [PrimaryKey, Column("id")] public int id {get; set;}
    [Column("text")] public string text {get; set;}
    [Column("displayChoices")] public bool displayChoices {get; set;}
    [Column("awaitsChoice")] public bool awaitsChoice {get; set;}
}

[Table("items")]
public class Item {
    [PrimaryKey, Column("id"), AutoIncrement] public int id {get; set;}
    [Column("name")] public string name {get; set;}
}

[Table("defaultNextEvents")]
public class DefaultNextEvent {
    [PrimaryKey, Column("sourceEventID")] public int sourceEventID {get; set;}
    [Column("nextEventID")] public int nextEventID {get; set;}
}

[Table("choiceKeywords")]
public class ChoiceKeyword {
    [PrimaryKey, Column("keywordID"), AutoIncrement] public int keywordID {get; set;}
    [Column("choiceID")] public int choiceID {get; set;}
    [Column("keyword")] public string keyword {get; set;}
}

[Table("eventChoices")]
public class EventChoice {
    [PrimaryKey, Column("choiceID"), AutoIncrement] public int choiceID {get; set;}
    [Column("eventID")] public int eventID {get; set;}
    [Column("text")] public string text {get; set;}
    [Column("targetEventID")] public int targetEventID {get; set;}
}

[Table("choiceRewards")]
public class ChoiceReward {
    [PrimaryKey, Column("rewardID"), AutoIncrement] public int rewardID {get; set;}
    [Column("choiceID")] public int choiceID {get; set;}
    [Column("rewardItemID")] public int rewardItemID {get; set;}
}

[Table("choiceRequirements")]
public class ChoiceRequirement {
    [PrimaryKey, Column("reqID"), AutoIncrement] public int reqID {get; set;}
    [Column("choiceID")] public int choiceID {get; set;}
    [Column("requirementItemID")] public int requirementItemID {get; set;}
}

