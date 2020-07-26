using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using UnityEngine.Events;

public class Hurdle
{
    public int pos, power;
}

public class TrackSpawner : MonoBehaviour
{
    public AnimationCurve curve;
    public GameObject startPoint;
    public GameObject trackCube;
    public GameObject powerCube;
    public GameObject hurdleCube;
    public GameObject track;
    public int No_of_players;
    public GameObject player;
    public int no_of_hurdles;
    public Canvas Ready_popup;
    // Start is called before the first frame update
    Dictionary<string, GameObject[]> m_tracks = new Dictionary<string, GameObject[]>();
    Dictionary<string, GameObject> m_players = new Dictionary<string, GameObject>();
    Dictionary<string, int> m_player_pos = new Dictionary<string, int>();
    Dictionary<string, Hurdle[]> m_player_pow = new Dictionary<string, Hurdle[]>();
    int turn;
    bool ready, askingPlayer;
    public bool StartCountdown = false;

    ResetWeapon resetWeapon = new ResetWeapon();
    float trackDistance = 3.8f;
    float characterYaxisOffset = 0.7f;
    void Start()
    {
         InstantiateTrack();
         InstantiatePlayers();
       // RandomPowerPosition(1);
        /* GameObject[] obj =null;
         if (m_tracks.TryGetValue("player_2_Track", out obj)) {
             obj[21].transform.position = obj[21].transform.position+ Vector3.up;
         }*/

        /*
         * Instatiate character
         */
       
        turn = 1;
        ready = true;
        //askingPlayer = true;
        //Ready_popup.GetComponentInChildren<Text>().text = "Are you Ready Player_"+turn;
        StartCoroutine(StartGame());
        EventManager.AddShootListener(movePlayerListener);
        EventManager.AddReloadWeaponInvoker(this);
        
    }
    public void AddResetWeaponListener(UnityAction listener)
    {

        resetWeapon.AddListener(listener);
    }
    void InstantiatePlayers() {
        for (int i = 1; i <= No_of_players; i++)
        {
            GameObject[] obj = null;
            if (m_tracks.TryGetValue("player_" + i + "_Track", out obj))
            {
                GameObject pl = Instantiate(player, obj[0].transform.position + new Vector3(0, characterYaxisOffset, 0), Quaternion.identity);
                pl.name = "player_" + i;
                m_players.Add("player_" + i, pl);
                m_player_pos.Add("player_" + i, 0);

            }
        }


    }
    void InstantiateTrack() {
        for (int j = 1; j <= No_of_players; j++)
        {
            /*
            Instantiate tracks 
             */
            GameObject[] playerTrackArr = new GameObject[22];
            GameObject playerTrack = new GameObject("player_" + j + "_Track");
            // Instantiate(playerTrack);
            Vector3 pos = new Vector3(0, 0, 0);
            GameObject st = Instantiate(startPoint, pos + new Vector3(j * 3, 0, 0), Quaternion.identity);
            pos = st.transform.position;
            st.transform.parent = playerTrack.transform;
            st.name = "start_pos_" + j;
            playerTrackArr[0] = st;
           Hurdle[] hurdles= RandomPowerPosition(j);
            for (int i = 1; i <= 21; i++)
            {
                GameObject tc = Instantiate(trackCube, pos + new Vector3(0, 0, i * trackDistance), Quaternion.identity);
                tc.transform.parent = playerTrack.transform;
                tc.name = "block_" + i;
                playerTrackArr[i] = tc;
            }
            foreach(var hurdle in hurdles) {
                GameObject tc=null;
                if (hurdle.power % 2 == 1) {
                     tc = Instantiate(powerCube, pos + new Vector3(0, 0, hurdle.pos * trackDistance), Quaternion.identity);
                }
                else if (hurdle.power % 2 == 0)
                {
                     tc = Instantiate(hurdleCube, pos + new Vector3(0, 0, hurdle.pos * trackDistance), Quaternion.identity);
                }
                if (tc)
                {
                    Destroy(playerTrackArr[hurdle.pos]);
                    tc.transform.parent = playerTrack.transform;
                    tc.name = "block_" + hurdle.pos;
                    playerTrackArr[hurdle.pos] = tc;

                }
                
            }
            playerTrack.transform.SetParent(track.transform);

            m_tracks.Add("player_" + j + "_Track", playerTrackArr);
        }

    }
    Hurdle[] RandomPowerPosition(int playerNumber)
    {
        Hurdle[] hurdles = new Hurdle[no_of_hurdles];
        
        for (int i = 1; i <= no_of_hurdles; i++) {
            if (i == 1)
            {
                Hurdle hurdle = new Hurdle();
                hurdle.pos = UnityEngine.Random.Range(2, 20);
                hurdle.power = UnityEngine.Random.Range(1, 6);
                hurdles[i-1] = hurdle;
            }
            else {
                Hurdle hurdle = new Hurdle();
                hurdle.pos = UnityEngine.Random.Range(2, 20);
        Loopback:    while (true)
                {
                    for (int j = 0; j < i-1; j++)
                    {
                        if (Mathf.Abs(hurdle.pos - hurdles[j].pos) <= 3)
                        {
                            hurdle.pos = UnityEngine.Random.Range(2, 20);
                            goto Loopback;
                        }
                    }
                        break;     
                }
                
                hurdle.power = UnityEngine.Random.Range(1, 6);
                hurdles[i - 1] = hurdle;

            }

        }
        m_player_pow.Add("player_"+ playerNumber+ "_pow",hurdles);
        return hurdles;
    
    }
    private void Update()
    {
        if (ready&&!askingPlayer)
        {
            StartCoroutine(StartGame());
        }        
    }

    IEnumerator StartGame()
    {
        Ready_popup.GetComponentInChildren<Text>().text = "Are you Ready Player_" + turn;
        Ready_popup.gameObject.SetActive(true);
        askingPlayer = true;
        yield return new WaitForSeconds(2f);
        ReadyButtonPressed();
    }

    void movePlayerListener(int stepsToMove) {
        print("movePlayerListener");
        if (!ready) {
            return;
        }
        ready = false;
        askingPlayer = false;
        movePlayer(turn, stepsToMove, true);
        //  CheckForHurdle(turn,pos);
        turn += 1;
        if (turn > No_of_players)
        {
            turn = 1;
        }

    }
    void CheckForHurdle(int playerNumber, int currentPosition) {
        Hurdle[] hurdles;
        m_player_pow.TryGetValue("player_" + playerNumber + "_pow", out hurdles);
        foreach (var hurdle in hurdles) {
            if (currentPosition == hurdle.pos) {
                print("playerNumber: " + playerNumber + " power " + hurdle.power);
                if (hurdle.power == 2 || hurdle.power == 4 || hurdle.power == 6)
                {
                    movePlayer(playerNumber, -2, false);
                }
                
            }
        
        }

    }
    int getPressedKey() {
        int steps = 0;
        if (Input.GetKeyUp(KeyCode.Alpha1))
        {
            steps = 1;
        }
        else if (Input.GetKeyUp(KeyCode.Alpha2))
        {
            steps = 2;

        }
        else if (Input.GetKeyUp(KeyCode.Alpha3))
        {
            steps = 3;
        }
        else if (Input.GetKeyUp(KeyCode.Alpha4))
        {
            steps = 4;
        }
        else if (Input.GetKeyUp(KeyCode.Alpha5))
        {
            steps = 5;
        }
        else if (Input.GetKeyUp(KeyCode.Alpha6))
        {
            steps = 6;
        }
        return steps;
    }

    int movePlayer(int playerNumber, int steps,bool movingForward) {
      //  print("movePlayer called");
        GameObject current_Player;
        m_players.TryGetValue("player_" + playerNumber, out current_Player);
        int pos;
        m_player_pos.TryGetValue("player_" + playerNumber, out pos);
        pos += steps;
        GameObject[] track;
        m_tracks.TryGetValue("player_" + playerNumber + "_Track", out track);
        //StartCoroutine(animationtimer(steps, current_Player));
        if (movingForward)
        {
            current_Player.transform.GetComponent<Rigidbody>().DOMove(
            track[pos].transform.position + new Vector3(0, characterYaxisOffset, 0), (float)steps+.5f).OnStart(() =>
           {
               current_Player.GetComponent<Animator>().SetBool("jump", true);
          //     print("Animation started");
           }).OnComplete(() =>
           {
               current_Player.GetComponent<Animator>().SetBool("jump", false);
               ready = true;
           }).SetEase(curve);
            StartCoroutine(animationtimer(playerNumber, pos, current_Player));
        }
        else {
           StartCoroutine( animationFall(track[pos].transform.position + new Vector3(0, characterYaxisOffset, 0), current_Player));
        }
        
        m_player_pos.Remove("player_" + playerNumber);
        m_player_pos.Add("player_" + playerNumber, pos);
        return pos;
        
    }
    IEnumerator animationFall(Vector3 finalPos, GameObject current_Player) {
        current_Player.GetComponent<Animator>().SetBool("fall", true);
        yield return new WaitForSeconds(.5f);
        yield return new WaitForSeconds(2f);
        // yield return new WaitWhile(() => current_Player.GetComponent<Animator>().GetBool("fall") == true);
        current_Player.GetComponent<Animator>().SetBool("fall",false);
        current_Player.transform.position = finalPos;

    }
    IEnumerator animationtimer(int playerNumber,int currentPosition, GameObject current_Player) {
        yield return new WaitForSeconds(.5f);
        yield return new WaitWhile(() => current_Player.GetComponent<Animator>().GetBool("jump") == true);
        yield return new WaitForSeconds(1f);
       CheckForHurdle(playerNumber, currentPosition);

    }
    public void ReadyButtonPressed() {
        Ready_popup.gameObject.SetActive(false);
        gameObject.GetComponent<SwitchCamera>().ShootCameraEnable(true);
        Controller.Instance.DisplayCursor(false);
        resetWeapon.Invoke();
        StartCountdown = true;
       // GameObject.FindGameObjectWithTag("Weapon").GetComponent<Weapon>().Reset();
    }

}
