using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public int coinAmount;
    [SerializeField] float progressedDepth;
    [SerializeField] TextMeshProUGUI depthUI;

    [SerializeField] Transform player;
    PlayerContoller playCont;

    [SerializeField] Transform tunnelTran;
    [SerializeField] Object obstaclePrfb, boardPrfb, coinPrefab;

    [SerializeField] float fallSpeed;
    [SerializeField] float speedMultiplier = 0;
    float targSpeedMultiplier;
    float speedDampVel = 0;
    float speedDampTime = 0.5f;

    float timestamp;
    [SerializeField] float delay;
    [SerializeField] float tunnelSize;
    [SerializeField] float spawnDepth;
    [SerializeField] float spawnAmount;

    [SerializeField] float CoinFreq;
    float coinTimestamp;

    [SerializeField] Material scrollingMat;

    bool dead;

    // Start is called before the first frame update
    void Awake()
    {
        Time.timeScale = 1;
        timestamp = Time.time + delay;
        scrollingMat.SetFloat("Vector1_79A07576", fallSpeed);
        playCont = player.gameObject.GetComponent<PlayerContoller>();
    }

    public IEnumerator DEATH()
    {
        dead = true;
        player.gameObject.SetActive(false);
        fallSpeed = fallSpeed / 4;
        Camera.main.transform.position = Camera.main.transform.position + Vector3.up * 5;
        yield return new WaitForSecondsRealtime(3);
        Reset();
    }

    public void Reset()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    // Update is called once per frame
    void Update()
    {
        if (playCont.fast) { targSpeedMultiplier = 2.5f; } else { targSpeedMultiplier = 1; }
        speedMultiplier = /*tmpSpeedMultiplier;*/ Mathf.SmoothDamp(speedMultiplier, targSpeedMultiplier, ref speedDampVel, speedDampTime);

        if (Time.time >= timestamp)
        {
            for(int i = 0; i < spawnAmount; i++)
            {
                GameObject spawnOb = GameObject.Instantiate(obstaclePrfb, tunnelTran) as GameObject;
                spawnOb.transform.position = new Vector3(Random.Range(-tunnelSize / 2, tunnelSize / 2), spawnDepth, Random.Range(-tunnelSize / 2, tunnelSize / 2));
                spawnOb.transform.eulerAngles = new Vector3(0, Random.Range(0, 360), 0);
                spawnOb.transform.parent = tunnelTran;
            }

            GameObject spawnOb2 = GameObject.Instantiate(boardPrfb, tunnelTran) as GameObject;
            spawnOb2.transform.position = new Vector3(Random.Range(-tunnelSize / 2, tunnelSize / 2), spawnDepth, Random.Range(-tunnelSize / 2, tunnelSize / 2));
            spawnOb2.transform.eulerAngles = new Vector3(0, Random.Range(0, 360), 0);
            spawnOb2.transform.parent = tunnelTran;

            timestamp = Time.time + delay / speedMultiplier;
        }

        if (Time.time >= coinTimestamp)
        {
            GameObject coin = Instantiate(coinPrefab, tunnelTran) as GameObject;
            coin.transform.position = new Vector3(Random.Range(-tunnelSize / 2, tunnelSize / 2), spawnDepth, Random.Range(-tunnelSize / 2, tunnelSize / 2));

            coinTimestamp = Time.time + delay / speedMultiplier;
        }


        if (tunnelTran.childCount > 0)
        {
            for (int i = 0; i < tunnelTran.childCount; i++)
            {
                Transform ob = tunnelTran.GetChild(i);
                float speed = fallSpeed * speedMultiplier;
                ob.transform.Translate(Vector3.up * speed * Time.deltaTime);

                if(ob.position.y > player.position.y + 10)
                {
                    Destroy(ob.gameObject);
                }
            }
        }

        if (!dead) {
            progressedDepth += Time.deltaTime * fallSpeed * speedMultiplier;
            depthUI.text = progressedDepth.ToString("(0m)");
        }

        Camera.main.fieldOfView = Mathf.Clamp(60 * speedMultiplier, 60, 80);

        float scrollSpeed = Time.time * fallSpeed * speedMultiplier;
        scrollingMat.SetFloat("Vector1_79A07576", 0.016f * scrollSpeed);
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireCube(Vector3.zero, new Vector3(tunnelSize, spawnDepth, tunnelSize));
    }
}
