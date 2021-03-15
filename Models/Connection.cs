using System;

namespace ProjektASD.Models
{
    class Connection // Class to define connection between Subnets
    {
        // Destination Subnet from THIS Subnet
        public Subnet destSubnet; 
        // Capacity
        public int capacity;

        public Connection(Subnet destSubnet, int capacity)
        {
            this.destSubnet = destSubnet;
            this.capacity = capacity;
        }
    }
}
