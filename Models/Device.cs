using System;

namespace ProjektASD
{
    class Device // Class to define Device to use in AVL tree structure
    {
        // Height in AVL tree
        public int height;

        // Address and its integer representation to use in AVL tree search
        public string address;
        public uint addressInt;

        // This device subnet
        public Subnet subnet;

        // Left and right child to use in AVL tree structure
        public Device leftDevice, rightDevice;

        // Device parameter constructor
        public Device(string address, uint addressInt, Subnet subnet)
        {
            this.height = 1;
            this.subnet = subnet;

            // Set address and subnetAddress
            this.address = address;
            this.addressInt = addressInt;
        }

        // Function to return height of specific Device
        public static int CheckHeight(Device device)
        {
            if (device == null)
            {
                return 0;
            }

            return device.height;
        }

        // Function to calculate weight of specific Device in AVL tree
        public static int CheckDeviceWeight(Device device)
        {
            if (device == null)
            {
                return 0;
            }

            // Weight = height of left subtree - height of right subtree
            int weight = Device.CheckHeight(device.leftDevice) - Device.CheckHeight(device.rightDevice);

            return weight;
        }

        // Function to return Device with lowest integer representation from specific place in a tree
        public static Device CheckLowestDevice(Device wezel)
        {
            Device temp = wezel;

            while (temp.leftDevice != null)
            {
                temp = temp.leftDevice;
            }

            return temp;
        }
    }
}
