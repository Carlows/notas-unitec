###Links de interes:

- Carpeta personal de dropbox con contenido sobre C#, Jquery, ASP.Net, LINQ, etc
  https://www.dropbox.com/sh/0dhg5tk897aqlrb/AACRkwcy_45ijSbVme1SE-Xua?dl=0
  (Recomiendo descargar PRO ASP.Net MVC 5, Learning JQuery y LINQ In Action)

- Sitio oficial de ASP.NET
  http://asp.net/
  (Recomiendo leer las secciones de ASP.NET MVC y Entity Framework) 

- Sitio de preguntas y respuestas sobre cualquier tema de programación
  http://stackoverflow.com/
  (Leer el FAQ para principiantes antes de hacer cualquier pregunta, por lo general ya otra persona ha tenido el mismo problema,   asi que antes de abrir una pregunta, busca muy bien dentro de la página.)

#### Angular JS

 - http://codewala.net/2014/05/28/learning-angularjs-with-examplespart-1/
   Introduccion a AngularJS (El inglés no está muy bien escrito, pero se entiende bien).

- https://www.dropbox.com/sh/i8gie2q379urpet/AAANSRvn7ZlKVNmvlt1ke6dsa?dl=0
  Libro Pro AngularJS de Adam Freeman (de los mejores, para mi)
-----------------------------------------------------------------------------------------------------------------

#Centro de Innovación Tecnológica
=========

###Lenguaje y Tecnologías que utilizará la aplicación

-**ASP.NET MVC 5** 		- 	Framework  para el desarrollo de aplicaciones web

-**Entity Framework 6**	- 	ORM Entidad-relación para controlar los datos

-**JQuery**			-	Programacion para el cliente

-**Twitter Bootstrap**	-	Diseño de la página web

-**LocalDB - SQL Server**	-	Almacenamiento de datos

Patron de diseño MVC 

MODELO - Almacenamiento de los datos, operaciones que hagamos hacia la base de datos

VISTAS - Html, Jquery, CSS

CONTROLADORES - Darle un formato a los datos de nuestro modelo para que los consuman las vistas


------------------------------------------------------------------------------

###Aplicación para el control de asistencias, notas, y minutas de los equipos de proyectos.

Tipos de usuario -> Coordinador de celula, Profesores.

- Inicialmente, la aplicación llevará el control de las minutas para los equipos de proyecto, 
te permitirá redactar la minuta desde el sistema y también subirlas como archivo de texto.
- Al profesor se le permite cambiar el status de los alumnos, tanto como a los coordinadores de celula,
como a los integrantes de cada una.
- Cuando a un alumno se le asigna el status de coordinador de celula, a este se le permite cargar minutas
para su celula. También podrá controlar la asistencia de los integrantes de su celula.
- Los profesores tendran una sección para controlar el calendario de actividades; las minutas podrán cargarse
de acuerdo a este calendario.
- Las minutas subidas por cada coordinador de celula necesitarán aprobación del profesor, por lo que este 
tendrá una sección para aprobar las minutas.

#### Diagrama inicial de la base de datos

http://prntscr.com/4xie2h

#### Pantalla de Log-in

http://prntscr.com/503ymm

#### Diseño inicial

http://prntscr.com/503ywh

#### Lista Profesores

http://prntscr.com/5261bu

#### Agregar Profesor

http://prntscr.com/5261gw

#### Crear Proyecto

http://prntscr.com/5261kx

#### Lista de Proyectos

http://prntscr.com/5261vo

----------------------------------------------------------------------------------

###TO-DOs

- El profesor, puede ver los alumnos de su proyecto, y cambiarlos de celula.
- Extender la informacion que se muestra al usuario.
- Las vistas proyectos, celulas, etc, necesitan un mejor diseño.

#### Done

- Modelamiento de la base de datos, definir las entidades y las relaciones.
- Diseño básico de la interfaz (podría ser mejorada luego).
- Controladores Administrador, Profesor, Coordinador, al usuario loguearse, este es redireccionado hacia el controlador segun su rol.
- Log-in.
- El profesor debe poder editar la informacion del proyecto.
- El profesor debe poder editar las celulas creadas.
- El admin debe poder editar los proyectos creados.

- El coordinador Entra y encuentra informacion de su celula en el inicio.
- El coordinador puede editar la informacion de su celula.
- El coordinador encuentra tambien en el inicio la semana actual en la que se encuentra el calendario del proyecto, se muestra la informacion de la semana.
- Puede agregar, editar y borrar alumnos de su celula.
- El coordinador puede subir minutas a la semana.
- El profesor puede revisar la lista de minutas enviadas en la semana.
- Resetear los estilos del elemento \<pre\>
- La lista de profesores, coordinadores y alumnos se ordena por nombre y apellido
- El admin puede revisar todas las celulas que tiene un proyecto, pero no las puede editar.
- El profesor puede, en la lista de las celulas, revisar una lista de minutas enviadas por cada una de las celulas.
- El administrador puede agregar un nuevo administrador.
- El profesor puede agregar un nuevo profesor al proyecto.
- El profesor puede aprobar estas minutas.
- El coordinador puede revisar sus minutas enviadas, no puede editarlas.
- El coordinador no puede editar la minuta actual cuando ya esta aprobada.
- Se le muestran al profesor una lista de minutas pendientes por aprobar antes de terminar la semana.
- El coordinador manda una lista de asistencia desde su celula, en esta semana. No puede editar esta lista luego de enviarla.
- Tabla de asistencias en el profesor, alumnos desde sem 1 a sem 12. #BUG -> Workaround, al terminar la semana, se le asigna inasistente a todos los alumnos de las celulas que no mandaron la asistencia.
- Al finalizar la semana 12 se cierra el calendario
- Al cerrar el calendario, se crean notas para cada alumno del proyecto.
- Las notas por minutas se calculan con la sumatoria de las minutas enviadas en cada celula este calendario.
- Las notas por asistencias se calculan con la sumatoria de las asistencias en este calendario por cada alumno.
- Se crea una tabla con notas para cada alumno, con las notas por minuta de su celula, y las notas por sus asistencias.
- Sale un nuevo panel en el index del profesor dandole la opcion de subir las notas por la evaluacion final.
- Cuando se hayan subido esas notas, el panel dira que el calendario ha sido finalizado.
- Al finalizar el calendario, el profesor podra ver una tabla con las notas de los alumnos.
- EL profesor puede cambiar de calendario, es decir, crear uno nuevo.
- El coordinador puede subir la minuta a traves de un archivo de texto
- Permitir a todos los usuarios cambiar su contraseña.
- Cuando un proyecto termina el calendario actual, el administrador, verificara el estado actual del calendario del proyecto, y si esta finalizado y no hay uno nuevo, exportara la informacion de los alumnos a csv.
- Hacer el calendario configurable
- File uploader, datos a español.
- Corregir login validacion

----------------------------------------------------------------------------------
