Característica: Gestión básica de documentos de ayuda
	Como documentalista
	Quiero gestionar los documentos de ayuda
	Para que se puedan consultar en la ayuda de mis aplicaciones

Escenario: Crear un nuevo documento
	Dado que tenemos escrito un documento de texto
	Y el título es "Login en el sistema"
	Y la descripción es "Modos de acceso disponibles a SIMA 2"
	Y el contenido es "El usuario podrá acceder al sistema indicando su usuario"
	Y el idioma es "es-ES"
	Cuando el documentalista guarda el documento
	Entonces se crea un documento de ayuda con título "Login en el sistema"
	Y el contenido es "El usuario podrá acceder al sistema indicando su usuario"
	Y la descripción "Modos de acceso disponibles a SIMA 2"
	Y el idioma es "es-ES"
	Y podremos encontrar el documento con título "Login en el sistema" en la biblioteca de documentos

# Criterios de aceptación:
# * Todos los campos existentes en el documento deben ser modificables

Escenario: Modificar un documento existente
	Dado un documento en la biblioteca de documentos con el identificador "12345" y el título "Login en el sistema"
	Cuando el documentalista modifique el título del documento a "Entrar en el sistema"
	Entonces podremos encontrar el documento con título "Entrar en el sistema" en la biblioteca de documentos
	Y NO podremos encontrar el documento con título "Login en el sistema" en la biblioteca de documentos

Escenario: Eliminar un documento existente
	Dado un documento en la biblioteca de documentos
	Cuando el documentalista elija eliminar dicho documento
	Entonces ese documento dejará de aparecer en la biblioteca de documentos


Escenario: Verificar que el Servicio de documento añade un id automaticamente al guardar si no posee uno
	