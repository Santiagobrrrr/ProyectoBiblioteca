using ProyectoBiblioteca.Modelos;

namespace ProyectoBiblioteca.Estructuras
{
    public class ArbolBPlus
    {
        private class Nodo
        {
            public bool EsHoja;
            public int CantidadClaves;
            public int[] Claves;
            public Libro?[] Libros;
            public Nodo?[] Hijos;
            public Nodo? Siguiente;

            public Nodo(int orden, bool esHoja)
            {
                EsHoja = esHoja;
                CantidadClaves = 0;

                // Máximo de claves = orden - 1
                Claves = new int[orden - 1];

                // Los libros solamente se utilizan realmente en hojas.
                Libros = new Libro?[orden - 1];

                // Máximo de hijos = orden
                Hijos = new Nodo?[orden];

                Siguiente = null;
            }
        }

        private Nodo raiz;
        private int orden;

        public ArbolBPlus(int orden = 4)
        {
            if (orden < 3)
            {
                orden = 3;
            }

            this.orden = orden;

            // Al principio la raíz también es una hoja.
            raiz = new Nodo(orden, true);
        }

        public void Insertar(Libro libro)
        {
            // No permitimos códigos duplicados.
            if (Buscar(libro.Codigo) != null)
            {
                throw new InvalidOperationException(
                    "Ya existe un libro con ese código."
                );
            }

            bool huboDivision = InsertarRecursivo(
                raiz,
                libro,
                out int clavePromovida,
                out Nodo? nuevoDerecho
            );

            // Si la raíz se dividió, necesitamos crear una nueva raíz.
            if (huboDivision && nuevoDerecho != null)
            {
                Nodo nuevaRaiz = new Nodo(orden, false);

                nuevaRaiz.Claves[0] = clavePromovida;

                nuevaRaiz.Hijos[0] = raiz;
                nuevaRaiz.Hijos[1] = nuevoDerecho;

                nuevaRaiz.CantidadClaves = 1;

                raiz = nuevaRaiz;
            }
        }

        private bool InsertarRecursivo(
            Nodo nodo,
            Libro libro,
            out int clavePromovida,
            out Nodo? nuevoDerecho)
        {
            clavePromovida = 0;
            nuevoDerecho = null;

            // CASO 1: llegamos a una hoja.
            if (nodo.EsHoja)
            {
                // Si todavía hay espacio, insertamos normalmente.
                if (nodo.CantidadClaves < orden - 1)
                {
                    InsertarEnHoja(nodo, libro);

                    return false;
                }

                // Si está llena, debemos dividirla.
                DividirHoja(
                    nodo,
                    libro,
                    out clavePromovida,
                    out nuevoDerecho
                );

                return true;
            }

            // CASO 2: estamos en un nodo interno.

            int indiceHijo = 0;

            while (
                indiceHijo < nodo.CantidadClaves &&
                libro.Codigo >= nodo.Claves[indiceHijo]
            )
            {
                indiceHijo++;
            }

            Nodo? hijo = nodo.Hijos[indiceHijo];

            if (hijo == null)
            {
                throw new InvalidOperationException(
                    "El Árbol B+ contiene un hijo inválido."
                );
            }

            bool hijoSeDividio = InsertarRecursivo(
                hijo,
                libro,
                out int claveDelHijo,
                out Nodo? derechoDelHijo
            );

            // Si el hijo no se dividió, no tenemos nada más que hacer.
            if (!hijoSeDividio || derechoDelHijo == null)
            {
                return false;
            }

            // El hijo se dividió y este nodo todavía tiene espacio.
            if (nodo.CantidadClaves < orden - 1)
            {
                InsertarEnNodoInterno(
                    nodo,
                    indiceHijo,
                    claveDelHijo,
                    derechoDelHijo
                );

                return false;
            }

            // Este nodo también está lleno.
            // Por lo tanto también debemos dividirlo.
            DividirNodoInterno(
                nodo,
                indiceHijo,
                claveDelHijo,
                derechoDelHijo,
                out clavePromovida,
                out nuevoDerecho
            );

            return true;
        }

        private void InsertarEnHoja(Nodo hoja, Libro libro)
        {
            int i = hoja.CantidadClaves - 1;

            // Movemos los valores mayores hacia la derecha.
            while (i >= 0 && libro.Codigo < hoja.Claves[i])
            {
                hoja.Claves[i + 1] = hoja.Claves[i];
                hoja.Libros[i + 1] = hoja.Libros[i];

                i--;
            }

            hoja.Claves[i + 1] = libro.Codigo;
            hoja.Libros[i + 1] = libro;

            hoja.CantidadClaves++;
        }

        private void DividirHoja(
            Nodo hoja,
            Libro libro,
            out int clavePromovida,
            out Nodo? nuevaHoja)
        {
            // La hoja normalmente tiene orden - 1 claves.
            // Creamos arreglos temporales con un espacio adicional.
            int[] clavesTemporales = new int[orden];
            Libro?[] librosTemporales = new Libro?[orden];

            int posicion = 0;

            // Buscamos dónde debe quedar el nuevo libro.
            while (
                posicion < hoja.CantidadClaves &&
                hoja.Claves[posicion] < libro.Codigo
            )
            {
                posicion++;
            }

            // Copiamos los elementos anteriores.
            for (int i = 0; i < posicion; i++)
            {
                clavesTemporales[i] = hoja.Claves[i];
                librosTemporales[i] = hoja.Libros[i];
            }

            // Colocamos el nuevo libro.
            clavesTemporales[posicion] = libro.Codigo;
            librosTemporales[posicion] = libro;

            // Copiamos los elementos posteriores.
            for (int i = posicion; i < hoja.CantidadClaves; i++)
            {
                clavesTemporales[i + 1] = hoja.Claves[i];
                librosTemporales[i + 1] = hoja.Libros[i];
            }

            int totalClaves = hoja.CantidadClaves + 1;

            // Dividimos aproximadamente por la mitad.
            int cantidadIzquierda = (totalClaves + 1) / 2;

            hoja.Claves = new int[orden - 1];
            hoja.Libros = new Libro?[orden - 1];

            hoja.CantidadClaves = cantidadIzquierda;

            // Primera mitad permanece en la hoja original.
            for (int i = 0; i < cantidadIzquierda; i++)
            {
                hoja.Claves[i] = clavesTemporales[i];
                hoja.Libros[i] = librosTemporales[i];
            }

            // Creamos la nueva hoja derecha.
            nuevaHoja = new Nodo(orden, true);

            int cantidadDerecha = totalClaves - cantidadIzquierda;

            nuevaHoja.CantidadClaves = cantidadDerecha;

            for (int i = 0; i < cantidadDerecha; i++)
            {
                nuevaHoja.Claves[i] =
                    clavesTemporales[cantidadIzquierda + i];

                nuevaHoja.Libros[i] =
                    librosTemporales[cantidadIzquierda + i];
            }

            // Conectamos las hojas.
            nuevaHoja.Siguiente = hoja.Siguiente;
            hoja.Siguiente = nuevaHoja;

            // La primera clave de la hoja derecha
            // sube al nodo padre como separador.
            clavePromovida = nuevaHoja.Claves[0];
        }

        private void InsertarEnNodoInterno(
            Nodo nodo,
            int indiceHijo,
            int clave,
            Nodo nuevoDerecho)
        {
            // Movemos claves e hijos hacia la derecha.
            for (int i = nodo.CantidadClaves; i > indiceHijo; i--)
            {
                nodo.Claves[i] = nodo.Claves[i - 1];
                nodo.Hijos[i + 1] = nodo.Hijos[i];
            }

            nodo.Claves[indiceHijo] = clave;
            nodo.Hijos[indiceHijo + 1] = nuevoDerecho;

            nodo.CantidadClaves++;
        }

        private void DividirNodoInterno(
            Nodo nodo,
            int indiceHijo,
            int nuevaClave,
            Nodo nuevoHijoDerecho,
            out int clavePromovida,
            out Nodo? nuevoDerecho)
        {
            // Un nodo lleno tendrá orden - 1 claves.
            // La nueva inserción crea temporalmente orden claves.
            int[] clavesTemporales = new int[orden];
            Nodo?[] hijosTemporales = new Nodo?[orden + 1];

            // Copiamos las claves anteriores al punto de inserción.
            for (int i = 0; i < indiceHijo; i++)
            {
                clavesTemporales[i] = nodo.Claves[i];
            }

            // Insertamos la nueva clave.
            clavesTemporales[indiceHijo] = nuevaClave;

            // Copiamos el resto de claves.
            for (int i = indiceHijo; i < nodo.CantidadClaves; i++)
            {
                clavesTemporales[i + 1] = nodo.Claves[i];
            }

            // Copiamos hijos anteriores.
            for (int i = 0; i <= indiceHijo; i++)
            {
                hijosTemporales[i] = nodo.Hijos[i];
            }

            // Insertamos el nuevo hijo derecho.
            hijosTemporales[indiceHijo + 1] = nuevoHijoDerecho;

            // Copiamos los hijos restantes.
            for (
                int i = indiceHijo + 1;
                i <= nodo.CantidadClaves;
                i++)
            {
                hijosTemporales[i + 1] = nodo.Hijos[i];
            }

            int totalClaves = nodo.CantidadClaves + 1;

            int medio = totalClaves / 2;

            // La clave central sube al padre.
            clavePromovida = clavesTemporales[medio];

            // Reconstruimos el nodo izquierdo.
            nodo.Claves = new int[orden - 1];
            nodo.Hijos = new Nodo?[orden];

            nodo.CantidadClaves = medio;

            for (int i = 0; i < medio; i++)
            {
                nodo.Claves[i] = clavesTemporales[i];
            }

            for (int i = 0; i <= medio; i++)
            {
                nodo.Hijos[i] = hijosTemporales[i];
            }

            // Creamos el nodo derecho.
            nuevoDerecho = new Nodo(orden, false);

            int cantidadDerecha = totalClaves - medio - 1;

            nuevoDerecho.CantidadClaves = cantidadDerecha;

            for (int i = 0; i < cantidadDerecha; i++)
            {
                nuevoDerecho.Claves[i] =
                    clavesTemporales[medio + 1 + i];
            }

            for (int i = 0; i <= cantidadDerecha; i++)
            {
                nuevoDerecho.Hijos[i] =
                    hijosTemporales[medio + 1 + i];
            }
        }

        public Libro? Buscar(int codigo)
        {
            Nodo actual = raiz;

            // Bajamos por nodos internos hasta encontrar una hoja.
            while (!actual.EsHoja)
            {
                int i = 0;

                while (
                    i < actual.CantidadClaves &&
                    codigo >= actual.Claves[i]
                )
                {
                    i++;
                }

                Nodo? siguiente = actual.Hijos[i];

                if (siguiente == null)
                {
                    return null;
                }

                actual = siguiente;
            }

            // Buscamos dentro de la hoja.
            for (int i = 0; i < actual.CantidadClaves; i++)
            {
                if (actual.Claves[i] == codigo)
                {
                    return actual.Libros[i];
                }
            }

            return null;
        }

        public void Imprimir()
        {
            Nodo? actual = raiz;

            // Llegamos hasta la hoja ubicada más a la izquierda.
            while (actual != null && !actual.EsHoja)
            {
                actual = actual.Hijos[0];
            }

            // Recorremos todas las hojas mediante Siguiente.
            while (actual != null)
            {
                for (int i = 0; i < actual.CantidadClaves; i++)
                {
                    Libro? libro = actual.Libros[i];

                    if (libro != null)
                    {
                        Console.WriteLine(
                            $"{libro.Codigo} - {libro.Titulo}"
                        );
                    }
                }

                actual = actual.Siguiente;
            }
        }
    }
}