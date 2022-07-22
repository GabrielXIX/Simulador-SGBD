# Proyecto Simulador-SGBD
Este proyecto desarrollado en C# con .NET 5 tiene como propósito imitar un sistema gestor de base de datos con las funcionalidades básicas de definición y manipulación de objetos utilizando la consola de comandos simulando el lenguaje de SQL para consultas, por tanto, el programa usa los archivos y directorios del sistema operativo donde se ejecuta para poder imitar el funcionamiento de una base de datos real. Cuando se define una tabla se crean dos archivos: un archivo .est que sirve para almacenar su estructura y un archivo .dat para sus registros, cuando el usuario guarda datos en una tabla el programa escribe un registro de izquierda a derecha con cada campo en orden y sin espacios entre estos, la única vez en la que se encuentran espacios en blanco dentro de un campo es cuando el valor que se le dió es menor al espacio.

## Funcionalidades y sintaxis:

- ### Crear base
  `crea base <nombreBase>;`
  
  <img src="https://user-images.githubusercontent.com/65438145/180336322-8f2fc07c-7612-432c-94a9-d2f8538d62a9.png" width="750">
 
- ### Mostrar bases
  `muestra bases;`
  
  <img src="https://user-images.githubusercontent.com/65438145/180337005-d8c8180e-2dc3-43aa-9a9f-5e629b226546.png" width="750">
 
- ### Borrar base
  `borra base <nombreBase>;`
  
  <img src="https://user-images.githubusercontent.com/65438145/180337029-76eb0bb7-e9c5-4957-a5ab-051b8dcc2fdf.png" width="750">
 
- ### Usar base
  `usa base <nombreBase>;`
  
  <img src="https://user-images.githubusercontent.com/65438145/180337037-f444dc86-eb68-4990-b839-76789648d4e7.png" width="375">
  <img src="https://user-images.githubusercontent.com/65438145/180337065-c1da9f19-2f56-44e1-99dc-d3decb48b16b.png" width="375">
  
- ### Crear tabla
  `crea tabla <nombreTabla>, <nombreCampo1>, <tipoCampo1>, <espacioCampo1>, <nombreCampo2>, <tipoCampo2>, <espacioCampo2>, ..., <nombreCampoN>, <tipoCampoN>, <espacioCampoN>;`
  
  <img src="https://user-images.githubusercontent.com/65438145/180337089-50fcccb4-b4ad-4123-a088-35719eb2811c.png" width="550">
  <img src="https://user-images.githubusercontent.com/65438145/180337257-ca3a2820-e418-4c75-aa44-30a372562159.png" width="200">
 
- ### Mostrar tablas
  `muestra tablas;`
  
  <img src="https://user-images.githubusercontent.com/65438145/180337291-bcbd5053-bd12-4f96-8651-db02a28e1ecb.png" width="750">
 
- ### Borrar tabla
  `borra tabla <nombreTabla>;`
  
  <img src="https://user-images.githubusercontent.com/65438145/180337329-e76a4f7a-91ba-4a79-8632-58f206346b74.png" width="750">
 
- ### Agregar campo
  `agrega campo <nombreTabla>, <nombreCampo>, <tipoCampo>, <espacioCampo>;`
  
  <img src="https://user-images.githubusercontent.com/65438145/180337375-604c2568-ee2a-441c-8ca2-644e05657ce2.png" width="750">
 
- ### Borrar campo
  `borra campo <nombreTabla>, <nombreCampo>;`
  
  <img src="https://user-images.githubusercontent.com/65438145/180337463-450e91a7-2886-4e50-bcb8-5fed42896d49.png" width="750">
 
- ### Insertar en tabla
  `inserta en <nombreTabla>, <nombreCampo1> = <valorCampo1>, <nombreCampo2> = <valorCampo2>, ..., <nombreCampoN> = <valorCampoN>;`
  
  <img src="https://user-images.githubusercontent.com/65438145/180337605-f4a9a83b-361f-48b1-87d3-fe01a63abc64.png" width="750">
  <img src="https://user-images.githubusercontent.com/65438145/180337614-e2c0fc62-5445-450d-9825-8ba0ec35c054.png" width="750">
  <img src="https://user-images.githubusercontent.com/65438145/180337622-e5bfd1e2-8e7e-4aeb-a3ff-fecf12164255.png" width="750">
 
- ### Eliminar en tabla
  `elimina en <nombreTabla> donde <nombreCampo> = <valorCampo>;`
  
  <img src="https://user-images.githubusercontent.com/65438145/180337917-2a1febd5-0620-4077-b90b-48bf0584b3d4.png" width="750">
 
- ### Modificar registros de tabla
  `modifica en <nombreTabla>, <nombreCampo1> = <valorCampo1>, <nombreCampo2> = <valorCampo2>, ..., <nombreCampoN> = <valorCampoN>, donde <nombreCampo> = <valorCampo>;`
  
  <img src="https://user-images.githubusercontent.com/65438145/180337991-787ecd63-10bd-41f1-819e-78169401ed8c.png" width="750">
 
- ### Listar registros
  `lista * de <nombreTabla>;`
  
  <img src="https://user-images.githubusercontent.com/65438145/180338195-112c8331-3f3c-408b-bcf7-80c80d5f7c77.png" width="400">
  
  `lista * de <nombreTabla> donde <nombreCampo> = <valorCampo>;`
  
  <img src="https://user-images.githubusercontent.com/65438145/180338281-f9d12c01-48c4-48ae-ad64-9d341c424939.png" width="400">
  
  `lista <nombreCampo> de <nombreTabla>;`
  
  <img src="https://user-images.githubusercontent.com/65438145/180338314-6de679dc-7715-46e0-97a0-6d0522b1ac92.png" width="400">
  
  `lista <nombreCampo> de <nombreTabla> donde <nombreCampo> = <valorCampo>;`
  
  <img src="https://user-images.githubusercontent.com/65438145/180338562-7c4230ee-3e16-4794-8de8-a90b1db422cd.png" width="400">
  
  `lista <nombreCampo1>, <nombreCampo2>, ..., <nombreCampoN> de <nombreTabla>;`
  
  <img src="https://user-images.githubusercontent.com/65438145/180338702-8cd00e9d-aafe-4992-ae07-c90a57509ccc.png" width="400">
  
  `lista <nombreCampo1>, <nombreCampo2>, ..., <nombreCampoN> de <nombreTabla> donde <nombreCampo> = <valorCampo>;`
  
  <img src="https://user-images.githubusercontent.com/65438145/180338721-a8ad39c1-fe1c-4479-ab01-64660a537b2b.png" width="400">
 
 
 
 
 
 












