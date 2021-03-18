using System;

namespace ProjektASD.Models
{
    class Device // Class to define Device to use in AVL tree structure
    {
        // Height in AVL tree
        public int height;

        // Address and its integer representation to use in AVL tree search
        private string address;
        private uint addressInt;

        // This device subnet
        private Subnet subnet;

        // Left and right child to use in AVL tree structure
        private Device leftDevice, rightDevice;

        // Device parameter constructor
        public Device(string address, uint addressInt, Subnet subnet)
        {
            // AVL related atribs
            this.height = 1;

            // Set address and int representation of address
            this.address = address;
            this.addressInt = addressInt;

            // Connect to subnet
            this.subnet = subnet;
        }

        // GET/SET private attributes
        public Device LeftDevice
        {
            get => leftDevice;
            set => leftDevice = value;
        }
        public Device RightDevice
        {
            get => rightDevice;
            set => rightDevice = value;
        }
        public Subnet Subnet
        {
            get => subnet;
            set => subnet = value;
        }
        public string Address
        {
            get => address;
            set => address = value;
        }
        public uint AddressInt
        {
            get => addressInt;
            set => addressInt = value;
        }

        // Function to return height of specific Device
        public static int CheckHeight(Device device)
        {
            return device == null ? 0 : device.height;
        }

        // Function to calculate weight of specific Device in AVL tree
        public static int CheckDeviceWeight(Device device)
        {
            if (device == null)
            {
                return 0;
            }
            else
            {
                // Weight = height of left subtree - height of right subtree
                int weight = Device.CheckHeight(device.LeftDevice) - Device.CheckHeight(device.RightDevice);

                return weight;
            }
        }
    }
}
