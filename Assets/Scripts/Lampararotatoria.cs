using UnityEngine;

public class Lampararotatoria : MonoBehaviour
{
    [SerializeField] private Vector3 ejeRot = Vector3.up;
    public float rotSpeed;

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(ejeRot * rotSpeed * Time.deltaTime);
    }
}
