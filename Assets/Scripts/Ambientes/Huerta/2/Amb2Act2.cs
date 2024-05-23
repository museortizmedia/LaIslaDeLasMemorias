using MuseCoderLibrary;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class Amb2Act2 : ExperienceController
{
    [field:SerializeField]
    public bool IsRegaderaMoving{get;set;}
    public UI_ImageOscillator RegaderaOscilator;
    public GameObject FB_Positivo, FB_Negativo, FB_FIN;
    public Image SemillaImage;
    
    [SerializeField] float _speed = 200;
    public float Speed { get=>_speed; set{_speed = RegaderaOscilator.xSpeed = RegaderaOscilator.ySpeed = value;} }

    [SerializeField] TextMeshProUGUI _cartelTextMesh;
    [SerializeField] UI_FollowPath _semillaAnim;

    ScriptableActivitiesData.ActivityData _semillaEscogida;

    [SerializeField] bool _isMovingInX;

    public Vector3 targetPoint; // Punto objetivo
    public Vector2 radiusX, radiusY;

    [SerializeField] bool _hasInZone;

    [SerializeField] int estadoPlant;

    [SerializeField] bool _isWatering = true;


    public override void Start()
    {
        base.Start(); //llamada obligatoria al comenzar
        Debug.Log(Manager.GetManager<InputManager>().name);

        EscogerAleatoriamenteSemilla();
        
        //iniciar la animacion, al finalizar setear el letrero
        _semillaAnim.OnStop.AddListener(()=>{ _cartelTextMesh.text = _semillaEscogida.Name; });
        DesactiveInputManager();

    }
    void EscogerAleatoriamenteSemilla(){
        /*if(_semillaEscogida == null){
            SemillaImage.enabled=true;
            SemillaImage.sprite = ActivityData.Data[ ActivityData.Data.IndexOf(_semillaEscogida) + estadoPlant ].Imagen;
        }*/
        Debug.Log(_semillaEscogida);

        int indexEscogido = (ActivityData.Data.Count/4)+4;
        _semillaEscogida = ActivityData.Data[Random.Range(0, indexEscogido-1)];
    }
    public void StartRegadera()
    {
        RegaderaOscilator.oscillateX = true;
        ActiveInputManager();
    }    
    private void Update()
    {
        #if UNITY_EDITOR
        CheckCercania();
        #endif
    }
    public void UsersSelection()
    {
        
        if(IsRegaderaMoving)
        {
            if(_isMovingInX)
            {
                _selectedPosX = RegaderaOscilator.transform.position.x;
            }else{
                _selectedPosY = RegaderaOscilator.transform.position.y;
                IsRegaderaMoving=_isMovingInX=RegaderaOscilator.oscillateX=_isMovingInX=RegaderaOscilator.oscillateY=false;
                DesactiveInputManager();
                VerificarPosicion(_selectedPosX,_selectedPosY);
                return;
            }
            _isMovingInX=!_isMovingInX;
            RegaderaOscilator.oscillateX = _isMovingInX;
            RegaderaOscilator.oscillateY = !_isMovingInX;
        }else{
            RegaderaOscilator.oscillateX=RegaderaOscilator.oscillateY=false;
        }
        
    }
    public void IniciarNuevamente()
    {
        ActiveInputManager();
        _selectedPosX=_selectedPosY=0;
        _isMovingInX=RegaderaOscilator.oscillateX=true;
        RegaderaOscilator.oscillateY=false;
        IsRegaderaMoving=true;

        _isWatering = Random.Range(0,100) < ActivityData.AbsurdPercent;
    }
    [SerializeField] float _selectedPosX = 0, _selectedPosY = 0;
    void VerificarPosicion(float x, float y)
    {
        Debug.Log($"Posicion Guardada {x} y {y}");
        CheckCercania();
        if(_hasInZone){
            if(estadoPlant >= 4){
                EsperaYRealiza(5f, ()=>{FB_FIN.SetActive(true);});
                DesactiveInputManager();
                return;
            }

            //feedback positivo
            PositiveFeedback();
            _selectedPosX=_selectedPosY=0;
            estadoPlant++;
            SemillaImage.enabled=true;
            SemillaImage.sprite = ActivityData.Data[ ActivityData.Data.IndexOf(_semillaEscogida) + estadoPlant ].Imagen;
            
        }else{
            //feedback negativo
            NegativeFeedback();
        }
    }
    void CheckCercania(){
        _hasInZone = !(Vector3.Distance(RegaderaOscilator.transform.position, targetPoint) > (radiusY.y-radiusY.x));
    }

    void PositiveFeedback(){
        FB_Positivo.SetActive(true);
        IsRegaderaMoving=RegaderaOscilator.oscillateX=RegaderaOscilator.oscillateY=false;
    }

    void NegativeFeedback()
    {
        FB_Negativo.SetActive(true);
        IsRegaderaMoving=RegaderaOscilator.oscillateX=RegaderaOscilator.oscillateY=false;
    }

    public void Finalizar(){
        gameObject.AddComponent<UI_FadeTransition>().Iniciar(()=>{EndExperience();});
    }
    
    private void OnDrawGizmos()
    {
        Gizmos.color = _hasInZone?Color.blue:Color.red;
        Gizmos.DrawWireSphere(targetPoint, radiusX.y-radiusX.x);
    }
}
