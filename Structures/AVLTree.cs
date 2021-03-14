using System;

namespace ProjektASD
{
    class AVLTree // AVL Tree class
    {
        // Root of this tree
        public Device deviceRoot = null;


        // Single right rotation in unbalanced Device tree
        Device RightRotate(Device y) // Starting root
        {
            Device x = y.leftDevice;                          
            Device T2 = x.rightDevice;                                  
                                                                      
            x.rightDevice = y;                                                
            y.leftDevice = T2;    

            // Update Heights
            y.height = Math.Max(Device.CheckHeight(y.leftDevice), Device.CheckHeight(y.rightDevice)) + 1;
            x.height = Math.Max(Device.CheckHeight(x.leftDevice), Device.CheckHeight(x.rightDevice)) + 1;

            return x;   // Return new root 
        }


        // Single left rotation in unbalanced Device tree
        Device LeftRotate(Device x) // Starting root
        {
            Device y = x.rightDevice;    // Defining helpers
            Device T2 = y.leftDevice;       
                                                                            
            y.leftDevice = x;           // Left Rotation             
            x.rightDevice = T2;                                                 

            // Update heights
            x.height = Math.Max(Device.CheckHeight(x.leftDevice), Device.CheckHeight(x.rightDevice)) + 1;
            y.height = Math.Max(Device.CheckHeight(y.leftDevice), Device.CheckHeight(y.rightDevice)) + 1;

            return y;       // Return new root
        }


        // Function to check how many devices are connected to specific Subnet
        public int HowManyInSubnet(Device device, String szukanyAddress)
        {
            Subnet tmp = SearchAndReturnSubnet(device, szukanyAddress);
            if(tmp == null)
            {
                return 0;
            }
            else
            {
                return tmp.countDevices;
            }
        }


        // Function to search for and return Device with specific IP address
        public Device SearchAndReturnDevice(Device device, string deviceAddress) // Funkcja wyszukująca i zwracająca odpowiedni węzeł
        {
            uint deviceAddressInt = AddressOperator.ConvertAddressToInt(deviceAddress);

            while (device != null)
            {
                if (deviceAddressInt < device.addressInt) // Jeżeli lewy węzeł istnieje a poszukiwana podsieć jest mniejsza od adresu aktualnie analizowanego węzła to szukaj w lewo
                {
                    device = device.leftDevice;
                }
                else if (deviceAddressInt > device.addressInt) // Jeżeli prawy węzeł istnieje a poszukiwana podsieć jest większa od adresu aktualnie analizowanego węzła to szukaj w prawo
                {
                    device = device.rightDevice;
                }
                else
                {
                    return device;
                }
            }

            return null;
        }


        // Find first device with specific subnet address and return it
        public Subnet SearchAndReturnSubnet(Device device, string subnetAddress)
        {
            uint subnetAddressInt = AddressOperator.ConvertAddressToInt(subnetAddress);

            while (device != null)
            {
                if (subnetAddressInt < device.subnet.subnetAddressInt) // Jeżeli lewy węzeł istnieje a poszukiwana podsieć jest mniejsza od adresu aktualnie analizowanego węzła to szukaj w lewo
                {
                    device = device.leftDevice;
                }
                else if (subnetAddressInt > device.subnet.subnetAddressInt) // Jeżeli prawy węzeł istnieje a poszukiwana podsieć jest większa od adresu aktualnie analizowanego węzła to szukaj w prawo
                {
                    device = device.rightDevice;
                }
                else
                {
                    return device.subnet;
                }
            }

            return null;
        }


        // Function to check if device with specific address exists in the tree
        public string CheckIfDeviceExists(Device root, string address) // Funkcja sprawdzająca czy urządzenie o podanym adresie znajduje się w drzewie (zwraca TAK/NIE)
        {
            // Call function to find device or return null
            Device device = SearchAndReturnDevice(root, address);

            // If device is not found
            if (device == null)
            {
                return "NIE";
            }
            // If device is found
            else
            {
                return "TAK";
            }
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
            if (newAddressInt < currentDevice.addressInt)
            {
                currentDevice.leftDevice = Add(currentDevice.leftDevice, newAddress, newAddressInt, subnet);
            }
            else if (newAddressInt > currentDevice.addressInt)
            {
                currentDevice.rightDevice = Add(currentDevice.rightDevice, newAddress, newAddressInt, subnet);
            }
            // Duplicate IP is not allowed
            else
            {
                return currentDevice;
            }

            // Update height of currentDevice
            currentDevice.height = 1 + Math.Max(Device.CheckHeight(currentDevice.leftDevice), Device.CheckHeight(currentDevice.rightDevice));

            // Check if weights are balanced, otherwise rotate. Unbalanced weights are indicated by numbers 2 and -2
            int check = Device.CheckDeviceWeight(currentDevice);
            
            // LL rotation
            if (check > 1 && newAddressInt < currentDevice.leftDevice.addressInt)
            {                                                                                            //            wezel                     y
                return RightRotate(currentDevice);                                                       //            /   \                   /   \
            }                                                                                            //           y     T4                x    wezel
                                                                                                         //         /   \                    / \   / \
                                                                                                         //        x     T3       -->       T1 T2 T3  T4
                                                                                                         //       /  \
                                                                                                         //      T1   T2

            // LR rotation
            if (check > 1 && newAddressInt > currentDevice.leftDevice.addressInt)                            //        wezel                  wezel                      x
            {                                                                                                //        /  \                   /  \                      / \
                currentDevice.leftDevice = LeftRotate(currentDevice.leftDevice);                             //       y    T4                x    T4                  y    wezel
                return RightRotate(currentDevice);                                                           //      / \            -->     /  \          -->        / \   /  \
            }                                                                                                //     T1  x                  y    T3                 T1  T2 T3  T4
                                                                                                             //        / \                / \
                                                                                                             //       T2  T3             T1  T2

            // RR rotation
            if (check < -1 && newAddressInt > currentDevice.rightDevice.addressInt)
            {                                                                                    //         wezel                       y
                return LeftRotate(currentDevice);                                                //         /  \                      /  \
            }                                                                                    //       T4    y                 wezel    x
                                                                                                 //            /  \        -->      / \   /  \
                                                                                                 //           T3   x              T4  T3 T2   T1
                                                                                                 //               /  \
                                                                                                 //              T2   T1

            // RL rotation
            if (check < -1 && newAddressInt < currentDevice.rightDevice.addressInt)                          //          wezel                 wezel                        x
            {                                                                                                //         /   \                  /  \                       /   \
                currentDevice.rightDevice = RightRotate(currentDevice.rightDevice);                          //        T4    y               T4     x                 wezel     y
                return LeftRotate(currentDevice);                                                            //             /  \     -->           /  \     -->       /   \   /   \
            }                                                                                                //            x    T1               T3    y             T4   T3 T2   T1
                                                                                                             //           /  \                        /  \
                                                                                                             //          T3   T2                     T2  T1

            return currentDevice;
        }

        // PODPUNKT C) z Drzewa
        // Remove Device from AVL tree
        public Device Delete(Device currentDevice, String deviceAddress, uint deviceAddressInt)
        {
            // BST Deletion
            if (currentDevice == null)
            {
                return currentDevice;
            }

            // Check if currentDevice address is lower or higher in integer representation than left and right one
            if (deviceAddressInt < currentDevice.addressInt)
            {
                currentDevice.leftDevice = Delete(currentDevice.leftDevice, deviceAddress, deviceAddressInt);
            }

            else if (deviceAddressInt > currentDevice.addressInt)  // Jeżeli szukany adresIP większy to szukamy w lewej stronie drzewa 
            {
                currentDevice.rightDevice = Delete(currentDevice.rightDevice, deviceAddress, deviceAddressInt);
            }

            // If device found, check how many child nodes does it have
            else
            {

                // For Device with one or none child node
                if ((currentDevice.leftDevice == null) || (currentDevice.rightDevice == null))
                {
                    Device temp = null;
                    
                    // If there's no left child, save right child in temp
                    if (temp == currentDevice.leftDevice)
                    {
                        temp = currentDevice.rightDevice;
                    }
                    // If there's no right child, save left one instead
                    else
                    {
                        temp = currentDevice.leftDevice;
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
                    }                         //         wezel          wezel
                                              //           \     ->     /    \
                                              //           temp        T1     T2
                                              //          /   \
                                              //        T1     T2
                }
                // If there's more than one child node
                else
                {
                    // Search for device with lowest int representation in left subtree
                    Device temp = Device.CheckLowestDevice(currentDevice.rightDevice);

                    // Copy his details to current Device
                    currentDevice.address = temp.address;
                    currentDevice.addressInt = temp.addressInt;
                    currentDevice.subnet = temp.subnet;
                    // Now continue Deletion with new target
                    currentDevice.rightDevice = Delete(currentDevice.rightDevice, temp.address, temp.addressInt);                                                                                                        
                }
            }

            // If current Device is null finish the search and go back recursively
            if (currentDevice == null)
            {
                return currentDevice;
            }

            // Update height
            currentDevice.height = Math.Max(Device.CheckHeight(currentDevice.leftDevice), Device.CheckHeight(currentDevice.rightDevice)) + 1;

            // Check if tree is still balanced
            int sprawdzajka = Device.CheckDeviceWeight(currentDevice);
            int sprawdzajkaLewa = Device.CheckDeviceWeight(currentDevice.leftDevice);
            int sprawdzajkaPrawa = Device.CheckDeviceWeight(currentDevice.rightDevice);

            // If not:
            // LL  rotation
            if (sprawdzajka > 1 && sprawdzajkaLewa >= 0)                                                                              //            wezel                     y
            {                                                                                                                         //            /   \                   /   \
                return RightRotate(currentDevice);                                                                                    //           y     T4                x    wezel
            }                                                                                                                         //         /   \                    / \   / \
                                                                                                                                      //        x     T3       -->       T1 T2 T3  T4
                                                                                                                                      //       /  \
                                                                                                                                      //      T1   T2

            // LR  rotation
            if (sprawdzajka > 1 && sprawdzajkaLewa < 0)                                                                               //        wezel                  wezel                      x
            {                                                                                                                         //        /  \                   /  \                      / \
                currentDevice.leftDevice = LeftRotate(currentDevice.leftDevice);                                                                                //       y    T4                x    T4                  y    wezel
                return RightRotate(currentDevice);                                                                                          //      / \            -->     /  \          -->        / \   /  \
            }                                                                                                                         //     T1  x                  y    T3                 T1  T2 T3  T4
                                                                                                                                      //        / \                / \
                                                                                                                                      //       T2  T3             T1  T2

            // RR  rotation
            if (sprawdzajka < -1 && sprawdzajkaPrawa <= 0)                                                                                //         wezel                       y
            {                                                                                                                             //         /  \                      /  \
                return LeftRotate(currentDevice);                                                                                               //       T4    y                  wezel    x
            }                                                                                                                             //            /  \        -->      / \   /  \
                                                                                                                                          //           T3   x              T4  T3 T2   T1
                                                                                                                                          //               /  \
                                                                                                                                          //              T2   T1


            // RL  rotation
            if (sprawdzajka < -1 && sprawdzajkaPrawa > 0)                                                                                  //          wezel                 wezel                        x
            {                                                                                                                              //         /   \                  /  \                       /   \
                currentDevice.rightDevice = RightRotate(currentDevice.rightDevice);                                                        //        T4    y               T4     x                 wezel     y
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

                Display(wezel.rightDevice, odstep);
                Console.Write("\n");
                for (int i = przestrzen; i < odstep; i++)
                {
                    Console.Write(" ");
                }
                Console.Write(wezel.address + " Wag: " + Device.CheckDeviceWeight(wezel) + " Wys: " + wezel.height + "\n");

                Display(wezel.leftDevice, odstep);
            }
        }
    }
}
