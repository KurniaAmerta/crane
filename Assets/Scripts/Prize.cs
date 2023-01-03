using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Prize : MonoBehaviour
{
    bool isCatch = false;

    [SerializeField] Rigidbody rb;

    private void Start()
    {
        isCatch = false;
    }

    public void SetCatch(bool _isCatch) {
        isCatch = _isCatch;
    }

    private void Update()
    {
        if (isCatch && (Crane.Ins.state == ClawState.rise || Crane.Ins.state == ClawState.riseEnd))
        {
            this.transform.position = Vector3.MoveTowards(this.transform.position, Crane.Ins.magnetObj.transform.position, 2f * Time.deltaTime);
            rb.useGravity = false;
        }
        if (Crane.Ins.state == ClawState.riseEnd) {
            rb.freezeRotation = true;
            rb.constraints = RigidbodyConstraints.FreezePositionY;
        }
        else if (Crane.Ins.state == ClawState.open) {
            rb.useGravity = true;
            rb.constraints = RigidbodyConstraints.None;
            isCatch = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(GameManager.SCORE)) {
            GameManager.Ins.SetScore();
        }
    }
}
