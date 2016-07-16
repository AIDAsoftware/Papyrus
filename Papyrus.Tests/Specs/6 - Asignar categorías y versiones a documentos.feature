Característica: Gestionar versiones y categorías de un documento
## Criterios de aceptación:
# * El listado de versiones debe ser configurable
# * Un documento debe poder ser asignado a varias versiones

Escenario: Asignar una versión de producto a un documento
	Dado un documento en la biblioteca de documentos
	Cuando le asignamos a ese documento el producto "Sima" y la versión "1.0"
	Entonces ese documento se guarda asignado al producto "Sima" y a la versión "1.0"
	Y está accesible en la biblioteca de documentos del producto "Sima" para la versión "1.0"

## Criterios de aceptación:
# * Un documento debe poder tener múltiples categorías asignadas
# * La misma categoría no puede ser asignada varias veces a un documento
# * Las categorías deben poderse seleccionar de una estructura arbórea
# * Las categorías deben poder ser configurables

Escenario: Asignar categorías a un documento
	Dado un documento en la biblioteca de documentos
	Cuando el documentalista asigne la categoría "General" al documento
	Y el documentalista asigne la categoría "Usuarios" al documento
	Entonces el documento tendrá la categoría "General"
	Y  el documento tendrá la categoría "Usuarios"

Escenario: Crear una nueva versión para un documento de ayuda
	Dado un documento en la biblioteca de documentos
	Y que está asignado al producto "Sima" y a la versión "1.0"
	Cuando el documentalista decide crear una nueva versión del documento
	Y selecciona la versión "1.1" del producto "Sima"
	Entonces se crea un nuevo documento de ayuda que es una copia del documento existente
	Y está asignado al producto "Sima" y la versión es la "1.1"
