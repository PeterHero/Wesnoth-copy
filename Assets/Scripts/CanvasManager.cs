using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CanvasManager : MonoBehaviour
{
    [SerializeField] private TMP_Text type;
    [SerializeField] private TMP_Text health;
    [SerializeField] private TMP_Text experience;
    [SerializeField] private TMP_Text movement;
    [SerializeField] private TMP_Text defence;
    [SerializeField] private TMP_Text level;

    public string Type { set { type.text = value; }}
    public string Health { set { health.text = value; }}
    public string Experience { set { experience.text = value; }}
    public string Movement { set { movement.text = value; }}
    public string Defence { set { defence.text = value; }}
    public string Level { set { level.text = value; }}
}
