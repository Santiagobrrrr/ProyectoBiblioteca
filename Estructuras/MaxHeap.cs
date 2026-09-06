using ProyectoBiblioteca.Modelos;

namespace ProyectoBiblioteca.Estructuras
{
    public class MaxHeap
    {
        private Libro[] heap;
        private int cantidad;

        // Constructor
        public MaxHeap(int capacidadInicial)
        {
            if (capacidadInicial <= 0)
            {
                capacidadInicial = 1;
            }

            heap = new Libro[capacidadInicial];
            cantidad = 0;
        }

        // Inserta un libro en el Max Heap
        public void Insertar(Libro libro)
        {
            if (cantidad == heap.Length)
            {
                AumentarCapacidad();
            }

            // Se inserta al final
            heap[cantidad] = libro;

            // Se reorganiza hacia arriba
            Subir(cantidad);

            cantidad++;
        }

        // Devuelve el libro con más préstamos
        public Libro VerMaximo()
        {
            if (cantidad == 0)
            {
                throw new InvalidOperationException(
                    "El Max Heap está vacío."
                );
            }

            return heap[0];
        }

        // Busca un libro por su código
        public Libro? BuscarPorCodigo(int codigo)
        {
            for (int i = 0; i < cantidad; i++)
            {
                if (heap[i].Codigo == codigo)
                {
                    return heap[i];
                }
            }

            return null;
        }

        // Elimina el libro con mayor cantidad de préstamos
        public Libro EliminarMaximo()
        {
            if (cantidad == 0)
            {
                throw new InvalidOperationException(
                    "El Max Heap está vacío."
                );
            }

            // Guardamos el máximo antes de modificar el Heap
            Libro maximo = heap[0];

            // Reducimos la cantidad de elementos
            cantidad--;

            if (cantidad > 0)
            {
                // El último elemento pasa a la raíz
                heap[0] = heap[cantidad];

                // Reorganizamos hacia abajo
                Bajar(0);
            }

            return maximo;
        }

        // Reorganiza un elemento hacia arriba
        private void Subir(int indice)
        {
            while (indice > 0)
            {
                int padre = (indice - 1) / 2;

                // Si el padre ya es mayor o igual, terminamos
                if (heap[indice].VecesPrestado <=
                    heap[padre].VecesPrestado)
                {
                    break;
                }

                Intercambiar(indice, padre);

                indice = padre;
            }
        }

        // Reorganiza un elemento hacia abajo
        private void Bajar(int indice)
        {
            while (true)
            {
                int izquierdo = (2 * indice) + 1;
                int derecho = (2 * indice) + 2;
                int mayor = indice;

                // Compara con el hijo izquierdo
                if (izquierdo < cantidad &&
                    heap[izquierdo].VecesPrestado >
                    heap[mayor].VecesPrestado)
                {
                    mayor = izquierdo;
                }

                // Compara con el hijo derecho
                if (derecho < cantidad &&
                    heap[derecho].VecesPrestado >
                    heap[mayor].VecesPrestado)
                {
                    mayor = derecho;
                }

                // Si el actual ya es el mayor, terminamos
                if (mayor == indice)
                {
                    break;
                }

                Intercambiar(indice, mayor);

                indice = mayor;
            }
        }

        // Intercambia dos libros dentro del arreglo
        private void Intercambiar(int indice1, int indice2)
        {
            Libro temporal = heap[indice1];

            heap[indice1] = heap[indice2];

            heap[indice2] = temporal;
        }

        // Duplica la capacidad del arreglo
        private void AumentarCapacidad()
        {
            Libro[] nuevoHeap =
                new Libro[heap.Length * 2];

            for (int i = 0; i < cantidad; i++)
            {
                nuevoHeap[i] = heap[i];
            }

            heap = nuevoHeap;
        }

        // Imprime los elementos del Heap
        public void Imprimir()
        {
            for (int i = 0; i < cantidad; i++)
            {
                Console.WriteLine(
                    $"{heap[i].Titulo} - " +
                    $"Préstamos: {heap[i].VecesPrestado}"
                );
            }
        }
    }
}