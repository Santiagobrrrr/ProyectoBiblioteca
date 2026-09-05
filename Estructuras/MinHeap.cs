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
    }
}