using ProyectoBiblioteca.Modelos;

namespace ProyectoBiblioteca.Estructuras
{
    public class MinHeap
    {
        private Libro[] heap;
        private int cantidad;

        public MinHeap(int capacidadInicial)
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

        public Libro VerMinimo()
        {
            if (cantidad == 0)
            {
                throw new InvalidOperationException("El Min Heap está vacío.");
            }

            return heap[0];
        }

        public Libro EliminarMinimo()
        {
            if (cantidad == 0)
            {
                throw new InvalidOperationException("El Min Heap está vacío.");
            }

            Libro minimo = heap[0];

            cantidad--;

            if (cantidad > 0)
            {
                heap[0] = heap[cantidad];
                Bajar(0);
            }

            return minimo;
        }

        private void Subir(int indice)
        {
            while (indice > 0)
            {
                int padre = (indice - 1) / 2;

                if (heap[indice].CopiasDisponibles >=
                    heap[padre].CopiasDisponibles)
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
                int menor = indice;

                if (izquierdo < cantidad &&
                    heap[izquierdo].CopiasDisponibles < heap[menor].CopiasDisponibles)
                {
                    menor = izquierdo;
                }

                if (derecho < cantidad &&
                    heap[derecho].CopiasDisponibles < heap[menor].CopiasDisponibles)
                {
                    menor = derecho;
                }

                if (menor == indice)
                {
                    break;
                }

                Intercambiar(indice, menor);
                indice = menor;
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
                    $"{heap[i].Titulo} - Copias disponibles: {heap[i].CopiasDisponibles}"
                );
            }
        }

        public void Reorganizar(int codigo)
        {
            int indice = BuscarIndicePorCodigo(codigo);

            if (indice == -1)
            {
                return;
            }

            Subir(indice);

            indice = BuscarIndicePorCodigo(codigo);

            if (indice != -1)
            {
                Bajar(indice);
            }
        }

        private int BuscarIndicePorCodigo(int codigo)
        {
            for (int i = 0; i < cantidad; i++)
            {
                if (heap[i].Codigo == codigo)
                {
                    return i;
                }
            }

            return -1;
        }

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

        public bool EliminarPorCodigo(int codigo)
        {
            int indice = BuscarIndicePorCodigo(codigo);

            if (indice == -1)
            {
                return false;
            }

            cantidad--;

            if (indice < cantidad)
            {
                Libro ultimo = heap[cantidad];

                heap[indice] = ultimo;

                Subir(indice);

                int nuevoIndice = BuscarIndicePorCodigo(ultimo.Codigo);

                if (nuevoIndice != -1)
                {
                    Bajar(nuevoIndice);
                }
            }

            return true;
        }
    }
}