using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using FTD2XX_NET;

namespace CommunicationAPI
{
    public class Class1
    {
        uint _ftdiDeviceCount = 0;

        public uint FtdiDeviceCount => _ftdiDeviceCount;
        public FTDI.FT_STATUS Status { get; } = FTDI.FT_STATUS.FT_OK;

        FTDI device = new FTDI();
        private FTDI.FT_DEVICE_INFO_NODE[] ftdiDeviceList;

        public Class1()
        {
            device.GetNumberOfDevices(ref _ftdiDeviceCount);
            if (FtdiDeviceCount != 0)
                ftdiDeviceList = new FTDI.FT_DEVICE_INFO_NODE[FtdiDeviceCount];
        }



    }
}
