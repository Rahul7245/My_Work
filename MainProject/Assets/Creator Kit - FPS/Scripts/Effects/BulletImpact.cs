using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletImpact : MonoBehaviour
{
    public GameObject bulletHole;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
  //  private void OnCollisionEnter(Collision collision)
  //  {
        /* ImpactManager impactManager=ImpactManager.Instance;
         Instantiate(bulletHole, impactManager.GetImpactPosition(), Quaternion.FromToRotation(Vector3.up, impactManager.GetImpactNormal()));
         if (collision.gameObject.tag=="Burgler")
             impactManager.PlayImpact();*/
      /*  ImpactManager impactManager = ImpactManager.Instance;
        Instantiate(bulletHole, impactManager.GetImpactPosition(), Quaternion.FromToRotation(Vector3.up, impactManager.GetImpactNormal()));
        if (collision.gameObject.tag == "Burgler")
        {
            impactManager.PlayImpact();
            
            impactManager.InvokeTheEvent(collision.gameObject.GetComponent<Burgler>().getValue());

        }
            
    }*/
  /*  private void OnCollisionExit(Collision collision)
    {
         ImpactManager impactManager=ImpactManager.Instance;
        Instantiate(bulletHole, impactManager.GetImpactPosition(), Quaternion.FromToRotation(Vector3.up, impactManager.GetImpactNormal()));
        if (collision.gameObject.tag=="Burgler")
            impactManager.PlayImpact();
    }*/
}
