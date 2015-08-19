Característica: Generar el índice de la ayuda
	Como documentalista
	Quiero crear el índice de contenidos de la ayuda de un producto concreto
	Y una versión concreta
	Y en un idioma concreto
	Para que los usuarios de ese producto puedan consultar la documentación adecuada para su instalación

## Criterios de aceptación
# * Una página de índice puede tener varias páginas hijas y a su vez puede ser hija de una página
# * Todos los índices tienen una página "Home" que sirve como nodo principal del índice.
# * El título de las páginas se podrá definir en varios idiomas.

Escenario: Definir una página en el índice de la ayuda
	Dado un producto con nombre "Sima"
	Y versión "1.0"
	Cuando el documentalista añada una página al índice con título "Login"
	Y la página sea hija de la página "Home"
	Y esté asociada al documento "Entrar en el sistema"
	Entonces se generará una nueva página en el índice
	Y se podrá acceder al documento "Entrar en el sistema" a través la página "Login"
