//Dueñas Nuñez Alan Gabriel 19211630
//impedir creacion de tablas bases y campos con nombres reservados
//cambiar trim end a espacios en dml
//cambiar ^ y - a espacios en blanco

using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;

namespace proyecto
{
    class Program
    {
        static string db = null;
        static List<string> palabras = new List<string>();
        static string rutaPrincipal = @"bases\";
        static string rutaBase = null;
        static string rutaEst = null;
        static string rutaDat = null;

        static List<string> espacios = new List<string>();
        static List<string> nombres = new List<string>();
        static List<string> tipos = new List<string>();

        static List<int> auxBlanco = new List<int>();

        static void Main(string[] args)
        {
            try 
            {
                //creacion de directorio para bases de datos
                if(!Directory.Exists(rutaPrincipal)) Directory.CreateDirectory(rutaPrincipal);

                //variables auxiliares
                int x = 1;
                string comando;

                do 
                {                              
                    Console.Clear();
                    Console.WriteLine("<Proyecto SGBD><" + db + ">");
                    Console.Write("\n>> ");
                    comando = Console.ReadLine();
                    comando = comando.ToLower();
                    comando = comando.Trim();

                    if(comando.EndsWith(";")) {

                        //DDL
                        if(comando.StartsWith("crea base")) {

                            CreaBase(comando);

                        }
                        else if(comando.StartsWith("borra base")) {

                            BorraBase(comando);

                        }
                        else if(comando.StartsWith("usa base")) {

                            UsaBase(comando);

                        }
                        else if(comando.StartsWith("crea tabla")) {

                            //verificar si hay una base seleccionada
                            if(BaseEstaSeleccionada()){
                                CreaTabla(comando);
                            }
                            else{
                                Console.Write("\nBase de datos no seleccionada!");
                                Console.ReadKey();
                            }
                        }
                        else if(comando.StartsWith("borra tabla")) {

                            if(BaseEstaSeleccionada()){
                                BorraTabla(comando);
                            }
                            else{
                                Console.Write("\nBase de datos no seleccionada!");
                                Console.ReadKey();
                            }
                        }
                        else if(comando.StartsWith("borra campo")) {

                            if(BaseEstaSeleccionada()){
                                BorraCampo(comando);
                            }
                            else{
                                Console.Write("\nBase de datos no seleccionada!");
                                Console.ReadKey();
                            }
                        }
                        else if(comando.StartsWith("agrega campo")) {
                            if(BaseEstaSeleccionada()){
                                AgregaCampo(comando);
                            }
                            else{
                                Console.Write("\nBase de datos no seleccionada!");
                                Console.ReadKey();
                            }
                        }
                        else if(comando == "muestra tablas;") {

                            if(BaseEstaSeleccionada()){
                                MuestraTablas();
                            }
                            else{
                                Console.Write("\nBase de datos no seleccionada!");
                                Console.ReadKey();
                            }
                        }
                        else if(comando == "muestra bases;"){

                            MuestraBases();
                        }

                        //DML
                        else if(comando.StartsWith("inserta en")) {

                            if(BaseEstaSeleccionada()){
                                InsertaEn(comando);
                            }
                            else{
                                Console.Write("\nBase de datos no seleccionada!");
                                Console.ReadKey();
                            }
                        }
                        else if(comando.StartsWith("elimina en")) {

                            if(BaseEstaSeleccionada()){
                                EliminaEn(comando);
                            }
                            else{
                                Console.Write("\nBase de datos no seleccionada!");
                                Console.ReadKey();
                            }
                        }

                        else if(comando.StartsWith("modifica en")) {
                            
                            if(BaseEstaSeleccionada()){
                                ModificaEn(comando);
                            }
                            else{
                                Console.Write("\nBase de datos no seleccionada!");
                                Console.ReadKey();
                            }
                        }

                        else if(comando.StartsWith("lista")) {
                            
                            if(BaseEstaSeleccionada()){
                                Lista(comando);
                            }
                            else{
                                Console.Write("\nBase de datos no seleccionada!");
                                Console.ReadKey();
                            }
                        }

                        //salir
                        else if(comando == "salir;") {
                            
                            x = 0;
                        }
                        else {
                            Console.WriteLine(comando);
                            Console.Write("\nComando no reconocido!");
                            Console.ReadKey();
                        }
                    }
                    else {
                        Console.Write("\nComando no finalizado!");
                        Console.ReadKey();
                    }
                }
                while(x == 1);
            }
            catch(Exception e)
            {
                Console.WriteLine("\nError en programa! --> " + e.Message);
                Console.WriteLine(e.StackTrace);
            }          
        }

        //DDL
        static void CreaBase(string comando){

            palabras = DividirComando(comando);

            //Crear base solo si no existe           
            if(!Directory.Exists(rutaPrincipal + palabras[2])) {

                Directory.CreateDirectory(rutaPrincipal + palabras[2]);
                
                Console.Write("\nBase de datos \"" + palabras[2] +  "\" creada con exito!");
                Console.ReadKey();
            }
            else{
                Console.Write("\nLa base de datos ya existe!");
                Console.ReadKey();
            }
        }

        static void BorraBase(string comando) {

            palabras = DividirComando(comando);

            rutaBase = rutaPrincipal + palabras[2];

            //Borra base solo si existe           
            if(Directory.Exists(rutaBase)) {


                foreach (string file in Directory.GetFiles(rutaBase)) {

                    File.Delete(file); 
                }

                Directory.Delete(rutaBase);

                Console.Write("\nBase de datos \"" + palabras[2] +  "\" borrada con exito!");

                if(db != null) db = null;
                
                Console.ReadKey();
            }
            else{
                Console.Write("\nLa base de datos no existe!");
                Console.ReadKey();
            }  
        }
        static void UsaBase(string comando) {

            palabras = DividirComando(comando);
            
            if(Directory.Exists(rutaPrincipal + palabras[2])) {
                
                //Usar base
                db = palabras[2];
                rutaBase = rutaPrincipal + db + @"\";
            }
            else{
                Console.Write("\nLa base de datos no existe!");
                Console.ReadKey();
            }
        }
        static string ObtenerBase() {
            return db;
        }

        static void CreaTabla(string comando) {
            
            //comando principal
            List<string> cmdPrincipal = ObtenerCmdPrincipal(comando);
            
            rutaEst = rutaBase + cmdPrincipal[2] + ".est";
            rutaDat = rutaBase + cmdPrincipal[2] + ".dat";

            if(!File.Exists(rutaEst)){

                palabras = DividirComandoTabla(comando);
                
                //eliminar instruccion base
                palabras.RemoveAt(0);

                //eliminar espacios
                palabras = EliminarEspaciosLista(palabras);

                if(CamposValidos(palabras)) {

                    StreamWriter datos = new StreamWriter(rutaDat);
                    datos.Close();

                    StreamWriter estructura = new StreamWriter(rutaEst);

                    //escribir campos a el archivo de estructura
                    for(int i = 0; i < palabras.Count; i++){

                        //si no es el ultimo
                        if(i != palabras.Count - 1){

                            //Si es de tamaño y el que sigue no es tamaño
                            if((palabras[i].All(char.IsDigit) && !palabras[i + 1].All(char.IsDigit)) || palabras[i] == "fecha"){

                                estructura.Write(palabras[i] + "\n");
                            }
                            else{
                                estructura.Write(palabras[i] + ", ");
                            }
                        }
                        else {
                            estructura.Write(palabras[i]);
                        }
                    }

                    estructura.Close();
                    
                    Console.Write("\nTabla \""+ cmdPrincipal[2] + "\" creada con exito!");
                    Console.ReadKey();
                }
                else {

                    Console.Write("\nNo se pudo crear la tabla!");
                    Console.ReadKey();
                }
            }
            else {
                Console.Write("\nLa tabla ya existe!");
                Console.ReadKey();
            }
        }

        static void BorraTabla(string comando) {

            palabras = DividirComando(comando);

            if(File.Exists(rutaBase + palabras[2] + ".est")) {

                File.Delete(rutaBase + palabras[2] + ".est");
                File.Delete(rutaBase + palabras[2] + ".dat");

                Console.Write("\nTabla \"" + palabras[2] + "\" borrada con exito!");
                Console.ReadKey();
            }
            else {

                Console.Write("\nLa tabla no existe!");
                Console.ReadKey();
            }
        }

        static void BorraCampo(string comando) { 
            
            List<string> cmdPrincipal = ObtenerCmdPrincipal(comando);
                
            rutaEst = rutaBase + cmdPrincipal[2] + ".est";
            rutaDat = rutaBase + cmdPrincipal[2] + ".dat";

            //Si existe la tabla
            if(File.Exists(rutaEst)) {

                palabras = DividirComandoTabla(comando);
                palabras.RemoveAt(0);
                palabras = EliminarEspaciosLista(palabras);

                string campoEliminar = palabras[0];

                if(palabras.Count <= 1) {

                    ObtenerInfoTabla(cmdPrincipal[2]);

                    if(!CamposNoExisten(palabras)) {

                        //Estructura
                        List<string> campos = File.ReadAllLines(rutaEst).ToList();
                        int numCampos = campos.Count;
                        List<string> camposNuevos = new List<string>();
                        List<string> auxLectura = new List<string>();

                        for(int i = 0; i < numCampos; i++) {

                            auxLectura = campos[i].Split(',').ToList();

                            if(auxLectura[0] != campoEliminar) {

                                camposNuevos.Add(campos[i]);
                            }
                        }

                        StreamWriter sobreescritura = new StreamWriter(rutaEst);

                        for(int i = 0; i < camposNuevos.Count; i++) {

                            if(i != camposNuevos.Count - 1) {

                                sobreescritura.WriteLine(camposNuevos[i]);
                            }
                            else {

                                sobreescritura.Write(camposNuevos[i]);
                            }
                        }

                        sobreescritura.Close();                                               

                        //Datos
                        List<int> PosTam = ObtenerPosicion(campoEliminar);
                        int pos = PosTam[0];
                        int tam = PosTam[1];

                        List<string> registros = File.ReadAllLines(rutaDat).ToList();

                        if(registros.Count > 0) {
                            for(int j = 0; j < registros.Count; j++) {

                            registros[j] = registros[j].Remove(pos, tam);
                            }

                            StreamWriter eliminarDat = new StreamWriter(rutaDat);

                            foreach(string reg in registros) {

                                eliminarDat.WriteLine(reg);
                            }

                            eliminarDat.Close();
                        }                

                        Console.Write("\nCampo \"" + campoEliminar + "\" borrado con exito!");
                        Console.ReadKey();
                    }
                    else {

                        Console.Write("\nEl campo no existe!");
                        Console.ReadKey();
                    }                  
                }
                else {

                    Console.Write("\nSolo se puede borrar un campo a la vez!");
                    Console.ReadKey();
                }              
            }
            else {

                Console.Write("\nLa tabla no existe!");
                Console.ReadKey();
            }
        }

        static void AgregaCampo(string comando) {

            List<string> cmdPrincipal = ObtenerCmdPrincipal(comando);

            rutaEst = rutaBase + cmdPrincipal[2] + ".est";
            rutaDat = rutaBase + cmdPrincipal[2] + ".dat";

            if(File.Exists(rutaEst)) {

                palabras = DividirComandoTabla(comando);
                palabras.RemoveAt(0);
                palabras = EliminarEspaciosLista(palabras);

                if(palabras.Count <= 4) {

                    if(CamposValidos(palabras)) {

                        List<string> auxLista = new List<string>();
                        auxLista.Add(palabras[0]);

                        ObtenerNombres(cmdPrincipal[2]);

                        if(CamposNoExisten(auxLista)) {

                            //agregar al est
                            string campoAgregar = palabras[0]; 
                
                            StreamWriter adjuntar = File.AppendText(rutaEst);
                            adjuntar.Write("\n");

                            for(int i = 0; i < palabras.Count; i++) {
                                
                                if(i != palabras.Count - 1){
                                    adjuntar.Write(palabras[i] + ", ");
                                }
                                else {
                                    adjuntar.Write(palabras[i]);
                                }
                            }
                            adjuntar.Close();

                            //agregar al dat

                            List<string> registros = File.ReadAllLines(rutaDat).ToList();

                            if(registros.Count > 0) {
                                string nuevoEspacio = null;

                                if(palabras.Count == 4) {
                                
                                    nuevoEspacio = new string(' ', (Int32.Parse(palabras[2]) + Int32.Parse(palabras[3])));
                                }
                                else if(palabras.Count == 2) {

                                    nuevoEspacio = new string(' ', 8);
                                }
                                else {

                                    nuevoEspacio = new string(' ', Int32.Parse(palabras[2]));
                                }

                                for(int j = 0; j < registros.Count; j++) {

                                    registros[j] = registros[j] + nuevoEspacio;
                                    
                                }

                                StreamWriter adjuntarDat = new StreamWriter(rutaDat);

                                foreach(string reg in registros) {

                                    adjuntarDat.WriteLine(reg);
                                }

                                adjuntarDat.Close();
                            }
                    
                            Console.Write("\nCampo \"" + campoAgregar + "\" agregado con exito!");
                            Console.ReadKey();
                        }
                        else {

                            Console.Write("\nEl campo ya existe!");
                            Console.ReadKey();
                        }                  
                    }
                    else {

                        Console.Write("\nNo se pudo crear la tabla!");
                        Console.ReadKey();
                    }            
                }
                else {

                    Console.Write("\nNo se puede agregar mas de un campo!");
                    Console.ReadKey();
                }              
            }
            else {

                Console.Write("\nLa tabla no existe!");
                Console.ReadKey();
            }
        }

        static void MuestraTablas() {

            if(Directory.GetFiles(rutaBase) != null){

                string contenidoEst = null;                

                foreach(string archivo in Directory.GetFiles(rutaBase, "*.est")){

                    Console.WriteLine("\n[" + Path.GetFileNameWithoutExtension(archivo) + "]");

                    StreamReader lectura = new StreamReader(archivo);
                    contenidoEst = lectura.ReadToEnd();
                    Console.WriteLine(contenidoEst);

                    lectura.Close();
                }
                Console.ReadKey();
            }
            else{
                Console.Write("\nNo existen tablas en \"!" + db + "\"");
                Console.ReadKey();
            }
        }
        static void MuestraBases(){

            if(Directory.GetDirectories(rutaPrincipal) != null){

                foreach(string dir in Directory.GetDirectories(rutaPrincipal)){
                    Console.WriteLine(Path.GetFileName(dir));
                }
                Console.ReadKey();
            }
            else{
                Console.Write("\nNo existen bases de datos!");
                Console.ReadKey();
            }
        }

        //DML
        static void InsertaEn(string comando) { //formato

            List<string> cmdPrincipal = ObtenerCmdPrincipal(comando);
            rutaDat = rutaBase + cmdPrincipal[2] + ".dat";

            if(!File.Exists(rutaDat)) {

                Console.Write("\nLa tabla no existe!");
                Console.ReadKey();
                return;
            }

            palabras = DividirComandoTabla(comando);
            palabras.RemoveAt(0);
            palabras = EliminarEspaciosLista(palabras);          

            int numAntiguos = palabras.Count;
            
            for(int i = 0; i < numAntiguos; i++) {

                palabras.AddRange(palabras[i].Split("="));
            }          
            
            palabras.RemoveRange(0, numAntiguos);
            
            ObtenerInfoTabla(cmdPrincipal[2]);

            List<string> auxPalabras = new List<string>();

            for(int i = 0; i < palabras.Count; i++) {

                if(i % 2 == 0) {
                    auxPalabras.Add(palabras[i]);
                }
            }

            if(CamposNoExisten(auxPalabras)) {

                Console.Write("\nUno o multiples campos no existen!");
                Console.ReadKey();
                return;
            }          

            string registro = null;
            int p = 0;
            int tam = 0;
            int longitud = 0;

            for(int i = 0; i < nombres.Count; i++) {
                
                longitud = 0;

                //si el nombre analizado (palabras) es igual a elemento de nombres
                if(nombres[i] == palabras[p]) {

                    if((tipos[i] == "fecha" || tipos[i] == "entero" || tipos[i] == "decimal") 
                    && !palabras[p + 1].All(char.IsDigit)) {

                        Console.WriteLine("\nError de formato!");
                        Console.ReadKey();
                        return;
                    }

                    if(palabras[p + 1].Length > ObtenerSuma(i)) {

                        Console.WriteLine("\nError de espacio!");
                        Console.ReadKey();
                        return;
                    }

                    longitud = palabras[p + 1].Length;
                    registro = registro + palabras[p + 1];  

                    //si no es el ultimo
                    if(p < palabras.Count - 2) {

                        p += 2;
                    }                  
                }

                 //si es decimal
                if(tipos[i] == "decimal") {

                    tam = espacios[i].Split(",").ToList().Sum(x => Convert.ToInt32(x)); //suma enteros y decimal                  
                }
                else {

                    tam = Int32.Parse(espacios[i]);
                }

                //si no se llena el espacio
                if((tam > longitud)) {

                    for(int k = 0; k < tam - longitud; k++){

                        registro = registro + ' ';
                    }
                }                         
            }

            //ingresar registro al dat
            Console.WriteLine("\nRegistro ingresado: [" + registro + "]");

            StreamWriter insertar = File.AppendText(rutaDat);
            insertar.WriteLine(registro);
            insertar.Close();

            Console.ReadKey();
        }

        static void EliminaEn(string comando) {
            comando = comando.TrimEnd(';');
            
            //elimina en tbl1 donde c1 = 4;
            palabras = comando.Split("donde").ToList();
            List<string> cmdPrincipal = palabras[0].Trim().Split(' ').ToList();

            rutaDat = rutaBase + cmdPrincipal[2] + ".dat";

            if(!File.Exists(rutaDat)) {

                Console.Write("\nLa tabla no existe!");
                Console.ReadKey();
                return;
            }

            palabras.RemoveAt(0);
            palabras = EliminarEspaciosLista(palabras);
            palabras.AddRange(palabras[0].Split("="));
            palabras.RemoveAt(0);

            ObtenerInfoTabla(cmdPrincipal[2]);

            string campoRef = palabras[0]; 
            string valorRef = palabras[1];

            List<int> PosTam = ObtenerPosicion(campoRef);
            int pos = PosTam[0];
            int tam = PosTam[1];

            //Eliminar del dat
            List<string> regAntiguos = File.ReadAllLines(rutaDat).ToList();
            List<string> regNuevos = new List<string>();
            string valorCampo = null;

            for(int i = 0; i < regAntiguos.Count; i++) {

                valorCampo = regAntiguos[i].Substring(pos, tam).TrimEnd(' ');

                if(valorCampo != valorRef) { 

                    regNuevos.Add(regAntiguos[i]);
                }
            }
            
            StreamWriter sobreescritura = new StreamWriter(rutaDat);

            for(int i = 0; i < regNuevos.Count; i++) {

                sobreescritura.WriteLine(regNuevos[i]);
            }
            
            sobreescritura.Close();

            Console.Write("\nRegistros eliminados!");
            Console.ReadKey();
        }

        static void ModificaEn(string comando) {

            List<string> cmdPrincipal = ObtenerCmdPrincipal(comando);
            rutaDat = rutaBase + cmdPrincipal[2] + ".dat";

            if(!File.Exists(rutaDat)) {

                Console.Write("\nLa tabla no existe!");
                Console.ReadKey();
                return;
            }

            palabras = DividirComandoTabla(comando);
            palabras.RemoveAt(0);
            palabras = EliminarEspaciosLista(palabras);                     

            int numAntiguos = palabras.Count;         
            for(int i = 0; i < numAntiguos; i++) {

                palabras.AddRange(palabras[i].Split("="));
            }          
            
            palabras.RemoveRange(0, numAntiguos);
            
            ObtenerInfoTabla(cmdPrincipal[2]);

            List<string> busqueda = palabras.GetRange(palabras.Count - 2, 2);
            palabras.RemoveRange(palabras.Count - 2, 2);

            busqueda[0] = busqueda[0].Substring(5);

            string campoRef = busqueda[0];
            string valorRef = busqueda[1];

            List<int> PosTam = ObtenerPosicion(campoRef);
            int pos = PosTam[0];
            int tam = PosTam[1];

            //Modifica el dat
            List<string> regAntiguos = File.ReadAllLines(rutaDat).ToList();
            List<string> regNuevos = new List<string>();
            string valorCampo = null;

            for(int i = 0; i < regAntiguos.Count; i++) {

                valorCampo = regAntiguos[i].Substring(pos, tam).TrimEnd(' ');

                if(valorCampo == valorRef) { //si el campo de busqueda coincide

                    //modificar registro
                    
                    string registro = null;
                    int p = 0; 
                    int tamCampo = 0; 
                    int longitud; 

                    for(int j = 0; j < nombres.Count; j++) {
                        
                        longitud = 0;

                        //si el nombre analizado (palabras) es igual a elemento de nombres
                        if(nombres[j] == palabras[p]) {

                            longitud = palabras[p + 1].Length;
                            registro = registro + palabras[p + 1];  

                            //si no es el ultimo
                            if(p < palabras.Count - 2) {

                                p += 2;
                            }   

                            //si es decimal
                            if(tipos[j] == "decimal") {

                                tamCampo = espacios[j].Split(",").ToList().Sum(x => Convert.ToInt32(x)); //suma enteros y decimal                  
                            }
                            else {

                                tamCampo = Int32.Parse(espacios[j]);
                            }

                            //si no se llena el espacio
                            if((tamCampo > longitud)) {

                                for(int k = 0; k < tamCampo - longitud; k++){

                                    registro = registro + ' ';
                                }
                            }                
                        }
                        else {
    
                            if(tipos[j] == "decimal") {
                                
                                longitud = espacios[j].Split(",").ToList().Sum(x => Convert.ToInt32(x));
                            }
                            else {
                                longitud = Int32.Parse(espacios[j]);
                            }

                            int suma = 0;

                            for(int c = 0; c < nombres.IndexOf(nombres[j]); c++) {

                                if(tipos[c] == "decimal") {

                                    suma = suma + espacios[c].Split(",").ToList().Sum(x => Convert.ToInt32(x));
                                }
                                else {
                                    suma = suma + Int32.Parse(espacios[c]);
                                }    
                            }

                            registro = registro + (regAntiguos[i].Substring(suma, longitud));
                        }                 
                    }                      

                    //agregar a nuevos
                    regNuevos.Add(registro);

                }
                else {
                    //Console.ReadKey();
                    regNuevos.Add(regAntiguos[i]);
                }
            }
            
            StreamWriter sobreescritura = new StreamWriter(rutaDat);

            for(int i = 0; i < regNuevos.Count; i++) {

                sobreescritura.WriteLine(regNuevos[i]);
            }
            
            sobreescritura.Close();

            Console.Write("\nRegistros modificados!");
            Console.ReadKey();
        }

        static void Lista(string comando) {   

            comando = comando.TrimEnd(';');    

            List<string> nombreCampos = new List<string>(); 
            nombreCampos.Clear();  

            if(comando.Contains(',')) {

                palabras = DividirComandoTabla(comando);

                palabras[0] = palabras[0].Remove(0, 6);
                palabras[palabras.Count - 1] = palabras[palabras.Count - 1].TrimStart();

                string tabla = null;

                if(comando.Contains("donde")) {

                    //donde
                    palabras.AddRange(palabras[palabras.Count - 1].Split("donde").ToList());

                    List<string> busqueda = palabras[palabras.Count - 1].Split('=').ToList();
                    busqueda = EliminarEspaciosLista(busqueda);
                    string campoRef = busqueda[0];
                    string valorRef = busqueda[1];

                    palabras.RemoveAt(palabras.Count - 1);

                    palabras[palabras.Count - 1] = palabras[palabras.Count - 1].TrimEnd();

                    //tabla
                    palabras.AddRange(palabras[palabras.Count - 1].Split(' ').ToList());
                    tabla = palabras[palabras.Count - 1];

                    palabras[palabras.Count - 5] = palabras[palabras.Count - 3];
                    palabras.RemoveRange(palabras.Count - 4, 4);

                    ObtenerInfoTabla(tabla);                   
                    rutaDat = rutaBase + tabla + ".dat";

                    List<string> registros = File.ReadAllLines(rutaDat).ToList();

                    
                    palabras = EliminarEspaciosLista(palabras);

                    List<int> PosTam = ObtenerPosicion(campoRef);
                    int pos = PosTam[0];
                    int tam = PosTam[1];
                    string valorCampo = null;
 
                    ImprimirNombres(palabras);

                    for(int i = 0; i < registros.Count; i++) {

                        valorCampo = registros[i].Substring(pos, tam).TrimEnd(' ');
                        

                        if(valorCampo == valorRef) { //si el campo de busqueda coincide

                            Console.Write("[");

                            int suma = 0;
                            int mod = 0;
                            int r = 0; 
                            string registroFinal = null;

                            List<int> PosTamImprimir = new List<int>();
                            int posImprimir;
                            int tamImprimir;

                            for(int h = 0; h < nombres.Count; h++) {                           
                                    
                                if(nombres[h] == palabras[r]) {

                                    PosTamImprimir = ObtenerPosicion(palabras[r]);
                                    posImprimir = PosTamImprimir[0];
                                    tamImprimir = PosTamImprimir[1];
                                    
                                    registroFinal = registroFinal + registros[i].Substring(posImprimir, tamImprimir);

                                    if(r < palabras.Count - 1) {

                                        r++;
                                    }  
                                }                                  
                            }

                            for(int j = 0; j < palabras.Count; j++) {
                                                                                                   
                                PosTamImprimir = ObtenerPosicion(palabras[j]);
                                posImprimir = PosTamImprimir[0];
                                tamImprimir = PosTamImprimir[1];                             

                                suma = ObtenerSuma(nombres.IndexOf(palabras[j])); 
                                suma = suma + mod;

                                string salto = new string(' ', auxBlanco[j]);

                                if(registroFinal.Length > suma) {

                                    registroFinal = registroFinal.Insert(suma, salto);
                                }
                                else {
                                    registroFinal = registroFinal + salto;
                                }
                                       
                                mod = suma + salto.Length;                                
                            }
                            
                            Console.Write(registroFinal);
                            Console.Write("]\n");                                                                                                       
                        }
                    }

                }
                else {
                    
                    //tabla
                    palabras.AddRange(palabras[palabras.Count - 1].Split(' ').ToList());
                    tabla = palabras[palabras.Count - 1];

                    palabras[palabras.Count - 4] = palabras[palabras.Count - 3];
                    palabras.RemoveRange(palabras.Count - 3, 3);

                    rutaDat = rutaBase + tabla + ".dat";
                    ObtenerInfoTabla(tabla);

                    palabras = EliminarEspaciosLista(palabras);
                    List<string> registros = File.ReadAllLines(rutaDat).ToList();

                    ImprimirNombres(palabras);

                    for(int i = 0; i < registros.Count; i++) {

                        Console.Write("[");

                        int suma = 0;
                        int mod = 0;
                        int r = 0; 
                        string registroFinal = null;

                        List<int> PosTamImprimir = new List<int>();
                        int posImprimir;
                        int tamImprimir;

                        for(int h = 0; h < nombres.Count; h++) {                           
                                
                            if(nombres[h] == palabras[r]) {

                                PosTamImprimir = ObtenerPosicion(palabras[r]);
                                posImprimir = PosTamImprimir[0];
                                tamImprimir = PosTamImprimir[1];
                                
                                registroFinal = registroFinal + registros[i].Substring(posImprimir, tamImprimir);

                                if(r < palabras.Count - 1) {
                                    r++;
                                }                             
                            }                                  
                        }

                        for(int j = 0; j < palabras.Count; j++) {
                                                                                                
                            PosTamImprimir = ObtenerPosicion(palabras[j]);
                            posImprimir = PosTamImprimir[0];
                            tamImprimir = PosTamImprimir[1];                             

                            suma = ObtenerSuma(nombres.IndexOf(palabras[j])); 
                            suma = suma + mod;

                            string salto = new string(' ', auxBlanco[j]);

                            if(registroFinal.Length > suma) {

                                registroFinal = registroFinal.Insert(suma, salto);
                            }
                            else {
                                registroFinal = registroFinal + salto;
                            }
                                    
                            mod = suma + salto.Length;                                
                        }
                        
                        Console.Write(registroFinal);
                        Console.Write("]\n");                                                                                                       
                        
                    }
                }
            }
            else {
                                
                palabras = comando.Split(' ').ToList();

                ObtenerInfoTabla(palabras[3]);
                    
                rutaDat = rutaBase + palabras[3] + ".dat";


                List<string> registros = File.ReadAllLines(rutaDat).ToList();

                if(palabras.Contains("donde")) {

                    string auxiliar = null;

                    for(int i = palabras.IndexOf("donde") + 1; i < palabras.Count; i++) {

                        auxiliar = auxiliar + palabras[i];
                    }

                    List<string> busqueda = new List<string>();
                    busqueda.AddRange(auxiliar.Split('=').ToList());
                    busqueda = EliminarEspaciosLista(busqueda);
                    palabras.RemoveRange(palabras.IndexOf("donde"), palabras.Count - palabras.IndexOf("donde"));

                    string campoRef = busqueda[0];
                    string valorRef = busqueda[1];

                    List<int> PosTam = ObtenerPosicion(campoRef);
                    int pos = PosTam[0];
                    int tam = PosTam[1];
                    string valorCampo = null;  

                    //si es impresion de todos
                    if(palabras[1] == "*") {

                        nombreCampos.Add("*"); 
                        ImprimirNombres(nombreCampos);
                                                                               
                        for(int i = 0; i < registros.Count; i++) {

                            valorCampo = registros[i].Substring(pos, tam).TrimEnd(' ');

                            if(valorCampo == valorRef) { //si el campo de busqueda coincide                                                                         
                                
                                Console.Write("[");

                                int suma = 0;
                                int mod = 0;

                                for(int j = 0; j < nombres.Count; j++) {

                                    suma = ObtenerSuma(j); 
                                    suma = suma + mod;

                                    string salto = new string(' ', auxBlanco[j]);

                                    registros[i] = registros[i].Insert(suma, salto);       
                                    mod = suma + salto.Length;
                                }

                                Console.Write(registros[i]);
                                Console.Write("]\n");
                            }
                        }
                    }
                    else {                   

                        string campoImprimir = palabras[1];

                        nombreCampos.Add(campoImprimir);
                        ImprimirNombres(nombreCampos);

                        List<int> PosTamImprimir = ObtenerPosicion(campoImprimir);
                        int posImprimir = PosTamImprimir[0];
                        int tamImprimir = PosTamImprimir[1];

                        for(int i = 0; i < registros.Count; i++) {

                            valorCampo = registros[i].Substring(pos, tam).TrimEnd(' ');

                            if(valorCampo == valorRef) { //si el campo de busqueda coincide   

                                Console.Write("[");

                                int suma = 0;

                                suma = ObtenerSuma(nombres.IndexOf(campoImprimir)); 

                                string salto = new string(' ', auxBlanco[0]);

                                registros[i] = registros[i].Insert(posImprimir + suma, salto);       

                                Console.Write(registros[i].Substring(posImprimir, (suma + salto.Length)));
                                Console.Write("]\n");                                                                                                       
                            }
                        }
                    }

                    
                }        
                else {

                    if(palabras[1] == "*") {     

                        
                        ImprimirNombres(nombres);
                                                                               
                        for(int i = 0; i < registros.Count; i++) {                                                                      
                                
                            Console.Write("[");

                            int suma = 0;
                            int mod = 0;

                            for(int j = 0; j < nombres.Count; j++) {

                                suma = ObtenerSuma(j); 
                                suma = suma + mod;

                                string salto = new string(' ', auxBlanco[j]);

                                registros[i] = registros[i].Insert(suma, salto);       
                                mod = suma + salto.Length;
                            }

                            Console.Write(registros[i]);
                            Console.Write("]\n");
                            
                        }
                    }
                    else {

                        string campoImprimir = palabras[1];

                        nombreCampos.Add(campoImprimir);
                        ImprimirNombres(nombreCampos);

                        List<int> PosTamImprimir = ObtenerPosicion(campoImprimir);
                        int posImprimir = PosTamImprimir[0];
                        int tamImprimir = PosTamImprimir[1];

                        for(int i = 0; i < registros.Count; i++) {

                            Console.Write("[");

                            int suma = 0;

                            suma = ObtenerSuma(nombres.IndexOf(campoImprimir)); 

                            string salto = new string(' ', auxBlanco[0]);

                            registros[i] = registros[i].Insert(posImprimir + suma, salto);       

                            Console.Write(registros[i].Substring(posImprimir, (suma + salto.Length)));
                            Console.Write("]\n");                                                                                                                                  
                        }
                    }
                }     
            }

            Console.ReadKey();
        }


        //Metodos auxiliares
        static List<string> DividirComando(string cmd){

            List<string> lista = new List<string>();

            cmd = cmd.TrimEnd(';');
            lista = cmd.Split(' ').ToList();

            //juntar el nombre en una sola cadena
            if(lista.Count > 3){
                
                lista[2] = JuntarLista(lista.GetRange(2, lista.Count - 2));
                lista.RemoveRange(3, lista.Count - 3);
            }

            return lista;
        }

        static List<string> DividirComandoTabla(string cmd) {

            List<string> lista = new List<string>();

            cmd = cmd.TrimEnd(';');
            lista = cmd.Split(',').ToList();

            return lista;
        }

        static bool CamposValidos(List<string> lista) {

            if(lista.Count >= 2){

                int x = 1;
                int bandera = 0;
                List<string> listaNombres = new List<string>();

                for(int i = 0; i < lista.Count; i++) {

                    switch(x){

                        case 1:

                            char primerChar = lista[i].ToCharArray().ElementAt(0);

                            if(!char.IsLetter(primerChar)) {
                                Console.Write("\nError de nombre de campo!");
                                return false;
                            }

                            x++;
                            listaNombres.Add(lista[i]);

                            break;

                        case 2:

                            if(lista[i] != "entero" 
                                && lista[i] != "caracter"
                                && lista[i] != "decimal"
                                && lista[i] != "fecha") {
                                    Console.Write("\nError de tipo de campo!");
                                    return false;
                                }

                            if(lista[i] == "decimal"){

                                bandera = 1;
                            }
                            else if(lista[i] == "fecha"){

                                x = 0;
                            }

                            x++;
                            break;

                        case 3:

                            //si todos no son digitos
                            if(!lista[i].All(char.IsDigit) || lista[i].All(c => c == '0')){
                                Console.Write("\nError de tamaño de campo!");
                                return false;
                            }

                            //si el campo no es decimal
                            if(bandera != 1){

                                x = 1;
                            }
                            else{

                                //es decimal pero no definio decimales al final del comando
                                if(i == lista.Count - 1){
                                    Console.Write("\nError de tamaño de campo!");
                                    return false;
                                }
                                else{
                                    bandera = 0; 
                                }                                              
                            }

                            //quitar 0 de la izquierda
                            if(lista[i].StartsWith("0")) lista[i] = lista[i].TrimStart('0');
                            
                            break;
                    }
                }

                //verificar que no haya nombres de campos iguales
                if(ListaTieneIguales(listaNombres)) {
                    Console.Write("\nExisten campos con el mismo nombre!");
                    return false;
                }

                return true;
            }
            else{
                Console.Write("\nDebe haber minimo un campo en la tabla!");
                return false;
            }           
        }

        static bool BaseEstaSeleccionada(){

            if(db != null) return true;
            else return false;
        }

        static List<string> EliminarEspaciosLista(List<string> lista){

            List<string> listaNueva = new List<string>();

            for(int i = 0; i < lista.Count; i ++){

                if(lista[i].Contains(' ')) {

                    listaNueva.Add(lista[i].Trim());
                    listaNueva[i] = lista[i].Replace(" ", "");
                }      
                else{
                    listaNueva.Add(lista[i]);
                }             
            }

            return listaNueva;
        }

        static string JuntarLista(List<string> lista){

            string ListaJunta = null;

            for(int i = 0; i < lista.Count; i++){

                ListaJunta = ListaJunta + lista[i];
            }

            return ListaJunta;
        }

        static bool ListaTieneIguales(List<string> lista){ 

            if(lista.Count != lista.Distinct().Count()) return true;
            else return false;           
        }

        static List<string> ObtenerEspacios(string tabla) { 

            List<string> espaciosCampos = new List<string>();

            rutaEst = rutaBase + tabla + ".est";
            List<string> aux = new List<string>();

            foreach(string linea in File.ReadAllLines(rutaEst)) {

                aux = linea.Split(",").ToList();

                if(aux.Count == 3) {
                    espaciosCampos.Add(aux[2].TrimStart());
                }
                else if(aux.Count == 4){
                    espaciosCampos.Add(aux[2].TrimStart() + "," + aux[3].TrimStart());
                }
                else if( aux.Count == 2) {
                    espaciosCampos.Add("8");
                }
            }
                        
            return espaciosCampos;
        }

        static List<string> ObtenerNombres(string tabla) {

            List<string> nombresCampos = new List<string>();

            rutaEst = rutaBase + tabla + ".est";
            List<string> aux = new List<string>();

            foreach(string linea in File.ReadAllLines(rutaEst)) {

                aux = linea.Split(",").ToList();

                nombresCampos.Add(aux[0]);
            }
            
            return nombresCampos;
        }

        static List<string> ObtenerTipos(string tabla) {

            List<string> tiposCampos = new List<string>();

            rutaEst = rutaBase + tabla + ".est";
            List<string> aux = new List<string>();

            foreach(string linea in File.ReadAllLines(rutaEst)) {
                aux = linea.Split(",").ToList();

                tiposCampos.Add(aux[1].TrimStart());
            }
                    
            return tiposCampos;
        }

        static List<string> ObtenerCmdPrincipal(string comando) {

            List<string> nuevaLista = DividirComandoTabla(comando)[0].Split(" ").ToList();
            nuevaLista = DividirComando(string.Join( " ", nuevaLista.ToArray()));

            return nuevaLista;
        }

        static List<int> ObtenerPosicion(string campoRef) {

            campoRef = campoRef.Trim();

            List<int> lista = new List<int>();
            int tamX = 0;
            int tamY = 0;                      
            int posX = 0;         
            int posY = ObtenerSuma(0) - 1;
            int numAnteriores = nombres.IndexOf(campoRef);

            if(numAnteriores > 0) {

                for(int i = 0; i < numAnteriores; i++) {

                    if(tipos[i] == "decimal") {

                        tamX = espacios[i].Split(",").ToList().Sum(x => Convert.ToInt32(x));                      
                    }
                    else {

                        tamX = Int32.Parse(espacios[i]);                      
                    }       

                    if(tipos[i + 1] =="decimal") {

                        tamY = espacios[i + 1].Split(",").ToList().Sum(x => Convert.ToInt32(x));
                    }
                    else {

                        tamY = Int32.Parse(espacios[i + 1]);
                    }

                    posX = posX + tamX;    
                    posY = posY + tamY;                             
                } 
            }
            else {

                tamY = ObtenerSuma(0);
            }


            lista.Add(posX);
            lista.Add(tamY);

            return lista;
        }

        static void ObtenerInfoTabla(string tabla) {

            espacios = ObtenerEspacios(tabla);          
            nombres = ObtenerNombres(tabla);          
            tipos = ObtenerTipos(tabla);
        }

        static void ImprimirNombres(List<string> nombreCampos) {

            auxBlanco.Clear();

            if(nombreCampos[0] == "*") {

                Console.Write("[");

                for(int i = 0; i < nombres.Count; i++) {

                    int tam = ObtenerSuma(i);

                    //si el nombre del campo es mas chico o igual
                    if(nombres[i].Length <= tam) {
                     
                        int blanco = tam - nombres[i].Length;

                        Console.Write(nombres[i]);

                            for(int j = 0; j < blanco; j++) {
                                Console.Write(" ");
                            }

                            auxBlanco.Add(0);                     
                    }
                    else {

                        Console.Write(nombres[i] + " ");

                        auxBlanco.Add(nombres[i].Length - tam + 1);
                    }

                }

                Console.Write("]");
                
            }
            else {

                int x = 0;

                Console.Write("[");

                for(int i = 0; i < nombres.Count; i++) {

                    if(nombres[i] == nombreCampos[x]) {

                        int tam = ObtenerSuma(i);

                        if(nombres[i].Length <= tam) {

                            int blanco = tam - nombres[i].Length;
                            
                            Console.Write(nombres[i]);

                            for(int j = 0; j < blanco; j++) {
                                Console.Write(" ");
                            }

                            auxBlanco.Add(0);
                        }
                        else {

                            Console.Write(nombres[i] + " ");        
                            auxBlanco.Add(nombres[i].Length - tam + 1);
                        }

                        if(x < nombreCampos.Count - 1) {
                            x++;
                        }
                    }
                }

                Console.Write("]");
            }

            Console.WriteLine();
        }

        static int ObtenerSuma(int indice) {

            if(tipos[indice] == "decimal") {

                return espacios[indice].Split(",").ToList().Sum(x => Convert.ToInt32(x));
            }
            else {

                return Int32.Parse(espacios[indice]);                           
            }
        }

        static bool CamposNoExisten(List<string> lista) {


            for(int i = 0; i < nombres.Count; i++) {

                for(int j = 0; j < lista.Count; j++) {

                    if(nombres[i] == lista[j]) {

                        return false;
                    }
                }
            }

            return true;
        }
    }
}
