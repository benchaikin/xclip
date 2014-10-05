using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;
using System.Windows;

namespace XClip.Core
{
    public class DataObjectConverter : IDataObjectConverter
    {
        public Clip CreateClip(IDataObject data)
        {
            var result = new Clip()
            {
                HostName = Environment.MachineName,
                TimeStamp = DateTime.UtcNow,
                ClipObjects = new List<ClipObject>()
            };

            var formats = data.GetFormats(false);
            foreach (var format in formats)
            {
                try
                {
                    if (format == DataFormats.MetafilePicture || format == DataFormats.EnhancedMetafile) continue;
                    var clipObject = GetDataCrappyRetry(data, format);
                    MemoryStream stream = clipObject as MemoryStream;
                    bool isStream = true;
                    if (stream == null)
                    {
                        isStream = false;
                        stream = new MemoryStream();
                        var formatter = new BinaryFormatter();
                        formatter.Serialize(stream, clipObject);
                    }

                    result.ClipObjects.Add(new ClipObject(format, stream.ToArray(), isStream));
                    stream.Close();
                }
                catch (Exception)
                {
                    // We can't handle this format... log?
                }
            }

            return result;
        }

        private object GetDataCrappyRetry(IDataObject data, string format)
        {
            for (int i = 0; i < 10; i++)
            {
                try
                {
                    var clipObject = data.GetData(format);
                    return clipObject;
                }
                catch 
                { 
                    // Really lame but this clipboard is unreliable
                    Thread.Sleep(10);
                }
            }

            throw new Exception("Unable to access clipboard");
        }

        public IDataObject CreateDataObject(Clip clip)
        {
            IDataObject result = new DataObject();

            foreach (ClipObject clipObject in clip.ClipObjects)
            {
                result.SetData(clipObject.Format, GetClipObjectData(clipObject));
            }

            return result;
        }

        public object GetClipObjectData(ClipObject clipObject)
        {
            object data;

            if (clipObject.IsStream)
            {
                data = new MemoryStream(clipObject.Data);
            }
            else
            {
                var formatter = new BinaryFormatter();
                using (MemoryStream stream = new MemoryStream(clipObject.Data))
                {
                    data = formatter.Deserialize(stream);
                }
            }
            return data;
        }
    }
}
