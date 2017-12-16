using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AB
{
    public struct AZKG
    {
        public static double Lez = 1.5; //м
        public static double Dez = 0.16; //м
        public static double Pz = 0.06; //Ом*м
        public static double q = 0.01; //кг/А*год
        public static int Ie = 6; //A
    }      

        public struct AZKF
        {
            public static double Lez = 1.8; //м
            public static double Dez = 0.16; //м
            public static double Pz = 0.06; //Ом*м
            public static double q = 0.15; //кг/А*год
            public static int Ie = 6; //A          
        }

        public struct EGP
        {
            public static double Le = 1; //м
            public static double b = 0.125; //м
            public static double h = 0.010; //м       
            public static double Gz = 2.6; //кг
            public static double q = 0.03; //кг/А*год
            public static int Ie = 5; //A
        }

        public struct EFP
        {
            public static double Le = 1.5; //м
            public static double De = 0.065; //м
            public static double Gz = 35; //кг
            public static double q = 0.2; //кг/А*год
            public static int Ie = 6; //A          
        }    

        
}
