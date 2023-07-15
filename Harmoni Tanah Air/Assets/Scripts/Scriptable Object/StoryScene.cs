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
        [TextArea(7, 7)]
        public string text;
        public Speaker speaker;
    }
}

public class GameScene : ScriptableObject
{

}
