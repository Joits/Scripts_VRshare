//Daniel Todorov - saso3d.com

using UnityEngine;
using System.Collections;
namespace VRStandardAssets.Utils
{
    public class ButtonDesign : VRInteractiveItem
    {

        public int channelToAdjust; //give the button a channel to send to the ScrollersController that will know what changel of the lighting to adjust
        public Texture[] stateTexture = new Texture[3]; //insert the textures for the buttons
        bool currentlyActive = false; //Is the button active?

        public GameObject projSphere;
        public GameObject reticle;

        private DesignController myScript;


        // Use this for initialization
        void Start()
        {

            if (projSphere == null)
            {
                projSphere = GameObject.Find("Sp1");
            }

            if (reticle == null)
            {
                reticle = GameObject.Find("GUIReticle");
            }

            myScript = projSphere.GetComponent<DesignController>();
        }

        public override void Over()
        {
            base.Over();
            if (!currentlyActive)
            {                                                           //if not the currently selected layer to adjust
                this.GetComponent<Renderer>().material.mainTexture = stateTexture[1];       // set the button's texture to the second of the array
                reticle.GetComponent<CanvasRenderer>().SetColor(new Vector4(1, 1, 1, 1));
            }
        }



        public override void Out()
        {
            base.Out();
            if (!currentlyActive)
            {
                this.GetComponent<Renderer>().material.mainTexture = stateTexture[0];       //give the button the first texture from the array
                reticle.GetComponent<CanvasRenderer>().SetColor(new Vector4(1, 1, 1, 0.3f));
            }
        }


        public override void DoubleClick()
        {
            base.DoubleClick();
            currentlyActive = myScript.changeChannel(channelToAdjust, this.GetComponent<ButtonDesign>());
            this.GetComponent<Renderer>().material.mainTexture = stateTexture[2];           //set the texture of the button to the third state
        }

        public void setToInactive()
        {
            currentlyActive = false;
            Out();
        }

    }
} 