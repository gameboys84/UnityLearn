using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Chinese chinese = new Chinese();
        Print(chinese.IntroduceSelf());
        Print(chinese.TellHobby("Unity"));
        
        English english = new English();
        Print(english.IntroduceSelf());
        
        Russian russian = new Russian();
        Print(russian.IntroduceSelf());
    }

    private void Print(string text)
    {
        Debug.Log(text);
    }
}
