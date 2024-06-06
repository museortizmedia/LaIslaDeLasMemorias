using UnityEngine;
using System.IO.Ports;
using System.Threading;
using UnityEngine.Events;
using System.Text.RegularExpressions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Concurrent;

public class SerialReader : MonoBehaviour, IManager
{
    [Tooltip("Ignora la apertura del puerto serial")]
    public bool _isSimulation;
    [Tooltip("Si está activo mostrará en consola las acciones por el Serial")]
    public bool IsDebuging;
    private Thread readThread;
    private ConcurrentQueue<string> dataQueue = new ConcurrentQueue<string>();
    [SerializeField] UnityEvent<ButtonData> OnDataRecive;
    [Tooltip("Establece el estado de la conexión de Brain")]
    [SerializeField] UnityEvent<bool> OnBrainConnectState;
    [SerializeField] UnityEvent<PopOverController.PopOverInfo> OnGetTableState;

    private List<SerialPort> arduinoPorts = new List<SerialPort>();
    private Dictionary<string, SerialPort> identifiedArduinos = new Dictionary<string, SerialPort>();
    private string[] arduinoIdentifiers = { "ARDUINO_01", "ARDUINO_02", "ARDUINO_03", "ARDUINO_04", "ARDUINO_05", "ARDUINO_06", "ARDUINO_07", "ARDUINO_08", "ARDUINO_09", "ARDUINO_10" };

    private bool isRunning = true;


    //Iniciación, Finalización
    void Start()
    {
        #if UNITY_STANDALONE && !UNITY_EDITOR
        _isSimulation = false;
        #endif
        if (!_isSimulation)
        {
            StartCoroutine(InitializeAndIdentifyArduinos());
        }
    }

    void OnApplicationQuit()
    {
        Finalizar();
    }

    /*private void OnEnable()
    {
        if (!_isSimulation)
        {
            StartCoroutine(InitializeAndIdentifyArduinos());
        }
    }*/

    private void OnDisable()
    {
        Finalizar();
    }

    void Finalizar()
    {
        if (_isSimulation) { return; }
        isRunning = false;
        foreach (var port in arduinoPorts)
        {
            if (port.IsOpen)
            {
                port.Close();
            }
        }
        if (readThread != null && readThread.IsAlive)
        {
            readThread.Join();
        }
    }

    IEnumerator InitializeAndIdentifyArduinos()
    {
        string[] ports = SerialPort.GetPortNames();

        foreach (string port in ports)
        {
            SerialPort serialPort = new SerialPort(port, 9600);
            bool portOpened = false;

            try
            {
                serialPort.Open();
                serialPort.ReadTimeout = 2000;
                portOpened = true;
            }
            catch (UnauthorizedAccessException)
            {
                Debug.LogWarning("Access to port " + port + " denied.");
            }
            catch (Exception ex)
            {
                Debug.LogWarning("Error opening port " + port + ": " + ex.Message);
            }

            if (portOpened)
            {
                yield return new WaitForSeconds(2);

                try
                {
                    serialPort.DiscardInBuffer();
                    string message = serialPort.ReadLine();

                    foreach (string identifier in arduinoIdentifiers)
                    {
                        if (message.Contains(identifier))
                        {
                            identifiedArduinos[identifier] = serialPort;
                            arduinoPorts.Add(serialPort);
                            SendDataToArduino(identifier, "CONNECTED");
                            OnBrainConnectState?.Invoke(true);
                            break;
                        }
                    }
                }
                catch (TimeoutException)
                {
                    Debug.LogWarning("Error reading from port " + port + ": The operation has timed out.");
                    serialPort.Close();
                }
                catch (Exception ex)
                {
                    Debug.LogWarning("Error reading from port " + port + ": " + ex.Message);
                    serialPort.Close();
                }
            }

            if (!identifiedArduinos.ContainsValue(serialPort))
            {
                if (serialPort.IsOpen)
                {
                    serialPort.Close();
                }
            }
        }

        if (identifiedArduinos.Count == 0)
        {
            Debug.LogWarning("No Arduinos identified.");
        }
        else
        {
            readThread = new Thread(ReadSerialPorts);
            readThread.Start();
        }
    }

    public void SendDataToArduino(string identifier, string data)
    {
        if (identifiedArduinos.ContainsKey(identifier))
        {
            SerialPort port = identifiedArduinos[identifier];
            if (port.IsOpen)
            {
                try
                {
                    port.WriteLine(data);
                }
                catch (Exception ex)
                {
                    Debug.LogError("Failed to send data to " + identifier + ": " + ex.Message);
                }
            }
            else
            {
                Debug.LogWarning("Port for " + identifier + " is not open.");
            }
        }
        else
        {
            Debug.LogWarning("Arduino with identifier " + identifier + " not found.");
        }
    }

    void ReadSerialPorts()
    {
        while (isRunning)
        {
            foreach (var kvp in identifiedArduinos)
            {
                SerialPort port = kvp.Value;
                if (port.IsOpen)
                {
                    try
                    {
                        string data = port.ReadLine();
                        if (data.Contains("ARDUINO")) SendDataToArduino(kvp.Key, "CONNECTED");
                        if (data != "KEEP_ALIVE") dataQueue.Enqueue(kvp.Key + "/" + data);
                    }
                    catch (TimeoutException)
                    {
                        // Ignorar excepciones de tiempo de espera
                    }
                    catch (System.Exception e)
                    {
                        Debug.LogError(e.Message);
                    }
                }
            }
        }
    }

    void Update()
    {
        while (dataQueue.TryDequeue(out string data))
        {
            DataActions(data);
        }
    }

    void DataActions(string receivedData)
    {
        
        if (IsDebuging)
        {
            Debug.Log("Secuencia detectada: " + receivedData);
        }

        // Extraer la parte relevante de la cadena
        string buttonData = receivedData.Trim('[').Trim(']');
        string[] parts = buttonData.Split('/');

        if (parts.Length == 2)
        {
            string devicePart = parts[0];
            string buttonPart = parts[1];

            string[] deviceParts = devicePart.Split('_');
            if (deviceParts.Length == 2)
            {
                short deviceId = short.Parse(deviceParts[1]);
                short buttonId = short.Parse(buttonPart);

                OnDataRecive?.Invoke(new ButtonData { DeviceId = deviceId, ButtonId = buttonId });
            }
        }

        receivedData = "";
        return;
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
            case "TableState":
                PopOverController.PopOverInfo _PopOverInfo = new()
                {
                    Title = response.ToString().Split(",")[0],
                    Content =response.ToString().Split(",")[1]
                };
                OnGetTableState?.Invoke(_PopOverInfo);
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
        /// Envia una configuracion al Vibrador, agregue [value] para definir el poder. (0-255)
        /// </summary>
        ConfigVibration,
        /// <summary>
        /// Solicita el estado de una mesa específica, agregue [value] para definir la mesa a conectar.
        /// </summary>
        TableState,
    }

    /// <summary>
    /// Envia un dato char al cerebro, considere usar comandos predefinidos para acciones epecíficas.
    /// </summary>
    /// <param name="data">Char que desea enviar</param>
    public void SendSerialPortData(char data)
    {
        foreach (var port in identifiedArduinos.Values)
        {
            if (port.IsOpen)
            {
                try
                {
                    port.Write(data.ToString());
                }
                catch (Exception ex)
                {
                    Debug.LogError("Failed to send data: " + ex.Message);
                }
            }
        }
    }

    /// <summary>
    /// Envia comandos predefinidos en el enum CerebroComds de esta clase al cerebro.
    /// </summary>
    /// <param name="command">comando escogido del CerebroComds</param>
    public void SendSerialPortData(CerebroComds command)
    {
        if(IsDebuging){Debug.Log("Enviando comando: "+command);}
        string data = $"[{command}]";
        SendSerialPortData(data);
    }
    /// <summary>
    /// Envia un dato string al cerebro, considere usar comandos predefinidos para acciones epecíficas.
    /// </summary>
    /// <param name="data">string que desea enviar</param>
    public void SendSerialPortData(string data)
    {
        data = $"[{data}]";
        foreach (var port in identifiedArduinos.Values)
        {
            if (port.IsOpen)
            {
                try
                {
                    port.WriteLine(data);
                }
                catch (Exception ex)
                {
                    Debug.LogError("Failed to send data: " + ex.Message);
                }
            }
        }
    }
}
