using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HudPlayer : MonoBehaviour
{
    public int coinCount;
    public Text coinText;

    public LifebarPlayer lifebar;

    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        coinText.text = coinCount.ToString();
    }

}
