using System;
using System.Collections.Generic;

namespace AB
{
    public static class Formulas
    {
        public static double R11(string type, double rg, double h) 
        {
            switch (type) {
                case "Ферросилидный":
                    return (rg / (2 * Math.PI * AZKF.Lez))
                        * (Math.Log(2 * AZKF.Lez / AZKF.Dez)
                            + Math.Log((AZKF.Lez + Math.Sqrt(AZKF.Lez * AZKF.Lez + 16 * h * h)) / (4 * h))
                            + (AZKF.Pz / rg) * Math.Log(AZKF.Dez/EFP.De));
                case "Графитопластовый":
                    return (rg / (2 * Math.PI * AZKG.Lez))
                        * (Math.Log(2 * AZKG.Lez / AZKG.Dez)
                            + Math.Log((AZKG.Lez + Math.Sqrt(AZKG.Lez * AZKG.Lez + 16 * h * h)) /( 4 * h))
                            + (AZKG.Pz / rg) * Math.Log(AZKG.Dez*Math.PI / (2*EGP.b)));
                default: return -1d;
            }

        }
        public static double R12(string type, double rg, double h) 
        {
            switch (type)
            {
                case "Ферросилидный":
                    return (rg / (2 * Math.PI * EFP.Le))
                        * (Math.Log(2 * EFP.Le / EFP.De)
                            + Math.Log((EFP.Le + Math.Sqrt(EFP.Le * EFP.Le + 16 * h * h)) / (4 * h)));
                case "Графитопластовый":
                    return (rg / (2 * Math.PI * EGP.Le))
                        * (Math.Log(EGP.Le / (EGP.b / Math.PI))
                            + Math.Log((EGP.Le + Math.Sqrt(EGP.Le * EGP.Le + 16 * h * h)) / (4 * h)));
                default: return -1d;
            }

        }
        public static double R13(string type, double rg, double h) 
        {
            switch (type)
            {
                case "Ферросилидный":
                    return (rg / (2 * Math.PI * AZKF.Lez))
                        * (Math.Log(2 * AZKF.Lez / AZKF.Dez)
                            + 0.5 * Math.Log((4 * h + AZKF.Lez) / (4 * h - AZKF.Lez))
                            + (AZKF.Pz / rg) * Math.Log(AZKF.Dez / EFP.De));
                case "Графитопластовый":
                    return (rg / (2 * Math.PI * AZKG.Lez))
                        * (Math.Log(2 * AZKG.Lez / AZKG.Dez)
                            + 0.5 * Math.Log((4 * h + AZKG.Lez) / (4 * h - AZKG.Lez))
                            + (AZKG.Pz / rg) * Math.Log(AZKG.Dez * Math.PI / (2 * EGP.b)));
                default: return -1d;

            }

        }
        public static double R14(string type, double rg, double h) //+
        {
            switch (type)
            {
                case "Ферросилидный":
                    return (rg / (2 * Math.PI * EFP.Le))
                          * (Math.Log(2 * EFP.Le / EFP.De)
                              + 0.5 * Math.Log((4 * h + EFP.Le) / (4 * h - EFP.Le)));
                case "Графитопластовый":
                    return (rg / (Math.PI * EGP.Le))
                      * (Math.Log(2 * Math.PI * EGP.Le / EGP.b)
                          + 0.5 * Math.Log((4 * h + EGP.Le) / (4 * h - EGP.Le)));
                default: return -1d;

            }

        }

        public static double T(string type, double Iav, double N)
        {
            switch (type)
            {
                case "Ферросилидный":
                    return (0.77d * N * EFP.Gz) / (EFP.q * Iav);
                case "Графитопластовый":
                    return (0.77d * N * EGP.Gz) / (EGP.q * Iav);
                default: return -1d;
            }
        }
        public static double Imax(string type, string material, double N)
        {
            switch (type)
            {
                case "Комплектный":
                    if (material.Equals("Графитопластовый")) return N*AZKG.Ie;
                    else return N * AZKF.Ie;
                case "Некомплектный":
                    if (material.Equals("Графитопластовый")) return N*EGP.Ie;
                    else return N*EFP.Ie;
            }
            return -1;
        }

        public static double L(string aligment, string type, string material, double N, double L1)
        {
            if (aligment == "Вертикальная") { return L1 * (N - 1) + 0.4;}
            switch (type)
            {
                case "Комплектный":                                     
                    if (material.Equals("Графитопластовый")) return L1 * (N - 1) + 0.6 + AZKG.Lez;
                    else return L1 * (N - 1) + 0.6 + AZKF.Lez;                 
                case "Некомплектный":
                    if (material.Equals("Графитопластовый")) return L1 * (N - 1) + 0.6 + EGP.Le;
                    else return L1 * (N - 1) + 0.6 + EFP.Le;                  
            }
            return -1;           
        }


        public static double nu(string type, string placement, double N, double L)
        {
            string key = N.ToString() + Math.Truncate(L).ToString();
            if (placement.Equals("Двухрядное")) return nupd[key];
            switch (type)
            {
                case "Вертикальная": return nuv[key];
                case "Горизонтальная": return nug[key];
                default:return -1d;
            }           
        }      

        private static readonly Dictionary<string, double> nug
                = new Dictionary<string, double>
                {
                    {"24", 0.86}, {"44", 0.85}, {"64", 0.8}, {"84", 0.78}, {"124", 0.74}, {"164", 0.72}, {"204", 0.72}, {"244", 0.7},
                    {"26", 0.9}, {"46", 0.89}, {"66", 0.85}, {"86", 0.83}, {"126", 0.79}, {"166", 0.76}, {"206", 0.76}, {"246", 0.75},
                    {"29", 0.94}, {"49", 0.94}, {"69", 0.89}, {"89", 0.87}, {"129", 0.85}, {"169", 0.83}, {"209", 0.83}, {"249", 0.8}
                };
        private static readonly Dictionary<string, double> nuv
               = new Dictionary<string, double>
                {
                    {"24", 0.78}, {"44", 0.75}, {"64", 0.66}, {"84", 0.65}, {"124", 0.6}, {"164", 0.58}, {"204", 0.57}, {"244", 0.56},
                    {"26", 0.82}, {"46", 0.8}, {"66", 0.75}, {"86", 0.75}, {"126", 0.68}, {"166", 0.65}, {"206", 0.64}, {"246", 0.63},
                    {"29", 0.9}, {"49", 0.88}, {"69", 0.84}, {"89", 0.84}, {"129", 0.75}, {"169", 0.76}, {"209", 0.75}, {"249", 0.75}
                };
        private static readonly Dictionary<string, double> nupd
            = new Dictionary<string, double>
                {
                    {"84", 0.9}, {"164", 0.86}, {"244", 0.84}, {"324", 0.83}, {"404", 0.81}, {"484", 0.8},
                    {"86", 0.9}, {"166", 0.86}, {"246", 0.85}, {"326", 0.84}, {"406", 0.83}, {"486", 0.83},
                    {"89", 0.9}, {"169", 0.88}, {"249", 0.87}, {"329", 0.86}, {"409", 0.86}, {"489", 0.85}
                };
    }
}
