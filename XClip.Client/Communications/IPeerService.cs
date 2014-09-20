using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using XClip.Client.Core;

namespace XClip.Client.Communications
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IService1" in both code and config file together.
    [ServiceContract]
    public interface IPeerService
    {
        [OperationContract]
        void SendClip(ClipMessage message);

        [OperationContract]
        Identity Greet(Identity identity);
    }

    
}
