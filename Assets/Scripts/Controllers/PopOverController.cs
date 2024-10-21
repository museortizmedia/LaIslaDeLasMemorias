using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PopOverController : MonoBehaviour
{
    [SerializeField] GameObject PopOverPrefab;
    GameObject _currentPopOver;

    public enum PopOverType{Confirmation, Warning, Error, Nude}
    public Sprite[] Icons;
    public PopOverType typeToCreate = PopOverType.Warning;

    public struct PopOverInfo{
        public string Title;
        public string Content;
    }

    public SerialReader SR;
    [SerializeField] int _currentTable;
    public int CurrentTable{
        get => _currentTable;
        set {
            _currentTable = value;
            //SR.SendSerialPortData(CerebroComds.TableState+"["+value+"]");
            /*if(SR._isSimulation){
                OnGetTableInfo(new PopOverController.PopOverInfo{
                    Title = $"Mesa {value}",
                    Content = $"Estado de la mesa {value}: Conectando..."
                });
            }*/
        }
    }
    private void Awake()
    {
        _currentPopOver = Instantiate(PopOverPrefab, transform);
    }
    private void Start()
    {
        gameObject.SetActive(false);
    }

    private void OnEnable() {
        MostrarPopOver();
    }
    private void OnDisable() {
        OcultarPopOver();
    }

    void MostrarPopOver(){
        _currentPopOver.SetActive(true);
        _currentPopOver.transform.position = new Vector3(_currentPopOver.transform.position.x, Input.mousePosition.y, _currentPopOver.transform.position.z);
    }

    void OcultarPopOver(){
        _currentPopOver.SetActive(false);
    }

    public void OnGetTableInfo(PopOverInfo info){
        if(_currentPopOver!=null){
            _currentPopOver.GetComponent<PopOverCreator>().CreatePopUP(info.Title, info.Content, Icons[(int)typeToCreate]??null);
        }
    }
}
