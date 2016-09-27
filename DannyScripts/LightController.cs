//Daniel Todorov - saso3d.com

using UnityEngine;
using System.Collections;
using VRStandardAssets.Utils;
using UnityEngine.UI;

    public class LightController : MonoBehaviour
    {
		private float m_MouseDif;

        //Declare variables
        int currentLayer = 0;
        int totalNrChannels;

        float vScrollbarValueM;
        float[] blendValuesMat = new float[8];
        public float[] wMSquare = new float[8];
        public string[] layerNames = new string[8];

		Text myText;
		string TotalWattage;
        Material myMaterial;
		Canvas myCanvas;

		public ButtonLight lastUsedButton;
		SlidersAndGUI slidersControllerRef;
		//GearVRInput sliderInput;


        void Start()
        {
			currentLayer = 0; 												//start from the first layer

			myMaterial = gameObject.GetComponent<MeshRenderer>().material; 	//Get the material of the object this is attached to

            for (int i = 1; i < 9; i++)										//Check how many textures there are in the blending shader material
            {
                if (myMaterial.GetTexture("_Blend" + i + "Light") != null)
                {
                    totalNrChannels = i;
                }
            }
            myCanvas = GameObject.Find("InstructionCanvas").GetComponent<Canvas>();	//Get the canvas with instructions


			slidersControllerRef = myCanvas.GetComponent<SlidersAndGUI> ();			
			//sliderInput = this.GetComponent<GearVRInput>(); 

        }

        // Keeping the texture and text on the screen updated
        void Update()
        {

		TotalWattage = "Total at: " + (wMSquare [Mathf.Abs (currentLayer)] * (vScrollbarValueM + m_MouseDif) +  wMSquare [0]*blendValuesMat[0] + wMSquare [1]*blendValuesMat[1] + wMSquare [2]*blendValuesMat[2] + wMSquare [3]*blendValuesMat[3]).ToString ("f2") + "w/m" + "²";
			//Updating the mateiral blending

		//This is where the blending of the material is controlelr from
		if (((vScrollbarValueM + m_MouseDif)) > 0.0f) {		//If aobve 0
			slidersControllerRef.UpdateText ((layerNames [Mathf.Abs (currentLayer)] + " at " + (wMSquare [Mathf.Abs (currentLayer)] * (vScrollbarValueM + m_MouseDif)).ToString ("f2") + "w/m" + "²"),TotalWattage); 	//Set the text to whatever we are controlling now, value, w/m²
			slidersControllerRef.UpdateSliderValue (currentLayer, vScrollbarValueM + m_MouseDif);																										  				//Update the slider value, according to the currently adjusted layer
			myMaterial.SetFloat (("_Blend" + (1 + Mathf.Abs (currentLayer))).ToString (), ((vScrollbarValueM + m_MouseDif)*(vScrollbarValueM + m_MouseDif)));															//Update the blend material; squared to compensate for gamma correction
		} else {											// Else set it to 0 and make sure it doesnt get negative
			slidersControllerRef.UpdateText (layerNames [Mathf.Abs (currentLayer)] + " at 0.00 w/m" + "\u00B2", TotalWattage);
			slidersControllerRef.UpdateSliderValue (currentLayer, vScrollbarValueM + m_MouseDif);
			myMaterial.SetFloat (("_Blend" + (1 + Mathf.Abs (currentLayer))).ToString (), 0.0f);
			vScrollbarValueM = 0.0f;
		}


		//handle input and update the values that control the blending
		if (Input.GetButtonDown ("Fire1")) {
			//sliderInput.checkCustomInputStartMouse ();
		}

		if (Input.GetButton ("Fire1")) {
			//m_MouseDif = sliderInput.checkCustomInputHold ();
		}

		if (Input.GetButtonUp ("Fire1")) {
			//vScrollbarValueM += sliderInput.checkCustomInputRelease ();
			m_MouseDif = 0;
		}

        }

	//this is the function that gets called from the button script and changes the currently controlled channel according to the value the button has
	public bool changeChannel(int i, ButtonLight q){
			
		if (lastUsedButton != null) {
			lastUsedButton.setToInactive();
			lastUsedButton = q;
		} else {
			lastUsedButton = q;
		}
			blendValuesMat [currentLayer] = vScrollbarValueM;
			currentLayer = (i);
			vScrollbarValueM = blendValuesMat[currentLayer];
			return true;
		}
}