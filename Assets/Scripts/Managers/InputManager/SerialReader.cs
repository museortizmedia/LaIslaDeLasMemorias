using UnityEngine;
using System.IO.Ports;
using System.Threading;
using UnityEngine.Events;
using System.Text.RegularExpressions;

public class SerialReader : MonoBehaviour
{
    public bool _isSimulation;
    private SerialPort serialPort;
    private Thread readThread;
    private string receivedData = "";
    [SerializeField] UnityEvent<ButtonData> OnDataRecive;
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
        Match matchButton = Regex.Match(receivedData, @"\[00-00\]");
        Match matchAPI = Regex.Match(receivedData, @"\[s:(.*?)\-r:(.*?)\]");

        //recibe una orden de interacción
        if (matchButton.Success)
        {
            Debug.Log("Secuencia detectada: " + receivedData);
            OnDataRecive?.Invoke(new ButtonData { DeviceId = short.Parse(receivedData.Split("-")[0]), ButtonId = short.Parse(receivedData.Split("-")[0]) });
            receivedData = ""; return;
        }

        //recibe una solicitud API
        if (matchAPI.Success)
        {
            string requestValue = matchAPI.Groups[1].Value;
            string responseValue = matchAPI.Groups[2].Value;

            Debug.Log("Secuencia [s: " + requestValue + " - r: " + responseValue + "] detectada");
            CerebroRequestResponse(requestValue, responseValue);
            receivedData = ""; return;
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
        string[] letters = command.ToString().Split();
        for (int i = 0; i < letters.Length; i++)
        {
            SendSerialPortData(char.Parse(letters[i]));
        }        
    }
}
