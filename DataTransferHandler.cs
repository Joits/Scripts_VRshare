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

        private LayerMask m_ExclusionLayers;           // Layers to exclude from the raycast.
                    // The reticle, if applicable.

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


            sync = 8; 

        }
        [PunRPC]
        private void EnablePing()
        {
            isTransfering = true;
       //     print("in enable ping");
        }
        [PunRPC]
        private void DisablePing()
        {
            isTransfering = false;
          //  Destroy()
         //   print("in disable ping");
        }



        public void userPing()
        {
            if (!isTransfering)
                photonView.RPC("EnablePing", PhotonTargets.All);
            else
                photonView.RPC("DisablePing", PhotonTargets.All);
        }
        // Update is called once per frame
        public void Update()
        {
            if (PhotonNetwork.isMasterClient)
            {
                transform.rotation = Quaternion.Lerp(transform.rotation, correctPlayerRot, Time.deltaTime * sync);
                if (isTransfering)
                {
                    rayCast();
                }//maybe add prediction to this model
            }
  
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

                 //   Destroy(pingPrefab);
                    
                    
                   // var pos = ray.GetPoint(m_RayLength);

                    // Debug.DrawRay(m_Camera.position, m_Camera.forward * m_DebugRayLength, Color.red, m_DebugRayDuration);
                    Quaternion normalHit = Quaternion.FromToRotation(Vector3.forward, hit.normal);
                    //GameObject clone = (GameObject)Instantiate(pingPrefab, hit.point, normalHit);

                    photonView.RPC("transferPing", PhotonTargets.Others, hit.point, normalHit);

                  //  Destroy(clone, 0.1f);
                   // m_ReticleTransform.rotation = Quaternion.FromToRotation(Vector3.forward, hit.normal);
                }

                // Something was hit, set at the hit position.
                //if (m_Reticle)
                //    m_Reticle.SetPosition(hit);

                //if (OnRaycasthit != null)
                //    OnRaycasthit(hit);
            }


        }
        [PunRPC]
        public void transferPing(Vector3 pos, Quaternion forwardV)
        {
            //pos.Normalize();
            pos.Scale(new Vector3 (0.99f,0.99f,0.99f));
            GameObject clone = (GameObject)Instantiate(pingPrefab, pos, forwardV);
            Destroy(clone, 0.1f);
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

