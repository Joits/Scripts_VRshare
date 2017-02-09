
using UnityEngine.UI;
using UnityEngine;

namespace VRStandardAssets.Utils
{
    public class VRLoginInteraction : VRInteractiveItem
    {

        //  protected bool m_IsOver;
        public GameObject reticle;
        public Texture[] stateTexture = new Texture[3];
        //public override bool IsOver
        //{
        //    get { return m_IsOver; }              // Is the gaze currently over this object?
        //}

        // The below functions are called by the VREyeRaycaster when the appropriate input is detected.
        // They in turn call the appropriate events should they have subscribers.
        //public override void Over()
        //{
        //    if (reticle == null)
        //    {
        //        reticle = GameObject.Find("reticle sprite");
        //    }
        //    base.Over();
        //    m_IsOver = true;
        //    print("fkjsdklfndsfln");

        //}

        public override void Over()
        {
            if (reticle == null)
            {
              //  reticle = GameObject.Find("reticle sprite");
            }


            base.Over();
           // print("fkjsdklfndsfln");
            //if not the currently selected layer to adjust
            this.GetComponent<Renderer>().material.mainTexture = stateTexture[1];
            // set the button's texture to the second of the array
            //  reticle.GetComponent<CanvasRenderer>().SetColor(new Vector4(1, 1, 1, 1));                                                              // reticle.GetComponent<CanvasRenderer>().SetColor(new Vector4(1, 1, 1, 1));

        }
        public override void Out()
        {
            m_IsOver = false;
            GetComponent<Renderer>().material.mainTexture = stateTexture[0];
        }
        public override void Click()
        {

            JoinButtonClicked();
        }

        public void JoinButtonClicked()
        {
            string serverName = this.GetComponentInChildren<Text>().text;
            PhotonNetwork.JoinRoom(serverName);

        }

        public override void DoubleClick()
        {

        }

        public override void Up()
        {

        }

        public override void Down()
        {

        }
    }
}
