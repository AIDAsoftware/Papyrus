Característica: Añadir referencias a documentos de ayuda
	Como documentalista
	Quiero vincular a un documento referencias a otros documentos
	Para proporcionar a los usuarios información relacionada con el documento que están viendo

## Criterios de aceptación
# * Un documento puede tener múltiples referencias
# * Un documento no puede tener dos veces la misma referencia
# * Aunque un documento esté vinculado a otro documento debe accederse a este documento a través del índice
# * Si un documento tiene referencias a otros documentos que no están en el índice del producto que el usuario
#   está consultando, entonces esas referencias no deben aparecer.

Escenario: Relacionar un documento con otros documentos existentes
	Dado un documento de ayuda en edición con título "Registro"
	Y un documento de ayuda con título "Login"
	Cuando el documentalista añade al documento en edición una referencia al documento "Login"
	Entonces podemos navegar al documento "Login" cuando visualizamos la versión HTML del documento "Registro"

## Criterios de aceptación
# * El listado de referencias no se debe pregenerar, se generará "en caliente" cada vez que un usuario acceda a ver el documento que contiene las referencias

Escenario: Añadir referencias por categorías
	Dado un documento de ayuda en edición con título "Registro"
	Y varios documentos de ayuda con la categoría "Gestión del usuario"
	Cuando el documentalista añade al documento en edición una referencia a los documentos con la categoría "Gestión del usuario"
	Entonces al visualizar la versión HTML del documento "Registro" podremos ver las referencias a todos los documentos con la categoría "Gestión del usuario"


