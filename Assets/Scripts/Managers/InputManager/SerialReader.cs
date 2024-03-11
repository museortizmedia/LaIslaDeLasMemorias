using UnityEngine;
using System.IO.Ports;
using System.Threading;
using UnityEngine.Events;
using System.Text.RegularExpressions;

public class SerialReader : MonoBehaviour, IManager
{
    [Tooltip("Ignora la apertura del puerto serial")]
    public bool _isSimulation;
    [Tooltip("Si está activo mostrará en consola las acciones por el Serial")]
    public bool IsDebuging;
    private SerialPort serialPort;
    private Thread readThread;
    private string receivedData = "";
    [SerializeField] UnityEvent<ButtonData> OnDataRecive;
    [Tooltip("Establece el estado de la conexión de Brain")]
    [SerializeField] UnityEvent<bool> OnBrainConnectState;


    //Iniciación, Finalización
    void Start()
    {
        Iniciar();
    }
    void OnApplicationQuit()
    {
        Finalizar();
    }
    private void OnEnable() {
        Iniciar();
    }
    private void OnDisable() {
        Finalizar();
    }
    void Iniciar(){
        if(_isSimulation){return;}
        string[] ports = SerialPort.GetPortNames(); // Encuentra el puerto serial disponible
        if (ports.Length > 0)
        {
            serialPort = new SerialPort(ports[0], 9600);
            serialPort.Open();

            // Inicia un hilo para leer los datos entrantes
            readThread = new Thread(ReadSerialPortData);
            readThread.Start();
        }
        else
        {
            Debug.LogError("No se encontró ningún puerto serial disponible.");
        }
    }
    void Finalizar(){
        if(_isSimulation){return;}
        serialPort?.Close();
        readThread?.Abort();
    }
    //Inalizacion, Finalizacion

    void ReadSerialPortData()
    {
        while (true)
        {
            try
            {
                string data = serialPort.ReadLine();
                receivedData += data;

                DataActions(receivedData);
            }
            catch (System.Exception e)
            {
                Debug.LogError(e.Message);
            }
        }
    }
    void DataActions(string receivedData){
        // Verifica si la cadena recibida contiene la secuencia deseada
        Match matchAPI = Regex.Match(receivedData, @"\[s:(.*?)\-r:(.*?)\]");
        Match matchButton = Regex.Match(receivedData, @"\[\d{2}-\d{2}\]");



        //recibe una solicitud API
        if (matchAPI.Success)
        {
            string requestValue = matchAPI.Groups[1].Value;
            string responseValue = matchAPI.Groups[2].Value;

            if(IsDebuging){Debug.Log("Secuencia [s: " + requestValue + " - r: " + responseValue + "] detectada");}
            CerebroRequestResponse(requestValue, responseValue);
            receivedData = ""; return;
        }

                //recibe una orden de interacción
        if (matchButton.Success)
        {
            if(IsDebuging){Debug.Log("Secuencia detectada: " + receivedData);}
            OnDataRecive?.Invoke(new ButtonData { DeviceId = short.Parse(receivedData.Trim('[').Trim(']').Split("-")[0]), ButtonId = short.Parse(receivedData.Trim('[').Trim(']').Split("-")[1]) });
            receivedData = "";
            return;
        }
    }
    public void SimuleSerialData(string simuledData){
        DataActions(simuledData);
    }


    //Cerebro API
    void CerebroRequestResponse(string request, string response){
        switch (request.ToString())
        {
            case "Connect":
                OnBrainConnectState?.Invoke(response.ToString()=="true");
            break;
            case "":
            break;
            
            default:
            break;
        }
    }
    /// <summary>
    /// Recopilación de funciones predefinidas en el cerebro que realizan acciones epecíficas
    /// </summary>
    public enum CerebroComds
    {
        /// <summary>
        /// Envia una solicitud Connect, en el caso de ser positiva el
        /// </summary>
        Connect,
        /// <summary>
        /// Envia una configuracion al Vibrador, agrege [value] para definir el poder.
        /// </summary>
        ConfigVibration,
    }

    /// <summary>
    /// Envia un dato char al cerebro, considere usar comandos predefinidos para acciones epecíficas.
    /// </summary>
    /// <param name="data">Char que desea enviar</param>
    public void SendSerialPortData(char data)
    {
        if (serialPort != null && serialPort.IsOpen)
        {
            serialPort.Write(data.ToString());
        }
    }

    /// <summary>
    /// Envia comandos predefinidos en el enum CerebroComds de esta clase al cerebro.
    /// </summary>
    /// <param name="command">comando escogido del CerebroComds</param>
    public void SendSerialPortData(CerebroComds command)
    {
        if(IsDebuging){Debug.Log("Enviando comando: "+command);}
        if (serialPort != null && serialPort.IsOpen)
        {
            string[] letters = command.ToString().Split();
            for (int i = 0; i < letters.Length; i++)
            {
                SendSerialPortData(char.Parse(letters[i]));
            }
        }       
    }
    /// <summary>
    /// Envia un dato string al cerebro, considere usar comandos predefinidos para acciones epecíficas.
    /// </summary>
    /// <param name="data">string que desea enviar</param>
    public void SendSerialPortData(string data)
    {
        if (serialPort != null && serialPort.IsOpen)
        {
            string[] letters = data.Split();
            for (int i = 0; i < letters.Length; i++)
            {
                SendSerialPortData(char.Parse(letters[i]));
            }
        }
    }
}
