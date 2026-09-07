using ProyectoBiblioteca.Modelos;
using ProyectoBiblioteca.Estructuras;

// Crear libros de prueba
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
    "Ciencia ficción",
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


// =======================
// MAX HEAP
// =======================

Console.WriteLine("\n===== MAX HEAP =====");

MaxHeap maxHeap = new MaxHeap(3);

maxHeap.Insertar(libro1);
maxHeap.Insertar(libro2);
maxHeap.Insertar(libro3);
maxHeap.Insertar(libro4);
maxHeap.Insertar(libro5);
maxHeap.Insertar(libro6);
maxHeap.Insertar(libro7);
maxHeap.Insertar(libro8);

Console.WriteLine("\nContenido:");
maxHeap.Imprimir();

Libro maximo = maxHeap.VerMaximo();

Console.WriteLine(
    $"\nMás prestado: {maximo.Titulo} - " +
    $"{maximo.VecesPrestado} préstamos"
);

Libro? encontradoMax = maxHeap.BuscarPorCodigo(1006);

if (encontradoMax != null)
{
    Console.WriteLine(
        $"Código 1006 encontrado: {encontradoMax.Titulo}"
    );
}
else
{
    Console.WriteLine("Código 1006 no encontrado.");
}

Libro eliminadoMax = maxHeap.EliminarMaximo();

Console.WriteLine(
    $"\nMáximo eliminado: {eliminadoMax.Titulo}"
);

Console.WriteLine("\nMax Heap después de eliminar:");
maxHeap.Imprimir();


// =======================
// MIN HEAP
// =======================

Console.WriteLine("\n===== MIN HEAP =====");

MinHeap minHeap = new MinHeap(3);

minHeap.Insertar(libro1);
minHeap.Insertar(libro2);
minHeap.Insertar(libro3);
minHeap.Insertar(libro4);
minHeap.Insertar(libro5);
minHeap.Insertar(libro6);
minHeap.Insertar(libro7);
minHeap.Insertar(libro8);

Console.WriteLine("\nContenido:");
minHeap.Imprimir();

Libro minimo = minHeap.VerMinimo();

Console.WriteLine(
    $"\nMenor disponibilidad: {minimo.Titulo} - " +
    $"{minimo.CopiasDisponibles} copias"
);

Libro eliminadoMin = minHeap.EliminarMinimo();

Console.WriteLine(
    $"\nMínimo eliminado: {eliminadoMin.Titulo}"
);

Console.WriteLine("\nMin Heap después de eliminar:");
minHeap.Imprimir();


// =======================
// ÁRBOL B+
// =======================

Console.WriteLine("\n===== ÁRBOL B+ =====");

ArbolBPlus arbol = new ArbolBPlus(4);

arbol.Insertar(libro1);
arbol.Insertar(libro2);
arbol.Insertar(libro3);
arbol.Insertar(libro4);
arbol.Insertar(libro5);
arbol.Insertar(libro6);
arbol.Insertar(libro7);
arbol.Insertar(libro8);

Console.WriteLine("\nEstructura:");
arbol.Imprimir();

Console.WriteLine("\nRecorrido:");
arbol.Recorrer();


// Buscar un código existente
Console.WriteLine("\nBuscando código 1006:");

Libro? encontradoArbol = arbol.Buscar(1006);

if (encontradoArbol != null)
{
    Console.WriteLine(
        $"Encontrado: {encontradoArbol.Titulo}"
    );
}
else
{
    Console.WriteLine("Libro no encontrado.");
}


// Buscar un código inexistente
Console.WriteLine("\nBuscando código 9999:");

Libro? inexistente = arbol.Buscar(9999);

if (inexistente == null)
{
    Console.WriteLine("Código 9999 no existe.");
}
else
{
    Console.WriteLine(
        $"Encontrado: {inexistente.Titulo}"
    );
}


// Eliminar del Árbol B+
Console.WriteLine("\nEliminando código 1004:");

bool eliminado = arbol.Eliminar(1004);

if (eliminado)
{
    Console.WriteLine("Libro eliminado correctamente.");
}
else
{
    Console.WriteLine("Libro no encontrado.");
}

Console.WriteLine("\nRecorrido después de eliminar 1004:");
arbol.Recorrer();


// Más eliminaciones para probar rebalanceo
Console.WriteLine("\nEliminando 1001 y 1002...");

arbol.Eliminar(1001);
arbol.Eliminar(1002);

Console.WriteLine("\nEstructura final:");
arbol.Imprimir();

Console.WriteLine("\nRecorrido final:");
arbol.Recorrer();