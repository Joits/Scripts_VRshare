using UnityEngine;
using System;
using System.Collections;
using UnityEngine.VR;

//script handles transfer of data from/to clients using the photonview attached to the gameobject
namespace VRStandardAssets.Utils
{
    public class DataTransferHandler : Photon.PunBehaviour
    {

        private Quaternion correctPlayerRot;

        public VRInteractiveItem interactible;
        public int sync; //variable to change the sync updates in realtime
        private Quaternion syncEndRotation;
        private Vector3 pingPos;
        private Transform pingTrans;

        private GameObject ret;
        [SerializeField]
        private Transform m_Camera;
        [SerializeField]
        private LayerMask m_ExclusionLayers;           // Layers to exclude from the raycast.
        [SerializeField]
        private Reticle m_Reticle;                     // The reticle, if applicable.
        [SerializeField]
        private VRInput m_VrInput;                     // Used to call input based events on the current VRInteractiveItem.
        [SerializeField]
        private bool m_ShowDebugRay;                   // Optionally show the debug ray.
        [SerializeField]
        private float m_DebugRayLength = 5000f;           // Debug ray length.
        [SerializeField]
        private float m_DebugRayDuration = 12f;         // How long the Debug ray will remain visible.
        [SerializeField]
        private float m_RayLength = 5000f;

        public bool isTransfering = false;
        public GameObject pingPrefab;

        void Start()
        {
            m_Camera = GetComponent<Camera>().transform;
            m_Reticle = GetComponent<Reticle>();
            m_VrInput = GetComponent<VRInput>();
            sync = 8; //Set the updates refresh rate
                      //ret.GetComponent<RectTransform>().transform;
                      //   ret = PhotonNetwork.Instantiate("userPing", new Vector3(0, 10, 0), Quaternion.identity, 0);

            //if (ret == null)
            //{
            //    ret = GameObject.Find("GUIReticle");
            //  }
            // PhotonNetwork.Destroy(ret);
        }
        [PunRPC]
        private void EnablePing()
        {
            isTransfering = true;
            print("in enable ping");
        }
        [PunRPC]
        private void DisablePing()
        {
            isTransfering = false;
          //  Destroy()
            print("in disable ping");
        }



        public void userPing()
        {
            if (!isTransfering)
                photonView.RPC("EnablePing", PhotonTargets.Others);
            else
                photonView.RPC("DisablePing", PhotonTargets.Others);
        }
        // Update is called once per frame
        public void Update()
        {
            if (PhotonNetwork.isMasterClient)
            {
                transform.rotation = Quaternion.Lerp(transform.rotation, correctPlayerRot, Time.deltaTime * sync);
                if (isTransfering)
                {// print(m_Reticle.transform.position);
                    syncEndRotation = correctPlayerRot;//Quaternion.Lerp(transform.rotation, correctPlayerRot, Time.deltaTime * sync);
                   // pingTrans.localPosition = pingPos; // Vector3.Lerp(transform.position, pingPos, Time.deltaTime * sync);
                   // print(pingTrans);
                    rayCast();
                }//maybe add prediction to this model
            }
            //else
            //{
            //  ret.transform.position = pingPos;
            //  rayCast();
            //print(correctPlayerRot);
            
            //   }
        }

        public void rayCast()
        {
            //print("in raycast");
            Vector3 forward = correctPlayerRot * Vector3.forward;
            RaycastHit hit;
            Ray ray = new Ray(pingPos, forward);
            
            if (Physics.Raycast(ray, out hit, m_RayLength, ~m_ExclusionLayers))
            {

                VRInteractiveItem interactible = hit.collider.GetComponent<VRInteractiveItem>(); //attempt to get the VRInteractiveItem on the hit object


                // If we hit an interactive item and it's not the same as the last interactive item, then call Over
                if (interactible)
                {
                    // interactible.Over();
                    
                    var pos = ray.GetPoint(m_RayLength);
                    
                    Debug.DrawRay(m_Camera.position, m_Camera.forward * m_DebugRayLength, Color.red, m_DebugRayDuration);

                    Instantiate(pingPrefab, hit.point, Quaternion.identity);
                }

                // Something was hit, set at the hit position.
                //if (m_Reticle)
                //    m_Reticle.SetPosition(hit);

                //if (OnRaycasthit != null)
                //    OnRaycasthit(hit);
            }


        }
        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {

            if (stream.isWriting)
            {

                //stream.SendNext(InputTracking.GetLocalRotation(VRNode.Head)); //Debug.Log("Serialize is sending something");
                stream.SendNext(transform.rotation);//send the transform data (rotation). 
                                                    // stream.SendNext(transform.position);

                //if (isTransfering)
                //{

                stream.SendNext(transform.position);
                //stream.SendNext(m_Reticle.transform.position);
                //    }
                //  stream.SendNext(ret.transform.position);
                //print(ret.transform.position);
            }

            else {
                correctPlayerRot = (Quaternion)stream.ReceiveNext(); //if im not sending, i am receivning data
                                                                     //if (isTransfering)
                                                                     //{
                pingPos = (Vector3)stream.ReceiveNext();
                //  pingTrans = (Vector3)stream.ReceiveNext();
                //  }
                //   pingPos = (Vector3)stream.ReceiveNext();
                //      print(stream.ReceiveNext());

            }

        }
    }
}

