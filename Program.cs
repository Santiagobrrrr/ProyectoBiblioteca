using ProyectoBiblioteca.Modelos;
using ProyectoBiblioteca.Servicios;

Biblioteca biblioteca = new Biblioteca();
GestorArchivos gestorArchivos = new GestorArchivos();

string rutaArchivo = Path.Combine(
    "Datos",
    "libros.csv"
);

gestorArchivos.CargarLibros(
    rutaArchivo,
    biblioteca
);

Console.WriteLine("\n===== CATÁLOGO CARGADO =====");

biblioteca.MostrarCatalogo();


Console.WriteLine("\n===== BÚSQUEDA =====");

Libro? encontrado =
    biblioteca.BuscarLibro(1006);

if (encontrado != null)
{
    Console.WriteLine(
        $"Encontrado: {encontrado.Titulo}"
    );
}
else
{
    Console.WriteLine(
        "Libro no encontrado."
    );
}


Console.WriteLine("\n===== MÁS PRESTADO =====");

Libro? masPrestado =
    biblioteca.ObtenerMasPrestado();

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