using System;
using System.Linq;
using Medallion.Collections;

namespace ProjektASD.Models
{
    class Connection // Class to define connection between Subnets
    {
        // Destination Subnet from THIS Subnet
        private Subnet destSubnet; 
        // Capacity
        public int capacity;

        public Connection(Subnet destSubnet, int capacity)
        {
            this.destSubnet = destSubnet;
            this.capacity = capacity;
        }

        public Subnet DestSubnet
        {
            get => destSubnet;
            set => destSubnet = value;
        }
    }
}
