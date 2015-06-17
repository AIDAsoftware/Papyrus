Característica: Añadir imágenes a un documento
	Como documentalista
	Quiero insertar imágenes en un documento de ayuda existente
	Para facilitar a los usuarios el entendimiento de la aplicación


Escenario: Añadir imágenes nuevas a un documento
	Dado un documento en formato Markdown con título "Entrar en el sistema"
	Cuando el documentalista suba una imagen con nombre "Login.png" mientras edita el contenido del documento
	Entonces la imagen quedará insertada en el documento
	Y en el documento podrá verse la imagen una vez convertido a HTML

Escenario: Añadir imágenes existentes a un documento
	Dado un documento en formato Markdown con título "Cambiar contraseña"
	Y una imagen subida previamente con nombre "Confirmar contraseña.png"
	Cuando el documentalista seleccione la imagen con nombre "Confirmar contraseña.png" mientras edita el contenido del documento
	Entonces la imagen quedará insertada en el documento
	Y en el documento podrá verse la imagen una vez convertido a HTML
