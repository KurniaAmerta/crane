using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crane : MonoBehaviour
{
    public static Crane Ins { get; private set; }

    public ClawState state { get; private set; } = ClawState.open;

    public GameObject magnetObj;

    private void Awake()
    {
        Ins = this;
    }

    public void ChangeState(ClawState _state) {

        if (_state != ClawState.rise)
        {
            state = _state;
        } 
        else if (_state == ClawState.open) {
            state = ClawState.openDelay;
            StartCoroutine(CorRise(2));
        }
        else {
            state = ClawState.riseDelay;
            StartCoroutine(CorRise(1));
        }

        IEnumerator CorRise(float wait)
        {
            yield return new WaitForSeconds(wait);
            state = _state;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.LogError(other.gameObject.name);
        if (other.CompareTag(GameManager.FRUIT) && other.gameObject.GetComponent<Prize>()) {
            other.gameObject.GetComponent<Prize>().SetCatch(true);
        }
    }
}
