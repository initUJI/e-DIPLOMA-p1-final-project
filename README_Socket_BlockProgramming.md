
## Preparar sistema para ejecutar el servidor

1. Para poder conectar los clientes (las instancias del juego) al servidor, el servidor necesita estar ejecutándose en un ordenador que esté conectado a la misma red que los clientes. (En caso de estar usando el "localhost")

2. Esta conexión puede hacerse a través de una red local (("localhost") los clientes y el servidor están conectados en una red LAN o WLAN) o a través de internet (el servidor tiene una dirección IP pública que puede ser alcanzada a través de internet).

3. Normalmente, si el servidor y los clientes están en la misma WLAN o LAN pueden alcanzarse sin ninguna configuración adicional ("localhost"). Las redes privadas de empresas normalmente permiten esta comunicación.

4. El servidor puede ser ejecutado en cualquier sistema operativo.

5. El servidor necesita tener Node.js instalado. Puedes descargarlo desde la página web oficial: https://nodejs.org/en/download/. El servidor debe ser ejecutado con Node.js 20 o superior.


## Ejecutar el servidor: localhost

1. Descarga los archivos del servidor y colócalos en cualquier lugar que desees en el ordenador del servidor (preferiblemente en la carpeta de instalación de la build, para que sea más accesible).

2. Colócate en la carpeta del servidor y abre una terminal en esa carpeta. Puedes hacer esto haciendo clic derecho en la carpeta y seleccionando la opción "Abrir terminal aquí" si estás en un entorno gráfico o moviéndote a través de las carpetas si estás en un entorno de línea de comandos.

3. Una vez que tengas la terminal abierta en la carpeta del servidor, ejecuta el siguiente comando: npm start. Este comando instalará todas las dependencias necesarias y ejecutará el servidor.

4. Para cerrar el servidor solo necesitas enfocarte en la ventana de la terminal y presionar Ctrl + C.

5. Para asegurar condiciones óptimas del servidor para cada prueba, se recomienda reiniciar el servidor. Para hacer esto, simplemente cierra el servidor y ejecuta el comando npm start de nuevo.

REINICIO DE SERVER: Realizar el paso 4 y después el paso 3.


## Ejecutar el servidor: 150.128.97.41 

1. Para ejecutar el servidor, el primer paso es conectarse a la dirección del servidor. En Windows, esto se puede hacer con el comando: “ssh ediploma@150.128.97.41” en cualquier aplicación de terminal (por ejemplo, Windows Powershell) y escribir la contraseña ediploma2024.

2. Una vez que hayas iniciado sesión, debes ejecutar el comando “cd socket_server” para colocarte en la carpeta desde donde puedes ejecutar el servidor.

3. Finalmente, ejecuta el comando “npm start” y el servidor estará en funcionamiento.

4. En caso de que quieras reiniciar el servidor, puedes presionar “Ctrl + C” para detener la ejecución y repetir el paso 3 para volver a encenderlo.

5. Es importante tener en cuenta que no se puede cerrar la terminal o el servidor se apagará.

6. Si deseas cerrar la ventana de la terminal antes del paso 3, tendrás que ejecutar el comando “screen -S server”. Una vez hecho esto, asegúrate de estar en la carpeta “socket_server” (si no, repite el paso 2) y luego completa el paso 3. Ahora, antes de cerrar la terminal, debes presionar “Ctrl + A” y luego presionar “D”.


REINICIO DE SERVER: Leer el paso 4.


## FLUJO DEL JUEGO

Una vez se inicie la aplicación, se mostrará un menu donde se podrá escoger entre tres escenarios diferentes: Zombies, Plants y Trash. Es importante que los dos jugadores elijan la misma escena para evitar que el flujo falle y tener sucesos inesperados.

- Zombies: El objetivo será pasar por los botones negros que levantan los muros exteriores.

- Trash: El objetivo será pasar por encima de las basuras esparcidas por el escenario

- Plants: El objetivo será pasar por encima de las plantas, y una vez encima, el coche deberá realizar la acción "GetHumidity" para cumplir el objetivo.

NOTA: En cada escenario hay 4 objetivos que cumplir. Conforme se vayan cumpliendo los objetivos, los jugadores tendrán más tipos de bloques de programación, pudiendo así realizar sentencias más complejas.


Elegido el escenario deseado, dará inicio un minijuego donde el usuario deberá montar su coche con la piezas de Arduino necesarias para la misión escogida. Si se ha elegido tanto la escena Zombies o Trash, se montará el coche con los mismos componentes. En la escena Plants, se deberá añadir además el sensor de humedad, acorde con la misión a cumplir.

Una vez completada la misión de montaje de nuestro coche, se lanzará la escena de BlockProgramming colaborativa. 

La escena de juego empieza con un fondo opaco con un mensaje de la aplicación tratando de conectarse al servidor. En caso de que la conexión sea satisfactoria, se mostrará al jugador el ID de player asignado así como el nombre de la sala. El primer jugador quedará en espera una vez haya hecho conexión con el servidor, ya que hasta que el segundo jugador no se conecte no dará inicio el ejercicio. El segundo jugador deberá hacer el mismo proceso de inicio de escena.

Con ambos jugadores conectados al servidor, la pantalla opaca desaparecerá y podremos ver el escenario del juego. A partir de ahora, los jugadores podrán montar sus bloques de código mientras ven en el "Partner Code" el código que está desarrollando el compañero. Cuando el usuario tenga su bloque de código definitivo, podrá darle al botón de "Play" para decirle al servidor que ese player está listo. Una vez el juego haya comprobado que el bloque de código no tiene ningún fallo y es válido para su ejecución, el jugador se quedará a la espera hasta que su compañero termine también de construir su bloque de código. 

Cuando el servidor haya detectado que ambos jugadores ya han lanzado su bloque de código válido para su ejecución, se procedera a procesar dichas sentencias simultáneamente en ambos dispositivos. Una vez terminada la ejecución de la sentencia de código, dará comienzo el siguiente turno.

El juego se termina cuando se cumple el objetivo establecido para el nivel escogido. Una vez terminado el ejercicio, se puede cerrar la aplicación. En esta Demo, una vez terminado el ejercicio no se puede continuar con el flujo de la aplicación.



## NOTAS IMPORTANTES 

- En caso de que el servidor funcione (detecte el ID del player y muestre el nombre de la sala), pero esté detectando mal los jugadores (detecta dos Player 1), se deberá reiniciar el servidor. Hay instrucciones sobre cómo realizar este reinicio en cada una de las modalidades de servidor. 

- Si se encuentras problemas con la conexión de diferentes sistemas, se puede iniciar el servidor de "localhost" y iniciar dos instancias de la aplicación en el mismo ordenador. Con "Alt+Tab" se puede intercambiar entre las dos instancias y llevar a cabo el flujo del juego.

