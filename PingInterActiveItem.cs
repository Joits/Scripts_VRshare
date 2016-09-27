
using UnityEngine.UI;
using UnityEngine;

namespace VRStandardAssets.Utils
{
    public class PingInterActiveItem : VRInteractiveItem
    {

        //  protected bool m_IsOver;
        public GameObject reticle;
        public Texture[] stateTexture = new Texture[3];
        DataTransferHandler dataTransfer;
        [SerializeField]
        private VRInput m_VrInput;                     // Used to call input based events on the current VRInteractiveItem.
        public override bool IsOver

        {
            get { return m_IsOver; }              // Is the gaze currently over this object?
        }

        ////The below functions are called by the VREyeRaycaster when the appropriate input is detected.
        ////They in turn call the appropriate events should they have subscribers.
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


        void Start()
        {
           
         //   m_VrInput.OnDown += HandleDown;
            //  dataTransfer = GameObject.Find("Slave_preFab2(Clone)").GetComponent<DataTransferHandler>();
        }
        public override void Over()
        {
            if (reticle == null)
            {
                reticle = GameObject.Find("GUIReticle");
            }


            base.Over();
           // reticle.GetComponent<CanvasRenderer>().SetColor(new Vector4(1, 1, 1, 1));
          // print ("yoylylylyoyoyoy");
        }

        private void OnEnable()
        {
            
        }

        private void transfer()
        {
            dataTransfer.rayCast();
        }
        public override void Out()
        {
            m_IsOver = false;
           // this.GetComponent<Renderer>().material.mainTexture = stateTexture[0];
        }

        public override void DoubleClick()
        {

        }
        
        public override void Up()
        {
            base.Up();
            if (!dataTransfer)
            {
                GameObject go = GameObject.Find("slave_povCam2");
                dataTransfer = go.GetComponent<DataTransferHandler>();
                m_VrInput = go.GetComponent<VRInput>();
            }
            if (dataTransfer)
            {

                dataTransfer.userPing();
                CancelInvoke();

            }
        }

        public override void Down()
        {
            
           // print("in down call");
            base.Down();
            if (!dataTransfer)
            {
                //  print("in find data trans");
                GameObject go = GameObject.Find("slave_povCam2");
                dataTransfer = go.GetComponent<DataTransferHandler>();
                m_VrInput = go.GetComponent<VRInput>();
                //     print("go is : " + go + " data trans" + dataTransfer);
            }
            if (dataTransfer)
            {
                dataTransfer.userPing();
                InvokeRepeating("transfer", 0f, 0.01f);


            }

        }
    }
}
