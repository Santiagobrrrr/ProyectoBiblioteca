using ProyectoBiblioteca.Modelos;
using ProyectoBiblioteca.Servicios;

Biblioteca biblioteca = new Biblioteca();

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

biblioteca.RegistrarLibro(libro1);
biblioteca.RegistrarLibro(libro2);
biblioteca.RegistrarLibro(libro3);
biblioteca.RegistrarLibro(libro4);

Console.WriteLine("===== CATÁLOGO =====");

biblioteca.MostrarCatalogo();


Console.WriteLine("\n===== BÚSQUEDA =====");

Libro? encontrado = biblioteca.BuscarLibro(1002);

if (encontrado != null)
{
    Console.WriteLine(
        $"Encontrado: {encontrado.Titulo}"
    );
}
else
{
    Console.WriteLine("Libro no encontrado.");
}


Console.WriteLine("\n===== MÁS PRESTADO =====");

Libro? masPrestado = biblioteca.ObtenerMasPrestado();

if (masPrestado != null)
{
    Console.WriteLine(
        $"{masPrestado.Titulo} - " +
        $"{masPrestado.VecesPrestado} préstamos"
    );
}


Console.WriteLine("\n===== MENOR DISPONIBILIDAD =====");

Libro? menorDisponibilidad =
    biblioteca.ObtenerMenorDisponibilidad();

if (menorDisponibilidad != null)
{
    Console.WriteLine(
        $"{menorDisponibilidad.Titulo} - " +
        $"{menorDisponibilidad.CopiasDisponibles} copias"
    );
}


Console.WriteLine("\n===== ESTRUCTURAS =====");

biblioteca.MostrarEstructuras();