using ProyectoBiblioteca.Modelos;
using ProyectoBiblioteca.Servicios;
// Se crean los servicios principales del sistema
Biblioteca biblioteca = new Biblioteca();
GestorArchivos gestorArchivos = new GestorArchivos();

string rutaArchivo = Path.Combine("Datos", "libros.csv");
// Carga inicial de libros desde el archivo CSV
gestorArchivos.CargarLibros(rutaArchivo, biblioteca);

int opcion;
// Menú principal del sistema
do
{
    Console.WriteLine("\n==============================");
    Console.WriteLine("     SISTEMA DE BIBLIOTECA");
    Console.WriteLine("==============================");
    Console.WriteLine("1. Registrar libro");
    Console.WriteLine("2. Buscar libro");
    Console.WriteLine("3. Eliminar libro");
    Console.WriteLine("4. Registrar préstamo");
    Console.WriteLine("5. Registrar devolución");
    Console.WriteLine("6. Mostrar catálogo");
    Console.WriteLine("7. Mostrar libro más prestado");
    Console.WriteLine("8. Mostrar menor disponibilidad");
    Console.WriteLine("9. Mostrar estructuras");
    Console.WriteLine("0. Salir");

    Console.Write("\nSeleccione una opción: ");

    if (!int.TryParse(Console.ReadLine(), out opcion))
    {
        Console.WriteLine("Opción inválida.");
        opcion = -1;
        continue;
    }

    switch (opcion)
    {
        case 1:
        {
            Console.WriteLine("\n--- REGISTRAR LIBRO ---");

            Console.Write("Código: ");

            if (!int.TryParse(Console.ReadLine(), out int codigo))
            {
                Console.WriteLine("Código inválido.");
                break;
            }

            Console.Write("Título: ");
            string titulo = Console.ReadLine() ?? "";

            Console.Write("Autor: ");
            string autor = Console.ReadLine() ?? "";

            Console.Write("Categoría: ");
            string categoria = Console.ReadLine() ?? "";

            Console.Write("Cantidad de copias: ");

            if (!int.TryParse(Console.ReadLine(), out int copias))
            {
                Console.WriteLine("Cantidad inválida.");
                break;
            }

            if (copias < 0)
            {
                Console.WriteLine("Las copias no pueden ser negativas.");
                break;
            }

            if (string.IsNullOrWhiteSpace(titulo) ||
                string.IsNullOrWhiteSpace(autor) ||
                string.IsNullOrWhiteSpace(categoria))
            {
                Console.WriteLine("Los datos no pueden quedar vacíos.");
                break;
            }

            Libro libro = new Libro(
                codigo,
                titulo,
                autor,
                categoria,
                copias,
                0
            );

            bool registrado = biblioteca.RegistrarLibro(libro);

            if (registrado)
            {
                Console.WriteLine("Libro registrado correctamente.");
            }
            else
            {
                Console.WriteLine("Ya existe un libro con ese código.");
            }

            break;
        }


        case 2:
        {
            Console.WriteLine("\n--- BUSCAR LIBRO ---");

            Console.Write("Código: ");

            if (!int.TryParse(Console.ReadLine(), out int codigo))
            {
                Console.WriteLine("Código inválido.");
                break;
            }

            Libro? libro = biblioteca.BuscarLibro(codigo);

            if (libro == null)
            {
                Console.WriteLine("Libro no encontrado.");
                break;
            }

            Console.WriteLine($"Código: {libro.Codigo}");
            Console.WriteLine($"Título: {libro.Titulo}");
            Console.WriteLine($"Autor: {libro.Autor}");
            Console.WriteLine($"Categoría: {libro.Categoria}");
            Console.WriteLine($"Copias disponibles: {libro.CopiasDisponibles}");
            Console.WriteLine($"Veces prestado: {libro.VecesPrestado}");

            break;
        }


        case 3:
        {
            Console.WriteLine("\n--- ELIMINAR LIBRO ---");

            Console.Write("Código: ");

            if (!int.TryParse(Console.ReadLine(), out int codigo))
            {
                Console.WriteLine("Código inválido.");
                break;
            }

            bool eliminado = biblioteca.EliminarLibro(codigo);

            if (eliminado)
            {
                Console.WriteLine("Libro eliminado correctamente.");
            }
            else
            {
                Console.WriteLine("Libro no encontrado.");
            }

            break;
        }


        case 4:
        {
            Console.WriteLine("\n--- REGISTRAR PRÉSTAMO ---");

            Console.Write("Código del libro: ");

            if (!int.TryParse(Console.ReadLine(), out int codigo))
            {
                Console.WriteLine("Código inválido.");
                break;
            }

            Libro? libro = biblioteca.BuscarLibro(codigo);

            if (libro == null)
            {
                Console.WriteLine("Libro no encontrado.");
                break;
            }

            if (libro.CopiasDisponibles <= 0)
            {
                Console.WriteLine("No hay copias disponibles.");
                break;
            }

            bool prestado = biblioteca.PrestarLibro(codigo);

            if (prestado)
            {
                Console.WriteLine("Préstamo realizado correctamente.");
                Console.WriteLine(
                    $"Copias disponibles: {libro.CopiasDisponibles}"
                );
            }
            else
            {
                Console.WriteLine("No fue posible realizar el préstamo.");
            }

            break;
        }


        case 5:
        {
            Console.WriteLine("\n--- REGISTRAR DEVOLUCIÓN ---");

            Console.Write("Código del libro: ");

            if (!int.TryParse(Console.ReadLine(), out int codigo))
            {
                Console.WriteLine("Código inválido.");
                break;
            }

            Libro? libro = biblioteca.BuscarLibro(codigo);

            if (libro == null)
            {
                Console.WriteLine("Libro no encontrado.");
                break;
            }

            bool devuelto = biblioteca.DevolverLibro(codigo);

            if (devuelto)
            {
                Console.WriteLine("Devolución realizada correctamente.");
                Console.WriteLine(
                    $"Copias disponibles: {libro.CopiasDisponibles}"
                );
            }
            else
            {
                Console.WriteLine("No hay copias pendientes de devolución.");
            }

            break;
        }


        case 6:
        {
            Console.WriteLine("\n--- CATÁLOGO ---");

            biblioteca.MostrarCatalogoOrdenadoPorTitulo();

            break;
        }


        case 7:
        {
            Console.WriteLine("\n--- LIBRO MÁS PRESTADO ---");

            Libro? libro = biblioteca.ObtenerMasPrestado();

            if (libro == null)
            {
                Console.WriteLine("No hay libros registrados.");
                break;
            }

            Console.WriteLine($"Código: {libro.Codigo}");
            Console.WriteLine($"Título: {libro.Titulo}");
            Console.WriteLine($"Préstamos: {libro.VecesPrestado}");

            break;
        }


        case 8:
        {
            Console.WriteLine("\n--- MENOR DISPONIBILIDAD ---");

            Libro? libro = biblioteca.ObtenerMenorDisponibilidad();

            if (libro == null)
            {
                Console.WriteLine("No hay libros registrados.");
                break;
            }

            Console.WriteLine($"Código: {libro.Codigo}");
            Console.WriteLine($"Título: {libro.Titulo}");
            Console.WriteLine(
                $"Copias disponibles: {libro.CopiasDisponibles}"
            );

            break;
        }


        case 9:
        {
            Console.WriteLine("\n--- ESTRUCTURAS ---");

            biblioteca.MostrarEstructuras();

            break;
        }


        case 0:
        {
            Console.WriteLine("\nPrograma finalizado.");
            break;
        }


        default:
        {
            Console.WriteLine("La opción no existe.");
            break;
        }
    }

} while (opcion != 0);