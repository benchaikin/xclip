using System;

namespace XClip.Core
{
    public interface IDataObjectConverter
    {
        Clip CreateClip(System.Windows.IDataObject data);
        System.Windows.IDataObject CreateDataObject(Clip clip);
        object GetClipObjectData(ClipObject clipObject);
    }
}
