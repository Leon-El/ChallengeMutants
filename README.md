# Examen Mutants

_Proyecto API-REST solicitado por Magneto para poder detectar si el ADN una persona persona es Humano o Mutante_

## Notas: 🚀

_En el desarollo de la APP se utilizaron variables para metrizadas en el archivo config de la app, también podrían haber sido pustos en una BD de parámetros_
_Estos valores parametrizados sirven para que la app pueda variar en tamaño de cadena de ADN (MAX_DNA_ARRAY), cambiar los caracteres aceptados en la cadena (VALID_DNA),  cantidad de veces que se tiene que repetir un caracter para ser mutante (MAX_SEQUENCE) y por último la cantidad de cadenas válidas a encontrar para ser mutante (MIN_MATCHES)._
_En AppRepository se utilizaron las entidades en memoria, esto normalmente lo utilizo para los test unitarios, para pupular con datos efímeros que desaparecen al finalizar el test. Esto también es útil a la hora de publicar y testear la app en la nube. Además al implementar inyección de dependencias está listo para implementar las interfaces con otro proyecto dentro de la solución que utilice un ORM (por ej. Entity Framework) y reemplazarlo_
_La lógica de negocio esta implementada en el proyecto de AppServices_
_El proyecto Infrestructura se encuentran modeladas las entidades de dominio, excepciones, y las interfaces tanto de los repositorios como de los services_


### Web Api en Azure 🔧

_la web se encuenta hosteada en Azure utilizando la version gratuita del servicio_

| Servicio Mutant | https://mutantchallenge.azurewebsites.net |
| Ubicación | Sur de Brasil | 

## Cómo consumir la WebApi ⚙️

_Dentro de la API vamos a encontrar los siguientes endpoint_

ENDPOINT: Mutant
URL: http://mutantchallenge.azurewebsites.net/api/Mutant
PETICION HTTP: POST
RESPUESTA:  Mutant: 200-OK  Human: 403-Forbidden / InvalidDNA: 400-BadRequest

ENDPOINT: Mutant
URL: http://mutantchallenge.azurewebsites.net/api/Mutant
PETICION HTTP: http://mutantchallenge.azurewebsites.net/api/Stats
RESPUESTA:  Mutant: 200-OK 

ENDPOINT: Stats
URL: http://mutantchallenge.azurewebsites.net/api/Mutant
PETICION HTTP: GET
RESPUESTA:  Mutant: 200-OK JSON


### Ejemplos 🔩

_POST → /mutant/_

```
DNA Humano:
{
	"dna": ["ATGCGA",
			"CAGTGC",
			"TTATTT",
			"AGACGG",
			"GCGTCA",
			"AAAAAA" ]
}

Response: 403-Forbidden
```

```
DNA Mutante:
{
	"dna": ["ATGCGA"
			"ACGTGC"
			"ATATGT"
			"AGAAGG"
			"GCCCTA"
			"ACACTG"]
}

Response: 200-OK
```

```
DNA Inválido:
{
	"dna": ["ATGCGA",
			"CAGTGG",
			"TCCACT",
			"ATaGGG",
			"CCTAAA",
			"TCATTG"]
}

Response: 400-BadRequest
```

_DELETE → /mutant/_
```
Response: Response: 200-OK
```

_GET → /stats/_
```
Response: 
{
    "count_mutant_dna": 1,
    "count_human_dna": 1,
    "ratio": 1.0
}
```


## Construido con 🛠️

_Menciona las herramientas que utilizaste para crear tu proyecto_

* [NET Framework 4.5](http://www.dropwizard.io/1.0.2/docs/) - El framework utilizado
* [Unity](http://unitycontainer.org/) - Inyeccion de dependencias
* [MSTest framework](https://docs.microsoft.com/en-us/visualstudio/test/using-microsoft-visualstudio-testtools-unittesting-members-in-unit-tests?view=vs-2019) - Utilizado para Los Test de Unidad


## Autor ✒️

* **Peña Medus, Leonel I.** - *Desarrollador* - [Leon-El](https://github.com/Leon-El)

## Licencia 📄

GNU GENERAL PUBLIC LICENSE - mira el archivo [LICENSE.md](LICENSE.md) para detalles

## Plantilla Readme🎁

* [Villanuevand](https://github.com/Villanuevand)

