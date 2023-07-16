using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewChooseScene", menuName = "Data/New Choose Scene")]
[System.Serializable]
public class ChooseScene : GameScene
{
    public string question;
    public List<ChooseLabel> labels;
 
    [System.Serializable]
    public struct ChooseLabel
    {
        [TextArea(1, 1)]
        public string text;
        public StoryScene nextScene;
    }
}
