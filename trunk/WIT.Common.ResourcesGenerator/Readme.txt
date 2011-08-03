Es una aplicación de consola que a partir de un archivo excel genera el archivo de recursos para usar en aplicaciones .NET.

Hay 3 campos para personalizar en el web.config:

	importResourcesExcelConnectionString:   Es el connection string para leer del archivo excel. Donde dice Data Source poner la ruta del archivo excel que contiene los recursos.
	importResourceExcelDestination:			Es la ruta de destino del archivo de recursos.
	resourcesType:							El tipo de los recursos.

Luego de generar el archivo de recursos,
abrir un archivo ya existente y copiar el código que está encerrado entre
<xsd:schema> y </xsd:schema>

Dejo un excel de ejemplo llamado resources.xls