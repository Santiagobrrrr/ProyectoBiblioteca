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

MaxHeap maxHeap = new MaxHeap(3);

maxHeap.Insertar(libro1);
maxHeap.Insertar(libro2);
maxHeap.Insertar(libro3);
maxHeap.Insertar(libro4);

maxHeap.Imprimir();

Console.WriteLine("\nMAX HEAP INICIAL:");
maxHeap.Imprimir();

Console.WriteLine("\nLIBRO MÁS PRESTADO:");

Libro mayor = maxHeap.VerMaximo();

Console.WriteLine(
    $"{mayor.Titulo} - {mayor.VecesPrestado} préstamos"
);

Console.WriteLine("\nELIMINANDO EL MÁXIMO:");

Libro eliminado = maxHeap.EliminarMaximo();

Console.WriteLine(
    $"Eliminado: {eliminado.Titulo}"
);

Console.WriteLine("\nMAX HEAP DESPUÉS DE ELIMINAR:");

maxHeap.Imprimir();

MinHeap minHeap = new MinHeap(3);

minHeap.Insertar(libro1);
minHeap.Insertar(libro2);
minHeap.Insertar(libro3);
minHeap.Insertar(libro4);

Console.WriteLine("\nMIN HEAP:");

minHeap.Imprimir();