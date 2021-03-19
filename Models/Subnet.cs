using System;
using System.Collections.Generic;

namespace Network.Models
{
    class Subnet : IComparable<Subnet>
    {
        // Address and it's integer representation
        private string subnetAddress;
        private uint subnetAddressInt;

        // Amount of devices in this Subnet
        private int countDevices;

        // Parameters to find best routing route from one Subnet to another using Dijkstra algorithm
        public int distanceFromSource;
        public Subnet predecessor;
        public Boolean visited;
        private List<Connection> connectedSubnets;

        public Subnet(string subnetAddress)
        {
            this.countDevices = 0;

            this.subnetAddress = subnetAddress;
            this.subnetAddressInt = AddressOperator.ConvertAddressToInt(subnetAddress);

            this.connectedSubnets = new List<Connection>();
        }

        // GET/SET Private params
        public List<Connection> ConnectedSubnets
        {
            get => connectedSubnets;
            set => connectedSubnets = value;
        }
        public string SubnetAddress
        {
            get => subnetAddress;
            set => subnetAddress = value;
        }
        public uint SubnetAddressInt
        {
            get => subnetAddressInt;
            set => subnetAddressInt = value;
        }
        public int CountDevices
        {
            get => countDevices;
            set => countDevices = value;
        }

        // Compare Subnets by distanceFromSource
        public int CompareTo(Subnet subnet)
        {
            return this.distanceFromSource.CompareTo(subnet.distanceFromSource);
        }

        // Add new connection from this Subnet to target Subnet
        public void Connect(Subnet destSubnet, int capacity)
        {
            connectedSubnets.Add(new Connection(destSubnet, capacity));
        }

        // Remove between this Subnet and destination Subnet
        public void Disconnect(Subnet destSubnet)
        {
            foreach (Connection connection in connectedSubnets)
            {
                if (connection.DestSubnet == destSubnet)
                {
                    connectedSubnets.Remove(connection);
                    return;
                }
            }
        }
    }
}
