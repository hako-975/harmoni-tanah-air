using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewStoryScene", menuName = "Data/New Story Scene")]
[System.Serializable]
public class StoryScene : GameScene
{
    public List<Sentence> sentences;
    public Sprite background;
    public GameScene nextScene;

    [System.Serializable]
    public struct Sentence
    {
        [TextArea(3, 10)]
        public string text;
        public Speaker speaker;
        public List<Action> actions;
        public AudioClip music;
        public AudioClip sound;
        public bool soundLoop;

        [System.Serializable]
        public struct Action
        {
            public Speaker speaker;
            public Sprite sprite;
            public ActionType actionType;

            public float startCoords;
            public float endCoords;
            public float speed;
            public float coordX;
            public float duration;

            [System.Serializable]
            public enum ActionType
            {
                NONE,
                APPEAR,
                MOVE,
                DISAPPEAR
            }
        }
    }
}

public class GameScene : ScriptableObject
{

}
