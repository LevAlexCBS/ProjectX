using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using FTD2XX_NET;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            //var timer = new Timer(CheckStatus, null, 500, 250);

            //Console.WriteLine(timer.GetLifetimeService());

            UInt32 ftdiDeviceCount = 0;
            FTDI.FT_STATUS ftStatus = FTDI.FT_STATUS.FT_OK;

            // Create new instance of the FTDI device class
            FTDI myFtdiDevice = new FTDI();
            // Open first device in our list by serial number
            ftStatus = myFtdiDevice.OpenBySerialNumber("A99C59XB");
            if (ftStatus != FTDI.FT_STATUS.FT_OK)
            {
                // Wait for a key press
                Console.WriteLine("Failed to open device (error " + ftStatus.ToString() + ")");
                Console.ReadKey();
                return;
            }

            // Set up device data parameters
            // Set Baud rate to 9600
            ftStatus = myFtdiDevice.SetBaudRate(9600);
            if (ftStatus != FTDI.FT_STATUS.FT_OK)
            {
                // Wait for a key press
                Console.WriteLine("Failed to set Baud rate (error " + ftStatus.ToString() + ")");
                Console.ReadKey();
                return;
            }

            // Set data characteristics - Data bits, Stop bits, Parity
            ftStatus = myFtdiDevice.SetDataCharacteristics(FTDI.FT_DATA_BITS.FT_BITS_8, FTDI.FT_STOP_BITS.FT_STOP_BITS_1, FTDI.FT_PARITY.FT_PARITY_NONE);
            if (ftStatus != FTDI.FT_STATUS.FT_OK)
            {
                // Wait for a key press
                Console.WriteLine("Failed to set data characteristics (error " + ftStatus.ToString() + ")");
                Console.ReadKey();
                return;
            }

            Console.WriteLine("Ready for write");
            Console.ReadKey();
            // Perform loop back - make sure loop back connector is fitted to the device
            // Write string data to the device
            //string dataToWrite = "Hello world!";
            byte[] greating = {1, 159};
            UInt32 numBytesWritten = 0;
            // Note that the Write method is overloaded, so can write string or byte array data
            ftStatus = myFtdiDevice.Write(greating, greating.Length, ref numBytesWritten);
            if (ftStatus != FTDI.FT_STATUS.FT_OK)
            {
                // Wait for a key press
                Console.WriteLine("Failed to write to device (error " + ftStatus.ToString() + ")");
                Console.ReadKey();
                return;
            }


            //Check the amount of data available to read
            // In this case we know how much data we are expecting,
            // so wait until we have all of the bytes we have sent.
            UInt32 numBytesAvailable = 0;
            do
            {
                ftStatus = myFtdiDevice.GetRxBytesAvailable(ref numBytesAvailable);
                if (ftStatus != FTDI.FT_STATUS.FT_OK)
                {
                    // Wait for a key press
                    Console.WriteLine("Failed to get number of bytes available to read (error " + ftStatus.ToString() + ")");
                    Console.ReadKey();
                    return;
                }
                Thread.Sleep(10);
            } while (numBytesAvailable < greating.Length);

            //// Now that we have the amount of data we want available, read it
            //string readData;
            byte[] inbuf = new byte[2];
            UInt32 numBytesRead = 0;
            // Note that the Read method is overloaded, so can read string or byte array data
            ftStatus = myFtdiDevice.Read(inbuf, numBytesAvailable, ref numBytesRead);
            if (ftStatus != FTDI.FT_STATUS.FT_OK)
            {
                // Wait for a key press
                Console.WriteLine("Failed to read data (error " + ftStatus.ToString() + ")");
                Console.ReadKey();
                return;
            }

            foreach (var readData in inbuf)
            {
                Console.WriteLine(readData);
            }

            // Close our device
            ftStatus = myFtdiDevice.Close();

            // Wait for a key press
            Console.WriteLine("Press any key to continue.");
            Console.ReadKey();


            Console.ReadKey();
        }

        public static void CheckStatus(object state)
        {
            UInt32 ftdiDeviceCount = 0;
            FTDI.FT_STATUS ftStatus = FTDI.FT_STATUS.FT_OK;

            // Create new instance of the FTDI device class
            FTDI myFtdiDevice = new FTDI();

            // Determine the number of FTDI devices connected to the machine
            ftStatus = myFtdiDevice.GetNumberOfDevices(ref ftdiDeviceCount);
            // Check status

            // Allocate storage for device info list
            FTDI.FT_DEVICE_INFO_NODE[] ftdiDeviceList = new FTDI.FT_DEVICE_INFO_NODE[ftdiDeviceCount];

            // Populate our device list
            ftStatus = myFtdiDevice.GetDeviceList(ftdiDeviceList);

            if (ftStatus == FTDI.FT_STATUS.FT_OK)
            {
                for (UInt32 i = 0; i < ftdiDeviceCount; i++)
                {
                    if(ftdiDeviceList[i].SerialNumber == "A99C59XB")
                        Console.WriteLine("Device detected");
                }
            }
        }
    }
}
