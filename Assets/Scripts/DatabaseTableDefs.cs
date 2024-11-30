using System;
using SQLite4Unity3d;

[Table("events")]
public class GameEvent {
    [PrimaryKey, Column("id")] public Guid id {get; set;}
    [Column("text")] public string text {get; set;}
    [Column("displayChoices")] public bool displayChoices {get; set;}
    [Column("awaitsChoice")] public bool awaitsChoice {get; set;}
}

[Table("choices")]
public class Choice {
    [PrimaryKey, Column("id")] public Guid id {get; set;}
    [Column("text")] public string text {get; set;}
    [Column("targetEventID")] public Guid targetEventID {get; set;}
}

[Table("items")]
public class Item {
    [PrimaryKey, Column("id")] public Guid id {get; set;}
    [Column("name")] public string name {get; set;}
}

[Table("defaultNextEvents")]
public class DefaultNextEvent {
    [PrimaryKey, Column("sourceEventID")] public Guid sourceEventID {get; set;}
    [Column("nextEventID")] public Guid nextEventID {get; set;}
}

[Table("choiceKeywords")]
public class ChoiceKeyword {
    [PrimaryKey, Column("choiceID")] public Guid choiceID {get; set;}
    [Column("keyword")] public string keyword {get; set;}
}

[Table("eventChoices")]
public class EventChoice {
    [PrimaryKey, Column("eventID")] public Guid eventID {get; set;}
    [PrimaryKey, Column("choiceID")] public Guid choiceID {get; set;}
}

[Table("choiceRewards")]
public class ChoiceReward {
    [PrimaryKey, Column("choiceID")] public Guid choiceID {get; set;}
    [PrimaryKey, Column("rewardItemID")] public Guid rewardItemID {get; set;}
}

[Table("choiceRequirements")]
public class ChoiceRequirement {
    [PrimaryKey, Column("choiceID")] public Guid choiceID {get; set;}
    [PrimaryKey, Column("requirementItemID")] public Guid requirementItemID {get; set;}
}

