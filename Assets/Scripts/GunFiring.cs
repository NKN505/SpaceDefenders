using UnityEngine;

public class GunFiring : MonoBehaviour
{

    //Asignamos las variables para el gameobject, la posici�n donde sale el laser y la velocidad
    public GameObject laserBeam;
    public Transform spawnPoint;
    public float laserSpeed = 50f;


    //m�todo FireLaser para que se instancie el objeto (en este caso el l�ser) en una posici�n determinada y a una velocidad concreta. Cuando pasen 15 seg. Se eliminar� el gameObject.
    public void FireLaser()
    {
        GameObject spawnLaser = Instantiate(laserBeam);

        spawnLaser.transform.position = spawnPoint.position;
        spawnLaser.GetComponent<Rigidbody>()
            .AddForce (spawnPoint.forward*laserSpeed, ForceMode.Impulse);

        Destroy(spawnLaser, 15);
    }

}
