using System;

public enum TaskAction
{
    TALK, ACTION, EMOTION
}

public enum TaskEmotion
{
    NEUTRAL, HAPPY, SAD, ANGRY, SCARED
}

[Serializable]
public class Task
{
    public TaskAction action;
    public int actor;
    public TaskEmotion emotion;
    public string dialogue;
}