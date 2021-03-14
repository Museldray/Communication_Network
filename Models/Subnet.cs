using System;
using System.Collections.Generic;

namespace ProjektASD
{
    class Subnet : IComparable<Subnet>
    {
        // Address and it's integer representation
        public string subnetAddress;
        public uint subnetAddressInt;
        
        // Amount of devices in this Subnet
        public int countDevices;

        // Parameters to find best routing route from one Subnet to another using Dijkstra algorithm
        public int distanceFromSource;
        public Subnet predecessor;
        public Boolean visited;
        public List<Connection> connectedSubnets;

        public Subnet(string subnetAddress)
        {
            this.countDevices = 0;

            this.subnetAddress = subnetAddress;
            this.subnetAddressInt = AddressOperator.ConvertAddressToInt(subnetAddress);

            this.connectedSubnets = new List<Connection>();
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

        // Remove connection from this Subnet to target Subnet
        public void Disconnect(Subnet destSubnet)
        {
            foreach (Connection connection in connectedSubnets)
            {
                if (connection.destSubnet == destSubnet)
                {
                    connectedSubnets.Remove(connection);
                    return;
                }
            }
        }
    }
}
