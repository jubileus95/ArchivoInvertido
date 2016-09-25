using System;
using System.Collections.Generic;
using System.IO;
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
        private byte[] docs;

        public string Consulta
        {
            get
            {
                return consulta;
            }

            set
            {
                consulta = value;
            }
        }

        public int Freq
        {
            get
            {
                return freq;
            }

            set
            {
                freq = value;
            }
        }

        public float Peso
        {
            get
            {
                return peso;
            }

            set
            {
                peso = value;
            }
        }

        public byte[] Docs
        {
            get
            {
                return docs;
            }

            set
            {
                docs = value;
            }
        }

        public TerminoConsulta(String pConsulta,int nsubi,int totalDocs,int inicio,string aiPath) {
            if (pConsulta[0] == '+')
            {
                pConsulta = pConsulta.Substring(1);
                freq = 4;
            }
            else if (pConsulta[0] == '-') {
                freq = 1;
                pConsulta = pConsulta.Substring(1);
            }

            else
            {
                freq = 2;
            }
            Consulta = pConsulta;
            calcularPesoConsulta(nsubi,totalDocs);

            docs = new byte[12*nsubi];
            docs= File.ReadAllBytes(aiPath).Skip(12*inicio).Take(12*nsubi).ToArray();

            
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
