using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ProBuilder;

public class Rikayon_Spit : MonoBehaviour
{
    private AI_Agent_Rikayon _rikayon;
    [SerializeField] private GameObject _toxicSpit_Prefab;

    private void Start()
    {
        _rikayon = GameObject.FindObjectOfType<AI_Agent_Rikayon>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Ground"))
        {
            switch (_rikayon._currentBossStage)
            {
                case 0:
                    GameObject toxicSpit;
                    toxicSpit = Instantiate(_toxicSpit_Prefab, new Vector3(transform.position.x, other.transform.position.y + other.GetComponent<Renderer>().bounds.max.y, transform.position.z), Quaternion.identity);
                    Destroy(gameObject);
                    break;
                case 1:
                    break;
                case 2:
                    break;
            }
        }
    }
}
