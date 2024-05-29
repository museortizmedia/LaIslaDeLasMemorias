using UnityEngine;
using System.IO;
using System.Diagnostics;

public class PDFOpener : MonoBehaviour
{
    public string pdfFileName = "mi_archivo"; // Nombre del archivo PDF sin extensión

    public void OpenPDF()
    {
        // Cargar el archivo PDF desde Resources
        TextAsset pdfAsset = Resources.Load<TextAsset>(pdfFileName);
        if (pdfAsset == null)
        {
            UnityEngine.Debug.LogError("El archivo PDF no se encontró en la carpeta Resources.");
            return;
        }

        // Crear una ruta temporal para guardar el PDF
        string tempPath = Path.Combine(Application.persistentDataPath, pdfFileName + ".pdf");

        // Escribir el archivo PDF en la ruta temporal
        File.WriteAllBytes(tempPath, pdfAsset.bytes);

        // Abrir el archivo PDF
        if (Application.platform == RuntimePlatform.WindowsPlayer || Application.platform == RuntimePlatform.WindowsEditor)
        {
            Process.Start(new ProcessStartInfo(tempPath) { UseShellExecute = true });
        }
        else if (Application.platform == RuntimePlatform.OSXPlayer || Application.platform == RuntimePlatform.OSXEditor)
        {
            Process.Start("open", tempPath);
        }
        else
        {
            UnityEngine.Debug.LogError("Plataforma no soportada");
        }

        UnityEngine.Debug.Log($"PDF abierto desde {tempPath}");
    }
}