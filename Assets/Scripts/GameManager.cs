using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Ins { get; set; }

    [SerializeField] Transform craneTrf;
    [SerializeField] Transform[] randomPosTrf;
    [SerializeField] GameObject[] prizeObj;
    [SerializeField] float speed;
    [SerializeField] GameObject panelObj;

    int score, time = 60;

    [SerializeField] Text scoreTxt, resultTxt, timerTxt;

    public const string FRUIT = "fruit";
    public const string SCORE = "score";

    Vector3 pos, cranePos;

    [SerializeField] Camera secondCam;

    private void Awake()
    {
        Ins = this;
    }

    private void Start()
    {
        pos = Crane.Ins.transform.position;
        cranePos = craneTrf.position;

        //random position
        int startPos = Random.Range(0, randomPosTrf.Length);

        foreach (var i in prizeObj) {
            i.transform.position = randomPosTrf[startPos].position;
            
            startPos++;
            if (startPos==randomPosTrf.Length) {
                startPos = 0;
            }
        }

        StartCoroutine(CorTimer());

        IEnumerator CorTimer() {

            var wait = new WaitForSeconds(1f);

            while (time>0) { 
                yield return wait;

                time--;
                timerTxt.text = "TIme: "+time.ToString();
            }

            SetWinner();
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Crane.Ins.ChangeState(ClawState.close);
        }
    }

    private void FixedUpdate()
    {
        float translationX = Crane.Ins.state == ClawState.open ? Input.GetAxis("Vertical") * speed : 0;
        float translationZ = Crane.Ins.state == ClawState.open ? Input.GetAxis("Horizontal") * speed : 0;

        float y = 0;

        if (Crane.Ins.state == ClawState.close)
        {
            if (craneTrf.localPosition.y > -3)
            {
                y += speed * -10 * Time.deltaTime;
            }
            else
            {
                Crane.Ins.ChangeState(ClawState.rise);
            }
        }
        else if (Crane.Ins.state == ClawState.rise) {
            if (craneTrf.localPosition.y < pos.y) y += speed * 10 * Time.deltaTime;
            else Crane.Ins.ChangeState(ClawState.riseEnd);
        }

        if (Crane.Ins.state == ClawState.riseEnd)
        {
            craneTrf.position = Vector3.MoveTowards(craneTrf.position, cranePos, 1.5f * Time.deltaTime);
            if (Vector3.Distance(craneTrf.position, cranePos) == 0) {
                Crane.Ins.ChangeState(ClawState.open);
            }
        }
        else { 
            craneTrf.Translate(translationX, y, translationZ);
        }
    }

    public void SetScore() {
        score++;
        scoreTxt.text = "Fruit: " + score;
        if(score == prizeObj.Length) SetWinner();
    }

    public void SetWinner() {
        resultTxt.text = score == prizeObj.Length ? "Success" : "Fail";
        panelObj.SetActive(true);
    }

    public void TryAgain() {
        SceneManager.LoadScene(0);
    }

    public void SecondCamera() {
        secondCam.enabled = !secondCam.enabled;
    }

    public void Quit() {
        Application.Quit();
    }
}

public enum ClawState
{
    open, openDelay, close, riseDelay, rise, riseEnd
}
