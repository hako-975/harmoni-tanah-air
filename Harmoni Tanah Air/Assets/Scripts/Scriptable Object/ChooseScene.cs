using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewChooseScene", menuName = "Data/New Choose Scene")]
[System.Serializable]
public class ChooseScene : GameScene
{
    public List<ChooseLabel> labels;
    [System.Serializable]
    public struct ChooseLabel
    {
        [TextArea(7, 7)]
        public string text;
        public StoryScene nextScene;
    }
}
