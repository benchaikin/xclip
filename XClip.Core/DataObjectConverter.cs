using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Windows;

namespace XClip.Core
{
    public class DataObjectConverter
    {
        public Clip CreateClip(IDataObject data)
        {
            var result = new Clip();

            var formats = data.GetFormats(false);
            foreach (var format in formats)
            {
                if (format == DataFormats.MetafilePicture) continue;
                var clipObject = data.GetData(format);
                MemoryStream stream = clipObject as MemoryStream;
                bool isStream = true;
                if(stream == null)
                {
                    isStream = false;
                    stream = new MemoryStream();
                    var formatter = new BinaryFormatter();
                    formatter.Serialize(stream, clipObject);
                }

                result.Add(new ClipObject(format, stream.ToArray(), isStream));
                stream.Close();
            }

            return result;
        }

        public IDataObject CreateDataObject(Clip clip)
        {
            IDataObject result = new DataObject();

            foreach (ClipObject clipObject in clip)
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
