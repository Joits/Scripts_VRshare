using UnityEngine;
using System.Collections;
using System.Collections.Generic;
//this script handles the buttons made in the interface. 
//

public class gridButtonScript : MonoBehaviour
{

    public GameObject Button_Template;
    List<string> imageList = new List<string>(); //list containing all the image file names
    changeTexture textureFileGrapper;

    // Use this for initialization
    void Start()
    {
        //get the script that handles the change of textures in visible to the viewer.
        textureFileGrapper = FindObjectOfType(typeof(changeTexture)) as changeTexture;

        imageList = textureFileGrapper.returnImageList();

        foreach (string str in imageList)
        {
            //create new instance  of a button template (specified in the inspector)
            GameObject go = Instantiate(Button_Template, new Vector3(0, 0, 45.5f), Quaternion.identity) as GameObject;
            go.SetActive(true);
            //set the name to of the button
            buttonScript TB = go.GetComponent<buttonScript>();
            TB.SetName(str);
            //correctly place the instantiated button the hierachy
            go.transform.SetParent(Button_Template.transform.parent);

        }


    }

    public void ButtonClicked(string str)
    {
        int index = imageList.IndexOf(str);
        textureFileGrapper.masterChangeTextureIndex(index); //Send the texture index to the changetexture function
    }
}
