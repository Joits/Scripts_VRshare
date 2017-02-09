
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

        void Start()
        {
                    
        }
        public override void Over()
        {
            if (reticle == null)
            {
                //reticle = GameObject.Find("GUIReticle");
            }


            base.Over();
          
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
                
   
            }
            if (dataTransfer)
            {
                dataTransfer.userPing(); //uncomment to transfer ping
                go = null; //reset the ping owner

            }
        }

        public override void Down()
        {
            
           // print("in down call");
            base.Down();
            if (!dataTransfer)
            {
              
                //  print("in find data trans");


                matchMaker = c.GetComponent<RandomMatchmaker>(); //find the cavnas mathcmaker who controls the users connected
                matchMaker.returnPlayerID();  //find the this players ID
                transferID = matchMaker.playerIDTransfering; 

                //based on ID find who wants to transfer
                switch (transferID){

                    case 0:
                        go = GameObject.Find("slave_povCam2"); //a bit stupid with hardcoded names..
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
   
            }
            if (dataTransfer)
            {
               // print(PhotonNetwork.player.userId);

              //  print(transferID);
               dataTransfer.userPing(); //i want to transfer my gaze data uncomment to enable transfering of gaze
            


            }

        }
    }
}
