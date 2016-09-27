//Daniel Todorov - saso3d.com

using UnityEngine;
using System.Collections;
namespace VRStandardAssets.Utils
{
    public class ButtonLight : VRInteractiveItem
    {

        // Variables for the cubes and reticle tha would change upon looking at this button
        public GameObject leftCube;
        public GameObject rightCube;
        public GameObject reticle;

        private LightController myScript;
        private LightController myScript2;

        public int channelToAdjust; //give the button a channel to send to the ScrollersController that will know what changel of the lighting to adjust
        public Texture[] stateTexture = new Texture[3]; //insert the textures for the buttons
        bool currentlyActive = false; //Is the button active?


        // Use this for initialization
        void Start()
        {

            // Get the cubes and reticle tha would change upon looking at this button
            if (leftCube == null)
            {
                leftCube = GameObject.Find("ProjCubeLeft");
            }
            if (rightCube == null)
            {
                rightCube = GameObject.Find("ProjCubeRight");
            }
            myScript = leftCube.GetComponent<LightController>();
            myScript2 = rightCube.GetComponent<LightController>();

            if (reticle == null)
            {
                reticle = GameObject.Find("GUIReticle");
            }
        }

        public override void Over()
        {
            base.Over();

            if (!currentlyActive)
            {                                                           //if not the currently selected layer to adjust
                this.GetComponent<Renderer>().material.mainTexture = stateTexture[1];       // set the button's texture to the second of the array
                reticle.GetComponent<CanvasRenderer>().SetColor(new Vector4(1, 1, 1, 1));   // and give the reticle a full color
            }

        }


        public override void Out()
        {
            base.Out();

            if (!currentlyActive)
            {                                                           //if not the currently selected layer to adjust
                this.GetComponent<Renderer>().material.mainTexture = stateTexture[0];       //give the button the first texture from the array
                reticle.GetComponent<CanvasRenderer>().SetColor(new Vector4(1, 1, 1, 0.3f));    // and make the reticle more transperant
            }
        }


        public override void DoubleClick()
        {
            base.DoubleClick();

            currentlyActive = myScript.changeChannel(channelToAdjust, this.GetComponent<ButtonLight>());    //Upon a double click, call the function from ...
            myScript2.changeChannel(channelToAdjust, this.GetComponent<ButtonLight>());                 //... ScrollersController component for both cubes
                                                                                                        // and set the main(left)'s currentlyActive's variable
            this.GetComponent<Renderer>().material.mainTexture = stateTexture[2];                           //set the texture of the button to the third state
        }

        public void setToInactive()
        {
            currentlyActive = false;
            Out();
        }

    }
}