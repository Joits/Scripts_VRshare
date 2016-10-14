
using UnityEngine.UI;
using UnityEngine;

namespace VRStandardAssets.Utils
{
    public class PingInterActiveItem : VRInteractiveItem
    {

        //  protected bool m_IsOver;
        public GameObject reticle;
    
        DataTransferHandler dataTransfer;
        RandomMatchmaker matchMaker;
        public Canvas c; 
        private int transferID;
        private GameObject go;
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
                
                //dataTransfer = go.GetComponent<DataTransferHandler>();

                //matchMaker = c.GetComponent<RandomMatchmaker>();
               
             //   m_VrInput = go.GetComponent<VRInput>();
            }
            if (dataTransfer)
            {
                dataTransfer.userPing();
                CancelInvoke();
                go = null;

            }
        }

        public override void Down()
        {
            
           // print("in down call");
            base.Down();
            if (!dataTransfer)
            {
              
                //  print("in find data trans");
                matchMaker = c.GetComponent<RandomMatchmaker>();
                matchMaker.returnPlayerID(); //move this to if sentence and based on return choose which slave # to use datatransfer from.
                transferID = matchMaker.playerIDTransfering;
                switch (transferID){

                    case 0:
                        go = GameObject.Find("slave_povCam2");
                        break;
                    case 1:
                        go = GameObject.Find("slave_povCam3");
                        break;
                    case 2:
                        go = GameObject.Find("slave_povCam4");
                        break;
                    case 3:
                        go = GameObject.Find("slave_povCam5");
                        break;
                    default:
                        go = null;
                        break;
                }
                if (go)
                {
                    dataTransfer = go.GetComponent<DataTransferHandler>();
                }
                


              //  matchMaker.returnPlayerID();
                //      m_VrInput = go.GetComponent<VRInput>();
                //     print("go is : " + go + " data trans" + dataTransfer);
            }
            if (dataTransfer)
            {
               // print(PhotonNetwork.player.userId);

                print(transferID);
                
                dataTransfer.userPing();
              //  InvokeRepeating("transfer", 0f, 0.01f);


            }

        }
    }
}
