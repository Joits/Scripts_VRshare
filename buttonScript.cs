using UnityEngine;
using UnityEngine.UI;
using System.Collections;
//Handling the text on buttons
public class buttonScript : MonoBehaviour
{

    private string Name;
    public Text ButtonText;
    public gridButtonScript ScrollView;

    public void SetName(string name)
    {
        Name = name;
        ButtonText.text = name;
    }
    public void Button_Click()
    {
        //Return name to the scrollview (parent)
        ScrollView.ButtonClicked(Name);

    }
}