using ProyectoBiblioteca.Modelos;
using ProyectoBiblioteca.Estructuras;

Libro libro1 = new Libro(
    1001,
    "El Principito",
    "Antoine de Saint-Exupéry",
    "Literatura",
    5,
    10
);

Libro libro2 = new Libro(
    1002,
    "Clean Code",
    "Robert C. Martin",
    "Programación",
    3,
    20
);

Libro libro3 = new Libro(
    1003,
    "Don Quijote",
    "Miguel de Cervantes",
    "Literatura",
    4,
    15
);

Libro libro4 = new Libro(
    1004,
    "1984",
    "George Orwell",
    "Literatura",
    2,
    25
);

Libro libro5 = new Libro(
    1005,
    "Harry Potter",
    "J. K. Rowling",
    "Fantasía",
    6,
    30
);

Libro libro6 = new Libro(
    1006,
    "El Hobbit",
    "J. R. R. Tolkien",
    "Fantasía",
    4,
    22
);

Libro libro7 = new Libro(
    1007,
    "Crónica de una muerte anunciada",
    "Gabriel García Márquez",
    "Literatura",
    3,
    17
);

Libro libro8 = new Libro(
    1008,
    "Fahrenheit 451",
    "Ray Bradbury",
    "Ciencia ficción",
    5,
    19
);

MaxHeap maxHeap = new MaxHeap(3);

maxHeap.Insertar(libro1);
maxHeap.Insertar(libro2);
maxHeap.Insertar(libro3);
maxHeap.Insertar(libro4);
maxHeap.Insertar(libro5);
maxHeap.Insertar(libro6);
maxHeap.Insertar(libro7);
maxHeap.Insertar(libro8);

Console.WriteLine("\nContenido del Max Heap:");

maxHeap.Imprimir();


Console.WriteLine("\nLibro más prestado:");

Libro libroMaximo = maxHeap.VerMaximo();

Console.WriteLine(
    $"{libroMaximo.Titulo} - " +
    $"{libroMaximo.VecesPrestado} préstamos"
);

// Prueba de búsqueda por código
Console.WriteLine("\nBuscando código 1006:");

Libro? encontradoMax = maxHeap.BuscarPorCodigo(1006);

if (encontradoMax != null)
{
    Console.WriteLine(
        $"Encontrado: {encontradoMax.Titulo}"
    );
}
else
{
    Console.WriteLine("Libro no encontrado.");
}


// Prueba de eliminación
Console.WriteLine("\nEliminando libro más prestado:");

Libro eliminadoMax = maxHeap.EliminarMaximo();

Console.WriteLine(
    $"Eliminado: {eliminadoMax.Titulo}"
);

Console.WriteLine("\nMax Heap después de eliminar:");

maxHeap.Imprimir();

MinHeap minHeap = new MinHeap(3);

minHeap.Insertar(libro1);
minHeap.Insertar(libro2);
minHeap.Insertar(libro3);
minHeap.Insertar(libro4);
minHeap.Insertar(libro5);
minHeap.Insertar(libro6);
minHeap.Insertar(libro7);
minHeap.Insertar(libro8);

Console.WriteLine("\nContenido del Min Heap:");

minHeap.Imprimir();


Console.WriteLine("\nLibro con menos copias disponibles:");

Libro libroMinimo = minHeap.VerMinimo();

Console.WriteLine(
    $"{libroMinimo.Titulo} - " +
    $"{libroMinimo.CopiasDisponibles} copias"
);


// Prueba de eliminación
Console.WriteLine("\nEliminando libro con menos copias:");

Libro eliminadoMin = minHeap.EliminarMinimo();

Console.WriteLine(
    $"Eliminado: {eliminadoMin.Titulo}"
);

Console.WriteLine("\nMin Heap después de eliminar:");

minHeap.Imprimir();

ArbolBPlus arbol = new ArbolBPlus(4);

arbol.Insertar(libro1);
arbol.Insertar(libro2);
arbol.Insertar(libro3);
arbol.Insertar(libro4);
arbol.Insertar(libro5);
arbol.Insertar(libro6);
arbol.Insertar(libro7);
arbol.Insertar(libro8);

Console.WriteLine("\nLibros almacenados en el Árbol B+:");

arbol.Imprimir();

Console.WriteLine("\nBuscando código 1006:");

Libro? encontradoArbol = arbol.Buscar(1006);

if (encontradoArbol != null)
{
    Console.WriteLine(
        $"Encontrado: {encontradoArbol.Titulo}"
    );

    Console.WriteLine(
        $"Autor: {encontradoArbol.Autor}"
    );

    Console.WriteLine(
        $"Categoría: {encontradoArbol.Categoria}"
    );
}
else
{
    Console.WriteLine("Libro no encontrado.");
}

Console.WriteLine("\nBuscando código 9999:");

Libro? noEncontrado = arbol.Buscar(9999);

if (noEncontrado != null)
{
    Console.WriteLine(
        $"Encontrado: {noEncontrado.Titulo}"
    );
}
else
{
    Console.WriteLine("Libro no encontrado.");
}
