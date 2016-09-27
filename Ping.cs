using UnityEngine;
using System.Collections;

namespace VRStandardAssets.Utils
{
    public class Ping : VRInteractiveItem
    {
        public GameObject reticle;
        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public override void Down()
        {
            base.Down();
            if (reticle == null)
            {
                reticle = GameObject.Find("GUIReticle");
            }
            // set enable transfer of ping

            
            
            //currentlyActive = myScript.changeChannel(channelToAdjust, this.GetComponent<ButtonDesign>());
            // this.GetComponent<Renderer>().material.mainTexture = stateTexture[2];           //set the texture of the button to the third state
            //  myScript.masterChangeTextureIndex(channelToAdjust);
        }
        public override void Up()
        {
            base.Up();
            //disable the transfer of Ping
        }

        //public void SetPosition(RaycastHit hit)
        //{
        //    reticle.position = hit.point;
        //    m_ReticleTransform.localScale = m_OriginalScale * hit.distance;

        //    // If the reticle should use the normal of what has been hit...
        //    if (m_UseNormal)
        //        // ... set it's rotation based on it's forward vector facing along the normal.
        //        m_ReticleTransform.rotation = Quaternion.FromToRotation(Vector3.forward, hit.normal);
        //    else
        //        // However if it isn't using the normal then it's local rotation should be as it was originally.
        //        m_ReticleTransform.localRotation = m_OriginalRotation;
        //}
    }
}