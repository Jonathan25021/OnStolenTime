using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CountdownUI : MonoBehaviour
{
    #region Player
    [SerializeField]
    private Text uiText;

    [SerializeField]
    [Tooltip("The player that the timer corresponds to.")]
    private PlayerScript m_Player;
    #endregion

    private float timer;

    // Start is called before the first frame update
    private void Awake()
    {
        timer = 0;
    }

    // Update is called once per frame
    void Update()
    {
        timer = m_Player.currTimer;
        uiText.text = timer.ToString("F");
    }
}
