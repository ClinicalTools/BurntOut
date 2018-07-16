using System;
using UnityEngine;

namespace Narrative
{
    public enum TaskType
    {
        Talk, Action, Emotion
    }

    public enum TaskEmotion
    {
        Neutral, Happy, Sad, Angry, Scared
    }

    public enum TaskAction
    {
        MoveTo, Show, Hide, Script, Sprite
    }

    [Serializable]
    public class Task
    {
        public TaskType type;

        public int actorId;
        public TaskEmotion emotion;
        public string dialogue;

        // Actions
        public TaskAction action;
        public PositionNode position;
        public GameObject activatable;
    }
}