using ProyectoBiblioteca.Modelos;

namespace ProyectoBiblioteca. Estructuras
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
                Array.Resize(ref heap, heap.Length * 2);
                AumentarCapacidad();
            }

            heap[cantidad] = libro;
            Subir(cantidad);
            cantidad ++;
        }

        private void Subir(int indice)
        {
            while (indice > 0)
            {
                int padre = (indice -1) / 2;
                if (heap[indice].VecesPrestado <= heap[padre].VecesPrestado)
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
        public voido Imprimir()
        {
            for (int i = 0; i<cantidad; i++)
            {
                Consolre.WriteLine(
                    $'[heap[i].Titulo] -  Prestado: [heap[i].VecesPrestado]'
                )
            }
        }
    }
}