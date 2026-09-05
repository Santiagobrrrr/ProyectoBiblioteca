using ProyectoBiblioteca.Modelos;

namespace ProyectoBiblioteca.Estructuras
{
    public class MaxHeap
    {
        private Libro[] heap;
        private int cantidad;

        public MaxHeap(int capacidadInicial)
        {
            heap = new Libro[capacidadInicial];
            cantidad = 0;
        }

        public void Insertar(Libro libro)
        {
            if (cantidad == heap.Length)
            {
                AumentarCapacidad();
            }

            heap[cantidad] = libro;
            Subir(cantidad);
            cantidad++;
        }

        public Libro VerMaximo()
        {
            if (cantidad == 0)
            {
                throw new InvalidOperationException("El Max Heap está vacío.");
            }

            return heap[0];
        }

        public Libro EliminarMaximo()
        {
            if (cantidad == 0)
            {
                throw new InvalidOperationException("El Max Heap está vacío.");
            }

            Libro maximo = heap[0];

            cantidad--;

            if (cantidad > 0)
            {
                heap[0] = heap[cantidad];
                Bajar(0);
            }

            return maximo;
        }

        private void Subir(int indice)
        {
            while (indice > 0)
            {
                int padre = (indice - 1) / 2;

                if (heap[indice].VecesPrestado <= heap[padre].VecesPrestado)
                {
                    break;
                }

                Intercambiar(indice, padre);
                indice = padre;
            }
        }

        private void Bajar(int indice)
        {
            while (true)
            {
                int izquierdo = (2 * indice) + 1;
                int derecho = (2 * indice) + 2;
                int mayor = indice;

                if (izquierdo < cantidad &&
                    heap[izquierdo].VecesPrestado > heap[mayor].VecesPrestado)
                {
                    mayor = izquierdo;
                }

                if (derecho < cantidad &&
                    heap[derecho].VecesPrestado > heap[mayor].VecesPrestado)
                {
                    mayor = derecho;
                }

                if (mayor == indice)
                {
                    break;
                }

                Intercambiar(indice, mayor);
                indice = mayor;
            }
        }

        private void Intercambiar(int indice1, int indice2)
        {
            Libro temporal = heap[indice1];
            heap[indice1] = heap[indice2];
            heap[indice2] = temporal;
        }

        private void AumentarCapacidad()
        {
            Libro[] nuevoHeap = new Libro[heap.Length * 2];

            for (int i = 0; i < cantidad; i++)
            {
                nuevoHeap[i] = heap[i];
            }

            heap = nuevoHeap;
        }

        public void Imprimir()
        {
            for (int i = 0; i < cantidad; i++)
            {
                Console.WriteLine(
                    $"{heap[i].Titulo} - Prestado: {heap[i].VecesPrestado}"
                );
            }
        }
    }
}