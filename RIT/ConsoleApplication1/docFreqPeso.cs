using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication1
{
    class docFreqPeso
    {
        private int docId;
        private List<int> freqs;
        private List<float> pesos;
        private float norma;
        private int totalTerms;

        public docFreqPeso(List<string> terminos, XMLFile doc, int[] nsubi, int totalDocs)
        {
            this.docId = doc.DocId;
            freqs = new List<int>();

            pesos = new List<float>();
            this.calcularFreqs(terminos,doc);
            totalTerms = freqs.Sum();
            this.calcularPesos(nsubi,totalDocs);
            foreach (float peso in pesos) {
                Norma += (float)Math.Pow(peso,2);
            }
            Norma = (float) Math.Sqrt(Norma);

        }

        public int DocId
        {
            get
            {
                return docId;
            }

            set
            {
                docId = value;
            }
        }

        public List<int> Freqs
        {
            get
            {
                return freqs;
            }

            set
            {
                freqs = value;
            }
        }

        public List<float> Pesos
        {
            get
            {
                return pesos;
            }

            set
            {
                pesos = value;
            }
        }

        public float Norma
        {
            get
            {
                return norma;
            }

            set
            {
                norma = value;
            }
        }

        public void calcularFreqs(List<String> terminos,XMLFile doc) {
            int counter = 0;
            foreach (String termino in terminos) {
                foreach (String term in doc.DictioWords) {
                    if (term == termino) {
                        counter++;
                    }
                }
                this.freqs.Add(counter);
                counter = 0;
            }

        }

        public void calcularPesos(int[] nsubi,int totalDocs) {
            int indiceFreq = 0;
            foreach (int ni in nsubi)
            {
                if (freqs[indiceFreq] == 0) { pesos.Add(0); }
                else {
                    float formulaPeso = (1 + (float)(Math.Log(freqs[indiceFreq], 2))) * (float)Math.Log((double)totalDocs / nsubi[indiceFreq],2);
                    if (formulaPeso == 0){
                        System.Console.WriteLine("");
                    }
                    pesos.Add(formulaPeso);
                }
                indiceFreq++;
            }
        }
    }
}
