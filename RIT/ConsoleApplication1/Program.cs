using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;

namespace ConsoleApplication1
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                System.Console.WriteLine("Utilice el comando -i para indexar, -c para consulta vectorial y -ce para consulta estructurada");

            }
            else
            {
                String command = args[0];

                if (command == "-i")
                { // Indexar

                    String collectionPath = args[1];
                    String archivoInvertidoPath = args[2];
                    String prefijo = args[3];
                    CrearArchivoInvertido(prefijo, collectionPath, archivoInvertidoPath);


                }

                else if (command == "-c")
                { //Consulta vectorial

                    String archivoInvertidoPath = args[1];
                    String prefijo = args[2];
                    List<String> consulta = new List<String>();

                    for (int i = 3; i < args.Length; i++)
                    {
                        if (args[i][0] == '!')
                        {
                            args[i]=args[i].Substring(1);
                            try
                            {
                                consulta.Add(args[i] + " " + args[i + 1]);
                                i++;
                            }
                            catch (System.IndexOutOfRangeException)
                            {
                                consulta.Add(args[i]);
                            }
                            
                        }
                        else {
                            consulta.Add(args[i]);
                        }
                        
                    }

                    DataTable diccionarioTable = cargarDiccionario(archivoInvertidoPath + "diccionario.txt");
                    DataTable documentosTable = cargarDocumentos(archivoInvertidoPath + "documentos.txt");

                    List<TerminoConsulta> consultas = new List<TerminoConsulta>();




                    System.Console.WriteLine("Done");
                }

                else if (command == "-ce")
                { // consulta estructurada


                }
                else
                {
                    System.Console.WriteLine("Utilice el comando -i para indexar, -c para consulta vectorial y -ce para consulta estructurada");
                }
            }

        }

        static void CrearArchivoInvertido(string prefijo, string collectionPath, string archivoInvertidoPath)
        {
            List<XMLFile> docs = new List<XMLFile>();
            string[] documentos = Directory.GetFiles(collectionPath);

            foreach (String document in documentos)
            { //Abre todos los documentos 
                XMLFile doc = new XMLFile(document);
                docs.Add(doc);
            }

            List<String> terminos = new List<string>();
            foreach (XMLFile doc in docs)
            {
                terminos = terminos.Union(doc.DictioWords).ToList();
            }
            terminos.Sort();

            int[] nsubi = Enumerable.Repeat(0, terminos.Count).ToArray();


            int indiceForTerm = 0;

            foreach (String termino in terminos)
            {
                foreach (XMLFile doc in docs)
                {
                    foreach (String term in doc.DictioWords)
                        if (term == termino)
                        {
                            nsubi[indiceForTerm] += 1;
                            break;
                        }
                }
                indiceForTerm++;
            }

            List<docFreqPeso> matrizDocTerm = new List<docFreqPeso>();
            List<termFreqPeso> matrizTermDoc = new List<termFreqPeso>();
            foreach (XMLFile doc in docs)
            {
                matrizDocTerm.Add(new docFreqPeso(terminos, doc, nsubi, docs.Count));
            }

            int termIndice = 0;
            foreach (String termino in terminos)
            {
                matrizTermDoc.Add(new termFreqPeso(termIndice, termino, matrizDocTerm));
                termIndice++;
            }

            DataTable documentosTable = new DataTable();
            DataTable diccionarioTable = new DataTable();
            DataTable postingsTable = new DataTable();

            documentosTable.Columns.Add("docID", typeof(int));
            documentosTable.Columns.Add("docPath", typeof(string));

            diccionarioTable.Columns.Add("termino", typeof(string));
            diccionarioTable.Columns.Add("inicio", typeof(int));
            diccionarioTable.Columns.Add("numDocs", typeof(int));


            postingsTable.Columns.Add("docID", typeof(int));
            postingsTable.Columns.Add("frecuencia", typeof(int));
            postingsTable.Columns.Add("peso", typeof(float));



            foreach (XMLFile doc in docs)
            {
                documentosTable.Rows.Add(doc.DocId, doc.FileName);
            }

            int termIndex = 0;
            int initPos = 0;

            foreach (int numdocs in nsubi)
            {
                diccionarioTable.Rows.Add(terminos[termIndex], initPos, numdocs);
                termIndex++;
                initPos += numdocs;
            }


            int wordInddice = 0;
            foreach (termFreqPeso termino in matrizTermDoc)
            {
                foreach (int freq in termino.FreqxDoc)
                {
                    if (freq != 0)
                    {
                        postingsTable.Rows.Add(wordInddice, termino.FreqxDoc[wordInddice], termino.PesoxDoc[wordInddice]);
                    }
                    wordInddice++;

                }
                wordInddice = 0;

            }

            byte[] archivoInvertido = new byte[12 * postingsTable.Rows.Count];
            int writeIndex = 0;
            foreach (DataRow row in postingsTable.Rows)
            {
                byte[] docBytes = BitConverter.GetBytes((int)row.ItemArray[0]);
                byte[] freqBytes = BitConverter.GetBytes((int)row.ItemArray[1]);
                byte[] pesoBytes = BitConverter.GetBytes((float)row.ItemArray[2]);
                System.Buffer.BlockCopy(docBytes, 0, archivoInvertido, writeIndex, 4);
                System.Buffer.BlockCopy(freqBytes, 0, archivoInvertido, writeIndex + 4, 4);
                System.Buffer.BlockCopy(pesoBytes, 0, archivoInvertido, writeIndex + 8, 4);
                writeIndex += 12;
            }
            

            File.WriteAllBytes(archivoInvertidoPath + prefijo + "postings", archivoInvertido);
            generarArchivos(archivoInvertidoPath, prefijo, "diccionario", diccionarioTable);
            generarArchivos(archivoInvertidoPath, prefijo, "documentos", documentosTable);



        }
        static public void generarArchivos(String path,String prefijo,String tipoArchivo, DataTable myDt)
        {
            int i;
            File.Delete(path + prefijo + tipoArchivo + ".txt");
            StreamWriter swExtLogFile = new StreamWriter(path+prefijo+tipoArchivo+".txt", true);
            foreach (DataRow row in myDt.Rows)
            {
                object[] array = row.ItemArray;
                for (i = 0; i < array.Length - 1; i++)
                {
                    swExtLogFile.Write(array[i].ToString() + ",");
                }
                swExtLogFile.WriteLine(array[i].ToString());
            }
            swExtLogFile.Write("*****END OF DATA****" + DateTime.Now.ToString());
            swExtLogFile.Flush();
            swExtLogFile.Close();

        }

        static public DataTable cargarDiccionario(String strFilePath) {

            DataTable diccionarioTable = new DataTable();

            diccionarioTable.Columns.Add("termino", typeof(string));
            diccionarioTable.Columns.Add("inicio", typeof(int));
            diccionarioTable.Columns.Add("numDocs", typeof(int));

  

            StreamReader sr = new StreamReader(strFilePath);


            while (!sr.EndOfStream)
            {
                string[] rows = Regex.Split(sr.ReadLine(), ",(?=(?:[^\"]*\"[^\"]*\")*[^\"]*$)");
                if (rows.Length > 1) { 
                DataRow dr = diccionarioTable.NewRow();
                for (int i = 0; i < diccionarioTable.Columns.Count; i++)
                {
                    dr[i] = rows[i];
                }
                diccionarioTable.Rows.Add(dr);
                }
           
            }
            return diccionarioTable;

        }
        
        static public DataTable cargarDocumentos(String strFilePath)
        {

            DataTable documentosTable = new DataTable();

            documentosTable.Columns.Add("docID", typeof(int));
            documentosTable.Columns.Add("docPath", typeof(string));



            StreamReader sr = new StreamReader(strFilePath);


            while (!sr.EndOfStream)
            {
                string[] rows = Regex.Split(sr.ReadLine(), ",(?=(?:[^\"]*\"[^\"]*\")*[^\"]*$)");
                if (rows.Length > 1)
                {
                    DataRow dr = documentosTable.NewRow();
                    for (int i = 0; i < documentosTable.Columns.Count; i++)
                    {
                        dr[i] = rows[i];
                    }
                    documentosTable.Rows.Add(dr);
                }

            }
            return documentosTable;

        }
    }
}
