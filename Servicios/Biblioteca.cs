using ProyectoBiblioteca.Modelos;
using ProyectoBiblioteca.Estructuras;

namespace ProyectoBiblioteca.Servicios
{
    public class Biblioteca
    {
        private readonly ArbolBPlus arbol;
        private readonly MaxHeap maxHeap;
        private readonly MinHeap minHeap;

        private int cantidadLibros;

        public Biblioteca()
        {
            arbol = new ArbolBPlus(4);
            maxHeap = new MaxHeap(10);
            minHeap = new MinHeap(10);

            cantidadLibros = 0;
        }

        public bool RegistrarLibro(Libro libro)
        {
            if (arbol.Buscar(libro.Codigo) != null)
            {
                return false;
            }

            arbol.Insertar(libro);
            maxHeap.Insertar(libro);
            minHeap.Insertar(libro);

            cantidadLibros++;

            return true;
        }

        public Libro? BuscarLibro(int codigo)
        {
            return arbol.Buscar(codigo);
        }

        public Libro? ObtenerMasPrestado()
        {
            if (cantidadLibros == 0)
            {
                return null;
            }

            return maxHeap.VerMaximo();
        }

        public Libro? ObtenerMenorDisponibilidad()
        {
            if (cantidadLibros == 0)
            {
                return null;
            }

            return minHeap.VerMinimo();
        }

        public void MostrarCatalogo()
        {
            if (cantidadLibros == 0)
            {
                Console.WriteLine("No hay libros registrados.");
                return;
            }

            arbol.Recorrer();
        }

        public void MostrarEstructuras()
        {
            Console.WriteLine("\nÁRBOL B+:");
            arbol.Imprimir();

            Console.WriteLine("\nMAX HEAP:");
            maxHeap.Imprimir();

            Console.WriteLine("\nMIN HEAP:");
            minHeap.Imprimir();
        }

        public bool PrestarLibro(int codigo)
        {
            Libro? libro = arbol.Buscar(codigo);

            if (libro == null)
            {
                return false;
            }

            if (libro.CopiasDisponibles <= 0)
            {
                return false;
            }

            libro.CopiasDisponibles--;
            libro.VecesPrestado++;

            maxHeap.Reorganizar(codigo);
            minHeap.Reorganizar(codigo);

            return true;
        }

        public bool DevolverLibro(int codigo)
        {
            Libro? libro = arbol.Buscar(codigo);

            if (libro == null)
            {
                return false;
            }

            if (libro.CopiasDisponibles >= libro.CopiasTotales)
            {
                return false;
            }

            libro.CopiasDisponibles++;

            minHeap.Reorganizar(codigo);

            return true;
        }

        public void MostrarCatalogoOrdenadoPorTitulo()
        {
            if (cantidadLibros == 0)
            {
                Console.WriteLine("No hay libros registrados.");
                return;
            }

            Libro[] libros = new Libro[cantidadLibros];

            int cantidad = arbol.CopiarLibros(libros);

            // Ordenamiento por inserción
            for (int i = 1; i < cantidad; i++)
            {
                Libro actual = libros[i];
                int j = i - 1;

                while (
                    j >= 0 &&
                    string.Compare(
                        libros[j].Titulo,
                        actual.Titulo,
                        StringComparison.OrdinalIgnoreCase
                    ) > 0
                )
                {
                    libros[j + 1] = libros[j];
                    j--;
                }

                libros[j + 1] = actual;
            }

            for (int i = 0; i < cantidad; i++)
            {
                Console.WriteLine(
                    $"{libros[i].Codigo} | " +
                    $"{libros[i].Titulo} | " +
                    $"{libros[i].Autor} | " +
                    $"{libros[i].Categoria} | " +
                    $"Copias: {libros[i].CopiasDisponibles} | " +
                    $"Préstamos: {libros[i].VecesPrestado}"
                );
            }
        }

        public bool EliminarLibro(int codigo)
        {
            Libro? libro = arbol.Buscar(codigo);

            if (libro == null)
            {
                return false;
            }

            arbol.Eliminar(codigo);
            maxHeap.EliminarPorCodigo(codigo);
            minHeap.EliminarPorCodigo(codigo);

            cantidadLibros--;

            return true;
        }
    }
}