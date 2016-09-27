using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;


namespace ConsoleApplication1
{
    class XMLFile
    {
        private static int counter = 0;
        private String fileName;
        private String taxonName;
        private String rank;
        private String description;
        private String terms;
        private string[] words;
        private List<String> dictioWords;
        private int docId;


        public XMLFile(String xmlPath) {
            this.FileName = xmlPath;
            this.DocId = counter;
                
            XmlDocument myDoc = new XmlDocument();
            myDoc.Load(xmlPath);

            this.description = myDoc.SelectSingleNode("//@taxon_description").Value;
            this.rank = myDoc.SelectSingleNode("//@rank").Value;
            this.taxonName = myDoc.SelectSingleNode("//@taxon_name").Value;
            dictioWords = new List<String>();

            quitarAcentos();
            quitarCaracteresEspeciales();
            indexTaxon();
            indexRank();
            dictioWords.Sort();

            myDoc = null;
            GC.Collect();
            counter++;

        }

        public string TaxonName
        {
            get
            {
                return taxonName;
            }

            set
            {
                taxonName = value;
            }
        }

        public string Rank
        {
            get
            {
                return rank;
            }

            set
            {
                rank = value;
            }
        }

        public string Description
        {
            get
            {
                return description;
            }

            set
            {
                description = value;
            }
        }

        public string Terms
        {
            get
            {
                return terms;
            }

            set
            {
                terms = value;
            }
        }

        public string[] Words
        {
            get
            {
                return words;
            }

            set
            {
                words = value;
            }
        }

        public List<string> DictioWords
        {
            get
            {
                return dictioWords;
            }

            set
            {
                dictioWords = value;
            }
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

        public string FileName
        {
            get
            {
                return fileName;
            }

            set
            {
                fileName = value;
            }
        }

        public int quitarStopwords() {
            string[] stopwords = new string[] {"", "a", "actualmente", "acuerdo", "adelante", "ademas", "además", "adrede", "afirmó", "agregó", "ahi", "ahora", "ahí", "al", "algo", "alguna", "algunas", "alguno", "algunos", "algún", "alli", "allí", "alrededor", "ambos", "ampleamos", "antano", "antaño", "ante", "anterior", "antes", "apenas", "aproximadamente", "aquel", "aquella", "aquellas", "aquello", "aquellos", "aqui", "aquél", "aquélla", "aquéllas", "aquéllos", "aquí", "arriba", "arribaabajo", "aseguró", "asi", "así", "atras", "aun", "aunque", "ayer", "añadió", "aún", "b", "bajo", "bastante", "bien", "breve", "buen", "buena", "buenas", "bueno", "buenos", "c", "cada", "casi", "cerca", "cierta", "ciertas", "cierto", "ciertos", "cinco", "claro", "comentó", "como", "con", "conmigo", "conocer", "conseguimos", "conseguir", "considera", "consideró", "consigo", "consigue", "consiguen", "consigues", "contigo", "contra", "cosas", "creo", "cual", "cuales", "cualquier", "cuando", "cuanta", "cuantas", "cuanto", "cuantos", "cuatro", "cuenta", "cuál", "cuáles", "cuándo", "cuánta", "cuántas", "cuánto", "cuántos", "cómo", "d", "da", "dado", "dan", "dar", "de", "debajo", "debe", "deben", "debido", "decir", "dejó", "del", "delante", "demasiado", "demás", "dentro", "deprisa", "desde", "despacio", "despues", "después", "detras", "detrás", "dia", "dias", "dice", "dicen", "dicho", "dieron", "diferente", "diferentes", "dijeron", "dijo", "dio", "donde", "dos", "durante", "día", "días", "dónde", "e", "ejemplo", "el", "ella", "ellas", "ello", "ellos", "embargo", "empleais", "emplean", "emplear", "empleas", "empleo", "en", "encima", "encuentra", "enfrente", "enseguida", "entonces", "entre", "era", "eramos", "eran", "eras", "eres", "es", "esa", "esas", "ese", "eso", "esos", "esta", "estaba", "estaban", "estado", "estados", "estais", "estamos", "estan", "estar", "estará", "estas", "este", "esto", "estos", "estoy", "estuvo", "está", "están", "ex", "excepto", "existe", "existen", "explicó", "expresó", "f", "fin", "final", "fue", "fuera", "fueron", "fui", "fuimos", "g", "general", "gran", "grandes", "gueno", "h", "ha", "haber", "habia", "habla", "hablan", "habrá", "había", "habían", "hace", "haceis", "hacemos", "hacen", "hacer", "hacerlo", "haces", "hacia", "haciendo", "hago", "han", "hasta", "hay", "haya", "he", "hecho", "hemos", "hicieron", "hizo", "horas", "hoy", "hubo", "i", "igual", "incluso", "indicó", "informo", "informó", "intenta", "intentais", "intentamos", "intentan", "intentar", "intentas", "intento", "ir", "j", "junto", "k", "l", "la", "lado", "largo", "las", "le", "lejos", "les", "llegó", "lleva", "llevar", "lo", "los", "luego", "lugar", "m", "mal", "manera", "manifestó", "mas", "mayor", "me", "mediante", "medio", "mejor", "mencionó", "menos", "menudo", "mi", "mia", "mias", "mientras", "mio", "mios", "mis", "misma", "mismas", "mismo", "mismos", "modo", "momento", "mucha", "muchas", "mucho", "muchos", "muy", "más", "mí", "mía", "mías", "mío", "míos", "n", "nada", "nadie", "ni", "ninguna", "ningunas", "ninguno", "ningunos", "ningún", "no", "nos", "nosotras", "nosotros", "nuestra", "nuestras", "nuestro", "nuestros", "nueva", "nuevas", "nuevo", "nuevos", "nunca", "o", "ocho", "os", "otra", "otras", "otro", "otros", "p", "pais", "para", "parece", "parte", "partir", "pasada", "pasado", "paìs", "peor", "pero", "pesar", "poca", "pocas", "poco", "pocos", "podeis", "podemos", "poder", "podria", "podriais", "podriamos", "podrian", "podrias", "podrá", "podrán", "podría", "podrían", "poner", "por", "porque", "posible", "primer", "primera", "primero", "primeros", "principalmente", "pronto", "propia", "propias", "propio", "propios", "proximo", "próximo", "próximos", "pudo", "pueda", "puede", "pueden", "puedo", "pues", "q", "qeu", "que", "quedó", "queremos", "quien", "quienes", "quiere", "quiza", "quizas", "quizá", "quizás", "quién", "quiénes", "qué", "r", "raras", "realizado", "realizar", "realizó", "repente", "respecto", "s", "sabe", "sabeis", "sabemos", "saben", "saber", "sabes", "salvo", "se", "sea", "sean", "segun", "segunda", "segundo", "según", "seis", "ser", "sera", "será", "serán", "sería", "señaló", "si", "sido", "siempre", "siendo", "siete", "sigue", "siguiente", "sin", "sino", "sobre", "sois", "sola", "solamente", "solas", "solo", "solos", "somos", "son", "soy", "soyos", "su", "supuesto", "sus", "suya", "suyas", "suyo", "sé", "sí", "sólo", "t", "tal", "tambien", "también", "tampoco", "tan", "tanto", "tarde", "te", "temprano", "tendrá", "tendrán", "teneis", "tenemos", "tener", "tenga", "tengo", "tenido", "tenía", "tercera", "ti", "tiempo", "tiene", "tienen", "toda", "todas", "todavia", "todavía", "todo", "todos", "total", "trabaja", "trabajais", "trabajamos", "trabajan", "trabajar", "trabajas", "trabajo", "tras", "trata", "través", "tres", "tu", "tus", "tuvo", "tuya", "tuyas", "tuyo", "tuyos", "tú", "u", "ultimo", "un", "una", "unas", "uno", "unos", "usa", "usais", "usamos", "usan", "usar", "usas", "uso", "usted", "ustedes", "v", "va", "vais", "valor", "vamos", "van", "varias", "varios", "vaya", "veces", "ver", "verdad", "verdadera", "verdadero", "vez", "vosotras", "vosotros", "voy", "vuestra", "vuestras", "vuestro", "vuestros", "w", "x", "y", "ya", "yo", "z", "él", "ésa", "ésas", "ése", "ésos", "ésta", "éstas", "éste", "éstos", "última", "últimas", "último", "últimos" };
            string[] toDictionary= Words.Where(x => !stopwords.Contains(x)).ToArray();
            dictioWords.AddRange(toDictionary);
            return toDictionary.Length;
        }

        public void quitarAcentos() { 
            byte[] tempBytes;
            tempBytes = System.Text.Encoding.GetEncoding("ISO-8859-8").GetBytes(description);
            string asciiStr = System.Text.Encoding.UTF8.GetString(tempBytes);
            description = asciiStr;
            tempBytes = System.Text.Encoding.GetEncoding("ISO-8859-8").GetBytes(taxonName);
            asciiStr = System.Text.Encoding.UTF8.GetString(tempBytes);
            taxonName = asciiStr;
            tempBytes = System.Text.Encoding.GetEncoding("ISO-8859-8").GetBytes(rank);
            asciiStr = System.Text.Encoding.UTF8.GetString(tempBytes);
            rank = asciiStr; 

        }

        public void indexTaxon() {

            taxonName = taxonName.ToLower();
            StringBuilder sb = new StringBuilder();

            foreach (char c in taxonName)
            {
                if ((c >= '0' && c <= '9') || (c >= 'A' && c <= 'Z') || (c >= 'a' && c <= 'z') || c == ' ' || c == 'ñ' || c == 'Ñ')
                { sb.Append(c); }
                else { sb.Append(' '); }
            }

            terms = sb.ToString();
            terms = Regex.Replace(terms, @"\s+", " ");
            words = terms.Split(' ');   
                     
            int cant = quitarStopwords();

            if (cant > 2) { 
            cant--;
            cant--;
            DictioWords.RemoveRange(DictioWords.Count-cant,cant);
            }
        }

        public void indexRank() {
            rank = rank.ToLower();
            StringBuilder sb = new StringBuilder();

            foreach (char c in rank)
            {
                if ((c >= '0' && c <= '9') || (c >= 'A' && c <= 'Z') || (c >= 'a' && c <= 'z') || c == ' ' || c == 'ñ' || c == 'Ñ')
                { sb.Append(c); }
                else { sb.Append(' '); }
            }

            terms = sb.ToString();
            terms = Regex.Replace(terms, @"\s+", " ");
            words = terms.Split(' ');

            quitarStopwords();

        }

        public void quitarCaracteresEspeciales()
        {

            description = description.ToLower();
            StringBuilder sb = new StringBuilder();

            foreach (char c in description)
            {
                if ((c >= '0' && c <= '9') || (c >= 'A' && c <= 'Z') || (c >= 'a' && c <= 'z') || c == ' ' || c=='ñ' || c=='Ñ')
                { sb.Append(c); }
                else { sb.Append(' '); }
            }

            terms = sb.ToString();
            terms = Regex.Replace(terms, @"\s+", " ");
            words = terms.Split(' ');

            quitarStopwords();

        }
    }

}
