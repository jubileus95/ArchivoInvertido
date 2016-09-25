﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication1
{
    class TerminoConsulta
    {
        private String consulta;
        private int freq;
        private float peso;

        public TerminoConsulta(String pConsulta,int nsubi,int totalDocs) {
            if (pConsulta[0] == '+')
            {
                freq = 4;
            }
            else if (pConsulta[0] == '-') {
                freq = 1;
            }

            else
            {
                freq = 2;
            }
            consulta = pConsulta;
            calcularPesoConsulta(nsubi,totalDocs);
        }

        public void calcularPesoConsulta(int nsubi, int totalDocs) {
           
            float formulaPeso = (1 + (float)(Math.Log(freq, 2))) * (float)Math.Log((double)totalDocs /nsubi,2);
            if (formulaPeso == 0){
                System.Console.WriteLine("");
            }
            peso=formulaPeso;
        }
  }
}