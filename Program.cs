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

Console.WriteLine("\n===== PRÉSTAMO =====");

Libro? antesPrestamo =
    biblioteca.BuscarLibro(1001);

if (antesPrestamo != null)
{
    Console.WriteLine(
        $"Antes: {antesPrestamo.CopiasDisponibles} copias - " +
        $"{antesPrestamo.VecesPrestado} préstamos"
    );
}

bool prestado =
    biblioteca.PrestarLibro(1001);

if (prestado)
{
    Console.WriteLine("Préstamo realizado correctamente.");
}
else
{
    Console.WriteLine("No fue posible realizar el préstamo.");
}

Libro? despuesPrestamo =
    biblioteca.BuscarLibro(1001);

if (despuesPrestamo != null)
{
    Console.WriteLine(
        $"Después: {despuesPrestamo.CopiasDisponibles} copias - " +
        $"{despuesPrestamo.VecesPrestado} préstamos"
    );
}


Console.WriteLine("\n===== DEVOLUCIÓN =====");

bool devuelto =
    biblioteca.DevolverLibro(1001);

if (devuelto)
{
    Console.WriteLine("Devolución realizada correctamente.");
}
else
{
    Console.WriteLine("No fue posible realizar la devolución.");
}

Libro? despuesDevolucion =
    biblioteca.BuscarLibro(1001);

if (despuesDevolucion != null)
{
    Console.WriteLine(
        $"Después de devolver: " +
        $"{despuesDevolucion.CopiasDisponibles} copias - " +
        $"{despuesDevolucion.VecesPrestado} préstamos"
    );
}