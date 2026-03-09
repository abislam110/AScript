using UnityEngine;
using System.Collections;
using System.Threading;
using System.Threading.Tasks;
using TMPro;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System;
public class Translator : MonoBehaviour
{
    string script = "(Namek)\n[<Goku><Vegeta>] = Hello there! Would you like to see Super Saiyan 5?\n[<Vegeta><Goku>] = Hmph! I don't have time for your games, Goku.\n[<Goku><Vegeta>] = You need to shut up Vegeta.\n*<Vegeta> walks to <Goku>*\n[<Vegeta><Goku>] = Power isn't everything. We need to focus on training, not transformations.\n[<Goku><Vegeta>] = Alright, suit yourself. But you're missing out!";
    public TMP_Text text;
    public Camera mainCamera;
    public GameObject goku;
    private Vector3 characterPosition;
    public GameObject vegeta;
    private UnityEngine.AI.NavMeshAgent gokuAgent;
    private UnityEngine.AI.NavMeshAgent vegetaAgent;
    private GameObject character1;
    private GameObject character2;
    private Dictionary<string, GameObject> characterDict = new Dictionary<string, GameObject>();
    private bool camParent = false;
    
    void Awake()
    {
        characterDict.Add("Goku", goku);
        characterDict.Add("Vegeta", vegeta);
        gokuAgent = goku.GetComponent<UnityEngine.AI.NavMeshAgent>();
        vegetaAgent = vegeta.GetComponent<UnityEngine.AI.NavMeshAgent>();
        gokuAgent.enabled = false;
        vegetaAgent.enabled = false;
    }
    async void Start()
    {
        //Debug.Log(AIController.responseMessage);
        string[] lines = script.Split('\n');
        foreach(string line in lines)
        {
            
            Debug.Log(line);
            if(camParent)
            {
                mainCamera.GetComponent<Transform>().parent = null;
                mainCamera.GetComponent<Transform>().rotation = Quaternion.Euler(mainCamera.GetComponent<Transform>().rotation.eulerAngles.x, 0, mainCamera.GetComponent<Transform>().rotation.eulerAngles.z);
                camParent = false;
                if(character1 != null)
                {
                    character1.GetComponent<Animator>().SetBool("walking", false);
                    character1 = null;
                }
            }
            if(line.StartsWith("(Namek)"))
            {
                text.text = "Setting scene to Namek...";
            }
            else if(line.StartsWith("[<"))
            {
                string characterName1 = line.Substring(line.IndexOf("<") + 1, line.IndexOf(">") - line.IndexOf("<") - 1);
                string characterName2 = line.Substring(line.IndexOf("<", line.IndexOf(">")) + 1, line.LastIndexOf(">") - line.IndexOf("<", line.IndexOf(">")) - 1);
                character1 = characterDict[characterName1];
                character2 = characterDict[characterName2];
                mainCamera.GetComponent<Transform>().position = new Vector3(character1.GetComponent<Transform>().position.x, character1.GetComponent<Transform>().position.y + 59.7f, character1.GetComponent<Transform>().position.z - 57.1f);
                character1.GetComponent<Transform>().LookAt(character2.GetComponent<Transform>());
                text.text = characterName1 + ":" + line.Substring(line.IndexOf("] = ") + 3);
            }
            else if(line.StartsWith("*"))
            {
                string characterName1 = line.Substring(line.IndexOf("<") + 1, line.IndexOf(">") - line.IndexOf("<") - 1);
                string characterName2 = line.Substring(line.LastIndexOf("<") + 1, line.LastIndexOf(">") - line.LastIndexOf("<") - 1);
                character1 = characterDict[characterName1];
                character2 = characterDict[characterName2];
                mainCamera.GetComponent<Transform>().position = new Vector3(character1.GetComponent<Transform>().position.x, character1.GetComponent<Transform>().position.y + 59.7f, character1.GetComponent<Transform>().position.z - 57.1f);
                UnityEngine.AI.NavMeshAgent agent1 = character1.GetComponent<UnityEngine.AI.NavMeshAgent>();
                agent1.enabled = true;
                if(Physics.Raycast(character1.GetComponent<Transform>().position, character2.GetComponent<Transform>().position - character1.GetComponent<Transform>().position, out RaycastHit hit))
                {
                    Debug.Log("Hit: " + hit.collider.gameObject.name);
                }
                character1.GetComponent<Animator>().SetBool("walking", true);
                agent1.SetDestination(new Vector3(hit.point.x, character2.GetComponent<Transform>().position.y, hit.point.z));
                mainCamera.GetComponent<Transform>().parent = character1.GetComponent<Transform>();
                camParent = true;
            }
            else
            {
                text.text = line;
            }
            await Task.Delay(2000);
        }
    }

    
}
