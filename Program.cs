using System;
using System.Collections.Generic;
using System.IO;
using System.Diagnostics;
using System.Linq;

namespace ProjektASD
{
    class Program
    {
        public static void Main()
        {
            // Timer start
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            // Initialize objects
            AVLTree tree = new AVLTree();
            Graph graph = new Graph();

            string[] input_lines;

            // Check if there's file to read
            try
            {
                input_lines = File.ReadAllLines("instrukcje.txt");
            }
            catch
            {
                Console.WriteLine("There's no file to load!");
                Console.ReadKey();
                return;
            }


            // Check and execute commands from file
            foreach (string line in input_lines)
            {
                string[] command = line.Split(' ');

                // Add new Device to tree and create/modify Subnet
                if (command[0] == "DK")
                {
                    string subnetAddress = AddressOperator.ConvertAddressToSubnet(command[1]);
                    Device device = tree.SearchAndReturnDevice(tree.deviceRoot, command[1]);
                    Subnet subnet = tree.SearchAndReturnSubnet(tree.deviceRoot, subnetAddress);

                    // If there is no device and subnet, create both of them
                    if (device == null && subnet == null)
                    {
                        subnet = new Subnet(subnetAddress);
                        tree.deviceRoot = tree.Add(tree.deviceRoot, command[1], AddressOperator.ConvertAddressToInt(command[1]), subnet);
                        subnet.countDevices++;
                    }
                    // If there is no device with that address but there is already subnet, create device, connect Subnet to it and iterate number of devices in subnet
                    else if (device == null && subnet != null)
                    {
                        tree.deviceRoot = tree.Add(tree.deviceRoot, command[1], AddressOperator.ConvertAddressToInt(command[1]), subnet);
                        subnet.countDevices++;
                    }
                }


                // Delete specified device from the tree
                if (command[0] == "UK")
                {
                    Device device = tree.SearchAndReturnDevice(tree.deviceRoot, command[1]);

                    // If device exists
                    if (device != null)
                    {
                        // So there exists subnet
                        Subnet subnet = device.subnet;

                        // If there's only one computer in subnet, delete connections with other subnets and delete this subnet
                        if (subnet.countDevices == 1)
                        {
                            // Delete connections from all subnets connected to this subnet
                            LinkedList<Subnet> tmp = new LinkedList<Subnet>();
                            int helper = 0;

                            foreach (Connection connection in subnet.connectedSubnets)
                            {
                                helper++;
                                tmp.AddLast(connection.destSubnet);
                            }

                            for (int i = 0; i < helper; i++)
                            {
                                graph.Disconnect(tmp.Last(), subnet);
                                tmp.RemoveLast();
                            }

                            // Delete this Subnet
                            device.subnet = null;

                            // Delete this Device
                            tree.deviceRoot = tree.Delete(tree.deviceRoot, device.address, device.addressInt);
                        }
                        // If there's more then one device in this Subnet then delete destination Device
                        else
                        {
                            tree.deviceRoot = tree.Delete(tree.deviceRoot, command[1], AddressOperator.ConvertAddressToInt(command[1]));
                            subnet.countDevices--;
                        }
                    }
                }


                // Search for device and write "NIE" for no device and "TAK" if device exists
                if (command[0] == "WK")
                {
                    Console.WriteLine(tree.CheckIfDeviceExists(tree.deviceRoot, command[1]));
                }


                // Check how many devices belong to specific subnet and write the number
                if (command[0] == "LK")
                {
                    String adres = command[1] + ".0";   // + ".0" ponieważ w pliku podaje się tylko 3 pierwsze oktety, następuje ręczne dodanie czwartego w celu poprawnego działania
                                                        
                    Console.WriteLine(tree.HowManyInSubnet(tree.deviceRoot, adres));
                }


                // Display Device tree
                if (command[0] == "WY")
                {
                    Console.WriteLine("Drzewo urządzeń");
                    tree.Display(tree.deviceRoot, 0);
                }
                

                // Add Connection between Subnets
                if (command[0] == "DP")
                {
                    Subnet komputerA = tree.SearchAndReturnSubnet(tree.deviceRoot, command[1] + ".0"); // Source Subnet
                    Subnet komputerB = tree.SearchAndReturnSubnet(tree.deviceRoot, command[2] + ".0"); // Target Subnet

                    // If there is no source or target Subnet, write "NIE"
                    if (komputerA == null || komputerB == null)
                    {
                        Console.WriteLine("NIE");
                    }
                    // Else
                    else
                    {
                        // Calculate capacity
                        int capacity = 0;
                        char p = command[3].Last();
                        command[3] = command[3].Remove(command[3].Length - 1);
                        if (p == 'M')
                        {
                            capacity = 100000 / Convert.ToInt32(command[3]);
                        }
                        else if (p == 'G')
                        {
                            capacity = 100 / Convert.ToInt32(command[3]);
                        }
                        // Create Connection between Subnets
                        graph.Connect(komputerA, komputerB, capacity);
                    }
                }


                // Delete Connection between Subnets
                if (command[0] == "UP")
                {
                    Subnet komputerA = tree.SearchAndReturnSubnet(tree.deviceRoot, command[1] + ".0"); // Source Subnet
                    Subnet komputerB = tree.SearchAndReturnSubnet(tree.deviceRoot, command[2] + ".0"); // Target Subnet

                    // If source or target Subnet don't exist, write "NIE"
                    if (komputerA == null || komputerB == null)
                    {
                        Console.WriteLine("NIE");
                    }
                    // Else
                    else
                    {
                        graph.Disconnect(komputerA, komputerB);
                    }
                }


                // Calculate the best routing route between subnets
                if (command[0] == "NP")
                {
                    String adres1 = AddressOperator.ConvertAddressToSubnet(command[1]);
                    String adres2 = AddressOperator.ConvertAddressToSubnet(command[2]);
                    Subnet komputerA = tree.SearchAndReturnSubnet(tree.deviceRoot, adres1); // Source Subnet
                    Subnet komputerB = tree.SearchAndReturnSubnet(tree.deviceRoot, adres2); // Target Subnet

                    // If target or source Subnet don't exist, write "NIE"
                    if (komputerA == null || komputerB == null)
                    {
                        Console.WriteLine("NIE");
                    }
                    // Else, find best route
                    else
                    {
                        graph.FindFastestRoute(tree.deviceRoot, komputerA, komputerB);
                    }
                }
            } // End - Foreach

            // Stop timer
            stopWatch.Stop();

            // Format and Display elapsed time
            TimeSpan ts = stopWatch.Elapsed;
            string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}", ts.Hours, ts.Minutes, ts.Seconds, ts.Milliseconds);
            Console.WriteLine("Czas: " + elapsedTime);

            Console.ReadKey();
        }
    }
}
