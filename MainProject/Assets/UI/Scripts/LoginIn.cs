using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public  class Credentials {
    string Number;
    string Password;
}
public class LoginIn : MonoBehaviour
{
    public InputField mobileNumberIF;
    public InputField passwordIF;
    Dictionary<string, string> credentials = new Dictionary<string, string>();    // Start is called before the first frame update
    void Start()
    {
        credentials.Add("9602001030", "yes");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
   public void CheckCredentials() {
        string num = mobileNumberIF.textComponent.text;
        string pass = passwordIF.textComponent.text;
        string val;
        if (credentials.TryGetValue(num, out val))
        {
            print("credentials found");
        if (val == pass)
            {
                SceneManager.LoadSceneAsync(1);
            }
        }
        else { 
        print(val+"credentials not found"); }

    }
}
