using ProyectoBiblioteca.Modelos;

Libro libroPrueba = new Libro(
    1001,
    "El Principito",
    "Antoine de Saint-Exupéry",
    "Literatura",
    5,
    10
);

Console.WriteLine(libroPrueba.Titulo); // Imprime: El Principito
Console.WriteLine(libroPrueba.Autor); // Imprime: Antoine de Saint-Exupéry
Console.WriteLine(libroPrueba.Categoria); // Imprime: Literatura