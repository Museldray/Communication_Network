using System;
using System.Linq;
using Medallion.Collections;
using Network.Models;

namespace Network.Structures
{
    class Graph
    {
        // Add connection between two Subnets
        public static void Connect(Subnet sourceSubnet, Subnet destSubnet, int capacity)
        {
            foreach (Connection polaczenie in sourceSubnet.ConnectedSubnets)
            {
                if (polaczenie.DestSubnet == destSubnet)
                {
                    Console.WriteLine("NIE");
                    return;
                }
            }

            // Add new connections to lists in sourceSubnet and destSubnet
            destSubnet.Connect(sourceSubnet, capacity);
            sourceSubnet.Connect(destSubnet, capacity);
        }

        // Remove connection between two Subnets
        public static void Disconnect(Subnet sourceSubnet, Subnet destSubnet)
        {
            foreach (Connection polaczenie in sourceSubnet.ConnectedSubnets)
            {
                if (polaczenie.DestSubnet == destSubnet)
                {
                    sourceSubnet.Disconnect(destSubnet);
                    destSubnet.Disconnect(sourceSubnet);
                    return;
                }
            }
        }

        // Find best routing route from one subnet to another and write results in console
        public static void FindFastestRoute(Device root, Subnet sourceSubnet, Subnet destSubnet)
        {
            // Call function to set parameters for Dijkstra algorithm
            SetParemetersForDijkstra(root, sourceSubnet); 

            // Call Dijkstra method to try to find the best route
            Dijkstra(sourceSubnet, destSubnet);

            // If route is not found, write "NIE"
            if (destSubnet.distanceFromSource == int.MaxValue)
            {                                             
                Console.WriteLine("NIE");
            }
            // Else write results
            else
            {
                Console.WriteLine(destSubnet.distanceFromSource);
            }
        }

        // Dijkstra algorithm to find best transfer route ( O(n * log n + m) n - unvisited Subnets, m - amount of connections )
        private static void Dijkstra(Subnet sourceSubnet, Subnet destSubnet)
        {
            // Create new Priority Queue and add first element on it
            PriorityQueue<Subnet> pQueue = new PriorityQueue<Subnet>();
            pQueue.Enqueue(sourceSubnet);                 
            
            // While there are elements on the queue
            while (pQueue.Any()) // O(n)
            {
                // Get Subnet with lowest distance from source and take it off from queue ( O(log n) )
                Subnet subnet = pQueue.Dequeue();

                // If Subnet already visited, do not visit it again
                if (subnet.visited == true)
                {
                    continue;
                }

                // Else, set visited as true and update distance from source to connected Subnets
                subnet.visited = true;
                foreach (Connection connectedSubnets in subnet.ConnectedSubnets) // O(m)
                {
                    // Analize every connected Subnet
                    Subnet connectedSubnet = connectedSubnets.DestSubnet;

                    // If distance from currently analized subnet to connected Subnet is better than currently set distance From Source then update it and set predecessor
                    if (subnet.distanceFromSource == int.MaxValue || subnet.distanceFromSource + connectedSubnets.capacity < connectedSubnet.distanceFromSource)                                                                                             
                    {                                                                                                    
                        connectedSubnet.distanceFromSource = subnet.distanceFromSource + connectedSubnets.capacity;
                        connectedSubnet.predecessor = subnet; 

                        // Add this Subnet on queue for further operations ( O(log n) )
                        pQueue.Enqueue(connectedSubnet);
                    }
                }

                // If destSubnet found, stop the algorithm
                if (subnet == destSubnet)
                {
                    return;
                }

            }
        }

        // Set params to run Dijkstra (everything but sourceSubnet should have distanceFromSource set to int.MaxValue, predecessors as null and visisted to false. Source Subnet should have distance from source set to 0)
        private static void SetParemetersForDijkstra(Device currentSubnet, Subnet sourceSubnet)
        {
            sourceSubnet.distanceFromSource = 0;

            if (currentSubnet == null)
            {
                return;
            }
            else
            {
                currentSubnet.Subnet.distanceFromSource = int.MaxValue;
                currentSubnet.Subnet.predecessor = null;
                currentSubnet.Subnet.visited = false;
            }

            SetParemetersForDijkstra(currentSubnet.LeftDevice, sourceSubnet);
            SetParemetersForDijkstra(currentSubnet.RightDevice, sourceSubnet);
        }
    }
}
