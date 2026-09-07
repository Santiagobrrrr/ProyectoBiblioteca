using ProyectoBiblioteca.Modelos;
using ProyectoBiblioteca.Servicios;

Biblioteca biblioteca = new Biblioteca();
GestorArchivos gestor = new GestorArchivos();

string ruta = Path.Combine(
    "Datos",
    "libros.csv"
);

gestor.CargarLibros(ruta, biblioteca);

Console.WriteLine("CATÁLOGO ANTES:");
biblioteca.MostrarCatalogo();

Console.WriteLine("\nEliminando libro 1004...");

bool eliminado = biblioteca.EliminarLibro(1004);

if (eliminado)
{
    Console.WriteLine("Libro eliminado.");
}
else
{
    Console.WriteLine("Libro no encontrado.");
}

Console.WriteLine("\nCATÁLOGO DESPUÉS:");
biblioteca.MostrarCatalogo();

Console.WriteLine("\nBUSCANDO 1004:");

Libro? libro = biblioteca.BuscarLibro(1004);

if (libro == null)
{
    Console.WriteLine("El libro ya no existe.");
}
else
{
    Console.WriteLine("El libro todavía existe.");
}

Console.WriteLine("\nESTRUCTURAS:");
biblioteca.MostrarEstructuras();