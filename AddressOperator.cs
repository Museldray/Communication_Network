using System;
using System.Net;

namespace ProjektASD
{
    
    class AddressOperator // Class to operate on IP addresses
    {
        // Convert IP address to integer to use in AVL tree
        public static uint ConvertAddressToInt(string addressIP)
        {
            IPAddress IPv4 = IPAddress.Parse(addressIP);
            byte[] byteIPv4 = IPv4.GetAddressBytes();

            // Wyrażenie adresu IPv4 w postaci liczby poprzez zastosowanie przesunięcia binarnego
            uint ip = (uint)byteIPv4[0] << 24;
            ip = ip + (uint)(byteIPv4[1] << 16);
            ip = ip + (uint)(byteIPv4[2] << 8);
            ip = ip + byteIPv4[3];

            return ip;
        }

        // Convert IP address to subnet address with /24 bit mask
        public static String ConvertAddressToSubnet(string addressIP) // Funkcja zamieniająca podany adres IPv4 na adresIP podsieci z maską /24
        {
            string[] octets = addressIP.Split('.');
            octets[3] = octets[3].Replace(octets[3], "0");
            string adres = octets[0] + "." + octets[1] + "." + octets[2] + "." + octets[3];
            return adres;
        }

        // Convert IP address to IPAddress type
        public static IPAddress ConvertStringToIPAddress(string addressIP)
        {
            return IPAddress.Parse(addressIP);
        }
    }
}
