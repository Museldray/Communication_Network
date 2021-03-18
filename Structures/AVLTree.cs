using System;
using ProjektASD.Models;

namespace ProjektASD.Structures
{
    class AVLTree // AVL Tree class
    {
        // Root of this tree
        private Device deviceRoot = null;

        public Device DeviceRoot
        {
            get => deviceRoot;
            set => deviceRoot = value;
        }


        // Single right rotation in unbalanced Device tree
        private Device RightRotate(Device y) // Starting root
        {
            Device x = y.LeftDevice;                          
            Device T2 = x.RightDevice;                                  
                                                                      
            x.RightDevice = y;                                                
            y.LeftDevice = T2;    

            // Update Heights
            y.height = Math.Max(Device.CheckHeight(y.LeftDevice), Device.CheckHeight(y.RightDevice)) + 1;
            x.height = Math.Max(Device.CheckHeight(x.LeftDevice), Device.CheckHeight(x.RightDevice)) + 1;

            return x;   // Return new root 
        }


        // Single left rotation in unbalanced Device tree
        private Device LeftRotate(Device x) // Starting root
        {
            Device y = x.RightDevice;    // Defining helpers
            Device T2 = y.LeftDevice;       
                                                                            
            y.LeftDevice = x;           // Left Rotation             
            x.RightDevice = T2;                                                 

            // Update heights
            x.height = Math.Max(Device.CheckHeight(x.LeftDevice), Device.CheckHeight(x.RightDevice)) + 1;
            y.height = Math.Max(Device.CheckHeight(y.LeftDevice), Device.CheckHeight(y.RightDevice)) + 1;

            return y;       // Return new root
        }


        // Function to return Device with lowest integer representation from specific place in a tree
        private Device CheckLowestDevice(Device wezel)
        {
            Device temp = wezel;

            while (temp.LeftDevice != null)
            {
                temp = temp.LeftDevice;
            }

            return temp;
        }


        // Function to check how many devices are connected to specific Subnet
        public int HowManyInSubnet(Device device, String szukanyAddress)
        {
            Subnet tmp = SearchAndReturnSubnet(device, szukanyAddress);

            return tmp == null ? 0 : tmp.CountDevices;
        }


        // Function to search for and return Device with specific IP address
        public Device SearchAndReturnDevice(Device device, string deviceAddress) // Funkcja wyszukująca i zwracająca odpowiedni węzeł
        {
            uint deviceAddressInt = AddressOperator.ConvertAddressToInt(deviceAddress);

            // If null, there's no such device in network, otherwise continue directed search
            while (device != null)
            {
                // Check address and decide which direction you should search for target device address
                if (deviceAddressInt < device.AddressInt)
                {
                    device = device.LeftDevice;
                }
                else if (deviceAddressInt > device.AddressInt)
                {
                    device = device.RightDevice;
                }
                else
                {
                    return device;
                }
            }

            // No device found
            return null;
        }


        // Find first device with specific subnet address and return it
        public Subnet SearchAndReturnSubnet(Device device, string subnetAddress)
        {
            uint subnetAddressInt = AddressOperator.ConvertAddressToInt(subnetAddress);

            // If null, there's no such subnet in network, otherwise continue directed search
            while (device != null)
            {
                // Check address and decide which direction you should search for target device address
                if (subnetAddressInt < device.Subnet.SubnetAddressInt)
                {
                    device = device.LeftDevice;
                }
                else if (subnetAddressInt > device.Subnet.SubnetAddressInt)
                {
                    device = device.RightDevice;
                }
                else
                {
                    return device.Subnet;
                }
            }

            // No subnet found
            return null;
        }


        // Function to check if device with specific address exists in the tree
        public string CheckIfDeviceExists(Device root, string address) // Funkcja sprawdzająca czy urządzenie o podanym adresie znajduje się w drzewie (zwraca TAK/NIE)
        {
            // Call function to find device or return null
            Device device = SearchAndReturnDevice(root, address);

            // If device is not found print "NIE", else "TAK"
            return device == null ? "NIE" : "TAK";
        }


        // Add new Device to AVL tree (O(log n))
        public Device Add(Device currentDevice, String newAddress, uint newAddressInt, Subnet subnet)
        {
            // BST Insert
            if (currentDevice == null)  // Jeżeli drzewo było puste to dodaj korzeń
            {
                return new Device(newAddress, newAddressInt, subnet);
            }

            // If target address have lower/higher int representation than currentDevice, search leftDevice or rightDevice
            if (newAddressInt < currentDevice.AddressInt)
            {
                currentDevice.LeftDevice = Add(currentDevice.LeftDevice, newAddress, newAddressInt, subnet);
            }
            else if (newAddressInt > currentDevice.AddressInt)
            {
                currentDevice.RightDevice = Add(currentDevice.RightDevice, newAddress, newAddressInt, subnet);
            }
            // Duplicate IP is not allowed
            else
            {
                return currentDevice;
            }

            // Update height of currentDevice
            currentDevice.height = 1 + Math.Max(Device.CheckHeight(currentDevice.LeftDevice), Device.CheckHeight(currentDevice.RightDevice));

            // Check if weights are balanced, otherwise rotate. Unbalanced weights are indicated by numbers 2 and -2
            int check = Device.CheckDeviceWeight(currentDevice);
            
            // LL rotation
            if (check > 1 && newAddressInt < currentDevice.LeftDevice.AddressInt)
            {                                                                                            //           Device                     y
                return RightRotate(currentDevice);                                                       //            /   \                   /   \
            }                                                                                            //           y     T4                x    Device
                                                                                                         //         /   \                    / \   / \
                                                                                                         //        x     T3       -->       T1 T2 T3  T4
                                                                                                         //       /  \
                                                                                                         //      T1   T2

            // LR rotation
            if (check > 1 && newAddressInt > currentDevice.LeftDevice.AddressInt)                            //       Device                  Device                     x
            {                                                                                                //        /  \                   /  \                      / \
                currentDevice.LeftDevice = LeftRotate(currentDevice.LeftDevice);                             //       y    T4                x    T4                  y    Device
                return RightRotate(currentDevice);                                                           //      / \            -->     /  \          -->        / \   /  \
            }                                                                                                //     T1  x                  y    T3                 T1  T2 T3  T4
                                                                                                             //        / \                / \
                                                                                                             //       T2  T3             T1  T2

            // RR rotation
            if (check < -1 && newAddressInt > currentDevice.RightDevice.AddressInt)
            {                                                                                    //        Device                       y
                return LeftRotate(currentDevice);                                                //         /  \                      /  \
            }                                                                                    //       T4    y                Device    x
                                                                                                 //            /  \        -->      / \   /  \
                                                                                                 //           T3   x              T4  T3 T2   T1
                                                                                                 //               /  \
                                                                                                 //              T2   T1

            // RL rotation
            if (check < -1 && newAddressInt < currentDevice.RightDevice.AddressInt)                          //         Device                 Device                       x
            {                                                                                                //         /   \                  /  \                       /   \
                currentDevice.RightDevice = RightRotate(currentDevice.RightDevice);                          //        T4    y               T4     x                 Device    y
                return LeftRotate(currentDevice);                                                            //             /  \     -->           /  \     -->       /   \   /   \
            }                                                                                                //            x    T1               T3    y             T4   T3 T2   T1
                                                                                                             //           /  \                        /  \
                                                                                                             //          T3   T2                     T2  T1

            return currentDevice;
        }

        // Remove Device from AVL tree
        public Device Delete(Device currentDevice, String deviceAddress, uint deviceAddressInt)
        {
            // BST Deletion
            if (currentDevice == null)
            {
                return currentDevice;
            }

            // Check if currentDevice address is lower or higher in integer representation than left and right one
            if (deviceAddressInt < currentDevice.AddressInt)
            {
                currentDevice.LeftDevice = Delete(currentDevice.LeftDevice, deviceAddress, deviceAddressInt);
            }

            else if (deviceAddressInt > currentDevice.AddressInt)  // Jeżeli szukany adresIP większy to szukamy w lewej stronie drzewa 
            {
                currentDevice.RightDevice = Delete(currentDevice.RightDevice, deviceAddress, deviceAddressInt);
            }

            // If device found, check how many child nodes does it have
            else
            {

                // For Device with one or none child node
                if ((currentDevice.LeftDevice == null) || (currentDevice.RightDevice == null))
                {
                    Device temp = null;
                    
                    // If there's no left child, save right child in temp
                    if (temp == currentDevice.LeftDevice)
                    {
                        temp = currentDevice.RightDevice;
                    }
                    // If there's no right child, save left one instead
                    else
                    {
                        temp = currentDevice.LeftDevice;
                    }

                    // If there's no child nodes, delete current Device without rotations
                    if (temp == null)
                    {
                        currentDevice = null;
                    }
                    // If there's only one child node, replace current Device with that child node 
                    else
                    {
                        currentDevice = temp; // Kopiowanie wartości dziecka do węzła aktualnie analizowanego
                    }                         //         Device          Device
                                              //           \     ->     /    \
                                              //           temp        T1     T2
                                              //          /   \
                                              //        T1     T2
                }
                // If there's more than one child node
                else
                {
                    // Search for device with lowest int representation in left subtree
                    Device temp = CheckLowestDevice(currentDevice.RightDevice);

                    // Copy his details to current Device
                    currentDevice.Address = temp.Address;
                    currentDevice.AddressInt = temp.AddressInt;
                    currentDevice.Subnet = temp.Subnet;

                    // Now continue Deletion with new target
                    currentDevice.RightDevice = Delete(currentDevice.RightDevice, temp.Address, temp.AddressInt);                                                                                                        
                }
            }

            // If current Device is null finish the search and go back recursively
            if (currentDevice == null)
            {
                return currentDevice;
            }

            // Update height
            currentDevice.height = Math.Max(Device.CheckHeight(currentDevice.LeftDevice), Device.CheckHeight(currentDevice.RightDevice)) + 1;

            // Check if tree is still balanced
            int check = Device.CheckDeviceWeight(currentDevice);
            int checkLeft = Device.CheckDeviceWeight(currentDevice.LeftDevice);
            int checkRight = Device.CheckDeviceWeight(currentDevice.RightDevice);

            // If not:
            // LL  rotation
            if (check > 1 && checkLeft >= 0)                                                                                          //           Device                     y
            {                                                                                                                         //            /   \                   /   \
                return RightRotate(currentDevice);                                                                                    //           y     T4                x    Device
            }                                                                                                                         //         /   \                    / \   / \
                                                                                                                                      //        x     T3       -->       T1 T2 T3  T4
                                                                                                                                      //       /  \
                                                                                                                                      //      T1   T2

            // LR  rotation
            if (check > 1 && checkLeft < 0)                                                                                           //       Device                  Device                     x
            {                                                                                                                         //        /  \                   /  \                      / \
                currentDevice.LeftDevice = LeftRotate(currentDevice.LeftDevice);                                                      //       y    T4                x    T4                  y    Device
                return RightRotate(currentDevice);                                                                                    //      / \            -->     /  \          -->        / \   /  \
            }                                                                                                                         //     T1  x                  y    T3                 T1  T2 T3  T4
                                                                                                                                      //        / \                / \
                                                                                                                                      //       T2  T3             T1  T2

            // RR  rotation
            if (check < -1 && checkRight <= 0)                                                                                            //       Device                       y
            {                                                                                                                             //         /  \                      /  \
                return LeftRotate(currentDevice);                                                                                         //       T4    y                  Device  x
            }                                                                                                                             //            /  \        -->      / \   /  \
                                                                                                                                          //           T3   x              T4  T3 T2   T1
                                                                                                                                          //               /  \
                                                                                                                                          //              T2   T1


            // RL  rotation
            if (check < -1 && checkRight > 0)                                                                                              //         Device                Device                        x
            {                                                                                                                              //         /   \                  /  \                       /   \
                currentDevice.RightDevice = RightRotate(currentDevice.RightDevice);                                                        //        T4    y               T4     x                 Device     y
                return LeftRotate(currentDevice);                                                                                          //             /  \     -->           /  \     -->       /   \   /   \
            }                                                                                                                              //            x    T1               T3    y             T4   T3 T2   T1
                                                                                                                                           //           /  \                        /  \
                                                                                                                                           //          T3   T2                     T2  T1
            return currentDevice;
        }
       

        // Display AVL tree on the console (only for small amount of devices on the tree)
        public void Display(Device wezel, int odstep)
        {
            int przestrzen = 20;

            if (wezel != null)
            {
                odstep += przestrzen;

                Display(wezel.RightDevice, odstep);
                Console.Write("\n");
                for (int i = przestrzen; i < odstep; i++)
                {
                    Console.Write(" ");
                }
                Console.Write(wezel.Address + " Wag: " + Device.CheckDeviceWeight(wezel) + " Wys: " + wezel.height + "\n");

                Display(wezel.LeftDevice, odstep);
            }
        }
    }
}
