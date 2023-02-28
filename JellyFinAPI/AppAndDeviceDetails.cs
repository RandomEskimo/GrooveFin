using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JellyFinAPI
{
    public record class AppAndDeviceDetails(
        string AppName,
        string Device,
        string DeviceID,
        string Version
    );
}
