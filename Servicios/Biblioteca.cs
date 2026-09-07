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
    }
}