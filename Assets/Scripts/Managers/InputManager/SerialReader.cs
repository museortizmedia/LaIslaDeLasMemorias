#define DEBUG

using UnityEngine;
using System.IO.Ports;
using System.Threading;
using System.Collections.Generic;
using System;
using UnityEngine.Events;
using System.Linq;

public class SerialReader : MonoBehaviour
{
    public List<string> MesasConectadas;
    [Header("Configuración del Puerto Serial")]
    [Tooltip("Intervalo para la lectura de dispositivos conectados por USB")]
    public int secondsToRefind = 2;
    [Tooltip("Intervalo para la lectura de botones en dispositivos")]
    public float buttonCodeReadInterval = 0.5f;
    [Tooltip("Codigos de indentificación de los dispositivos USB")]
    public string[] keywords = { "MESA_01", "MESA_02", "MESA_03", "MESA_04", "MESA_05", "MESA_06", "MESA_07", "MESA_08", "MESA_09", "MESA_10" };
    [Tooltip("Codigos de identificación de los botones de los dispositivos siendo el primer numero el id del dispositivo y el segundo el id del botón")]
    public string[] butoncodes =
    {
        "[01-01]", "[01-02]", "[01-03]", "[01-04]", "[01-05]", "[01-06]", "[01-07]", "[01-08]", "[01-09]", "[01-10]", "[01-11]", "[01-12]",
        "[02-01]", "[02-02]", "[02-03]", "[02-04]", "[02-05]", "[02-06]", "[02-07]", "[02-08]", "[02-09]", "[02-10]", "[02-11]", "[02-12]",
        "[03-01]", "[03-02]", "[03-03]", "[03-04]", "[03-05]", "[03-06]", "[03-07]", "[03-08]", "[03-09]", "[03-10]", "[03-11]", "[03-12]",
        "[04-01]", "[04-02]", "[04-03]", "[04-04]", "[04-05]", "[04-06]", "[04-07]", "[04-08]", "[04-09]", "[04-10]", "[04-11]", "[04-12]",
        "[05-01]", "[05-02]", "[05-03]", "[05-04]", "[05-05]", "[05-06]", "[05-07]", "[05-08]", "[05-09]", "[05-10]", "[05-11]", "[05-12]",
        "[06-01]", "[06-02]", "[06-03]", "[06-04]", "[06-05]", "[06-06]", "[06-07]", "[06-08]", "[06-09]", "[06-10]", "[06-11]", "[06-12]",
        "[07-01]", "[07-02]", "[07-03]", "[07-04]", "[07-05]", "[07-06]", "[07-07]", "[07-08]", "[07-09]", "[07-10]", "[07-11]", "[07-12]",
        "[08-01]", "[08-02]", "[08-03]", "[08-04]", "[08-05]", "[08-06]", "[08-07]", "[08-08]", "[08-09]", "[08-10]", "[08-11]", "[08-12]",
        "[09-01]", "[09-02]", "[09-03]", "[09-04]", "[09-05]", "[09-06]", "[09-07]", "[09-08]", "[09-09]", "[09-10]", "[09-11]", "[09-12]",
        "[10-01]", "[10-02]", "[10-03]", "[10-04]", "[10-05]", "[10-06]", "[10-07]", "[10-08]", "[10-09]", "[10-10]", "[10-11]", "[10-12]"
    };

    [Header("Only Read")]
    [SerializeField] private bool keepReading;
    [SerializeField] private bool keepReadingButtonCodes;
    private Dictionary<string, string> connectedPorts = new Dictionary<string, string>();
    private Thread serialThread;
    private Thread buttonCodeThread;

    [Header("Events")]
    public UnityEvent<int> OnHardwareJoin;
    public UnityEvent<int> OnHardwareLeave;
    public UnityEvent<ButtonData> OnButtonTouched;
    //[SerializeField] UnityEvent<PopOverController.PopOverInfo> OnGetTableState;


    #region DEBUG
    void DebugSerialAlert(string mensaje)
    {
#if DEBUG
        Debug.Log($"<color=red>[SERIAL ALERT]</color> {mensaje}");
#endif
    }
    void DebugSerialInfo(string mensaje)
    {
#if DEBUG
        Debug.Log($"<color=blue>[SERIAL INFO]</color> {mensaje}");
#endif
    }
    void DebugSerialWarning(string mensaje)
    {
#if DEBUG
        Debug.Log($"<color=yellow>[SERIAL WARNING]</color> {mensaje}");
#endif
    }
    #endregion



    void Start()
    {
        OnHardwareJoin.AddListener((int mesaId) => DebugSerialAlert("SR: conectada mesa: " + mesaId));
        OnHardwareLeave.AddListener((int mesaId) => DebugSerialAlert("SR: desconectada mesa: " + mesaId));
        StartButtonCodeReading();
    }

    void OnDisable()
    {
        StopReading();
        StopButtonCodeReading();
    }

    void OnEnable()
    {
        StartReading();
    }

    #region Serial Logic
    [ContextMenu("Start Find Ports")]
    private void StartReading()
    {
        keepReading = true;
        serialThread = new Thread(new ThreadStart(FindSerialPorts));
        serialThread.Start();
    }

    [ContextMenu("Stop Find Ports")]
    private void StopReading()
    {
        keepReading = false;
        if (serialThread != null && serialThread.IsAlive)
        {
            serialThread.Join();
        }
    }

    [ContextMenu("Start Read Button Codes")]
    private void StartButtonCodeReading()
    {
        keepReadingButtonCodes = true;
        buttonCodeThread = new Thread(new ThreadStart(ReadButtonCodesPeriodically));
        buttonCodeThread.Start();
    }

    [ContextMenu("Stop Read Button Codes")]
    private void StopButtonCodeReading()
    {
        keepReadingButtonCodes = false;
        if (buttonCodeThread != null && buttonCodeThread.IsAlive)
        {
            buttonCodeThread.Join();
        }
    }

    void FindSerialPorts()
    {
        while (keepReading)
        {
            string[] ports = SerialPort.GetPortNames();
            HashSet<string> newPorts = new(ports);

            // Detectar desconexiones
            List<string> portsToRemove = connectedPorts.Keys.Where(port => !newPorts.Contains(port)).ToList();
            foreach (string port in portsToRemove)
            {
                int mesaIndex = Array.IndexOf(keywords, connectedPorts[port]);
                if (mesaIndex >= 0)
                {
                    MesasConectadas.Remove(keywords[mesaIndex]);
                    OnHardwareLeave?.Invoke(mesaIndex);
                }
                connectedPorts.Remove(port);
            }

            // Revisar todos los puertos
            foreach (string portName in ports)
            {
                if (connectedPorts.ContainsKey(portName)) continue;

                SerialPort port = null;
                try
                {
                    port = new SerialPort(portName, 9600);
                    port.Open();
                    port.ReadTimeout = secondsToRefind * 1000;

                    string response = null;
                    try
                    {
                        response = port.ReadLine().Trim();
                    }
                    catch (TimeoutException) { }

                    if (response != null)
                    {
                        // Detectar y manejar keycodes
                        for (int i = 0; i < keywords.Length; i++)
                        {
                            if (response.Contains(keywords[i]))
                            {
                                // Conectar nueva mesa
                                int mesaIndex = i;
                                connectedPorts[portName] = keywords[i];
                                MesasConectadas.Add(keywords[i]);
                                OnHardwareJoin?.Invoke(mesaIndex);

                                // Enviar mensaje de conexión al puerto
                                SendCommandToPort("[CONECTED]", mesaIndex);
                                break;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    DebugSerialWarning($"Error accessing port {portName}: {ex.Message}");
                }
                finally
                {
                    if (port != null && port.IsOpen)
                    {
                        port.Close();
                    }
                }
            }

            // Actualizar la lista de puertos actuales
            Thread.Sleep(secondsToRefind * 1000);
        }
    }

    public void SendCommandToPort(string command, int index)
    {
        if (index >= 0 && index < keywords.Length)
        {
            string portName = connectedPorts.Keys.ElementAt(index);

            if (!string.IsNullOrEmpty(portName))
            {
                SerialPort port = null;
                try
                {
                    port = new SerialPort(portName, 9600);
                    port.Open();
                    port.WriteLine(command);
                    DebugSerialWarning($"Comando '{command}' enviado al puerto {portName} para {keywords[index]}");
                }
                catch (Exception ex)
                {
                    DebugSerialWarning($"Error enviando comando al puerto {portName}: {ex.Message}");
                }
                finally
                {
                    if (port != null && port.IsOpen)
                    {
                        port.Close();
                    }
                }
            }
            else
            {
                DebugSerialWarning($"Puerto no encontrado para el índice {index}");
            }
        }
        else
        {
            DebugSerialWarning($"Índice {index} fuera de rango");
        }
    }

    // Se adminsitra el DataAction por medio de u Queue porque no se puede ejecutar funciones en hilos que no sean el principal
    #region QUEUE Actions
    private Queue<System.Action> actionQueue = new Queue<System.Action>();

    void Update()
    {
        lock (actionQueue)
        {
            while (actionQueue.Count > 0)
            {
                var action = actionQueue.Dequeue();
                action();
            }
        }
    }

    void EnqueueAction(System.Action action)
    {
        lock (actionQueue)
        {
            actionQueue.Enqueue(action);
        }
    }
    #endregion

    void ReadButtonCodesPeriodically()
    {
        while (keepReadingButtonCodes)
        {
            foreach (string portName in connectedPorts.Keys.ToList())
            {
                SerialPort port = null;
                try
                {
                    port = new SerialPort(portName, 9600);
                    port.Open();
                    port.ReadTimeout = 1000;

                    string response = null;
                    try
                    {
                        response = port.ReadLine().Trim();
                    }
                    catch (TimeoutException) { }

                    if (response != null && Array.Exists(butoncodes, code => response.Contains(code)))
                    {
                        EnqueueAction(() => 
                        {
                            DebugSerialInfo($"Código de botón detectado: {response}");
                            DataActions(response);
                        });
                    }
                }
                catch (Exception ex)
                {
                    DebugSerialWarning($"Error accediendo al puerto {portName} para códigos de botón: {ex.Message}");
                }
                finally
                {
                    if (port != null && port.IsOpen)
                    {
                        port.Close();
                    }
                }
            }

            Thread.Sleep((int)(buttonCodeReadInterval * 1000));
        }
    }
    #endregion


    #region DATA ACTIONS
    void DataActions(string receivedData)
    {
        // Extraer la parte relevante de la cadena
        string buttonData = receivedData.Trim('[').Trim(']');
        string[] parts = buttonData.Split('-');
        if (parts.Length == 2)
        {
            short deviceId = short.Parse(parts[0]);
            short buttonId = short.Parse(parts[1]);

            OnButtonTouched?.Invoke(new ButtonData { DeviceId = deviceId, ButtonId = buttonId });
        }
        return;
    }
    #endregion
}
