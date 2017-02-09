using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class popupMessage : MonoBehaviour {

	// Use this for initialization
	void Start () 
	{
		this.GetComponent<CanvasGroup> ().blocksRaycasts = false;
		//popUpMessage ("test");
	}
	
	// Update is called once per frame
	void Update () 
	{
		
	}

	public void popUpMessage (string message)
	{
	//	print ("in popupmessage function");
		this.GetComponentInChildren<Text> ().text = message;
	
		this.GetComponentInChildren<Text>().color = new Vector4(170,170,170,255);
		this.GetComponentInChildren<Image> ().color = new Vector4 (0, 0, 0,255);
		StartCoroutine(delayFade(2));

	}
	private IEnumerator delayFade(int sec)
	{
	//	print ("in fade");
		yield return new WaitForSeconds (sec);
		fade ();
	}
	private void fade()
	{
		this.GetComponentInChildren<Image>().CrossFadeAlpha (0, 3.0f, false);
		this.GetComponentInChildren<Text>().CrossFadeAlpha (0, 3.0f, false);
	}


}
