using UnityEngine;

public class GunFiring : MonoBehaviour
{

    //Asignamos las variables para el gameobject, la posición donde sale el laser y la velocidad
    public GameObject laserBeam;
    public Transform spawnPoint;
    public float laserSpeed = 50f;


    //método FireLaser para que se instancie el objeto (en este caso el láser) en una posición determinada y a una velocidad concreta. Cuando pasen 15 seg. Se eliminará el gameObject.
    public void FireLaser()
    {
        GameObject spawnLaser = Instantiate(laserBeam);

        spawnLaser.transform.position = spawnPoint.position;
        spawnLaser.GetComponent<Rigidbody>()
            .AddForce (spawnPoint.forward*laserSpeed, ForceMode.Impulse);

        Destroy(spawnLaser, 15);
    }

}
