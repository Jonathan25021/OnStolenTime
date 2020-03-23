using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CountdownUI : MonoBehaviour
{
    #region Player
    [SerializeField]
    [Tooltip("The location for the sprite to be displayed.")]
    private Image m_Image;

    [SerializeField]
    [Tooltip("The location for the time to be displayed.")]
    private Text m_Text;

    [SerializeField]
    [Tooltip("The player that the timer corresponds to.")]
    private PlayerScript m_Player;

    [SerializeField]
    [Tooltip("The different sprite stages.")]
    private Sprite[] m_Sprites;
    #endregion

    int slice = 0;

    private void Start()
    {
        int diffSprites = m_Sprites.Length;
        slice = m_Player.startTimer / diffSprites;
        m_Text.text = m_Player.startTimer.ToString();
        m_Image.sprite = m_Sprites[0];
    }

    // Update is called once per frame
    void Update()
    {
        float portion = m_Player.currTimer / slice;
        int rounded = Mathf.RoundToInt(portion);
        m_Image.sprite = m_Sprites[rounded - 1];
        m_Text.text = (Mathf.Round(m_Player.currTimer * 100.0f) / 100.0f).ToString();
    }
}
