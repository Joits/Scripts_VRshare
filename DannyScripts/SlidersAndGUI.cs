//Daniel Todorov - saso3d.com

using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SlidersAndGUI : MonoBehaviour {


	public Canvas myCanvas; 				// Create a slot for the canvas reference
	public Slider sliderPrefab; 			// Slot for the slider prefab
	public int totalNrSliders = 4; 			// Total number of sliders, public value, soon to be changes to be automatic
	public Slider[] sliderArray = new Slider[4];  	// Array that contains all the sliders
	GameObject textShell;					// Empty that will contain the text
	public Text myText;							// Text variable that will be added to the empty
	public Text totalWText;


	void Start () {							// Create the sliders and text
		
	}
	 
	public void UpdateText(string newText,string newTextT){	// Function that updates the text
		myText.text = newText;
		totalWText.text = newTextT;
	}

	public void UpdateSliderValue (int chanelToUpdate, float newSliderValue){	// Function that updates the values of the sliders 
		sliderArray [chanelToUpdate].value = newSliderValue;					// according to the channel that is currently adjusted
	}
		
}
 

/* This is the old code that creates the sliders and text dynamically:

 //Daniel Todorov - saso3d.com

using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SlidersAndGUI : MonoBehaviour {


	public Canvas myCanvas; 				// Create a slot for the canvas reference
	float offsetX = -2f;						// Offset from the center of the canvas used for the sliders and text
	float offsetY = 1.2f;
	public Slider sliderPrefab; 			// Slot for the slider prefab
	public int totalNrSliders = 4; 			// Total number of sliders, public value, soon to be changes to be automatic
	Slider[] sliderArray = new Slider[8];  	// Array that contains all the sliders
	GameObject textShell;					// Empty that will contain the text
	Text myText;							// Text variable that will be added to the empty
	string displayText = "Initial Text";	// The actual text string that will be displayted on the text inside the empty on the canvas



	void Start () {							// Create the sliders and text
		CreateSliders (totalNrSliders);	

		textShell = new GameObject ("InstructionTextDisplay");
		myText = textShell.AddComponent<Text> ();
		myText.text = displayText;
		myText.transform.SetParent(myCanvas.transform);
		myText.transform.position = new Vector3(myCanvas.transform.position.x + 2*offsetX/60.89f, myCanvas.transform.position.y + 2*offsetY/40.5f, myCanvas.transform.position.z);
		myText.transform.localScale = Vector3.one;
		myText.fontSize = 35;
		myText.GetComponent<RectTransform>().sizeDelta = new Vector2 (400, 80);
		myText.font = Resources.GetBuiltinResource (typeof(Font), "Arial.ttf") as Font;

		UpdateText ("It has started!");
	}

	public void UpdateText(string newText){	// Function that updates the text
		myText.text = newText;
	}

	public void UpdateSliderValue (int chanelToUpdate, float newSliderValue){	// Function that updates the values of the sliders 
		sliderArray [chanelToUpdate].value = newSliderValue;					// according to the channel that is currently adjusted
	}

	public void CreateSliders (int totalNrumberOfSliders){						// Function that creates the sliders

		for (int i = 0; i < totalNrumberOfSliders; i++) {
			//Create the sliders from the prefab to the canvas that is already attached to the camera as a child;
			//Parent the sliders to the canvas, rename them, add them to the array of sliders for later reference and rescale them to 1;
			//Lastly destroy the current clone slider, since not needed anymore
			Slider currentClone = (Instantiate (sliderPrefab, new Vector3(myCanvas.transform.position.x + i*0.22f + offsetX, myCanvas.transform.position.y + offsetY, myCanvas.transform.position.z), Quaternion.identity) as Slider);
			currentClone.transform.SetParent (myCanvas.transform);
			currentClone.name = ("Slider" + i).ToString ();
			currentClone.transform.localScale = Vector3.one;
			sliderArray [i] = currentClone;
			Destroy (currentClone);
		}

	}
}

*/