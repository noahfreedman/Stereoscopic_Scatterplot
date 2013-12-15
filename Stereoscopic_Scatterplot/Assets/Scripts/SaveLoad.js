import System;
import System.IO;
import System.IO.Stream;
import System.Runtime.Serialization;
import System.Runtime.Serialization.Formatters.Binary;

class SaveLoad {
        public static function SaveFile(filename:String, obj:Object):void {
                try {
                        //Debug.Log("Writing Stream to Disk.", SaveLoad);
                        var fileStream:Stream = File.Open(filename, FileMode.Create);
                        var formatter:BinaryFormatter = new BinaryFormatter();
                        formatter.Serialize(fileStream, obj);
                        fileStream.Close();
                } catch(e:Exception) {
                        Debug.LogWarning("Save.SaveFile(): Failed to serialize object to a file " + filename + " (Reason: " + e.ToString() + ")");
                }
        }

        public static function LoadFile(filename:String):Object {
                try {
                        //Debug.Log("Reading Stream from Disk.", SaveLoad);
                        Debug.Log("Reading Stream from Disk.");
                        var fileStream:Stream = File.Open(filename, FileMode.Open, FileAccess.Read);
                        var formatter:BinaryFormatter = new BinaryFormatter();
                        var obj:Object= formatter.Deserialize(fileStream);
                        fileStream.Close();
                        return obj;
                } catch(e:Exception) {
                        Debug.LogWarning("SaveLoad.LoadFile(): Failed to deserialize a file " + filename + " (Reason: " + e.ToString() + ")");
                        return null;
                }       
        }
}