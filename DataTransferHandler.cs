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
        private GameObject clone;

        void Start()
        {
            sync = 8;
            clone = GameObject.Find("userPing");// pingPrefab;// (GameObject)Instantiate(pingPrefab,new Vector3(0,122,12),Quaternion.identity);
           // clone.GetComponent<PhotonView>().RequestOwnership();
        }


        [PunRPC]
        private void EnablePing()
        {
            
            isTransfering = true;
            //  print("in enable ping");
            SpriteRenderer sr = clone.GetComponentInChildren<SpriteRenderer>();
            Color c = sr.color;
            c.a = 255;
            sr.GetComponent<SpriteRenderer>().material.SetColor("_Color", c);
        }

        [PunRPC]
        private void DisablePing()
        {
            isTransfering = false;
            SpriteRenderer sr = clone.GetComponentInChildren<SpriteRenderer>();
            Color c = sr.color;
            c.a = 0;
            sr.GetComponent<SpriteRenderer>().material.SetColor("_Color", c);

            //  Destroy()
            //    print("in disable ping");
        }



        public void userPing()
        {
            if (!isTransfering)
            {
               // clone.GetComponent<PhotonView>().RequestOwnership();
                clone.GetComponent<PhotonView>().RequestOwnership();
                photonView.RPC("EnablePing", PhotonTargets.All);
            }
            else
                photonView.RPC("DisablePing", PhotonTargets.All);
        }
        // Update is called once per frame
        public void Update()
        {
            if (PhotonNetwork.isMasterClient)
            {
                transform.rotation = Quaternion.Lerp(transform.rotation, correctPlayerRot, Time.deltaTime * sync);
                //transform.position = Vector3.Lerp(transform.position, pingPos, Time.deltaTime * sync);
                //print(pingPos);
                if (isTransfering)
                {
                    rayCast();
                }//maybe add prediction to this model
            }

        }

        public void rayCast()
        {
            //print("in raycast");
            Vector3 forward = transform.rotation * Vector3.forward;
            RaycastHit hit;
            Ray ray = new Ray(transform.position, forward);

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

                    photonView.RPC("transferPing", PhotonTargets.All, hit.point, normalHit);

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
        public void transferPing(Vector3 pos, Quaternion forwardN)
        {
            //pos.Normalize();
            //if (!PhotonNetwork.isMasterClient)
            //{
            //   print(pingPos);
            //if (clone.GetComponent<PhotonView>().ownerId != PhotonNetwork.player.ID)
            //{
                pos.Scale(new Vector3(0.99f, 0.99f, 0.99f));
                //clone = (GameObject)Instantiate(pingPrefab, pos, forwardN);
                clone.transform.position = pos;
                clone.transform.rotation = forwardN;
          //  }
      
        //    }

            //if (!clone)
            //{
            //   Destroy(clone, 0.1f);
            //}
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
                //if(!PhotonNetwork.isMasterClient)
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

