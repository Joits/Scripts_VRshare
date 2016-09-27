using UnityEngine;
using System.Collections;
namespace VRStandardAssets.Utils
{
    public class ButtonVRshare : VRInteractiveItem
    {
        public int channelToAdjust;
        public Texture[] stateTexture = new Texture[3]; //insert the textures for the buttons
        public GameObject reticle;
        changeTexture myScript;

        // Use this for initialization
        void Start()
        {
            myScript = GameObject.Find("projectionSphere3").GetComponent<changeTexture>();

        }

        public override void Over()
        {
            if (reticle == null)
            {
                reticle = GameObject.Find("reticle sprite");
            }


            base.Over();
            //if not the currently selected layer to adjust
            this.GetComponent<Renderer>().material.mainTexture = stateTexture[1];
            // set the button's texture to the second of the array
                                    //  reticle.GetComponent<CanvasRenderer>().SetColor(new Vector4(1, 1, 1, 1));                                                              // reticle.GetComponent<CanvasRenderer>().SetColor(new Vector4(1, 1, 1, 1));

        }



        public override void Out()
        {
            base.Out();

            this.GetComponent<Renderer>().material.mainTexture = stateTexture[0];       //give the button the first texture from the array
           // reticle.GetComponent<CanvasRenderer>().SetColor(new Vector4(1, 1, 1, 0.3f));                                                         // reticle.GetComponent<CanvasRenderer>().SetColor(new Vector4(1, 1, 1, 0.3f));

        }

      
        public override void DoubleClick()
        {
            base.DoubleClick();
            //currentlyActive = myScript.changeChannel(channelToAdjust, this.GetComponent<ButtonDesign>());
            this.GetComponent<Renderer>().material.mainTexture = stateTexture[2];           //set the texture of the button to the third state
            myScript.masterChangeTextureIndex(channelToAdjust);
        }

        public void setToInactive()
        {
            Out();
        }

    }
}