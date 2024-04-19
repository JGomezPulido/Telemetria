using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Telemetria.src
{
    public class Prueba_dll
    {

        public int a;
        public bool b;


        public void Pruebacion(int x, int y)
        {
            a = x - y;

            if (a > 15)
            {
                b = true;
            }
            else
            {
                b = false;
            }
        }
    }
}
