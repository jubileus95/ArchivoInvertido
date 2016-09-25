using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication1
{
    class termFreqPeso
    {
        private int posDic;
        private String termino;
        private List<int> freqxDoc;
        private List<float> pesoxDoc;

       
        public termFreqPeso(int pPosDic, String ptermino, List<docFreqPeso> matrizFreqPesos)
        {
            posDic = pPosDic;
            termino = ptermino;
            freqxDoc = new List<int>();

            pesoxDoc = new List<float>();
            calcularFreqs(matrizFreqPesos);
            calcularPesos(matrizFreqPesos);
            

        }

        private void calcularFreqs(List<docFreqPeso> matrizFreqPesos)
        {

            foreach (docFreqPeso doc in matrizFreqPesos)
            {
                freqxDoc.Add(doc.Freqs[posDic]);
            }

        }

        private void calcularPesos(List<docFreqPeso> matrizFreqPesos)
        {

            foreach (docFreqPeso doc in matrizFreqPesos) {
                pesoxDoc.Add(doc.Pesos[posDic]);    
            }
            
        }

        public int PosDic
        {
            get
            {
                return posDic;
            }

            set
            {
                posDic = value;
            }
        }

        public string Termino
        {
            get
            {
                return termino;
            }

            set
            {
                termino = value;
            }
        }

        public List<int> FreqxDoc
        {
            get
            {
                return freqxDoc;
            }

            set
            {
                freqxDoc = value;
            }
        }

        public List<float> PesoxDoc
        {
            get
            {
                return pesoxDoc;
            }

            set
            {
                pesoxDoc = value;
            }
        }

    }

}
