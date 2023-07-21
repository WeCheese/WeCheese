//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEditor;
////using Python.Runtime;
//using UnityEditor.Scripting.Python;

//public class MyPythonScript : MonoBehaviour
//{
//    /*
//    [MenuItem("Python/Hello World")]
//    static public void PrintHelloWorldFromPython()
//    {
//        PythonRunner.RunString(@"
//                import UnityEngine;
//                UnityEngine.Debug.Log('hello world')
//                ");
//    }
//    */


//    [MenuItem("MyPythonScript/Run")]
//    public static Vector3 Run(Vector3 vec)
//    {
//        PythonRunner.EnsureInitialized();
//        using (Py.GIL())
//        {
//            try
//            {
//                dynamic sys = Py.Import("sys");
//                UnityEngine.Debug.Log($"python version: {sys.version}");
//                return vec * 5.0f;
//            }
//            catch (PythonException e)
//            {
//                UnityEngine.Debug.LogException(e);
//                return vec * (-1.0f);
//            }
//        }
//    }

//}
